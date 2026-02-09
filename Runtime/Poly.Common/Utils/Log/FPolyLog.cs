using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Poly.Common
{
	public static class FPolyLog
	{
		private const string POLY_LOG = "WITH_POLY_LOG";
		
		private static void Message(string category, LogType logType, string message,
			UnityEngine.GameObject context = null)
		{
			if (!Debug.isDebugBuild)
			{
				return;
			}
			
			Debug.LogFormat(logType, LogOption.None, context, $"[{category}] {message}");
		}

		[Conditional(POLY_LOG)]
		public static void Log(string category, string message, UnityEngine.GameObject context = null)
		{
			Message(category, LogType.Log, message, context);
		}

		[Conditional(POLY_LOG)]
		public static void Warning(string category, string message, UnityEngine.GameObject context = null)
		{
			Message(category, LogType.Warning, message, context);
		}

		[Conditional(POLY_LOG)]
		public static void Error(string category, string message, UnityEngine.GameObject context = null)
		{
			Message(category, LogType.Error, message, context);
		}

		[Conditional(POLY_LOG)]
		public static void Assert(string category, string message, UnityEngine.GameObject context = null)
		{
			Message(category, LogType.Assert, message, context);
		}
	}
}