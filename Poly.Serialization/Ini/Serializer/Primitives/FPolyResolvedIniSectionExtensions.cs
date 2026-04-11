using System;
using UnityEngine;

namespace Poly.Serialization
{
	public static class FPolyResolvedIniSectionExtensions
	{
		public static bool TryGetString(this FPolyResolvedIniSection section, string key, out string value)
		{
			if (section == null)
			{
				throw new ArgumentNullException(nameof(section));
			}

			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			if (section.TryGetScalar(key, out var text))
			{
				value = text;
				return true;
			}

			value = null!;
			return false;
		}
		
		public static bool TryGetBoolean(this FPolyResolvedIniSection section, string key, out bool value)
		{
			return TryGetScalar(section, key, FPolyIniValueSerializers.Boolean, out value);
		}

		public static bool TryGetInt(this FPolyResolvedIniSection section, string key, out int value)
		{
			return TryGetScalar(section, key, FPolyIniValueSerializers.Integer, out value);
		}

		public static bool TryGetSingle(this FPolyResolvedIniSection section, string key, out float value)
		{
			return TryGetScalar(section, key, FPolyIniValueSerializers.Single, out value);
		}
		
		public static bool TryGetGuid(this FPolyResolvedIniSection section, string key, out Guid value)
		{
			return section.TryGetScalar(key, FPolyIniValueSerializers.Guid, out value);
		}
		
		public static bool TryGetVector2(this FPolyResolvedIniSection section, string key, out Vector2 value)
		{
			return section.TryGetScalar(key, FPolyIniValueSerializers.Vector2, out value);
		}
		
		public static bool TryGetVector3(this FPolyResolvedIniSection section, string key, out Vector3 value)
		{
			return section.TryGetScalar(key, FPolyIniValueSerializers.Vector3, out value);
		}
		
		public static bool TryGetColor(this FPolyResolvedIniSection section, string key, out Color value)
		{
			return section.TryGetScalar(key, FPolyIniValueSerializers.Color, out value);
		}
		
		public static bool TryGetQuaternion(this FPolyResolvedIniSection section, string key, out Quaternion value)
		{
			return section.TryGetScalar(key, FPolyIniValueSerializers.Quaternion, out value);
		}

		public static string GetStringOrDefault(this FPolyResolvedIniSection section, string key, string defaultValue = "")
		{
			return section.TryGetString(key, out var value) ? value : defaultValue;
		}

		public static bool GetBooleanOrDefault(this FPolyResolvedIniSection section, string key, bool defaultValue = default)
		{
			return section.TryGetBoolean(key, out var value) ? value : defaultValue;
		}

		public static int GetInt32OrDefault(this FPolyResolvedIniSection section, string key, int defaultValue = default)
		{
			return section.TryGetInt(key, out var value) ? value : defaultValue;
		}

		public static float GetSingleOrDefault(this FPolyResolvedIniSection section, string key, float defaultValue = default)
		{
			return section.TryGetSingle(key, out var value) ? value : defaultValue;
		}

        public static Guid GetGuidOrDefault(this FPolyResolvedIniSection section, string key, Guid defaultValue = default)
        {
            return section.TryGetGuid(key, out Guid value) ? value : defaultValue;
        }
        
        public static Vector2 GetVector2OrDefault(this FPolyResolvedIniSection section, string key, Vector2 defaultValue = default)
        {
            return section.TryGetVector2(key, out Vector2 value) ? value : defaultValue;
        }
        
        public static Vector3 GetVector3OrDefault(this FPolyResolvedIniSection section, string key, Vector3 defaultValue = default)
        {
            return section.TryGetVector3(key, out Vector3 value) ? value : defaultValue;
        }
        
        public static Color GetColorOrDefault(this FPolyResolvedIniSection section, string key, Color defaultValue = default)
        {
            return section.TryGetColor(key, out Color value) ? value : defaultValue;
        }
        
        public static Quaternion GetQuaternionOrDefault(this FPolyResolvedIniSection section, string key, Quaternion defaultValue = default)
        {
            return section.TryGetQuaternion(key, out Quaternion value) ? value : defaultValue;
        }
		
		public static bool TryGetStringArray(this FPolyResolvedIniSection section, string key, out string[] values)
		{
			if (section == null)
			{
				throw new ArgumentNullException(nameof(section));
			}

			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			var array = section.GetArray(key);
			if (array.Count == 0)
			{
				values = Array.Empty<string>();
				return false;
			}

			values = new string[array.Count];
			for (int i = 0; i < array.Count; i++)
			{
				values[i] = array[i];
			}

			return true;
		}
		
		public static bool TryGetBooleanArray(this FPolyResolvedIniSection section, string key, out bool[] values)
		{
			return TryGetArray(section, key, FPolyIniValueSerializers.Boolean, out values);
		}

		public static bool TryGetInt32Array(this FPolyResolvedIniSection section, string key, out int[] values)
		{
			return TryGetArray(section, key, FPolyIniValueSerializers.Integer, out values);
		}

		public static bool TryGetSingleArray(this FPolyResolvedIniSection section, string key, out float[] values)
		{
			return TryGetArray(section, key, FPolyIniValueSerializers.Single, out values);
		}
		
		public static bool TryGetGuidArray(this FPolyResolvedIniSection section, string key, out Guid[] values)
		{
			return TryGetArray(section, key, FPolyIniValueSerializers.Guid, out values);
		}
		
		public static bool TryGetVector2Array(this FPolyResolvedIniSection section, string key, out Vector2[] values)
		{
			return TryGetArray(section, key, FPolyIniValueSerializers.Vector2, out values);
		}
		
		public static bool TryGetVector3Array(this FPolyResolvedIniSection section, string key, out Vector3[] values)
		{
			return TryGetArray(section, key, FPolyIniValueSerializers.Vector3, out values);
		}
		
		public static bool TryGetQuaternionArray(this FPolyResolvedIniSection section, string key, out Quaternion[] values)
		{
			return TryGetArray(section, key, FPolyIniValueSerializers.Quaternion, out values);
		}
		
		public static bool TryColorArray(this FPolyResolvedIniSection section, string key, out Color[] values)
		{
			return TryGetArray(section, key, FPolyIniValueSerializers.Color, out values);
		}
		
		public static bool TryGetScalar<T>(
			this FPolyResolvedIniSection section,
			string key,
			IPolyIniValueSerializer<T> serializer,
			out T value)
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

			if (!section.TryGetScalar(key, out var text))
			{
				value = default!;
				return false;
			}

			return serializer.TryParse(text.AsSpan(), out value!);
		}
		
		public static bool TryGetArray<T>(
			this FPolyResolvedIniSection section,
			string key,
			IPolyIniValueSerializer<T> serializer,
			out T[] values)
		{
			if (section == null) throw new ArgumentNullException(nameof(section));
			if (key == null) throw new ArgumentNullException(nameof(key));
			if (serializer == null) throw new ArgumentNullException(nameof(serializer));

			var rawValues = section.GetArray(key);
			if (rawValues.Count == 0)
			{
				values = Array.Empty<T>();
				return false;
			}

			var parsed = new T[rawValues.Count];

			for (int i = 0; i < rawValues.Count; i++)
			{
				if (!serializer.TryParse(rawValues[i].AsSpan(), out parsed[i]!))
				{
					values = Array.Empty<T>();
					return false;
				}
			}

			values = parsed;
			return true;
		}
	}
}