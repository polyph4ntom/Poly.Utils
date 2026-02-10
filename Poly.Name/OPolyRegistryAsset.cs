using System;
using System.Collections.Generic;
using UnityEngine;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Poly.Name.Editor")]

namespace Poly.Name
{
    internal sealed class OPolyRegistryAsset : ScriptableObject
    {
	    [Serializable]
	    internal struct Entry
	    {
		    public Hash128 id; 
		    public string text;
	    }

	    [SerializeField] 
	    internal List<Entry> entries = new();
    }
}
