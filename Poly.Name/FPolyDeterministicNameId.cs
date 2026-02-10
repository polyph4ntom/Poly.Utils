using UnityEngine;
using System.Security.Cryptography;
using System.Text;

namespace Poly.Name
{
    internal static class FPolyDeterministicNameId
    {
        internal static Hash128 FromString(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return default;
            }

            using var md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
            
            uint a = System.BitConverter.ToUInt32(bytes, 0);
            uint b = System.BitConverter.ToUInt32(bytes, 4);
            uint c = System.BitConverter.ToUInt32(bytes, 8);
            uint d = System.BitConverter.ToUInt32(bytes, 12);
            return new Hash128(a, b, c, d);
        }
    }
}
