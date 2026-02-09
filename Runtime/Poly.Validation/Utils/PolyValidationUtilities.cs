using Poly.Validation.DataContainers;
using UnityEditor;
using UnityEngine.Events;

namespace Poly.Validation.Utils
{
	public static class PolyValidationUtilities
	{
		[MenuItem("Assets/Validate Data", true)]
		internal static bool ValidateAsset_Validate()
		{
			return Selection.activeObject != null;
		}

		[MenuItem("Assets/Validate Data", false, 100)]
		internal static void ValidateAsset()
		{
			var selected = Selection.activeObject;
			var path = AssetDatabase.GetAssetPath(selected);
			//Validate(path, selected);
		}
		
		// [MenuItem("Tools/Polyphantom/Validation/Validate All", false)]
		// public static void ValidateAll()
		// {
		// 	var selected = Selection.activeObject;
		// 	var path = AssetDatabase.GetAssetPath(selected);
		// 	//Validate(path, selected);
		// }
		
		[MenuItem("Tools/Polyphantom/Validation/Register Validators", false)]
		internal static void RegisterValidators()
		{
			PolyValidationSubsystem.RegisterNativeValidators();
		}

		public static PolyValidationResult CombinePolyValidationResults(PolyValidationResult result1, PolyValidationResult result2)
		{
			/*
			 * Anything combined with an Invalid result is Invalid. Any result combined with a NotValidated result is the same result
			 *
			 * The combined results should match the following matrix
			 *
			 *				|	NotValidated	|	Valid	|	Invalid |
			 * -------------+-------------------+-----------+----------
			 * NotValidated	|	NotValidated	|	Valid	|	Invalid |
			 * Valid		|	Valid			|	Valid	|	Invalid |
			 * Invalid		|	Invalid			|	Invalid	|	Invalid |
			 *
			 */

			if (result1 == PolyValidationResult.Invalid || result2 == PolyValidationResult.Invalid)
			{
				return PolyValidationResult.Invalid;
			}

			if (result1 == PolyValidationResult.Valid || result2 == PolyValidationResult.Valid)
			{
				return PolyValidationResult.Valid;
			}

			return PolyValidationResult.NotValidated;
		}
	}
}