using System.Collections.Generic;
using Poly.Common;
using UnityEngine;

namespace Poly.Memory
{
	[CreateAssetMenu(menuName = "Polyphantom/MemoryPool/New Memory Pool")]
	public class OPolyMemoryPoolDefinition : ScriptableObject
	{
		public string DisplayName;
		
		[FPolySubclassPicker(typeof(FPolyMemoryPoolIdentifier)), Tooltip("The unique type class that identifies this memory pool.")]
		public FPolyTypeReference PoolType;

		[Tooltip("Optional: Parent memory pool. Leave empty for root.")]
		public OPolyMemoryPoolDefinition ParentPool;

		[Tooltip("Budget in bytes (e.g. 4 * 1024 * 1024 for 4MB)")]
		public int BudgetBytes = 1024 * 1024;

		[HideInInspector]
		public List<OPolyMemoryPoolDefinition> Children = new(); // populated at runtime

		private void OnValidate()
		{
			if (string.IsNullOrEmpty(DisplayName))
				DisplayName = name;
		}

	}
}
