using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Poly.Serialization
{
	public sealed class FPolyResolvedIniValue
	{
		public string Key { get; }
		public List<string> Values { get; }

		public FPolyResolvedIniValue(string key, int capacity = 1)
		{
			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(capacity));
			}
			
			Key = key;
			Values = new List<string>(capacity);
		}
		
		public bool HasValue => Values.Count > 0;

		[CanBeNull]
		public string ScalarValue => Values.Count > 0 ? Values[^1] : null;

		public IReadOnlyList<string> ArrayValues => Values;
	}
}


