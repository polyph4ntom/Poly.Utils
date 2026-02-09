using System.Collections.Generic;
using UnityEngine;

namespace Poly.Memory
{
    [CreateAssetMenu(menuName = "Polyphantom/MemoryPool/New Budget Asset")]
    public class OPolyMemoryBudgetAsset : ScriptableObject
    {
        [SerializeField]
        private List<OPolyMemoryPoolDefinition> pools = new();

        public List<OPolyMemoryPoolDefinition> Pools => pools;
    }
}
