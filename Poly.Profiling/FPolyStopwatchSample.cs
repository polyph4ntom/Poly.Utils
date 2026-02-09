using System;
using System.Runtime.CompilerServices;
using Poly.Log;

namespace Poly.Profiling
{
	public class FPolyStopwatchSample : IDisposable
	{
		private readonly string sample;
		private readonly System.Diagnostics.Stopwatch sw;

		public FPolyStopwatchSample(string name)
		{
			sample = name;
			sw = System.Diagnostics.Stopwatch.StartNew();
		}

		public void Dispose()
		{
			sw.Stop();
			FPolyLog.Log("Poly.Profiling", $"{sample}: {sw.ElapsedMilliseconds} ms ({sw.ElapsedMilliseconds / 1000f:F2} s)");
		}

		public static FPolyStopwatchSample Auto<T>([CallerMemberName] string memberName = null)
		{
#if DEBUG || UNITY_EDITOR
			if (string.IsNullOrWhiteSpace(memberName))
			{
				FPolyLog.Warning("Poly.Profiling", $"FPolyProfilingSample.Auto: Member name was empty for {typeof(T).Name}");
			}
#endif
			return new FPolyStopwatchSample($"{typeof(T).Name}.{memberName}");
		}
	}
}
