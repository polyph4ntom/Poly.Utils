using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Poly.Serialization
{
	public class FPolyResolvedIniSection
	{
		private readonly Dictionary<string, FPolyResolvedIniValue> valuesByKey;
		
		public string Name { get; }
		public StringComparer KeyComparer { get; }

		public FPolyResolvedIniSection(string name, [CanBeNull] StringComparer keyComparer = null, int capacity = 4)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(capacity));
			}
			
			
			Name = name ?? throw new ArgumentNullException(nameof(name));
			KeyComparer = keyComparer ?? StringComparer.Ordinal;
			valuesByKey = new Dictionary<string, FPolyResolvedIniValue>(capacity, KeyComparer);
		}
		
		public IEnumerable<string> Keys => valuesByKey.Keys;
		public IEnumerable<FPolyResolvedIniValue> Values => valuesByKey.Values;
		public int Count => valuesByKey.Count;
		
		public bool TryGetValue(string key, out FPolyResolvedIniValue value)
		{
			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			return valuesByKey.TryGetValue(key, out value!);
		}
		
		public bool TryGetScalar(string key, out string value)
		{
			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}
			
			if (valuesByKey.TryGetValue(key, out var resolved) && resolved.Values.Count > 0)
			{
				value = resolved.Values[^1];
				return true;
			}

			value = null!;
			return false;
		}
		
		public IReadOnlyList<string> GetArray(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			if (valuesByKey.TryGetValue(key, out var resolved))
			{
				return resolved.ArrayValues;
			}

			return Array.Empty<string>();
		}
		
		internal FPolyResolvedIniValue GetOrAdd(string key)
		{
			if (!valuesByKey.TryGetValue(key, out var value))
			{
				value = new FPolyResolvedIniValue(key);
				valuesByKey.Add(key, value);
			}

			return value;
		}

		internal bool RemoveKey(string key)
		{
			return valuesByKey.Remove(key);
		}
	}
}