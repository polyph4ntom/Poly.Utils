using System;
using UnityEngine;

namespace Poly.Serialization
{
	public static class FPolyIniSectionEnumWriteExtensions
	{
		public static void AddEnum<TEnum>(
			this FPolyIniSection section,
			string key,
			TEnum value,
			int lineNumber = 0)
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

			section.AddEntry(
				EPolyIniAssignmentKind.Set,
				key,
				FPolyEnumIniValueSerializer<TEnum>.@default.Format(value),
				lineNumber);
		}

		public static void AddEnum<TEnum>(
			this FPolyIniSection section,
			string key,
			TEnum value,
			FPolyEnumIniValueSerializer<TEnum> serializer,
			int lineNumber = 0)
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

			section.AddEntry(
				EPolyIniAssignmentKind.Set,
				key,
				serializer.Format(value),
				lineNumber);
		}

		public static void AddEnumArrayValue<TEnum>(
			this FPolyIniSection section,
			string key,
			TEnum value,
			int lineNumber = 0)
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

			section.AddEntry(
				EPolyIniAssignmentKind.Add,
				key,
				FPolyEnumIniValueSerializer<TEnum>.@default.Format(value),
				lineNumber);
		}

		public static void AddEnumArrayValue<TEnum>(
			this FPolyIniSection section,
			string key,
			TEnum value,
			FPolyEnumIniValueSerializer<TEnum> serializer,
			int lineNumber = 0)
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

			section.AddEntry(
				EPolyIniAssignmentKind.Add,
				key,
				serializer.Format(value),
				lineNumber);
		}
	}
}


