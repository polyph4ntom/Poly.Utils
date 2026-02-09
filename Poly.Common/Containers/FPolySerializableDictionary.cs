using System;
using System.Collections.Generic;
using UnityEngine;

namespace Poly.Common
{
	[Serializable]
	public class FPolySerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
	{
		[SerializeField]
		private List<FPolySerializableKeyValuePair<TKey, TValue>> entries = new();

		private Dictionary<TKey, TValue> dict;

		public Dictionary<TKey, TValue> Dictionary
		{
			get
			{
				if (dict == null)
				{
					dict = new Dictionary<TKey, TValue>();
					foreach (var entry in entries)
					{
						// If duplicates exist → last one overwrites (fine during edit)
						dict[entry.Key] = entry.Value;
					}
				}
				return dict;
			}
		}

		public void OnBeforeSerialize()
		{
			if (dict != null)
			{
				entries.Clear();
				foreach (var kvp in dict)
					entries.Add(new FPolySerializableKeyValuePair<TKey, TValue>(kvp.Key, kvp.Value));
			}
		}

		public void OnAfterDeserialize()
		{
			dict = null; // rebuilt lazily
		}
	}
}
