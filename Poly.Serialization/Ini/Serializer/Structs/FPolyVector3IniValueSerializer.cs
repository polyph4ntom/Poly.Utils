using System;
using System.Globalization;
using UnityEngine;

namespace Poly.Serialization
{
    public sealed class FPolyVector3IniValueSerializer : IPolyIniValueSerializer<Vector3>
    {
        public static readonly FPolyVector3IniValueSerializer instance = new();
        private FPolyVector3IniValueSerializer() { }
        
        public bool TryParse(ReadOnlySpan<char> text, out Vector3 value)
        {
            if (!TryParseNamedFloatTuple(text, "X", "Y", "Z", out float x, out float y, out float z))
            {
                value = default;
                return false;
            }

            value = new Vector3(x, y, z);
            return true;
        }
        
        public string Format(Vector3 value)
        {
            return $"(X={value.x:R},Y={value.y:R},Z={value.z:R})";
            
            // return string.Create(
            //     CultureInfo.InvariantCulture,
            //     $"(X={value.x:R},Y={value.y:R},Z={value.z:R})");
        }
        
        private static bool TryParseNamedFloatTuple(
            ReadOnlySpan<char> text,
            string key1,
            string key2,
            string key3,
            out float value1,
            out float value2,
            out float value3)
        {
            value1 = default;
            value2 = default;
            value3 = default;

            if (!FPolyIniStructuredValueParser.TryParseFields(text, out var fields))
            {
                return false;
            }

            bool has1 = false;
            bool has2 = false;
            bool has3 = false;

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
                else if (field.Name.Equals(key3, StringComparison.OrdinalIgnoreCase))
                {
                    if (!float.TryParse(field.Value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out value3))
                    {
                        return false;
                    }

                    has3 = true;
                }
            }

            return has1 && has2 && has3;
        }
    }
}