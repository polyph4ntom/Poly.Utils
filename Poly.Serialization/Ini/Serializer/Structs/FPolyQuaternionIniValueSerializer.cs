using System;
using System.Globalization;
using UnityEngine;

namespace Poly.Serialization
{
	public sealed class FPolyQuaternionIniValueSerializer : IPolyIniValueSerializer<Quaternion>
	{
		public static readonly FPolyQuaternionIniValueSerializer instance = new();
		private FPolyQuaternionIniValueSerializer() { }
		
		public bool TryParse(ReadOnlySpan<char> text, out Quaternion value)
		{
			if (!FPolyIniStructuredValueParser.TryParseFields(text, out var fields))
			{
				value = default;
				return false;
			}

			float x = 0f;
			float y = 0f;
			float z = 0f;
			float w = 1f;

			bool hasX = false;
			bool hasY = false;
			bool hasZ = false;
			bool hasW = false;

			for (int i = 0; i < fields.Count; i++)
			{
				var field = fields[i];

				if (field.Name.Equals("X", StringComparison.OrdinalIgnoreCase))
				{
					if (!TryParseFloat(field.Value, out x)) { value = default; return false; }
					hasX = true;
				}
				else if (field.Name.Equals("Y", StringComparison.OrdinalIgnoreCase))
				{
					if (!TryParseFloat(field.Value, out y)) { value = default; return false; }
					hasY = true;
				}
				else if (field.Name.Equals("Z", StringComparison.OrdinalIgnoreCase))
				{
					if (!TryParseFloat(field.Value, out z)) { value = default; return false; }
					hasZ = true;
				}
				else if (field.Name.Equals("W", StringComparison.OrdinalIgnoreCase))
				{
					if (!TryParseFloat(field.Value, out w)) { value = default; return false; }
					hasW = true;
				}
			}

			if (!hasX || !hasY || !hasZ || !hasW)
			{
				value = default;
				return false;
			}

			value = new Quaternion(x, y, z, w);
			return true;
		}
		
		public string Format(Quaternion value)
		{
			return $"(X={value.x:R},Y={value.y:R},Z={value.z:R},W={value.w:R})";
			// return string.Create(
			// 	CultureInfo.InvariantCulture,
			// 	$"(X={value.x:R},Y={value.y:R},Z={value.z:R},W={value.w:R})");
		}
		
		private static bool TryParseFloat(ReadOnlySpan<char> text, out float value)
		{
			return float.TryParse(
				text,
				NumberStyles.Float | NumberStyles.AllowThousands,
				CultureInfo.InvariantCulture,
				out value);
		}
		
	}
}