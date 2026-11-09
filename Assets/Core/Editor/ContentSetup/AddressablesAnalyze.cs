using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build.AnalyzeRules;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace Core.Editor.ContentSetup
{
    public static class AddressablesAnalyze
    {
        [MenuItem("Tools/Addressables/Analyze")]
        public static void Run()
        {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

            Analyze(settings, new CheckBundleDupeDependencies());
            Analyze(settings, new CheckResourcesDupeDependencies());
        }

        private static void Analyze(AddressableAssetSettings settings, AnalyzeRule rule)
        {
            List<AnalyzeRule.AnalyzeResult> results = rule.RefreshAnalysis(settings);
            Debug.Log($"AddressablesAnalyze: {rule.ruleName} -> {results.Count} result(s)");

            foreach (AnalyzeRule.AnalyzeResult result in results)
            {
                Debug.Log($"AddressablesAnalyze: [{result.severity}] {result.resultName}");
            }

            rule.ClearAnalysis();
        }
    }
}
