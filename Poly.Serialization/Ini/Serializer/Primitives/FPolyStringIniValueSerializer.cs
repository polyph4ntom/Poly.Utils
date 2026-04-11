using System;

namespace Poly.Serialization
{
	public sealed class FPolyStringIniValueSerializer : IPolyIniValueSerializer<string>
	{
		public static readonly FPolyStringIniValueSerializer instance = new();
		private FPolyStringIniValueSerializer() { }
		
		public bool TryParse(ReadOnlySpan<char> text, out string value)
		{
			value = text.ToString();
			return true;
		}

		public string Format(string value)
		{
			return value ?? string.Empty;
		}
	}
}