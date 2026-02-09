using UnityEngine;

namespace Poly.Common
{
    [System.Serializable]
    public class FPolyTagReference
    {
        [SerializeField] 
        private string tag;
        public string Tag => tag;
    }
}
