using System;
using System.Globalization;

namespace Poly.Serialization
{
    public sealed class FPolySingleIniValueSerializer : IPolyIniValueSerializer<float>
    {
        public static readonly FPolySingleIniValueSerializer instance = new();
        private FPolySingleIniValueSerializer() { }
		
        public bool TryParse(ReadOnlySpan<char> text, out float value)
        {
            text = TrimSpacesAndTabs(text);
            return float.TryParse(
                text,
                NumberStyles.Float | NumberStyles.AllowThousands,
                CultureInfo.InvariantCulture,
                out value);
        }

        public string Format(float value)
        {
            return value.ToString("R", CultureInfo.InvariantCulture);
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