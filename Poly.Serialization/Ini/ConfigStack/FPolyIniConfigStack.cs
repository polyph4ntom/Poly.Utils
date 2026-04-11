using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Poly.Serialization
{
	public sealed class FPolyIniConfigStack
	{
		private readonly List<FPolyIniConfigLayer> layers;

		public FPolyIniConfigStack(int capacity = 4)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(capacity));
			}
			
			layers = new List<FPolyIniConfigLayer>(capacity);
		}
		
		public int Count => layers.Count;
		public IReadOnlyList<FPolyIniConfigLayer> Layers => layers;
		public FPolyIniConfigLayer this[int index] => layers[index];

		public void AddLayer(FPolyIniConfigLayer layer)
		{
			if (layer == null)
			{
				throw new ArgumentNullException(nameof(layer));
			}
			
			layers.Add(layer);
		}
		
		public void AddLayer(string name, FPolyIniDocument document)
		{
			AddLayer(new FPolyIniConfigLayer(name, document));
		}
		
		public bool RemoveLayer(string name, [CanBeNull] StringComparer comparer = null)
		{
			if (name == null)
			{
				throw new ArgumentNullException(nameof(name));
			}

			comparer ??= StringComparer.Ordinal;

			for (int i = 0; i < layers.Count; i++)
			{
				if (comparer.Equals(layers[i].Name, name))
				{
					layers.RemoveAt(i);
					return true;
				}
			}

			return false;
		}
		
		public void Clear()
		{
			layers.Clear();
		}
		
		public bool TryGetLayer(string name, out FPolyIniConfigLayer layer, [CanBeNull] StringComparer comparer = null)
		{
			if (name == null)
			{
				throw new ArgumentNullException(nameof(name));
			}

			comparer ??= StringComparer.Ordinal;

			for (int i = 0; i < layers.Count; i++)
			{
				if (comparer.Equals(layers[i].Name, name))
				{
					layer = layers[i];
					return true;
				}
			}

			layer = null!;
			return false;
		}
		
		public bool HasSection(string sectionName)
		{
			for (int i = 0; i < layers.Count; i++)
			{
				if (layers[i].Document.GetSections(sectionName).Count > 0)
				{
					return true;
				}
			}

			return false;
		}
		
		public IEnumerable<(FPolyIniConfigLayer Layer, FPolyIniSection Section)> EnumerateSections(string sectionName)
		{
			if (sectionName == null)
			{
				throw new ArgumentNullException(nameof(sectionName));
			}

			for (int layerIndex = 0; layerIndex < Count; layerIndex++)
			{
				var layer = this[layerIndex];
				var sections = layer.Document.GetSections(sectionName);

				for (int sectionIndex = 0; sectionIndex < sections.Count; sectionIndex++)
				{
					yield return (layer, sections[sectionIndex]);
				}
			}
		}
	}
}