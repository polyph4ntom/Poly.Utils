using Poly.Validation.DataContainers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System;
using Poly.Validation.Utils;
using UnityEditor;
using UnityEngine.Assertions;
using UnityEngine;

namespace Poly.Validation
{
    internal static class PolyValidationSubsystem
    {
	    private static readonly Dictionary<Type, PolyEditorValidatorBase> validators = new();
        private static bool alreadyInitialized;
        private static byte disableValidateOnSaveCount = 0;

        #region LOOP_INIT
#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        public static void InitializeOnLoadMethod() => InitializeLoop();
#endif

        private static void InitializeLoop()
        {
            if (Application.isBatchMode)
            {
                return;
            }

            if (alreadyInitialized)
            {
                return;
            }

            alreadyInitialized = true;
            Application.wantsToQuit += WantsToQuit;
            RegisterNativeValidators();
        }

        private static bool WantsToQuit()
        {
            ClearValidators();
            return true;
        }

        public static void RegisterNativeValidators()
        {
	        validators.Clear();
	        
	        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

	        foreach (var assembly in assemblies)
	        {
		        Type[] types;

		        try
		        {
			        types = assembly.GetTypes();
		        }
		        catch (ReflectionTypeLoadException e)
		        {
			        types = e.Types.Where(t => t != null).ToArray();
		        }

		        foreach (var type in types)
		        {
			        if (type.IsAbstract || type.IsInterface)
				        continue;

			        // Only consider types that inherit from the abstract base class
			        if (!type.IsSubclassOf(typeof(PolyEditorValidatorBase)))
				        continue;

			        if (Activator.CreateInstance(type) is not PolyEditorValidatorBase instance)
			        {
						continue;
			        }

			        TryAddValidator(instance);
		        }
	        }
        }

        private static void ClearValidators()
        {
	        foreach (var validator in validators.Values)
	        {
		        TryRemoveValidator(validator);
	        }
        }

        private static bool TryAddValidator(PolyEditorValidatorBase validator)
        {
	        return validator != null && validators.TryAdd(validator.GetType(), validator);
        }
 
        private static bool TryRemoveValidator(PolyEditorValidatorBase validator)
        {
	        return validator != null && validators.Remove(validator.GetType());
        }

        #endregion

        /**
         * Push a new request to temporarily disable "validate on save".
         * Should be paired with a call to PopDisableValidateOnSave.
        */
        public static void PushDisableValidateOnSave()
        {
            Assert.IsTrue(disableValidateOnSaveCount == 0, "PushDisableValidateOnSave overflow!");
            ++disableValidateOnSaveCount;
        }

        /**
         * Pop a previous request to temporarily disable "validate on save".
         * Should be paired with a call to PushDisableValidateOnSave.
        */
        public static void PopDisableValidateOnSave()
        {
	        Assert.IsTrue(disableValidateOnSaveCount > 0, "PopDisableValidateOnSave underflow!");
	        --disableValidateOnSaveCount;
        }
        
        /**
         * Runs validation on assets with provided settings. Detailed results will be dumped to outResults.
         * @returns sum of errors and warning found during validation process
         */
        public static int ValidateAssetsWithSettings(HashSet<PolyAssetData> assetDataList, PolyValidateAssetsSettings settings, out PolyValidateAssetsResults outResults)
        {
	        var log = new PolyValidationMessageLog(settings.messageLogName);
	        var vResult = ValidateAssetsInternal(log, assetDataList, settings, out outResults);

	        if (settings.showMessageLogSeverity != PolyMessageSeverity.None)
	        {
		        log.Info($"Validation result: {vResult.ToString()}");
		        log.Print();
	        }

	        log.Flush();
	        return outResults.numWarnings + outResults.numInvalid;
        }
        
        /**
         * Runs registered validators on the provided object.
         * @return Returns Valid if the object contains valid data; returns Invalid if the object contains invalid data; returns NotValidated if no validations was performed on the object
         */
        public static PolyValidationResult IsObjectValid(UnityEngine.Object loadedObject, List<string> warnings, List<string> errors, PolyValidationUsecase usecase)
        {
	        var context = new PolyValidationContext(usecase);
	        var result = ValidateObjectInternal(loadedObject, context);
	        context.SplitIssues(warnings, errors);
	        return result;
        }

        /**
         * Runs registered validators on the provided object.
         * @return Returns Valid if the object contains valid data; returns Invalid if the object contains invalid data; returns NotValidated if no validations was performed on the object
        */
        public static PolyValidationResult IsObjectValidWithContext(UnityEngine.Object loadedObject, PolyValidationContext context)
        {
	        return loadedObject != null
		        ? ValidateObjectInternal(loadedObject, context)
		        : PolyValidationResult.NotValidated;
        }

        /**
         * Loads the object referred to by the provided AssetData and runs registered validators on it.
         * @return Returns Valid if the object pointed to by AssetData contains valid data; returns Invalid if the object contains invalid data or does not exist; returns NotValidated if no validations was performed on the object
        */
        public static PolyValidationResult IsAssetValid(PolyAssetData assetData, List<string> warnings, List<string> errors, PolyValidationUsecase usecase)
        {
	        if (assetData.IsValid())
	        {
				return PolyValidationResult.NotValidated;
	        }

	        var context = new PolyValidationContext(usecase);
	        var result = ValidateAssetInternal(assetData, context);
	        context.SplitIssues(warnings, errors);
	        return result;
        }
        
        /**
         * Loads the object referred to by the provided AssetData and runs registered validators on it.
         * @return Returns Valid if the object pointed to by AssetData contains valid data; returns Invalid if the object contains invalid data or does not exist; returns NotValidated if no validations was performed on the object
        */
        public static PolyValidationResult IsAssetValidWithContext(PolyAssetData assetData, PolyValidationContext context)
        {
	        return assetData.IsValid() ? 
		        ValidateAssetInternal(assetData, context) : 
		        PolyValidationResult.NotValidated;
        }

        private static PolyValidationResult ValidateAssetInternal(PolyAssetData inAssetData,
	        PolyValidationContext context)
        {
	        var result = PolyValidationResult.NotValidated;

	        if (inAssetData.IsValid())
	        {
		        ForEachEnabledValidator((validator) =>
		        {
			        if (validator.CanValidateAsset(inAssetData, context))
			        {
				        var newResult = validator.ValidateLoadedAsset(inAssetData, context);
				        result = PolyValidationUtilities.CombinePolyValidationResults(result, newResult);
			        }
			        return true;
		        });
	        }

	        return result;
        }

        private static PolyValidationResult ValidateObjectInternal(UnityEngine.Object inObject,
	        PolyValidationContext context)
        {
	        var result = PolyValidationResult.NotValidated;
	        var assetData = new PolyAssetData(inObject);

	        if (assetData.IsValid())
	        {
		        ForEachEnabledValidator(validator =>
		        {
			        if (!validator.CanValidateAsset(new PolyAssetData(inObject), context))
			        {
						return true;
			        }
			        
			        var newResult = validator.ValidateLoadedAsset(assetData, context);
			        result = PolyValidationUtilities.CombinePolyValidationResults(result, newResult);
			        return true;
		        });
	        }

	        return result;
        }

        private static PolyValidationResult ValidateAssetsInternal(PolyValidationMessageLog log,
	        HashSet<PolyAssetData> assets, in PolyValidateAssetsSettings inSettings,
	        out PolyValidateAssetsResults results)
        {
	        log.Info($"Starting to validate {assets.Count} assets");
	        log.Info($"Enabled Validators:");
	        ForEachEnabledValidator(validator =>
	        {
		        log.Info($"{validator.GetType()}");
		        return true;
	        });
	        log.Info($"----------");

	        PolyValidationDelegates.OnPreAssetValidation?.Invoke();

	        results = PolyValidateAssetsResults.GetDefault();
	        var vResult = PolyValidationResult.NotValidated;

	        results.numRequested = assets.Count;
	        foreach (var a in assets)
	        {
		        if (results.numChecked >= inSettings.maxAssetsToValidate)
		        {
			        results.isAssetLimitReached = true;
			        log.Info($"Max Assets Reached. Validation stopped: {results.numChecked} ");
			        break;
		        }

		        if (inSettings.skipExcludedDirectories)
		        {
			        //TODO: check if path is in excluded director
			        //++numSkipped
		        }

		        var context = new PolyValidationContext(inSettings.validationUsecase);
		        

		        a.LoadAsset();
		        if (!a.IsValid())
		        {
			        log.Warning($"Asset at path {a.objectPath} cannot be loaded. Skipping validation!");
					continue;
		        }

		        log.Info($"Validating Asset: {a.AssetObject.name} at path {a.objectPath}");
			 
		        var warnings = new List<string>();
		        var errors = new List<string>();
		        vResult = PolyValidationUtilities.CombinePolyValidationResults(vResult, IsAssetValidWithContext(a, context));
		        context.SplitIssues(warnings, errors);

		        if (warnings.Count > 0)
		        {
			        foreach (var warning in warnings)
			        {
				        log.Warning(warning);
			        }
		        }

		        if (errors.Count > 0)
		        {
			        foreach (var error in errors)
			        {
				        log.Error(error);
			        }
		        }

		        ++results.numChecked;
		        results.numWarnings += context.GetNumWarnings();

		        switch (vResult)
		        {
			        case PolyValidationResult.Invalid:
				        ++results.numInvalid;
				        break;
			        case PolyValidationResult.Valid:
				        ++results.numValid;
				        break;
			        case PolyValidationResult.NotValidated:
				        ++results.numUnableToValidate;
				        break;
			        default:
				        throw new ArgumentOutOfRangeException();
		        }

		        if (inSettings.collectPerAssetDetails)
		        {
			        //TODO
		        }
		        log.Info($"----------");
	        }
	        
	        PolyValidationDelegates.OnPostAssetValidation?.Invoke();
	        return vResult;
        }
        
		/**
		 * Iterate the enabled set of validators.
		 * @note Return true to continue iteration, or false to stop.
		*/
        private static void ForEachEnabledValidator( Func<PolyEditorValidatorBase, bool> callback)
        {
	        foreach (var validator in validators.Values)
	        {
		        if (!validator.IsEnabled())
		        {
			        continue;
		        }

		        if (!callback(validator))
		        {
			        break;
		        }
	        }
        }
    }
}
