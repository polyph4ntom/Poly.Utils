using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Poly.Memory
{
    public static class FPolyMemoryEstimator
    {
        private static readonly HashSet<object> visited = new();

        public static int EstimateObjectSize(object obj)
        {
            visited.Clear();
            return Estimate(obj);
        }

        private static int Estimate(object obj)
        {
            if (obj == null || visited.Contains(obj))
            {
                return 0;
            }

            visited.Add(obj);
            var type = obj.GetType();

            // Manual override
            var knownSizeAttr = type.GetCustomAttribute<FPolyKnownSizeAttribute>();
            if (knownSizeAttr != null)
            {
                return knownSizeAttr.Bytes;
            }

            // Unity type override
            var unitySize = TryEstimateUnityObjectSize(obj);
            if (unitySize.HasValue)
            {
                return unitySize.Value;
            }

            // --- PRIMITIVES ---
            if (type.IsPrimitive)
            {
                return Marshal.SizeOf(type);
            }

            // --- ENUMS ---
            if (type.IsEnum)
            {
                return Marshal.SizeOf(Enum.GetUnderlyingType(type));
            }

            // --- STRING ---
            if (obj is string str)
            {
                return sizeof(char) * str.Length + 20;
            }

            // --- ILists ---
            if (obj is IList list)
            {
                var size = 24; // small overhead
                foreach (var item in list)
                {
                    size += Estimate(item);
                }
                return size;
            }

            // --- IDictionary ---
            if (obj is IDictionary dict)
            {
                var size = 32; // overhead
                foreach (var key in dict.Keys)
                {
                    size += Estimate(key);
                    size += Estimate(dict[key]);
                }
                return size;
            }

            // --- VALUE TYPES (structs) ---
            // At this point, we've already handled primitives and enums.
            if (type.IsValueType)
            {
                return Marshal.SizeOf(type);
            }

            // --- REFERENCE TYPES ---
            // Never marshal classes directly (crash risk)
            int total = 24; // object header

            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var value = field.GetValue(obj);
                total += Estimate(value);
            }

            return total;
        }

        private static int? TryEstimateUnityObjectSize(object obj)
        {
            switch (obj)
            {
                case Vector2 _: return 8;
                case Vector3 _: return 12;
                case Vector4 _: return 16;
                case Quaternion _: return 16;
                case Color _: return 16;
                case Rect _: return 16;
                case Bounds _: return 24;
                case Matrix4x4 _: return 64;

                case Texture2D tex:
                    return tex.width * tex.height * GetBytesPerPixel(tex.format);

                case Mesh mesh:
                    return mesh.vertexCount * 32 +
                           mesh.triangles.Length * sizeof(int);

                case GameObject _:
                case Transform _:
                    return 128; // rough overhead

                default:
                    return null;
            }
        }

        private static int GetBytesPerPixel(TextureFormat format)
        {
            return format switch
            {
                TextureFormat.RGBA32 => 4,
                TextureFormat.ARGB32 => 4,
                TextureFormat.RGB24 => 3,
                TextureFormat.Alpha8 => 1,
                _ => 4
            };
        }
    }
}
