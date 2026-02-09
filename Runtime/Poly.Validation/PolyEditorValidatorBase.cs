using Poly.Validation.DataContainers;
using Poly.Validation.Serialisation;
using Poly.Validation.Utils;

namespace Poly.Validation
{
	public abstract class PolyEditorValidatorBase
	{
		public virtual bool IsEnabled()
		{
			return true;
		}

		public abstract bool CanValidateAsset(PolyAssetData assetData, PolyValidationContext context);

		public virtual PolyValidationResult ValidateLoadedAsset(PolyAssetData assetData, PolyValidationContext context)
		{
			return PolyValidationResult.NotValidated;
		}
	}
}