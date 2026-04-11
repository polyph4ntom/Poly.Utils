namespace Poly.Serialization
{
	public sealed class FPolyEnumIniValueSerializerOptions
	{
		public bool IgnoreCase { get; set; } = true;
		public bool AllowNumericValues { get; set; } = true;
		public bool AllowFlagsCombinations { get; set; } = true;
	}
}

