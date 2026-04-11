using System;

namespace Poly.Serialization
{
	public static class FPolyResolvedIniSectionEnumExtensions
	{
		public static bool TryGetEnum<TEnum>(
			this FPolyResolvedIniSection section,
			string key,
			out TEnum value)
			where TEnum : struct, Enum
		{
			if (section == null)
			{
				throw new ArgumentNullException(nameof(section));
			}

			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			return section.TryGetScalar(key, FPolyEnumIniValueSerializer<TEnum>.@default, out value);
		}

		public static bool TryGetEnum<TEnum>(
			this FPolyResolvedIniSection section,
			string key,
			FPolyEnumIniValueSerializer<TEnum> serializer,
			out TEnum value)
			where TEnum : struct, Enum
		{
			if (section == null)
			{
				throw new ArgumentNullException(nameof(section));
			}

			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			if (serializer == null)
			{
				throw new ArgumentNullException(nameof(serializer));
			}

			return section.TryGetScalar(key, serializer, out value);
		}
		
		public static TEnum GetEnumOrDefault<TEnum>(
			this FPolyResolvedIniSection section,
			string key,
			TEnum defaultValue = default)
			where TEnum : struct, Enum
		{
			if (section == null)
			{
				throw new ArgumentNullException(nameof(section));
			}

			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			return section.TryGetEnum(key, out TEnum value) ? value : defaultValue;
		}

		public static TEnum GetEnumOrDefault<TEnum>(
			this FPolyResolvedIniSection section,
			string key,
			FPolyEnumIniValueSerializer<TEnum> serializer,
			TEnum defaultValue = default)
			where TEnum : struct, Enum
		{
			if (section == null)
			{
				throw new ArgumentNullException(nameof(section));
			}

			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			if (serializer == null)
			{
				throw new ArgumentNullException(nameof(serializer));
			}

			return section.TryGetEnum(key, serializer, out TEnum value) ? value : defaultValue;
		}

		public static bool TryGetEnumArray<TEnum>(
			this FPolyResolvedIniSection section,
			string key,
			out TEnum[] values)
			where TEnum : struct, Enum
		{
			if (section == null)
			{
				throw new ArgumentNullException(nameof(section));
			}

			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			return section.TryGetArray(key, FPolyEnumIniValueSerializer<TEnum>.@default, out values);
		}

		public static bool TryGetEnumArray<TEnum>(
			this FPolyResolvedIniSection section,
			string key,
			FPolyEnumIniValueSerializer<TEnum> serializer,
			out TEnum[] values)
			where TEnum : struct, Enum
		{
			if (section == null)
			{
				throw new ArgumentNullException(nameof(section));
			}

			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			if (serializer == null)
			{
				throw new ArgumentNullException(nameof(serializer));
			}

			return section.TryGetArray(key, serializer, out values);
		}
	}    
}
