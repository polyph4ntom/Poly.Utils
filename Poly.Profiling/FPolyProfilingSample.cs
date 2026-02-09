using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Poly.Common;
using Poly.Log;
using Unity.Profiling;

namespace Poly.Profiling
{
    public class FPolyProfilingSample : IDisposable
    {
#if WITH_STOPWATCH
        private readonly string sample;
        private readonly System.Diagnostics.Stopwatch sw;
#endif
        private ProfilerMarker.AutoScope scope;
        public FPolyProfilingSample(string name)
        {
#if WITH_STOPWATCH
            sample = name;
            sw = System.Diagnostics.Stopwatch.StartNew();
#endif

            scope = new ProfilerMarker(name).Auto();
        }

        public void Dispose()
        {
            scope.Dispose();
            
#if WITH_STOPWATCH
            sw.Stop();
            FPolyLog.Log("Poly.Profiling", $"{sample}: {sw.ElapsedMilliseconds} ms ({sw.ElapsedMilliseconds / 1000f:F2} s)");
#endif
        }
        
        public static void VisibleSample(string name)
        {
#if UNITY_EDITOR
            using (new FPolyProfilingSample(name))
            {
                Thread.Sleep(1);
            }
#endif
        }

        public static FPolyProfilingSample Auto<T>([CallerMemberName] string memberName = null)
        {
#if WITH_POLY_DEBUG || UNITY_EDITOR
            if (string.IsNullOrWhiteSpace(memberName))
            {
                FPolyLog.Warning("Poly.Profiling", $"FPolyProfilingSample.Auto: Member name was empty for {typeof(T).Name}");
            }
#endif
            return new FPolyProfilingSample($"{typeof(T).Name}.{memberName}");
        }
    }
}
