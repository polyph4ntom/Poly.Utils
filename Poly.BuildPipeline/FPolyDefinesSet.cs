using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Poly.BuildPipeline
{
	[Serializable]
	public class FPolyDefinesSetEntry
	{
		public string name = string.Empty;
		public string defines = string.Empty;
	}

	[Serializable]
	public class FPolyDefinesSet
	{
		public List<FPolyDefinesSetEntry> entries = new();

		private const string FILENAME = "defines";
		private static readonly string ResourcesPath = Path.Combine(Application.dataPath, "Resources");
		private static readonly string FullPath = Path.Combine(ResourcesPath, FILENAME + ".json");
		
		public void Save()
		{
			var json = JsonUtility.ToJson(this, true);

			if (!Directory.Exists(ResourcesPath))
			{
				Directory.CreateDirectory(ResourcesPath);
			}
			
			File.WriteAllText(FullPath, json);
			
#if UNITY_EDITOR
			UnityEditor.AssetDatabase.Refresh();
#endif
		}

		public void Load()
		{
			var jsonFile = Resources.Load<TextAsset>(FILENAME);
			if (jsonFile == null)
			{
				return;
			}

			var loaded =  JsonUtility.FromJson<FPolyDefinesSet>(jsonFile.text);
			entries = loaded.entries;
		}

		public EPolyBuildConfig BuildFlagsForSymbol(string symbol)
		{
			var toReturn = EPolyBuildConfig.None;
			
			var test = Enum.GetName(typeof(EPolyBuildConfig), EPolyBuildConfig.Editor);
			foreach (var entry in entries)
			{
				if(entry.name == Enum.GetName(typeof(EPolyBuildConfig), EPolyBuildConfig.Editor))
				{
					if (entry.defines.Contains(symbol))
					{
						toReturn |= EPolyBuildConfig.Editor;
					}
				}
				else if (entry.name == Enum.GetName(typeof(EPolyBuildConfig), EPolyBuildConfig.Debug))
				{
					if (entry.defines.Contains(symbol))
					{
						toReturn |= EPolyBuildConfig.Debug;
					}
				}
				else if (entry.name == Enum.GetName(typeof(EPolyBuildConfig), EPolyBuildConfig.Development))
				{
					if (entry.defines.Contains(symbol))
					{
						toReturn |= EPolyBuildConfig.Development;
					}
				}
				else if (entry.name == Enum.GetName(typeof(EPolyBuildConfig), EPolyBuildConfig.Release))
				{
					if (entry.defines.Contains(symbol))
					{
						toReturn |= EPolyBuildConfig.Release;
					}
				}
			}
			
			return toReturn;
		}
	}
}
