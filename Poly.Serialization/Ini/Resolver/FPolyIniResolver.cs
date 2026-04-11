using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Poly.Serialization
{
	public static class FPolyIniResolver
	{
		public static FPolyResolvedIniSection ResolveSection(
			FPolyIniDocument document,
			string sectionName,
			[CanBeNull] FPolyIniResolveOptions options = null)
		{
			if (document == null)
			{
				throw new ArgumentNullException(nameof(document));
			}

			if (sectionName == null)
			{
				throw new ArgumentNullException(nameof(sectionName));
			}
			
			options ??= new FPolyIniResolveOptions();
			var resolved = new FPolyResolvedIniSection(sectionName, options.KeyComparer);
			
			var sections = document.GetSections(sectionName);
			for (int i = 0; i < sections.Count; i++)
			{
				ApplySection(sections[i], resolved, options);
			}

			return resolved;
		}

		public static void ApplySection(
			FPolyIniSection section,
			FPolyResolvedIniSection resolved,
			[CanBeNull] FPolyIniResolveOptions options = null)
		{
			if (section == null)
			{
				throw new ArgumentNullException(nameof(section));
			}

			if (resolved == null)
			{
				throw new ArgumentNullException(nameof(resolved));
			}
			
			options ??= new FPolyIniResolveOptions();
			for (int i = 0; i < section.Entries.Count; i++)
			{
				ApplyEntry(section.Entries[i], resolved, options);
			}
		}

		public static void ApplyEntry(
			in FPolyIniEntry entry,
			FPolyResolvedIniSection resolved,
			FPolyIniResolveOptions options)
		{
			switch (entry.kind)
			{
				case EPolyIniAssignmentKind.Set:
					ApplySet(entry.key, entry.value, resolved, options);
					break;

				case EPolyIniAssignmentKind.Add:
					ApplyAdd(entry.key, entry.value, resolved);
					break;

				case EPolyIniAssignmentKind.Remove:
					ApplyRemove(entry.key, entry.value, resolved, options);
					break;

				case EPolyIniAssignmentKind.AddUnique:
					ApplyAddUnique(entry.key, entry.value, resolved, options);
					break;

				case EPolyIniAssignmentKind.Clear:
					ApplyClear(entry.key, resolved);
					break;

				default:
					ApplySet(entry.key, entry.value, resolved, options);
					break;
			}
		}
		
		private static void ApplySet(
			string key,
			string value,
			FPolyResolvedIniSection resolved,
			FPolyIniResolveOptions options)
		{
			var target = resolved.GetOrAdd(key);
			if (options.SetReplacesExistingValues)
			{
				target.Values.Clear();
			}

			target.Values.Add(value);
		}
		
		private static void ApplyAdd(
			string key,
			string value,
			FPolyResolvedIniSection resolved)
		{
			var target = resolved.GetOrAdd(key);
			target.Values.Add(value);
		}
		
		private static void ApplyRemove(
			string key,
			string value,
			FPolyResolvedIniSection resolved,
			FPolyIniResolveOptions options)
		{
			if (!resolved.TryGetValue(key, out var target))
			{
				return;
			}

			RemoveAllMatchingValues(target.Values, value, options.ValueComparer);

			if (options.RemoveKeysWithNoValues && target.Values.Count == 0)
			{
				resolved.RemoveKey(key);
			}
		}
		
		private static void ApplyAddUnique(
			string key,
			string value,
			FPolyResolvedIniSection resolved,
			FPolyIniResolveOptions options)
		{
			var target = resolved.GetOrAdd(key);

			if (options.AddUniquePreventsDuplicates)
			{
				for (int i = 0; i < target.Values.Count; i++)
				{
					if (options.ValueComparer.Equals(target.Values[i], value))
					{
						return;
					}
				}
			}

			target.Values.Add(value);
		}
		
		private static void ApplyClear(
			string key,
			FPolyResolvedIniSection resolved)
		{
			resolved.RemoveKey(key);
		}
		
		private static void RemoveAllMatchingValues(
			List<string> values,
			string value,
			StringComparer comparer)
		{
			for (int i = values.Count - 1; i >= 0; --i)
			{
				if (comparer.Equals(values[i], value))
				{
					values.RemoveAt(i);
				}
			}
		}
	}
}