using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Poly.Serialization
{
	public static class FPolyIniParser
	{
		public static FPolyIniDocument Parse(
			string text, 
			[CanBeNull] FPolyIniParseOptions options = null,
			[CanBeNull] List<FPolyIniDiagnostic> diagnostics = null)
		{
			if (text == null)
			{
				throw new ArgumentNullException(nameof(text));
			}

			options ??= new FPolyIniParseOptions();

			var source = text.AsSpan();
			var document = new FPolyIniDocument(sectionNameComparer: options.SectionNameComparer);

			FPolyIniSection currentSection = null;

			var index = 0;
			var lineNumber = 1;

			while (index < source.Length)
			{
				var lineStart = index;
				var lineEnd = index;

				while (lineEnd < source.Length && source[lineEnd] != '\r' && source[lineEnd] != '\n')
				{
					++lineEnd;
				}

				var line = source.Slice(lineStart, lineEnd - lineStart);
				
				ParseLine(line, lineNumber, options, diagnostics, document, ref currentSection);

				if (lineEnd < source.Length)
				{
					if (source[lineEnd] == '\r' && lineEnd + 1 < source.Length && source[lineEnd + 1] == '\n')
					{
						index = lineEnd + 2;
					}
					else
					{
						index = lineEnd + 1;
					}
				}
				else
				{
					index = lineEnd;
				}
				++lineNumber;
			}
			
			return document;
		}

		private static void ParseLine(
			ReadOnlySpan<char> rawLine,
			int lineNumber,
			FPolyIniParseOptions options,
			[CanBeNull] List<FPolyIniDiagnostic> diagnostics,
			FPolyIniDocument document,
			[CanBeNull] ref FPolyIniSection currentSection)
		{
			var line = TrimLineOuterWhitespace(rawLine);

			if (line.Length == 0)
			{
				return;
			}

			var first = line[0];

			if (first is ';' or '#')
			{
				return;
			}

			if (first is '[')
			{
				ParseSectionLine(line, lineNumber, options, diagnostics, document, ref currentSection);
				return;
			}

			ParseEntryLine(line, lineNumber, options, diagnostics, ref currentSection);
		}

		private static void ParseSectionLine(
			ReadOnlySpan<char> line,
			int lineNumber,
			FPolyIniParseOptions options,
			[CanBeNull] List<FPolyIniDiagnostic> diagnostics,
			FPolyIniDocument document,
			[CanBeNull] ref FPolyIniSection currentSection)
		{
			if (line.Length < 2 || line[^1] != ']')
			{
				AddDiagnostic(diagnostics, EPolyIniDiagnosticId.UnterminatedSectionHeader, lineNumber);
				return;
			}
			
			var sectionName = line.Slice(1, line.Length - 2);

			if (options.TrimSectionsName)
			{
				sectionName = TrimSpacesAndTabs(sectionName);
			}

			if (sectionName.Length == 0)
			{
				AddDiagnostic(diagnostics, EPolyIniDiagnosticId.EmptySectionName, lineNumber);
				return;
			}

			currentSection = new FPolyIniSection(sectionName.ToString());
			document.AddSection(currentSection);
		}

		private static void ParseEntryLine(
			ReadOnlySpan<char> line,
			int lineNumber,
			FPolyIniParseOptions options,
			[CanBeNull] List<FPolyIniDiagnostic> diagnostics,
			[CanBeNull] ref FPolyIniSection currentSection)
		{
			if (currentSection == null)
			{
				AddDiagnostic(diagnostics, EPolyIniDiagnosticId.ContentOutsideSection, lineNumber);
				return;
			}

			var kind = EPolyIniAssignmentKind.Set;
			var first = line[0];
			if (first is '+' or '-' or '.' or '!')
			{
				kind = first switch
				{
					'+' => EPolyIniAssignmentKind.Add,
					'-' => EPolyIniAssignmentKind.Remove,
					'.' => EPolyIniAssignmentKind.AddUnique,
					'!' => EPolyIniAssignmentKind.Clear,
					_ => EPolyIniAssignmentKind.Set
				};

				line = line.Slice(1);
			}

			if (kind == EPolyIniAssignmentKind.Clear)
			{
				ParseClearEntry(line, lineNumber, options, diagnostics, currentSection);
				return;
			}
			
			var equalsIndex = line.IndexOf('=');
			if (equalsIndex < 0)
			{
				AddDiagnostic(diagnostics, EPolyIniDiagnosticId.MissingEquals, lineNumber);
			}
			
			var key = line.Slice(0, equalsIndex);
			var value = line.Slice(equalsIndex + 1);
			
			if (options.TrimWhitespaceAroundKeysAndValues)
			{
				key = TrimSpacesAndTabs(key);
				value = TrimSpacesAndTabs(value);
			}
			
			if (key.Length == 0)
			{
				AddDiagnostic(diagnostics, EPolyIniDiagnosticId.EmptyKey, lineNumber);
				return;
			}
			
			currentSection.AddEntry(
				kind,
				key.ToString(),
				value.ToString(),
				lineNumber);
		}

		private static void ParseClearEntry(
			ReadOnlySpan<char> line,
			int lineNumber,
			FPolyIniParseOptions options,
			[CanBeNull] List<FPolyIniDiagnostic> diagnostics,
			[NotNull] FPolyIniSection currentSection)
		{
			var key = line;

			if (options.TrimWhitespaceAroundKeysAndValues)
			{
				key = TrimSpacesAndTabs(key);
			}
			
			if (key.Length == 0)
			{
				AddDiagnostic(diagnostics, EPolyIniDiagnosticId.EmptyKey, lineNumber);
				return;
			}
			
			var equalsIndex = key.IndexOf('=');
			if (equalsIndex >= 0)
			{
				var actualKey = key.Slice(0, equalsIndex);
				if (options.TrimWhitespaceAroundKeysAndValues)
				{
					actualKey = TrimSpacesAndTabs(actualKey);
				}

				if (actualKey.Length == 0)
				{
					AddDiagnostic(diagnostics, EPolyIniDiagnosticId.EmptyKey, lineNumber);
					return;
				}
				
				AddDiagnostic(diagnostics, EPolyIniDiagnosticId.ClearOperationHasUnexpectedValue, lineNumber);
				
				currentSection.AddEntry(
					EPolyIniAssignmentKind.Clear,
					actualKey.ToString(),
					string.Empty,
					lineNumber);

				return;
			}
			
			currentSection.AddEntry(
				EPolyIniAssignmentKind.Clear,
				key.ToString(),
				string.Empty,
				lineNumber);
		}

		private static void AddDiagnostic(
			[CanBeNull] List<FPolyIniDiagnostic> diagnostics,
			EPolyIniDiagnosticId diagnosticId, 
			int lineNumber)
		{
			diagnostics?.Add(new FPolyIniDiagnostic(diagnosticId, lineNumber));
		}
		
		private static ReadOnlySpan<char> TrimLineOuterWhitespace(ReadOnlySpan<char> span)
		{
			return TrimSpacesAndTabs(span);
		}

		private static ReadOnlySpan<char> TrimSpacesAndTabs(ReadOnlySpan<char> span)
		{
			var start = 0;
			var end = span.Length - 1;
			
			while (start < span.Length && IsSpaceOrTab(span[start]))
			{
				++start;
			}
			
			while (end >= start && IsSpaceOrTab(span[end]))
			{
				--end;
			}
			
			return start <= end
				? span.Slice(start, end - start + 1)
				: ReadOnlySpan<char>.Empty;
		}
		
		private static bool IsSpaceOrTab(char c)
		{
			return c is ' ' or '\t';
		}
	}    
}