using System.Collections.Generic;
using System.IO;
using Core.Infrastructures.AssetManagement;
using Core.Logic.Battle;
using Core.Logic.Enemy;
using Core.Logic.Level;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build.AnalyzeRules;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

namespace Core.Editor.ContentSetup
{
    public static class AddressablesSetup
    {
        private const string ROOT_FOLDER = "Assets/AddressableAssets";
        private const string CORE_FOLDER = ROOT_FOLDER + "/Core";
        private const string UI_FOLDER = ROOT_FOLDER + "/UI";
        private const string ENEMIES_FOLDER = ROOT_FOLDER + "/Enemies";
        private const string LEVELS_FOLDER = ROOT_FOLDER + "/Levels";
        private const string CORE_GROUP = "Core";
        private const string UI_GROUP = "UI";
        private const string ENEMIES_GROUP = "Enemies";
        private const string SHARED_GROUP = "Shared";
        private const string LEVEL_LABEL_PREFIX = "level_";

        [MenuItem("Tools/Addressables/Setup")]
        public static void Run()
        {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.GetSettings(true);

            SetupFolder(settings, CORE_FOLDER, GetOrCreateGroup(settings, CORE_GROUP), AssetLabel.CORE);
            SetupFolder(settings, UI_FOLDER, GetOrCreateGroup(settings, UI_GROUP), AssetLabel.UI);
            SetupLevels(settings);
            SetupShared(settings);

            AssetDatabase.SaveAssets();
            Debug.Log("AddressablesSetup: done");
        }

        private static void SetupFolder(AddressableAssetSettings settings, string folder,
            AddressableAssetGroup group, string label)
        {
            foreach (string guid in AssetDatabase.FindAssets("t:Prefab", new[] { folder }))
            {
                CreateEntry(settings, guid, group, label);
            }
        }

        private static void SetupLevels(AddressableAssetSettings settings)
        {
            var enemyLabels = new Dictionary<string, HashSet<string>>();

            foreach (string levelFolder in AssetDatabase.GetSubFolders(LEVELS_FOLDER))
            {
                string groupName = Path.GetFileName(levelFolder);
                string label = LEVEL_LABEL_PREFIX + groupName.Substring(groupName.LastIndexOf('_') + 1);
                AddressableAssetGroup group = GetOrCreateGroup(settings, groupName);

                foreach (string guid in AssetDatabase.FindAssets("t:Prefab", new[] { levelFolder }))
                {
                    CreateEntry(settings, guid, group, label);
                    CollectEnemies(guid, label, enemyLabels);
                }
            }

            SetupEnemies(settings, enemyLabels);
        }

        private static void CollectEnemies(string levelGuid, string label,
            Dictionary<string, HashSet<string>> enemyLabels)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(levelGuid));
            if(prefab == null || prefab.GetComponent<LevelComponent>() == null)
                return;

            foreach (BattlePlace battlePlace in prefab.GetComponentsInChildren<BattlePlace>(true))
            {
                foreach (EnemyConfig enemy in battlePlace.Enemies)
                {
                    if(enemy == null || !enemy.Prefab.RuntimeKeyIsValid())
                        continue;

                    if (!enemyLabels.TryGetValue(enemy.Prefab.AssetGUID, out HashSet<string> labels))
                    {
                        labels = new HashSet<string>();
                        enemyLabels.Add(enemy.Prefab.AssetGUID, labels);
                    }

                    labels.Add(label);
                }
            }
        }

        private static void SetupEnemies(AddressableAssetSettings settings,
            Dictionary<string, HashSet<string>> enemyLabels)
        {
            AddressableAssetGroup group = GetOrCreateGroup(settings, ENEMIES_GROUP);

            foreach (string guid in AssetDatabase.FindAssets("t:Prefab", new[] { ENEMIES_FOLDER }))
            {
                AddressableAssetEntry entry = CreateEntry(settings, guid, group, null);
                foreach (string label in new List<string>(entry.labels))
                {
                    if (label.StartsWith(LEVEL_LABEL_PREFIX))
                    {
                        entry.SetLabel(label, false);
                    }
                }

                if (!enemyLabels.TryGetValue(guid, out HashSet<string> labels))
                {
                    Debug.LogWarning($"AddressablesSetup: enemy {entry.AssetPath} is not used on any level");
                    continue;
                }

                foreach (string label in labels)
                {
                    entry.SetLabel(label, true, true);
                }
            }
        }

        private static void SetupShared(AddressableAssetSettings settings)
        {
            AddressableAssetGroup group = GetOrCreateGroup(settings, SHARED_GROUP);
            var rule = new CheckBundleDupeDependencies();

            foreach (AnalyzeRule.AnalyzeResult result in rule.RefreshAnalysis(settings))
            {
                string path = result.resultName.Substring(result.resultName.LastIndexOf(':') + 1);
                string guid = AssetDatabase.AssetPathToGUID(path);
                if(string.IsNullOrEmpty(guid))
                    continue;

                settings.CreateOrMoveEntry(guid, group);
            }

            rule.ClearAnalysis();
        }

        private static AddressableAssetEntry CreateEntry(AddressableAssetSettings settings, string guid,
            AddressableAssetGroup group, string label)
        {
            AddressableAssetEntry entry = settings.CreateOrMoveEntry(guid, group);
            entry.address = Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(guid));

            if (label is not null)
            {
                entry.SetLabel(label, true, true);
            }

            return entry;
        }

        private static AddressableAssetGroup GetOrCreateGroup(AddressableAssetSettings settings, string name)
        {
            AddressableAssetGroup group = settings.FindGroup(name);
            if (group == null)
            {
                group = settings.CreateGroup(name, false, false, true, null,
                    typeof(BundledAssetGroupSchema), typeof(ContentUpdateGroupSchema));
            }

            var bundledSchema = group.GetSchema<BundledAssetGroupSchema>();
            bundledSchema.BuildPath.SetVariableByName(settings, AddressableAssetSettings.kLocalBuildPath);
            bundledSchema.LoadPath.SetVariableByName(settings, AddressableAssetSettings.kLocalLoadPath);
            bundledSchema.BundleMode = BundledAssetGroupSchema.BundlePackingMode.PackTogether;

            return group;
        }
    }
}
