using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Poly.Name
{
    internal static class FPolyNameRegistry
    {
        private const string AddressKey = "NameRegistry";

        private static Dictionary<Hash128, string> idToText;
        private static AsyncOperationHandle<OPolyRegistryAsset> handle;
        private static bool isLoadAttempted;

        public static async Task EnsureLoadAsync()
        {
            if (idToText != null)
            {
                return;
            }

            if (isLoadAttempted && handle.IsDone && handle.Result != null)
            {
                return;
            }

            isLoadAttempted = true;
            
            var h = Addressables.LoadAssetAsync<OPolyRegistryAsset>(AddressKey);
            handle = h;

            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded || handle.Result == null)
            {
                // Registry missing; ToString() will fall back to hex IDs.
                return;
            }

            var asset = handle.Result;
            idToText = new Dictionary<Hash128, string>(asset.entries.Count);

            foreach (var e in asset.entries)
            {
                idToText[e.id] = e.text;
            }
        }

        internal static bool TryGetString(Hash128 id, out string text)
        {
            if (idToText != null && idToText.TryGetValue(id, out text))
            {
                return true;
            }

            text = null;
            return false;
        }

        public static void Release()
        {
            idToText = null;

            if (handle.IsDone && handle.Result != null)
            {
                Addressables.Release(handle);
            }
        }
    }
}
