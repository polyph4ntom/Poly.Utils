using UnityEditor;
using UnityEngine;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

namespace Poly.Name.Editor
{
    internal static class FPolyNameRegistryEditor
    {
        internal const string AssetPath = "Assets/Main/Data/NameRegistry.asset";
        internal const string AddressKey = "NameRegistry";

        internal static OPolyRegistryAsset GetOrCreate()
        {
            var asset = AssetDatabase.LoadAssetAtPath<OPolyRegistryAsset>(AssetPath);
            if (asset != null)
            {
                EnsureAddressable(asset);
                return asset;
            }

            var folder = System.IO.Path.GetDirectoryName(AssetPath);
            if (!AssetDatabase.IsValidFolder(folder))
            {
                var parts = folder.Split('/');
                string cur = parts[0];
                for (int i = 1; i < parts.Length; i++)
                {
                    string next = $"{cur}/{parts[i]}";
                    if (!AssetDatabase.IsValidFolder(next))
                    {
                        AssetDatabase.CreateFolder(cur, parts[i]);
                    }

                    cur = next;
                }
            }

            asset = ScriptableObject.CreateInstance<OPolyRegistryAsset>();
            AssetDatabase.CreateAsset(asset, AssetPath);
            AssetDatabase.SaveAssets();
            
            EnsureAddressable(asset);
            return asset;
        }

        internal static void EnsureEntry(Hash128 id, string text)
        {
            if (!id.isValid || string.IsNullOrEmpty(text))
            {
                return;
            }

            var asset = GetOrCreate();

            for (int i = 0; i < asset.entries.Count; i++)
            {
                if (asset.entries[i].id == id)
                {
                    return;
                }
            }
            
            asset.entries.Add(new OPolyRegistryAsset.Entry{ id = id, text = text });
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
        }

        internal static bool TryGetText(Hash128 id, out string text)
        {
            var asset = GetOrCreate();
            for (int i = 0; i < asset.entries.Count; i++)
            {
                if (asset.entries[i].id == id)
                {
                    text = asset.entries[i].text;
                    return true;
                }
            }

            text = null;
            return false;
        }

        private static void EnsureAddressable(Object obj)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                return;
            }
            
            string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(obj));
            var entry = settings.FindAssetEntry(guid) ?? settings.CreateOrMoveEntry(guid, settings.DefaultGroup);

            if (entry.address != AddressKey)
            {
                entry.address = AddressKey;
            }
            
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);
        }
    }
}
