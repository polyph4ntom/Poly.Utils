using System;
using System.Globalization;
using UnityEngine;

namespace Poly.Serialization
{
    public sealed class FPolyVector2IniValueSerializer : IPolyIniValueSerializer<Vector2>
    {
        public static readonly FPolyVector2IniValueSerializer instance = new();
        private FPolyVector2IniValueSerializer() { }
		
        public bool TryParse(ReadOnlySpan<char> text, out Vector2 value)
        {
            if (!TryParseNamedFloatTuple(text, "X", "Y", out float x, out float y))
            {
                value = default;
                return false;
            }

            value = new Vector2(x, y);
            return true;
        }

        public string Format(Vector2 value)
        {
            return $"(X={value.x:R},Y={value.y:R})";
            //return string.Create(CultureInfo.InvariantCulture, $"(X={value.x:R},Y={value.y:R})");
        }
        
        private static bool TryParseNamedFloatTuple(
            ReadOnlySpan<char> text,
            string key1,
            string key2,
            out float value1,
            out float value2)
        {
            value1 = default;
            value2 = default;

            if (!FPolyIniStructuredValueParser.TryParseFields(text, out var fields))
            {
                return false;
            }

            var has1 = false;
            var has2 = false;

            for (int i = 0; i < fields.Count; i++)
            {
                var field = fields[i];

                if (field.Name.Equals(key1, StringComparison.OrdinalIgnoreCase))
                {
                    if (!float.TryParse(field.Value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out value1))
                    {
                        return false;
                    }

                    has1 = true;
                }
                else if (field.Name.Equals(key2, StringComparison.OrdinalIgnoreCase))
                {
                    if (!float.TryParse(field.Value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out value2))
                    {
                        return false;
                    }

                    has2 = true;
                }
            }

            return has1 && has2;
        }
    }
}