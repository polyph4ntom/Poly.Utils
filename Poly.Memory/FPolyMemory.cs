using UnityEngine;

namespace Poly.Memory
{
    public class FPolyMemory
    {
        public const string BUDGET_LABEL = "PolyMemoryBudget";
        
        public static void PrintMemoryMessage(string message, UnityEngine.Object context = null)
        {
            Debug.Log($"[Poly.Memory] {message}", context);
        }
    }
}
