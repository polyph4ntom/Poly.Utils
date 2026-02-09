using UnityEngine;

namespace Poly.BuildPipeline
{
	[CreateAssetMenu(menuName = "Polyphantom/Defines/New Defines Container")]
	public class OPolyDefinesContainer : ScriptableObject
	{
		[field: SerializeField] 
		public string[] Defines { get; private set; }
	}
}
