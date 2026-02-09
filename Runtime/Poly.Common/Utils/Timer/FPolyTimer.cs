using System;
using UnityEngine;

namespace Poly.Common
{
    public class FPolyTimer
    {
        public bool IsCompleted => currentTime >= 1f;
        public float TimeRemaining => Mathf.Max(0f, duration * (1f - currentTime));
        public float Progress => Mathf.Clamp01(currentTime);
        
        private readonly bool isLooping = false;
        private readonly bool isUnscaledTimer = false;
        private readonly Action callback;

        private float startTime;
        private readonly float duration;
        private float currentTime;
        private bool isCancelled = false;
        
        public FPolyTimer(float duration, Action callback, bool isLooping = false, bool isUnscaled = false)
        {
            isUnscaledTimer = isUnscaled;
            startTime = isUnscaled ? Time.time :  Time.unscaledTime;
            this.isLooping = isLooping;
            this.duration = duration;
            this.callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        public void Invalidate()
        {
            isCancelled = true;
        }

        public void Evaluate()
        {
            if ((IsCompleted && !isLooping) || isCancelled)
                return;

            var timeToUse = isUnscaledTimer ? Time.unscaledTime : Time.time;
            currentTime = (timeToUse - startTime) / duration;

            if (currentTime <= 1f)
            {
                return;
            }

            callback.Invoke();

            if (!isLooping)
            {
                return;
            }

            startTime = isUnscaledTimer ? Time.unscaledTime : Time.time;
            currentTime = 0f;
        }
    }
}