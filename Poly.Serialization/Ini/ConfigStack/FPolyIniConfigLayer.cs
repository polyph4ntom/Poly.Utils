using System;

namespace Poly.Serialization
{
	public sealed class FPolyIniConfigLayer
	{
		public string Name { get; }
		public FPolyIniDocument Document { get; }

		public FPolyIniConfigLayer(string name, FPolyIniDocument document)
		{
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Document = document ?? throw new ArgumentNullException(nameof(document));
		}
		
		public override string ToString() => Name;
	}
}