using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Android;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Tools.Editor
{
    public class BuildTeamCity : MonoBehaviour
    {
        private const string BuildLocationAndroid = "Builds/Android/";
        private const string BuildLocationIOS = "Builds/IOS/";

        [MenuItem("Tools/Build/BuildAndroid")]
        public static void BuildAndroid()
        {
            LogPaths();
            Build(BuildTarget.Android, BuildLocationAndroid + GetAPKName());
        }

        [MenuItem("Tools/Build/BuildIOS")]
        public static void BuildIOS()
        {
            Build(BuildTarget.iOS, BuildLocationIOS);
        }

        private static void LogPaths()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Android paths:");
            sb.AppendLine($"jdkRootPath = {AndroidExternalToolsSettings.jdkRootPath}");
            sb.AppendLine($"sdkRootPath = {AndroidExternalToolsSettings.sdkRootPath}");
            sb.AppendLine($"ndkRootPath = {AndroidExternalToolsSettings.ndkRootPath}");
            sb.AppendLine($"gradlePath = {AndroidExternalToolsSettings.gradlePath}");
            sb.AppendLine($"stopGradleDaemonsOnExit = {AndroidExternalToolsSettings.stopGradleDaemonsOnExit}");
            sb.AppendLine($"maxJvmHeapSize = {AndroidExternalToolsSettings.maxJvmHeapSize}");

            Debug.LogError(sb.ToString());
        }

        private static void Build(BuildTarget target, string locationPathName)
        {
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

            buildPlayerOptions.scenes = EditorBuildSettings.scenes.Select(scene => scene.path).ToArray();

            buildPlayerOptions.locationPathName = locationPathName;
            buildPlayerOptions.target = target;
            buildPlayerOptions.options = BuildOptions.None;

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"BuildTeamCity BuildResult.Succeeded\n{BuildSummaryToString(summary)}");
            }
            else if (summary.result == BuildResult.Failed)
            {
                Debug.Log($"BuildTeamCity BuildResult.Failed\n{BuildSummaryToString(summary)}");
            }
        }


        private static string GetAPKName()
        {
            string projectName = PlayerSettings.productName;
            string version = PlayerSettings.bundleVersion;
            string buildNumber = PlayerSettings.Android.bundleVersionCode.ToString();
            return $"{projectName}_{version}_{buildNumber}.apk";
        }

        private static string BuildSummaryToString(BuildSummary summary)
        {
            var sb = new StringBuilder();

            sb.AppendLine("--------------------------------");
            sb.AppendLine("BuildSummary info:");
            OneEntry("summary.result", summary.result.ToString());
            OneEntry("summary.outputPath", summary.outputPath);
            OneEntry("summary.totalWarnings", summary.totalWarnings.ToString());
            OneEntry("summary.totalErrors", summary.totalErrors.ToString());
            OneEntry("summary.totalSize", summary.totalSize.ToString());
            OneEntry("summary.platform", summary.platform.ToString());
            OneEntry("summary.platformGroup", summary.platformGroup.ToString());
            sb.AppendLine("--------------------------------");

            return sb.ToString();

            void OneEntry(string header, string value) => sb.AppendLine($"{header} = {value}");
        }
    }
}