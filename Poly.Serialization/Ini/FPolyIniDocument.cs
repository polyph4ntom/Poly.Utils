using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Poly.Serialization
{
    public enum EPolyIniAssignmentKind : byte
    {
        Set       = 0,        //  Key=Value
        Add       = 1,        // +Key=Value
        Remove    = 2,        // -Key=Value
        AddUnique = 3,        // .Key=Value
        Clear     = 4         // !Key
    }

    public readonly struct FPolyIniEntry
    {
        public readonly EPolyIniAssignmentKind kind;
        public readonly string key;
        public readonly string value;
        public readonly int lineNumber;

        public FPolyIniEntry(EPolyIniAssignmentKind kind, string key, string value, int lineNumber)
        {
            if (lineNumber < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lineNumber));
            }
            
            this.kind = kind;
            this.key = key ?? throw new ArgumentNullException(nameof(key));
            this.value = value ?? throw new ArgumentNullException(nameof(value));
            this.lineNumber = lineNumber;
        }

        public bool HasValue => value.Length > 0;

        public override string ToString()
        {
            return kind switch
            {
                EPolyIniAssignmentKind.Set       =>    $"{key}={value}",
                EPolyIniAssignmentKind.Add       =>    $"+{key}={value}",
                EPolyIniAssignmentKind.Remove    =>    $"-{key}={value}",
                EPolyIniAssignmentKind.AddUnique =>    $".{key}={value}",
                EPolyIniAssignmentKind.Clear     =>    $"!{key}",
                _ => $"{key}={value}"
            };
        }
    }

    public sealed class FPolyIniSection
    {
        // Small allocation-saving view when using the cached ordinal index.
        private sealed class FPolyEntryIndexView : IReadOnlyList<FPolyIniEntry>
        {
            private readonly FPolyIniSection section;
            private readonly List<int> indices;
            
            public FPolyEntryIndexView(FPolyIniSection section, List<int> indices)
            {
                this.section = section;
                this.indices = indices;
            }
            
            public FPolyIniEntry this[int index] => section.Entries[indices[index]];
            public int Count => indices.Count;
            
            public IEnumerator<FPolyIniEntry> GetEnumerator()
            {
                for (int i = 0; i < indices.Count; i++)
                {
                    yield return section.Entries[indices[i]];
                }
            }
            
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [CanBeNull] 
        private Dictionary<string, List<int>> entryIndicesByKey;
        
        public string Name { get; }
        public List<FPolyIniEntry> Entries { get; }

        public FPolyIniSection(string name, int entryCapacity = 4)
        {
            if (entryCapacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(entryCapacity));
            }
            
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Entries = new List<FPolyIniEntry>(entryCapacity);
        }

        public void AddEntry(FPolyIniEntry entry)
        {
            Entries.Add(entry);
            entryIndicesByKey = null;
        }

        public void AddEntry(EPolyIniAssignmentKind kind, string key, string value, int lineNumber)
        {
            AddEntry(new FPolyIniEntry(kind, key, value, lineNumber));
        }

        public IReadOnlyList<FPolyIniEntry> FindEntries(string key, [CanBeNull] StringComparer comparer = null)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            
            comparer ??= StringComparer.Ordinal;

            // Fast path for the default comparer using a cached index.
            if (ReferenceEquals(comparer, StringComparer.Ordinal))
            {
                var index = GetOrBuildOrdinalIndex();
                return index.TryGetValue(key, out var entryIndices)
                    ? new FPolyEntryIndexView(this, entryIndices)
                    : Array.Empty<FPolyIniEntry>();
            }
            
            // Fallback for custom comparers.
            var results = new List<FPolyIniEntry>(1);
            for (int i = 0; i < Entries.Count; ++i)
            {
                if (comparer.Equals(Entries[i].key, key))
                {
                    results.Add(Entries[i]);
                }
            }
            
            return results;
        }

        public bool TryGetLastEntry(string key, out FPolyIniEntry entry, [CanBeNull] StringComparer comparer = null)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            
            comparer ??= StringComparer.Ordinal;
            
            for (int i = Entries.Count - 1; i >= 0; i--)
            {
                if (!comparer.Equals(Entries[i].key, key))
                {
                    continue;
                }
                
                entry = Entries[i];
                return true;
            }
            
            entry = default;
            return false;
        }

        private Dictionary<string, List<int>> GetOrBuildOrdinalIndex()
        {
            if (entryIndicesByKey != null)
            {
                return entryIndicesByKey;
            }
            
            var map = new Dictionary<string, List<int>>(Entries.Count, StringComparer.Ordinal);

            for (int i = 0; i < Entries.Count; ++i)
            {
                var key = Entries[i].key;

                if (!map.TryGetValue(key, out var list))
                {
                    list = new List<int>(1);
                    map.Add(key, list);
                }
                
                list.Add(i);
            }
            
            entryIndicesByKey = map;
            return map;
        }
        
        public void AddSet(string key, string value, int lineNumber = 0)
        {
            AddEntry(EPolyIniAssignmentKind.Set, key, value, lineNumber);
        }

        public void AddAdd(string key, string value, int lineNumber = 0)
        {
            AddEntry(EPolyIniAssignmentKind.Add, key, value, lineNumber);
        }

        public void AddRemove(string key, string value, int lineNumber = 0)
        {
            AddEntry(EPolyIniAssignmentKind.Remove, key, value, lineNumber);
        }

        public void AddAddUnique(string key, string value, int lineNumber = 0)
        {
            AddEntry(EPolyIniAssignmentKind.AddUnique, key, value, lineNumber);
        }

        public void AddClear(string key, int lineNumber = 0)
        {
            AddEntry(EPolyIniAssignmentKind.Clear, key, string.Empty, lineNumber);
        }
    }

    public sealed class FPolyIniDocument
    {
        private sealed class FPolySectionIndexView : IReadOnlyList<FPolyIniSection>
        {
            private readonly FPolyIniDocument document;
            private readonly List<int> indices;
            
            public FPolySectionIndexView(FPolyIniDocument document, List<int> indices)
            {
                this.document = document;
                this.indices = indices;
            }
            
            public FPolyIniSection this[int index] => document.Sections[indices[index]];
            public int Count => indices.Count;
            
            public IEnumerator<FPolyIniSection> GetEnumerator()
            {
                for (int i = 0; i < indices.Count; i++)
                {
                    yield return document.Sections[indices[i]];
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private readonly Dictionary<string, List<int>> sectionIndicesByName;
        
        public List<FPolyIniSection> Sections { get; }
        public StringComparer SectionNameComparer { get; }

        public FPolyIniDocument(int sectionCapacity = 4, [CanBeNull] StringComparer sectionNameComparer = null)
        {
            if (sectionCapacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sectionCapacity));
            }
            
            SectionNameComparer = sectionNameComparer ?? StringComparer.Ordinal;
            Sections = new List<FPolyIniSection>(sectionCapacity);
            sectionIndicesByName = new Dictionary<string, List<int>>(SectionNameComparer);
        }

        public void AddSection(FPolyIniSection section)
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }
            
            var index = Sections.Count;
            Sections.Add(section);

            if (!sectionIndicesByName.TryGetValue(section.Name, out var list))
            {
                list = new List<int>(1);
                sectionIndicesByName.Add(section.Name, list);
            }
            
            list.Add(index);
        }

        public FPolyIniSection AddSection(string name, int entryCapacity = 0)
        {
            var section = new FPolyIniSection(name, entryCapacity);
            AddSection(section);
            return section;
        }

        public bool TryGetLastSection(string name, out FPolyIniSection section)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (sectionIndicesByName.TryGetValue(name, out var list) && list.Count > 0)
            {
                section = Sections[list[^1]];
                return true;
            }

            section = null!;
            return false;
        }

        public IReadOnlyList<FPolyIniSection> GetSections(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (sectionIndicesByName.TryGetValue(name, out var list))
            {
                return new FPolySectionIndexView(this, list);
            }
            
            return Array.Empty<FPolyIniSection>();
        }
        
        public IEnumerable<FPolyIniEntry> EnumerateEntries(string sectionName)
        {
            var sections = GetSections(sectionName);
            for (int i = 0; i < sections.Count; i++)
            {
                var section = sections[i];
                for (int j = 0; j < section.Entries.Count; j++)
                {
                    yield return section.Entries[j];
                }
            }
        }
    }
}