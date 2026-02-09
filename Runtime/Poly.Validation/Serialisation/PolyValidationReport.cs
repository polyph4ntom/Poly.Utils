using System.Collections.Generic;

namespace Poly.Validation.Serialisation
{
	[System.Serializable]
	internal class PolyValidationReport
	{
		internal string lastValidationDate;
		internal List<PolyValidationEntry> results;
	}
}