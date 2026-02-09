using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Poly.Memory
{
    public static class FPolyMemoryBudgetLoader
    {
        public static async Task LoadAllBudgets()
        {
            var handle = Addressables.LoadAssetsAsync<OPolyMemoryBudgetAsset>(FPolyMemory.BUDGET_LABEL);

            try
            {
                var budgets = await handle.Task;
                foreach (var budget in budgets)
                {
                    foreach (var def in budget.Pools)
                    {
                        if (def.PoolType == null)
                        {
                            FPolyMemory.PrintMemoryMessage($"Pool '{def.DisplayName}' missing PoolType.");
                            continue;
                        }

                        var type = def.PoolType.Type;

                        var parentType = def.ParentPool != null && def.ParentPool.PoolType != null
                            ? def.ParentPool.PoolType.Type
                            : null;

                        FPolyMemoryTracker.CreatePool(def.DisplayName, type, def.BudgetBytes, parentType);
                    }
                    FPolyMemory.PrintMemoryMessage($"Memory budget {budget.name} loaded with {budget.Pools.Count} pools.");
                }
            }
            finally
            {
                handle.Release();
            }
        }
    }
}