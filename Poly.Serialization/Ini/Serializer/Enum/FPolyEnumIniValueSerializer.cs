using System;
using System.Globalization;

namespace Poly.Serialization
{
	public sealed class FPolyEnumIniValueSerializer<TEnum> : IPolyIniValueSerializer<TEnum>
		where TEnum : struct, Enum
	{
		public static readonly FPolyEnumIniValueSerializer<TEnum> @default = new();
		
		private readonly FPolyEnumIniValueSerializerOptions options;
		private readonly bool isFlagsEnum;
		
		public FPolyEnumIniValueSerializer() : this(new FPolyEnumIniValueSerializerOptions()) { }
		
		public FPolyEnumIniValueSerializer(FPolyEnumIniValueSerializerOptions options)
		{
			this.options = options ?? throw new ArgumentNullException(nameof(options));
			isFlagsEnum = typeof(TEnum).IsDefined(typeof(FlagsAttribute), inherit: false);
		}
		
		public bool TryParse(ReadOnlySpan<char> text, out TEnum value)
		{
			text = FPolyIniTextUtility.TrimSpacesAndTabs(text);

			if (text.Length == 0)
			{
				value = default;
				return false;
			}

			if (isFlagsEnum && options.AllowFlagsCombinations && text.IndexOf(',') >= 0)
			{
				return TryParseFlags(text, out value);
			}

			if (TryParseSingleToken(text, out value))
			{
				return true;
			}

			value = default;
			return false;
		}
		
		public string Format(TEnum value)
		{
			return value.ToString();
		}
		
		private bool TryParseFlags(ReadOnlySpan<char> text, out TEnum value)
		{
			ulong combined = 0;

			while (true)
			{
				int commaIndex = text.IndexOf(',');

				ReadOnlySpan<char> token;
				if (commaIndex >= 0)
				{
					token = text.Slice(0, commaIndex);
					text = text.Slice(commaIndex + 1);
				}
				else
				{
					token = text;
					text = ReadOnlySpan<char>.Empty;
				}

				token = FPolyIniTextUtility.TrimSpacesAndTabs(token);

				if (token.Length == 0)
				{
					value = default;
					return false;
				}

				if (!TryParseSingleToken(token, out TEnum tokenValue))
				{
					value = default;
					return false;
				}

				combined |= ConvertToUInt64(tokenValue);

				if (text.Length == 0)
				{
					value = ConvertFromUInt64(combined);
					return true;
				}
			}
		}
		
		private bool TryParseSingleToken(ReadOnlySpan<char> text, out TEnum value)
		{
			text = FPolyIniTextUtility.TrimSpacesAndTabs(text);

			if (text.Length == 0)
			{
				value = default;
				return false;
			}

			bool looksNumeric = IsNumericToken(text);

			if (looksNumeric && !options.AllowNumericValues)
			{
				value = default;
				return false;
			}

			string tokenText = text.ToString();
			if (Enum.TryParse(tokenText, options.IgnoreCase, out TEnum parsed))
			{
				value = parsed;
				return true;
			}

			value = default;
			return false;
		}
		
		private static bool IsNumericToken(ReadOnlySpan<char> text)
		{
			if (text.Length == 0)
			{
				return false;
			}

			var start = (text[0] == '+' || text[0] == '-') ? 1 : 0;
			if (start == text.Length)
			{
				return false;
			}

			for (int i = start; i < text.Length; i++)
			{
				if (!char.IsDigit(text[i]))
				{
					return false;
				}
			}

			return true;
		}
		
		private static bool TryParseNumeric(ReadOnlySpan<char> text, out TEnum value)
        {
            var underlyingType = Enum.GetUnderlyingType(typeof(TEnum));

            object boxed;

            if (underlyingType == typeof(byte))
            {
                if (byte.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out byte v))
                {
                    boxed = v;
                    value = (TEnum)Enum.ToObject(typeof(TEnum), boxed);
                    return true;
                }
            }
            else if (underlyingType == typeof(sbyte))
            {
                if (sbyte.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out sbyte v))
                {
                    boxed = v;
                    value = (TEnum)Enum.ToObject(typeof(TEnum), boxed);
                    return true;
                }
            }
            else if (underlyingType == typeof(short))
            {
                if (short.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out short v))
                {
                    boxed = v;
                    value = (TEnum)Enum.ToObject(typeof(TEnum), boxed);
                    return true;
                }
            }
            else if (underlyingType == typeof(ushort))
            {
                if (ushort.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out ushort v))
                {
                    boxed = v;
                    value = (TEnum)Enum.ToObject(typeof(TEnum), boxed);
                    return true;
                }
            }
            else if (underlyingType == typeof(int))
            {
                if (int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out int v))
                {
                    boxed = v;
                    value = (TEnum)Enum.ToObject(typeof(TEnum), boxed);
                    return true;
                }
            }
            else if (underlyingType == typeof(uint))
            {
                if (uint.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out uint v))
                {
                    boxed = v;
                    value = (TEnum)Enum.ToObject(typeof(TEnum), boxed);
                    return true;
                }
            }
            else if (underlyingType == typeof(long))
            {
                if (long.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out long v))
                {
                    boxed = v;
                    value = (TEnum)Enum.ToObject(typeof(TEnum), boxed);
                    return true;
                }
            }
            else if (underlyingType == typeof(ulong))
            {
                if (ulong.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out ulong v))
                {
                    boxed = v;
                    value = (TEnum)Enum.ToObject(typeof(TEnum), boxed);
                    return true;
                }
            }

            value = default;
            return false;
        }
		
		private static ulong ConvertToUInt64(TEnum value)
		{
			return Convert.ToUInt64(value, CultureInfo.InvariantCulture);
		}

		private static TEnum ConvertFromUInt64(ulong value)
		{
			return (TEnum)Enum.ToObject(typeof(TEnum), value);
		}
	}
}
