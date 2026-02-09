using System;
using System.Collections.Generic;

namespace Poly.Common
{
    public class FPolyTimerManager
    {
        private readonly Dictionary<FPolyTimerHandle, FPolyTimer> activeTimers = new();
        private int nextTimerId = 0;
        
        public FPolyTimerHandle AddTimer(float duration, Action callback, bool isLooping = false, bool isUnscaled = false)
        {
            var timer = new FPolyTimer(duration, callback, isLooping, isUnscaled);
            var handle = new FPolyTimerHandle(nextTimerId++);
            
            activeTimers.Add(handle, timer);
            return handle;
        }

        public void CancelTimer(FPolyTimerHandle handle)
        {
            if (!activeTimers.TryGetValue(handle, out var timer))
            {
                return;
            }

            timer.Invalidate();
            activeTimers.Remove(handle);
        }

        public void Evaluate()
        {
            var completedHandles = new List<FPolyTimerHandle>();

            foreach (var pair in activeTimers)
            {
                pair.Value.Evaluate();
                if (pair.Value.IsCompleted)
                {
                    completedHandles.Add(pair.Key);
                }
            }

            foreach (var handle in completedHandles)
            {
                activeTimers.Remove(handle);
            }
        }
    }
}
