using System;
using System.Collections.Generic;
using UnityEngine;

namespace Poly.Serialization
{
	internal static class FPolyIniStructuredValueParser
	{
		internal readonly struct Field
		{
			public readonly string Name;
			public readonly string  Value;

			public Field(string name, string value)
			{
				Name = name;
				Value = value;
			}
		}
		
		internal static bool TryParseFields(ReadOnlySpan<char> text, out List<Field> fields)
        {
	        text = FPolyIniTextUtility.TrimSpacesAndTabs(text);

	        if (text.Length < 2 || text[0] != '(' || text[text.Length - 1] != ')')
	        {
		        fields = null!;
		        return false;
	        }

	        text = text.Slice(1, text.Length - 2);

	        fields = new List<Field>(4);

	        while (true)
	        {
		        text = FPolyIniTextUtility.TrimSpacesAndTabs(text);

		        if (text.Length == 0)
		        {
			        return fields.Count > 0;
		        }

		        int commaIndex = FindTopLevelComma(text);
		        ReadOnlySpan<char> segment;

		        if (commaIndex >= 0)
		        {
			        segment = text.Slice(0, commaIndex);
			        text = text.Slice(commaIndex + 1);
		        }
		        else
		        {
			        segment = text;
			        text = ReadOnlySpan<char>.Empty;
		        }

		        segment = FPolyIniTextUtility.TrimSpacesAndTabs(segment);
		        if (segment.Length == 0)
		        {
			        fields = null!;
			        return false;
		        }

		        int equalsIndex = segment.IndexOf('=');
		        if (equalsIndex <= 0 || equalsIndex == segment.Length - 1)
		        {
			        fields = null!;
			        return false;
		        }

		        ReadOnlySpan<char> nameSpan = FPolyIniTextUtility.TrimSpacesAndTabs(segment.Slice(0, equalsIndex));
		        ReadOnlySpan<char> valueSpan = FPolyIniTextUtility.TrimSpacesAndTabs(segment.Slice(equalsIndex + 1));

		        if (nameSpan.Length == 0 || valueSpan.Length == 0)
		        {
			        fields = null!;
			        return false;
		        }

		        fields.Add(new Field(nameSpan.ToString(), valueSpan.ToString()));
	        }
        }
		
		private static int FindTopLevelComma(ReadOnlySpan<char> text)
		{
			var depth = 0;

			for (int i = 0; i < text.Length; i++)
			{
				var c = text[i];

				if (c == '(')
				{
					depth++;
				}
				else if (c == ')')
				{
					depth--;
				}
				else if (c == ',' && depth == 0)
				{
					return i;
				}
			}

			return -1;
		}
	}
}
