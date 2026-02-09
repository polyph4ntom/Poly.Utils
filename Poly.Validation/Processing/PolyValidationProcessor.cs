using System.Collections.Generic;
using Poly.Validation.DataContainers;
using UnityEditor;
using UnityEngine;

namespace Poly.Validation.Processing
{
	internal class PolyValidationProcessor : AssetModificationProcessor
	{
		internal static string[] OnWillSaveAssets(string[] paths)
		{
			var assetsToValidate = new HashSet<PolyAssetData>();
			foreach (var path in paths)
			{
				var loadedObject = AssetDatabase.LoadMainAssetAtPath(path);
				if (loadedObject == null)
				{
					continue;
				}

				assetsToValidate.Add(new PolyAssetData(path));
			}
			
			if (assetsToValidate.Count != 0)
			{
				var settings = PolyValidateAssetsSettings.GetDefault();
				settings.validationUsecase = PolyValidationUsecase.Save;
	
				//TODO: Dump results to logger;
				var result = PolyValidationSubsystem.ValidateAssetsWithSettings(assetsToValidate, settings, out var results);
				Debug.Log($"[Validation] Problems found during save process: {result}");

			}
			
			return paths;
		}
	}
}