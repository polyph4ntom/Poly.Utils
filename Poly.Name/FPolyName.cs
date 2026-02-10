using System;
using UnityEngine;

namespace Poly.Name
{
    [Serializable]
    public struct FPolyName : IEquatable<FPolyName>
    {
        [SerializeField] 
        private Hash128 id;

        public Hash128 Id => id;
        
        public bool Equals(FPolyName other) => id.Equals(other.Id);
        public override int GetHashCode() => id.GetHashCode();

        public override string ToString() => FPolyNameRegistry.TryGetString(id, out var s) ? s : id.ToString();
    }
}
