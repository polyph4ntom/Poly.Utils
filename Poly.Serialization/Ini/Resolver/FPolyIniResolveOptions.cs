using System;

namespace Poly.Serialization
{
	public sealed class FPolyIniResolveOptions
	{
		public StringComparer KeyComparer { get; set; } = StringComparer.Ordinal;
		public StringComparer ValueComparer { get; set; } = StringComparer.Ordinal;
		public bool SetReplacesExistingValues { get; set; } = true;
		public bool AddUniquePreventsDuplicates { get; set; } = true;
		public bool RemoveKeysWithNoValues { get; set; } = true;
	}
}