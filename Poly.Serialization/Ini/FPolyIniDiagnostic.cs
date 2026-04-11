namespace Poly.Serialization
{
	public enum EPolyIniDiagnosticId : byte
	{
		EmptySectionName = 0,
		UnterminatedSectionHeader = 1,
		MissingEquals = 2,
		EmptyKey = 3,
		ClearOperationHasUnexpectedValue  = 4,
		ContentOutsideSection = 5
	}

	public readonly struct FPolyIniDiagnostic
	{
		public readonly EPolyIniDiagnosticId diagnosticId;
		public readonly int lineNumber;

		public FPolyIniDiagnostic(EPolyIniDiagnosticId diagnosticId, int lineNumber)
		{
			this.diagnosticId = diagnosticId;
			this.lineNumber = lineNumber;
		}
		
		public override string ToString() => $"{diagnosticId} at line {lineNumber}";
	}
}