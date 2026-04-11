using System;

namespace Poly.Serialization
{
	public sealed class FPolyIniParseOptions
	{
		public StringComparer SectionNameComparer { get; set; } = StringComparer.Ordinal;
		public bool TrimWhitespaceAroundKeysAndValues { get; set; } = true;
		public bool TrimSectionsName { get; set; } = true;
		public bool SkipMalformedLines { get; set; } = true;
	}
}
