namespace Poly.Validation.DataContainers
{
	/** Enum used by PolyValidation to see if an asset has been validated for correctness */
	public enum PolyValidationResult : byte
	{
		/* Asset has failed validation */
		Invalid,
		/* Asset has passed validation */
		Valid,
		/* Asset has not yet been validated */
		NotValidated
	}
}