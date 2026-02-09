using System;

namespace Poly.Common
{
    public struct FPolyTimerHandle : IEquatable<FPolyTimerHandle>
    {
        public readonly int id;
        
        internal FPolyTimerHandle(int id) => this.id = id;

        public bool Equals(FPolyTimerHandle other) => id == other.id;
        public override bool Equals(object obj) => obj is FPolyTimerHandle other && Equals(other);
        public override int GetHashCode() => id;

        public static readonly FPolyTimerHandle Invalid = new(-1);
    }
}
