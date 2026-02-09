using System;
using System.Collections.Generic;

namespace Poly.Validation.DataContainers
{
	public struct PolyValidateAssetsSettings
	{
		/** If true, will not validate files in excluded directories */
		public bool skipExcludedDirectories;

		/** If true, will add notifications for files with no validation and display even if everything passes */
		public bool showIfNoFailures;

		/** If true, will add an ValidateAssetsDetails for each asset to the results */
		public bool collectPerAssetDetails;

		/** The usecase requiring datavalidation */
		public PolyValidationUsecase validationUsecase;

		/** Maximum number of assets to attempt to validate */
		public uint maxAssetsToValidate;

		/**
		 * Minimum severity of validation messages to make the message log visible after validation.
		 * Defaults to warning, can be disabled by emptying the optional.
		*/
		public PolyMessageSeverity showMessageLogSeverity;
		
		/** The name of the message log to use for warnings/errors/etc */
		public string messageLogName;

		public static PolyValidateAssetsSettings GetDefault()
		{
			var toReturn = new PolyValidateAssetsSettings
			{
				skipExcludedDirectories = false,
				showIfNoFailures = false,
				collectPerAssetDetails = false,
				validationUsecase = PolyValidationUsecase.Script,
				maxAssetsToValidate = uint.MaxValue,
				showMessageLogSeverity = PolyMessageSeverity.Error,
				messageLogName = "Default Validation"
			};

			return toReturn;
		}
	}

	public struct PolyValidateAssetsDetails
	{
		/** Assembly Name */
		public string assemblyName;

		/** Asset Name */
		public string assetName;

		/** Validation Result */
		public PolyValidationResult result;

		/** Validation Errors */
		public List<string> validationErrors;

		/** Validation Warnings */
		public List<string> validationWarnings;
		
		public static PolyValidateAssetsDetails GetDefault()
		{
			var toReturn = new PolyValidateAssetsDetails
			{
				assemblyName = string.Empty,
				assetName = string.Empty,
				result = PolyValidationResult.NotValidated,
				validationErrors = new List<string>(),
				validationWarnings = new List<string>(),
			};

			return toReturn;
		}
	}

	public struct PolyValidateAssetsResults
	{
		/** Total amount of assets that were gathered to validate. */
		public int numRequested;

		/** Amount of tested assets */
		public int numChecked;

		/** Amount of assets without errors or warnings */
		public int numValid;

		/** Amount of assets with errors */
		public int numInvalid;

		/** Amount of assets skipped */
		public int numSkipped;

		/** Amount of assets with warnings */
		public int numWarnings;

		/** Amount of assets that could not be validated */
		public int numUnableToValidate;

		/** True if FValidateAssetSettings.MaxAssetsToValidation was reached */
		public bool isAssetLimitReached;

		public Dictionary<string, PolyValidateAssetsDetails> assetDetails;
		
		public static PolyValidateAssetsResults GetDefault()
		{
			var toReturn = new PolyValidateAssetsResults
			{
				numRequested = 0,
				numChecked = 0,
				numValid = 0,
				numInvalid = 0,
				numSkipped = 0,
				numWarnings = 0,
				numUnableToValidate = 0,
				isAssetLimitReached = false,
				assetDetails = new Dictionary<string, PolyValidateAssetsDetails>()
			};

			return toReturn;
		}
	}
}