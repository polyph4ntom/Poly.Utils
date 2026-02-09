using System;

namespace Poly.Memory
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class FPolyKnownSizeAttribute : Attribute
    {
        public int Bytes { get; }

        public FPolyKnownSizeAttribute(int bytes)
        {
            Bytes = bytes;
        }
    }
}
