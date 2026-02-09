using System;
using UnityEditor;
using UnityEngine;

namespace Poly.Validation.DataContainers
{
	/** A struct to hold important information about an assets found by the Asset Registry */
	public struct PolyAssetData : IEquatable<PolyAssetData>
	{
		private string assemblyName;
		private string assemblyPath;
		private string assetName;
		private UnityEngine.Object assetObject;
		
		/** The object path for the asset in the form Assembly.ObjectName */
		public readonly string objectPath;
		
		/* Loaded object if path is valid */
		public UnityEngine.Object AssetObject => assetObject;
		
		public PolyAssetData(string objectPath)
		{
			this.objectPath = objectPath;

			assetObject = null;
			assemblyName = string.Empty;
			assemblyPath = string.Empty;
			assetName = string.Empty;
		}

		public PolyAssetData(UnityEngine.Object assetObject)
		{
			objectPath = "<missing path>";
			this.assetObject = assetObject;
			assemblyName = assetObject.GetType().Assembly.FullName;
			assemblyPath = assetObject.GetType().Assembly.Location;
			assetName = assetObject.name;

		}

		internal bool LoadAsset()
		{
			if (AssetObject != null)
			{
				return true;
			}

			assetObject = AssetDatabase.LoadMainAssetAtPath(objectPath);
			if (assetObject == null)
			{
				Debug.LogError("[Validation] Asset at path cannot be loaded {}.");
				return false;
			}

			assemblyName = assetObject.GetType().Assembly.FullName;
			assemblyPath = assetObject.GetType().Assembly.Location;
			assetName = assetObject.name;
			return true;
		}

		public bool IsValid()
		{
			return AssetDatabase.AssetPathExists(objectPath) || assetObject != null;
		}

		public static bool operator==(PolyAssetData a, PolyAssetData b)
		{
			return a.assetName == b.assetName && a.assemblyName == b.assemblyName;
		}

		public static bool operator !=(PolyAssetData a, PolyAssetData b)
		{
			return a.assetName != b.assetName || a.assemblyName != b.assemblyName;
		}

		public static bool operator >(PolyAssetData a, PolyAssetData b)
		{
			if (a.assemblyName == b.assemblyName)
			{
				return string.Compare(b.assetName, a.assetName, StringComparison.Ordinal) < 0;
			}
			return string.Compare(b.assemblyName, a.assemblyName, StringComparison.Ordinal) < 0;
		}

		public static bool operator <(PolyAssetData a, PolyAssetData b)
		{
			if (a.assemblyName == b.assemblyName)
			{
				return string.Compare(a.assetName, b.assetName, StringComparison.Ordinal) < 0;
			}
			return string.Compare(a.assemblyName, b.assemblyName, StringComparison.Ordinal) < 0;
		}
		
		public bool Equals(PolyAssetData other)
		{
			return objectPath == other.objectPath && assemblyName == other.assemblyName &&
			       assemblyPath == other.assemblyPath
			       && assetName == other.assetName;
		}

		public override bool Equals(object obj)
		{
			return obj is PolyAssetData other && Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(objectPath, assemblyName, assemblyPath, assetName);
		}
	}
}