using System;
using System.Text;
using JetBrains.Annotations;

namespace Poly.Serialization
{
	public static class FPolyIniWriter
	{
		public static string Write(FPolyIniDocument document, [CanBeNull] FPolyIniWriteOptions options = null)
		{
			if (document == null)
			{
				throw new ArgumentNullException(nameof(document));
			}

			options ??= new FPolyIniWriteOptions();
			var sb = FPolyStringBuilderCache.Acquire(EstimateCapacity(document, options));

			try
			{
				Write(document, sb, options);

				if (options.OmitFinalTrailingNewLine)
				{
					TrimFinalNewLine(sb, options.NewLine);
				}

				return FPolyStringBuilderCache.GetStringAndRelease(sb);
			}
			catch
			{
				FPolyStringBuilderCache.Release(sb);
				throw;
			}
		}

		public static void Write(FPolyIniDocument document, StringBuilder sb, [CanBeNull] FPolyIniWriteOptions options = null)
		{
			if (document == null)
			{
				throw new ArgumentNullException(nameof(document));
			}

			if (sb == null)
			{
				throw new ArgumentNullException(nameof(sb));
			}
			
			options ??= new FPolyIniWriteOptions();
			var newLine = options.NewLine;

			for (int sectionIndex = 0; sectionIndex < document.Sections.Count; ++sectionIndex)
			{
				var section = document.Sections[sectionIndex];
				
				sb.Append('[');
				sb.Append(section.Name);
				sb.Append(']');
				sb.Append(newLine);
				
				for (int entryIndex = 0; entryIndex < section.Entries.Count; entryIndex++)
				{
					WriteEntry(sb, section.Entries[entryIndex], newLine);
				}

				var isLastSection = sectionIndex == document.Sections.Count - 1;
				if (!isLastSection && options.BlankLineBetweenSections)
				{
					sb.Append(newLine);
				}
			}
		}

		private static void WriteEntry(StringBuilder sb, in FPolyIniEntry entry, string newLine)
		{
			switch (entry.kind)
			{
				case EPolyIniAssignmentKind.Set:
					sb.Append(entry.key);
					sb.Append('=');
					sb.Append(entry.value);
					sb.Append(newLine);
					break;

				case EPolyIniAssignmentKind.Add:
					sb.Append('+');
					sb.Append(entry.key);
					sb.Append('=');
					sb.Append(entry.value);
					sb.Append(newLine);
					break;

				case EPolyIniAssignmentKind.Remove:
					sb.Append('-');
					sb.Append(entry.key);
					sb.Append('=');
					sb.Append(entry.value);
					sb.Append(newLine);
					break;

				case EPolyIniAssignmentKind.AddUnique:
					sb.Append('.');
					sb.Append(entry.key);
					sb.Append('=');
					sb.Append(entry.value);
					sb.Append(newLine);
					break;

				case EPolyIniAssignmentKind.Clear:
					sb.Append('!');
					sb.Append(entry.key);
					sb.Append(newLine);
					break;

				default:
					sb.Append(entry.key);
					sb.Append('=');
					sb.Append(entry.value);
					sb.Append(newLine);
					break;
			}
		}

		private static int EstimateCapacity(FPolyIniDocument document, FPolyIniWriteOptions options)
		{
			var newLineLength = options.NewLine?.Length ?? 1;
			var total = 0;

			for (int i = 0; i < document.Sections.Count; i++)
			{
				var section = document.Sections[i];
				total += 2 + section.Name.Length + newLineLength;
				
				for (int j = 0; j < section.Entries.Count; j++)
				{
					var entry = section.Entries[j];

					total += entry.key.Length + entry.value.Length + newLineLength + 2;

					switch (entry.kind)
					{
						case EPolyIniAssignmentKind.Add:
						case EPolyIniAssignmentKind.Remove:
						case EPolyIniAssignmentKind.AddUnique:
							total += 1;
							break;

						case EPolyIniAssignmentKind.Clear:
							total -= 1; // no '=' and no value written
							break;
					}
				}

				if (options.BlankLineBetweenSections && i < document.Sections.Count - 1)
				{
					total += newLineLength;
				}
			}
			
			return Math.Max(total, 64);
		}

		private static void TrimFinalNewLine(StringBuilder sb, string newLine)
		{
			if (sb.Length == 0 || string.IsNullOrEmpty(newLine))
			{
				return;
			}

			if (sb.Length < newLine.Length)
			{
				return;
			}

			var start = sb.Length - newLine.Length;
			for (int i = 0; i < newLine.Length; i++)
			{
				if (sb[start + i] != newLine[i])
				{
					return;
				}
			}

			sb.Length -= newLine.Length;
		}
	}
}


