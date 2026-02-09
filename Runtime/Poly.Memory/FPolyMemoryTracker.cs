using System;
using System.Collections.Generic;
using UnityEngine;

namespace Poly.Memory
{
	public static class FPolyMemoryTracker
	{
		private static Dictionary<Type, FPolyMemoryPool> pools = new();
		private static Dictionary<object, (Type poolType, int size)> trackedObjects = new();

		public static void CreatePool(string name, Type type, int budgetBytes, Type parentPool = null)
		{
			var pool = new FPolyMemoryPool(name, type, budgetBytes);
			pools[type] = pool;

			if (parentPool != null && pools.TryGetValue(parentPool, out var parent))
			{
				pool.SetParent(parent);
			}
		}

		public static void RegisterObject<TPool>(object obj)
		{
			if (!pools.TryGetValue(typeof(TPool), out var pool))
			{
				Debug.Log("[Poly.Memory] Pool you want to register to doesn't exist: " + typeof(TPool));
				return;
			}

			var estSize = FPolyMemoryEstimator.EstimateObjectSize(obj);
			trackedObjects[obj] = (typeof(TPool), estSize);
			pool.AddUsage(estSize);
		}

		public static void UnregisterObject(object obj)
		{
			if (!trackedObjects.TryGetValue(obj, out var data))
			{
				return;
			}

			if (pools.TryGetValue(data.poolType, out var pool))
			{
				pool.SubtractUsage(data.size);
			}

			trackedObjects.Remove(obj);
		}

		public static FPolyMemoryPool GetPool(Type poolType)
		{
			return pools.GetValueOrDefault(poolType);
		}

		public static IEnumerable<FPolyMemoryPool> GetAllPools() => pools.Values;
	}
}