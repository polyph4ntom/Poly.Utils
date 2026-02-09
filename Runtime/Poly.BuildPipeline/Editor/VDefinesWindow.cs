using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Poly.BuildPipeline.Editor
{
    public class VDefinesWindow : EditorWindow
    {
        private class FDefinesConfig
        {
            public string define;
            public EnumFlagsField defineFlags;
        }

        [SerializeField]
        private VisualTreeAsset treeAsset;

        [SerializeField] 
        private OPolyDefinesContainer definesContainer;

        private readonly List<FDefinesConfig> spawnedDefines = new();
        private FPolyDefinesSet loadedSet;

        [MenuItem("Tools/Polyphantom/Scripting Defines")]
        public static void ShowWindow()
        {
            var wnd = GetWindow<VDefinesWindow>();
            wnd.titleContent = new GUIContent("Poly Scripting Defines");
        }

        public void CreateGUI()
        {
            var root = rootVisualElement;
            root.Add( treeAsset.Instantiate());
            
            loadedSet = new FPolyDefinesSet();
            loadedSet.Load();
            
            var content = root.Q<VisualElement>("content");
            foreach (var symbol in definesContainer.Defines)
            {
                var symbolConf = new FDefinesConfig
                {
                    define = symbol,
                    defineFlags = new EnumFlagsField(loadedSet.BuildFlagsForSymbol(symbol))
                    {
                        label = symbol
                    }
                };
                content.Add(symbolConf.defineFlags);
                spawnedDefines.Add(symbolConf);
            }
            
            root.Q<Button>("save-button").clicked += SaveAndCompile;
            root.Q<Button>("DebugButton").clicked += UseDebugPreset;
            root.Q<Button>("DevelopmentButton").clicked += UseDevelopmentPreset;
            root.Q<Button>("ReleaseButton").clicked += UseReleasePreset;
        }

        private void UseDebugPreset()
        {
            foreach (var defConfig in spawnedDefines)
            {
                if (!defConfig.defineFlags.value.HasFlag(EPolyBuildConfig.Editor))
                {
                    continue;
                }

                var val = (EPolyBuildConfig)defConfig.defineFlags.value;
                val &= ~EPolyBuildConfig.Editor;
                defConfig.defineFlags.value = val;
            }
            
            foreach (var defConfig in spawnedDefines)
            {
                if (!defConfig.defineFlags.value.HasFlag(EPolyBuildConfig.Debug))
                {
                    continue;
                }

                var val = (EPolyBuildConfig)defConfig.defineFlags.value;
                val |= EPolyBuildConfig.Editor;
                defConfig.defineFlags.value = val;
            }
        }

        private void UseDevelopmentPreset()
        {
            foreach (var defConfig in spawnedDefines)
            {
                if (!defConfig.defineFlags.value.HasFlag(EPolyBuildConfig.Editor))
                {
                    continue;
                }

                var val = (EPolyBuildConfig)defConfig.defineFlags.value;
                val &= ~EPolyBuildConfig.Editor;
                defConfig.defineFlags.value = val;
            }
            
            foreach (var defConfig in spawnedDefines)
            {
                if (!defConfig.defineFlags.value.HasFlag(EPolyBuildConfig.Development))
                {
                    continue;
                }

                var val = (EPolyBuildConfig)defConfig.defineFlags.value;
                val |= EPolyBuildConfig.Editor;
                defConfig.defineFlags.value = val;
            }
        }

        private void UseReleasePreset()
        {
            foreach (var defConfig in spawnedDefines)
            {
                if (!defConfig.defineFlags.value.HasFlag(EPolyBuildConfig.Editor))
                {
                    continue;
                }
                
                var val = (EPolyBuildConfig)defConfig.defineFlags.value;
                val &= ~EPolyBuildConfig.Editor;
                defConfig.defineFlags.value = val;
            }
            
            foreach (var defConfig in spawnedDefines)
            {
                if (!defConfig.defineFlags.value.HasFlag(EPolyBuildConfig.Release))
                {
                    continue;
                }

                var val = (EPolyBuildConfig)defConfig.defineFlags.value;
                val |= EPolyBuildConfig.Editor;
                defConfig.defineFlags.value = val;
            }
        }

        private void SaveAndCompile()
        {
            loadedSet.entries.Clear();
            var editorSet = new FPolyDefinesSetEntry
            {
                name = Enum.GetName(typeof(EPolyBuildConfig), EPolyBuildConfig.Editor)
            };

            var debugSet = new FPolyDefinesSetEntry
            {
                name = Enum.GetName(typeof(EPolyBuildConfig), EPolyBuildConfig.Debug)
            };

            var developmentSet = new FPolyDefinesSetEntry
            {
                name = Enum.GetName(typeof(EPolyBuildConfig), EPolyBuildConfig.Development)
            };

            var releaseSet = new FPolyDefinesSetEntry
            {
                name = Enum.GetName(typeof(EPolyBuildConfig), EPolyBuildConfig.Release)
            };
            
            foreach (var defConfig in spawnedDefines)
            {
                if (defConfig.defineFlags.value.HasFlag(EPolyBuildConfig.Editor))
                {
                    editorSet.defines += $"{defConfig.define};";
                }
                
                if (defConfig.defineFlags.value.HasFlag(EPolyBuildConfig.Debug))
                {
                    debugSet.defines += $"{defConfig.define};";
                }
                
                if (defConfig.defineFlags.value.HasFlag(EPolyBuildConfig.Development))
                {
                    developmentSet.defines += $"{defConfig.define};";
                }
                
                if (defConfig.defineFlags.value.HasFlag(EPolyBuildConfig.Release))
                {
                    releaseSet.defines += $"{defConfig.define};";
                }
            }
            
            loadedSet.entries.Add(editorSet);
            loadedSet.entries.Add(debugSet);
            loadedSet.entries.Add(developmentSet);
            loadedSet.entries.Add(releaseSet);
            loadedSet.Save();

            var enabledSymbols = new List<string>();
            foreach (var defConfig in spawnedDefines)
            {
                if (defConfig.defineFlags.value.HasFlag(EPolyBuildConfig.Editor))
                {
                    enabledSymbols.Add(defConfig.define);
                }
            }
            
            var joint = string.Join(",", enabledSymbols);
            PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup), joint);
        }
    }
}
