using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Poly.Serialization
{
	public static class FPolyIniConfigStackResolver
	{
		public static FPolyResolvedIniSection ResolveSection(
			FPolyIniConfigStack stack,
			string sectionName,
			[CanBeNull] FPolyIniResolveOptions options = null)
		{
			if (stack == null)
			{
				throw new ArgumentNullException(nameof(stack));
			}

			if (sectionName == null)
			{
				throw new ArgumentNullException(nameof(sectionName));
			}
			
			options ??= new FPolyIniResolveOptions();
			var resolved = new FPolyResolvedIniSection(sectionName, options.KeyComparer);
			
			for (int layerIndex = 0; layerIndex < stack.Count; layerIndex++)
			{
				var document = stack[layerIndex].Document;
				var sections = document.GetSections(sectionName);

				for (int sectionIndex = 0; sectionIndex < sections.Count; sectionIndex++)
				{
					FPolyIniResolver.ApplySection(sections[sectionIndex], resolved, options);
				}
			}

			return resolved;
		}

		public static bool TryResolveScalar(
			FPolyIniConfigStack stack,
			string sectionName,
			string key,
			out string value,
			[CanBeNull] FPolyIniResolveOptions options = null)
		{

			if (stack == null)
			{
				throw new ArgumentNullException(nameof(stack));
			}

			if (sectionName == null)
			{
				throw new ArgumentNullException(nameof(sectionName));
			}

			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}
			
			var section = ResolveSection(stack, sectionName, options);
			return section.TryGetScalar(key, out value!);
		}

		public static IReadOnlyList<string> ResolveArray(
			FPolyIniConfigStack stack,
			string sectionName,
			string key,
			[CanBeNull] FPolyIniResolveOptions options = null)
		{
			if (stack == null)
			{
				throw new ArgumentNullException(nameof(stack));
			}

			if (sectionName == null)
			{
				throw new ArgumentNullException(nameof(sectionName));
			}

			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}
			
			var section = ResolveSection(stack, sectionName, options);
			return section.GetArray(key);
		}
	}
}