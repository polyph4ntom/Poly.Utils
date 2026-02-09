using UnityEngine;

namespace Poly.Settings
{
	public abstract class OPolyDevSettingsAsset<TData> : ScriptableObject
		where TData : class
	{
		[field: SerializeField] 
		public TData Data { get; private set; }
	}
}