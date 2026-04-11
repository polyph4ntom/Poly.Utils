using System;

namespace Poly.Serialization
{
    public sealed class FPolyGuidIniValueSerializer : IPolyIniValueSerializer<Guid>
    {
        public static readonly FPolyGuidIniValueSerializer instance = new();
        private FPolyGuidIniValueSerializer() { }
		
        public bool TryParse(ReadOnlySpan<char> text, out Guid value)
        {
            var tokenText = FPolyIniTextUtility.TrimSpacesAndTabs(text).ToString();
            return Guid.TryParse(tokenText, out value);
        }

        public string Format(Guid value)
        {
            return value.ToString("D");
        }
    }
}