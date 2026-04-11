using System;
using System.Globalization;
using UnityEngine;

public class FPolyColorIniValueSerializer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

namespace Poly.Serialization
{
    public sealed class FPolyColorIniValueSerializer : IPolyIniValueSerializer<Color>
    {
        public static readonly FPolyColorIniValueSerializer instance = new();
        private FPolyColorIniValueSerializer() { }
		
        public bool TryParse(ReadOnlySpan<char> text, out Color value)
        {
            if (!FPolyIniStructuredValueParser.TryParseFields(text, out var fields))
            {
                value = default;
                return false;
            }

            float r = 0f;
            float g = 0f;
            float b = 0f;
            float a = 1f;

            bool hasR = false;
            bool hasG = false;
            bool hasB = false;

            for (int i = 0; i < fields.Count; i++)
            {
                var field = fields[i];

                if (field.Name.Equals("R", StringComparison.OrdinalIgnoreCase))
                {
                    if (!TryParseFloat(field.Value, out r)) { value = default; return false; }
                    hasR = true;
                }
                else if (field.Name.Equals("G", StringComparison.OrdinalIgnoreCase))
                {
                    if (!TryParseFloat(field.Value, out g)) { value = default; return false; }
                    hasG = true;
                }
                else if (field.Name.Equals("B", StringComparison.OrdinalIgnoreCase))
                {
                    if (!TryParseFloat(field.Value, out b)) { value = default; return false; }
                    hasB = true;
                }
                else if (field.Name.Equals("A", StringComparison.OrdinalIgnoreCase))
                {
                    if (!TryParseFloat(field.Value, out a)) { value = default; return false; }
                }
            }

            if (!hasR || !hasG || !hasB)
            {
                value = default;
                return false;
            }

            value = new Color(r, g, b, a);
            return true;
        }

        public string Format(Color value)
        {
            return $"(R={value.r:R},G={value.g:R},B={value.b:R},A={value.a:R})";
            // return string.Create(
            //     CultureInfo.InvariantCulture,
            //     $"(R={value.r:R},G={value.g:R},B={value.b:R},A={value.a:R})");
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
