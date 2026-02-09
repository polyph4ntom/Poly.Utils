namespace Poly.Validation.Serialisation
{
	[System.Serializable]
	internal class PolyValidationEntry
	{
		internal string assetPath;
		internal string message;
		internal byte validationResult;
		internal string validatorType;
	}
}