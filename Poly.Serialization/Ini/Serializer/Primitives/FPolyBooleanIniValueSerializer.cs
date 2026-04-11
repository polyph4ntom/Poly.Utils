using System;

namespace Poly.Serialization
{
	public sealed class FPolyBooleanIniValueSerializer : IPolyIniValueSerializer<bool>
	{
		public static readonly FPolyBooleanIniValueSerializer instance = new();
		private FPolyBooleanIniValueSerializer() { }
		
		public bool TryParse(ReadOnlySpan<char> text, out bool value)
		{
			text = TrimSpacesAndTabs(text);

			if (text.Equals("true".AsSpan(), StringComparison.OrdinalIgnoreCase) ||
			    text.Equals("1".AsSpan(), StringComparison.Ordinal) ||
			    text.Equals("yes".AsSpan(), StringComparison.OrdinalIgnoreCase) ||
			    text.Equals("on".AsSpan(), StringComparison.OrdinalIgnoreCase))
			{
				value = true;
				return true;
			}

			if (text.Equals("false".AsSpan(), StringComparison.OrdinalIgnoreCase) ||
			    text.Equals("0".AsSpan(), StringComparison.Ordinal) ||
			    text.Equals("no".AsSpan(), StringComparison.OrdinalIgnoreCase) ||
			    text.Equals("off".AsSpan(), StringComparison.OrdinalIgnoreCase))
			{
				value = false;
				return true;
			}

			value = false;
			return false;
		}
		
		public string Format(bool value)
		{
			return value ? "True" : "False";
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


