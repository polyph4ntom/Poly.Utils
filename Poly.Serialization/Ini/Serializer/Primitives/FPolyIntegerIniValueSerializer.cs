using System;
using System.Globalization;

namespace Poly.Serialization
{
    public sealed class FPolyIntegerIniValueSerializer : IPolyIniValueSerializer<int>
    {
        public static readonly FPolyIntegerIniValueSerializer instance = new();
        private FPolyIntegerIniValueSerializer() { }

        public bool TryParse(ReadOnlySpan<char> text, out int value)
        {
            text = TrimSpacesAndTabs(text);
            return int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
        }

        public string Format(int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
        
        private static ReadOnlySpan<char> TrimSpacesAndTabs(ReadOnlySpan<char> span)
        {
            var start = 0;
            var end = span.Length - 1;

            while (start < span.Length && (span[start] == ' ' || span[start] == '\t'))
            {
                start++;
            }

            while (end >= start && (span[end] == ' ' || span[end] == '\t'))
            {
                end--;
            }

            return start <= end
                ? span.Slice(start, end - start + 1)
                : ReadOnlySpan<char>.Empty;
        }
    }
}
