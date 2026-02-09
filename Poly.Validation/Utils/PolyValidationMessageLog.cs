using System;
using System.Collections.Generic;
using System.Text;
using Poly.Validation.DataContainers;
using UnityEngine;

namespace Poly.Validation.Utils
{
	public class PolyValidationMessageLog
	{
		private struct PolyMessage
		{
			public readonly string message;
			public readonly PolyMessageSeverity severity;

			public PolyMessage(PolyMessageSeverity severity, string message)
			{
				this.message = message;
				this.severity = severity;
			}
		}

		private readonly List<PolyMessage> buffer = new();
		private readonly StringBuilder builder = new();
		private readonly string logName;
		
		public PolyValidationMessageLog(string logName)
		{
			this.logName = logName;
		}

		public void Print()
		{
			builder.AppendLine($"[Validation] Logger: {logName}");
			foreach (var m in buffer)
			{
				switch (m.severity)
				{
					case PolyMessageSeverity.Warning:
						builder.AppendLine($"- <color=yellow>[Warning]</color> {m.message}");
						break;
					case PolyMessageSeverity.Error:
						builder.AppendLine($"- <color=red>[Error]</color> {m.message}");
						break;
					case PolyMessageSeverity.PerformanceWarning:
						builder.AppendLine($"- <color=cyan>[PerformanceWarning]</color> {m.message}");
						break;
					case PolyMessageSeverity.Info:
						builder.AppendLine($"- <color=white>[Info]</color> {m.message}");
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			Debug.Log($"[Validation] {builder}");
		}

		public void Flush()
		{
			buffer.Clear();
			builder.Clear();
		}

		public void Message(PolyMessageSeverity severity, string message)
		{
			buffer.Add(new PolyMessage(severity, message));
		}
		
		public void Info(string message)
		{
			buffer.Add(new PolyMessage(PolyMessageSeverity.Info, message));
		}

		public void Error(string message)
		{
			buffer.Add(new PolyMessage(PolyMessageSeverity.Error, message));
		}
		
		public void PerformanceWarning(string message)
		{
			buffer.Add(new PolyMessage(PolyMessageSeverity.PerformanceWarning, message));
		}
		public void Warning(string message)
		{
			buffer.Add(new PolyMessage(PolyMessageSeverity.Warning, message));
		}

		public int NumMessages(PolyMessageSeverity severity = PolyMessageSeverity.Info)
		{
			var count = 0;
			foreach(var message in buffer)
			{
				if (message.severity == severity)
				{
					++count;
				}
			}

			return count;
		}
	}
}