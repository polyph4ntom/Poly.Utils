using System;
using UnityEditor;
using UnityEngine;

namespace Poly.Common
{
	[System.Serializable]
	public class FPolySceneReference : IEquatable<FPolySceneReference>
	{
#if UNITY_EDITOR
		[SerializeField] 
		private SceneAsset sceneAsset;
#endif

		[SerializeField] 
		private string scenePath;

		public string ScenePath => scenePath;


		public override int GetHashCode() => scenePath?.GetHashCode() ?? 0;

		public override bool Equals(object obj) => Equals(obj as FPolySceneReference);

		public bool Equals(FPolySceneReference other)
		{
			return other != null && scenePath == other.scenePath;
		}

	}
}
