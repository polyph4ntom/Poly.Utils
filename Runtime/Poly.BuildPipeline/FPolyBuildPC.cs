using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Poly.BuildPipeline
{
	public class FPolyBuildPC
	{
        private const string LOG_TAG = "[PolyBuild] ";
        
        private static readonly string[] Scenes = new string[]
        {
            "Assets/Main/Scenes/Gameplay/Unique/MainMenu/MainMenu.unity",
            "Assets/Main/Scenes/Gameplay/Unique/Loading/LoadingScene.unity",
            "Assets/Main/Scenes/Gameplay/Levels/01.unity"
        };
        
        private static readonly string[] DebugDefines = new string[]
        {
           
        };

        private static readonly string[] ReleaseDefines = new string[]
        {
            
        };
        
        [UsedImplicitly]
        public static void BuildWin64()
        {
            Log("BuildWin64 - start");
            var arguments = ParseArguments();
            if (!arguments.IsDevBuild)
            {
                Log($"Build is forced development - enabling DevBuild!");
                arguments.IsDevBuild = true;
            }
            Log($"arg {arguments}");

            var buildOptions = arguments.GetBuildOptions();

            SetDefines(NamedBuildTarget.Standalone, arguments.Defines + ";" + string.Join(";", DebugDefines), arguments.DisableSteam);
            var report = UnityEditor.BuildPipeline.BuildPlayer(buildOptions);
            FinishBuild(report.summary, "BuildWin64");
        }
        
        [UsedImplicitly]
        public static void BuildWin64Release()
        {
            Log("BuildWin64Release - start");
            var arguments = ParseArguments();
            if (arguments.IsDevBuild)
            {
                Log($"Build is forced release - disabling DevBuild!");
                arguments.IsDevBuild = false;
            }
            Log($"arg {arguments}");

            var buildOptions = arguments.GetBuildOptions();

            SetDefines(NamedBuildTarget.Standalone, string.Join(";", ReleaseDefines) + ";" + arguments.Defines, arguments.DisableSteam);
            var report = UnityEditor.BuildPipeline.BuildPlayer(buildOptions);
            FinishBuild(report.summary, "BuildWin64");
        }
        
        private static void SetDefines(NamedBuildTarget buildTarget, string defines, bool disableSteam)
        {
            var builder = new StringBuilder();
            if (disableSteam)
            {
                builder.Append(";DISABLESTEAMWORKS");
            }

            if (!string.IsNullOrEmpty(defines))
            {
                builder.Append(";");
                builder.Append(defines);
            }

            defines = builder.ToString();
            Log($"DEFINES: \"{defines}\"");
            PlayerSettings.SetScriptingDefineSymbols(buildTarget, defines);
        }
        
        private static void FinishBuild(BuildSummary summary, string name)
        {
            if (summary.result == BuildResult.Succeeded)
            {
                Log($"Build success size:{summary.totalSize}", true);
            }
            else
            {
                LogError($"failed: {summary.result}", true);
                EditorApplication.Exit(1);
            }

            Log($"{name} - finish");
        }
        
        private static Arguments ParseArguments()
        {
            var toReturn = new Arguments();
            var args = System.Environment.GetCommandLineArgs();
            
            var pattern = new Regex(@"\+(\w+)\:(.*)", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            foreach (var tmp in args)
            {
                var arg = tmp.Trim('"');
                if (string.IsNullOrWhiteSpace(arg) || arg[0] != '+')
                    continue;

                var match = pattern.Match(arg);
                if (!match.Success)
                {
                    LogError($"Unknown param {arg}");
                    continue;
                }

                var paramName = match.Groups[1].Value.ToLowerInvariant();
                var value = match.Groups[2].Value;

                if (string.IsNullOrWhiteSpace(value))
                {
                    Log($"Missing value for {paramName}");
                    continue;
                }

                Log($"parsed: {paramName} = {value}");

                switch (paramName)
                {
                    case "output": toReturn.OutputDir = value; break;
                    case "defines": toReturn.Defines = value; break;
                    case "define_group": toReturn.DefineGroup = value; break;
                    case "dev_build": bool.TryParse(value, out toReturn.IsDevBuild); break;
                    case "disable_steam": bool.TryParse(value, out toReturn.DisableSteam); break;
                    default:
                        LogError($"Unknown param {paramName} = {value}");
                        break;
                }
            }

            return toReturn;
        }
        
         /// <summary>
        /// +arg_name:value
        /// </summary>
        struct Arguments
        {
            /// <summary> +output:<dir> </summary>
            public string OutputDir;

            /// <summary> +defines:DEF1;DEF2 </summary>
            public string Defines;

            /// <summary> +define_group:GROUP1;GROUP2 </summary>
            public string DefineGroup;

            /// <summary> +dev_build:true </summary>
            public bool IsDevBuild;
            
            /// <summary> +disable_steam:true </summary>
            public bool DisableSteam;

            public override string ToString() => $"outputDir:\"{OutputDir}\" defines:\"{Defines}\" defineGroup:\"{DefineGroup}\" isDev:{IsDevBuild} defineGroup:\"{DisableSteam}\"";

            public BuildPlayerOptions GetBuildOptions()
            {
                var buildOptions = new BuildPlayerOptions();
                buildOptions.scenes = Scenes;
                buildOptions.target = BuildTarget.StandaloneWindows64;

                if (IsDevBuild)
                    buildOptions.options = BuildOptions.DetailedBuildReport | BuildOptions.Development;

                buildOptions.locationPathName = !string.IsNullOrWhiteSpace(OutputDir) ?
                    OutputDir : "..\\build_output\\arcanine";

                Log($"build output dir: {buildOptions.locationPathName}");
                return buildOptions;
            }
        }

        private static void LogError(string msg, bool asUnityLog = false)
        {
            if (asUnityLog)
                Debug.LogError(LOG_TAG + msg);
            else
                System.Console.Error.WriteLine(LOG_TAG + msg);
        }

        private static void Log(string msg, bool asUnityLog = false)
        {
            if (asUnityLog)
                Debug.Log(LOG_TAG + msg);
            else
                System.Console.WriteLine(LOG_TAG + msg);
        }
	}
}