using System;
using System.Text;
using JetBrains.Annotations;

namespace Poly.Serialization
{
	internal static class FPolyStringBuilderCache
	{
		[ThreadStatic, CanBeNull] 
		private static StringBuilder cachedInstance;

		public static StringBuilder Acquire(int capacity = 256)
		{
			var cached = cachedInstance;
			
			if (cached != null)
			{
				cachedInstance = null;

				if (cached.Capacity < capacity)
				{
					cached.Capacity = capacity;
				}

				cached.Clear();
				return cached;
			}

			return new StringBuilder(capacity);
		}
		
		public static string GetStringAndRelease(StringBuilder sb)
		{
			string result = sb.ToString();
			Release(sb);
			return result;
		}
		
		public static void Release(StringBuilder sb)
		{
			if (sb.Capacity <= 4096)
			{
				sb.Clear();
				cachedInstance = sb;
			}
		}
	}
}

