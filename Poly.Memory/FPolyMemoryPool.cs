using System;
using System.Collections.Generic;

namespace Poly.Memory
{
	public class FPolyMemoryPool
	{
		public string DisplayName { get; }
		public Type Id { get; }
		public int BudgetBytes { get; private set; }
		public int UsedBytes { get; private set; }

		public FPolyMemoryPool Parent { get; private set; }
		public List<FPolyMemoryPool> Children { get; } = new();

		public FPolyMemoryPool(string name, Type id, int budgetBytes)
		{
			DisplayName = name;
			Id = id;
			BudgetBytes = budgetBytes;
		}

		public void SetParent(FPolyMemoryPool parent)
		{
			Parent = parent;
			parent.Children.Add(this);
		}

		public void AddUsage(int bytes)
		{
			UsedBytes += bytes;
			Parent?.AddUsage(bytes);
		}

		public void SubtractUsage(int bytes)
		{
			UsedBytes -= bytes;
			Parent?.SubtractUsage(bytes);
		}

		public float UsageRatio => (float)UsedBytes / BudgetBytes;
	}
}
