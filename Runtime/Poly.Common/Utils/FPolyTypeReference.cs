using System;
using UnityEngine;

namespace Poly.Common
{
	[Serializable]
	public class FPolyTypeReference
	{
		[SerializeField]
		private string typeName;

		public Type Type => string.IsNullOrEmpty(typeName) ? null : Type.GetType(typeName);

		public void SetType(Type t)
		{
			typeName = t?.AssemblyQualifiedName;
		}

		public override string ToString() => Type?.Name ?? "(None)";
	}
}