using System;

namespace Poly.Serialization
{
	internal static class FPolyIniTextUtility
	{
		public static ReadOnlySpan<char> TrimSpacesAndTabs(ReadOnlySpan<char> span)
		{
			int start = 0;
			int end = span.Length - 1;

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
