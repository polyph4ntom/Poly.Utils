using UnityEngine.Events;

namespace Poly.Validation.Utils
{
	public static class PolyValidationDelegates
	{
		public static UnityAction OnPreAssetValidation { get; }
		public static UnityAction OnPostAssetValidation { get; }
	}
}