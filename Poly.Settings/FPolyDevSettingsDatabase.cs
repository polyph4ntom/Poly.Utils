using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Poly.Common;
using Poly.Log;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Poly.Settings
{
   public class FDatabaseEntry
	{
		public bool isLoading;

		protected FDatabaseEntry(bool isLoading)
		{
			this.isLoading = isLoading;
		}
	}

	public class FDatabaseEntry<TData> : FDatabaseEntry
		where TData : class
	{
		public TData value;

		public FDatabaseEntry(bool isLoading) : base(isLoading)
		{
			this.value = null;
		}
	}

	public static class FPolyDevSettingsDatabase
	{
		private static readonly Dictionary<System.Type, FDatabaseEntry> settingsDatabase = new();
		private static TaskCompletionSource<bool> isDatabaseReady = new();
		
		private static List<ScriptableObject> assets = new List<ScriptableObject>();
		
		private static int inLoading = 0;
		
		public static Task WaitForDatabase() => isDatabaseReady.Task;
		
		public static TData Get<TData>() where TData : class
		{
			if(!settingsDatabase.TryGetValue(typeof(TData), out var entry))
			{
				FPolyLog.Error("Poly.Settings", $"There is no {typeof(TData)} setting class registered in the database");
				return null;
			}

			if (entry.isLoading)
			{
				FPolyLog.Error("Poly.Settings", $"{typeof(TData)} Setting data is currently in loading process");
				return null;
			}

			if (entry is FDatabaseEntry<TData> entryCasted)
			{
				return entryCasted.value;
			}

			FPolyLog.Error("Poly.Settings", $"{typeof(TData)} cannot be retrieved from {entry.GetType().Name}");
			return null;
		}

		public static void Register<TData>(string assetID)
			where TData : class
		{
			var entry =  new FDatabaseEntry<TData>(true);
			settingsDatabase.Add(typeof(TData), entry);

			isDatabaseReady = new TaskCompletionSource<bool>();

			++inLoading;
			_ = LoadAsset<TData>(assetID);
		}

		private static async Task LoadAsset<TData>(string assetID) 
			where TData : class
		{
			var loaded = await LoadSettingAsset<TData>(assetID);
			
			var value = settingsDatabase[typeof(TData)] as FDatabaseEntry<TData>;
			if (value == null)
			{
				return;
			}

			value.isLoading = false;
			value.value = loaded;
			--inLoading;

			if (inLoading == 0)
			{
				isDatabaseReady.SetResult(true);
			}
		}
		
		private static async Task<TData> LoadSettingAsset<TData>(string assetId)
			where TData : class
		{
			var handle = Addressables.LoadAssetAsync<OPolyDevSettingsAsset<TData>>(assetId);

			try
			{
				var asset = await handle.Task;
				assets.Add(asset);

				return asset.Data;
			}
			catch (Exception e)
			{
				FPolyLog.Error("Poly.Settings", $"LoadSettingAsset failed: {e}");
				throw;
			}
			finally
			{
				//handle.Release();
			}
		}
	}
}