using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System;
using System.Drawing;

public class HeroPassiveSkillSOGenerator
{
    private const string CSV_PATH = "Data/CSV/PassiveSkillData"; // Resources 기준, 확장자 제외
    private const string SAVE_PATH = "Assets/Resources/Data/HeroSkill/PassiveSkill";

    [MenuItem("Tools/Generate PassiveSkillData SOs")]
    public static void GeneratePassiveSkillSOs()
    {
        if (!Directory.Exists(SAVE_PATH))
            Directory.CreateDirectory(SAVE_PATH);

        var rawDataList = PassiveSkillDataReader.ReadPassiveSkillData(CSV_PATH);
        var groupedBySkill = rawDataList.GroupBy(d => d.ID);

        foreach (var group in groupedBySkill)
        {
            var first = group.First();

            PassiveSkillData skillData = ScriptableObject.CreateInstance<PassiveSkillData>();
            skillData.name = first.SkillName;

            // 스탯 타입별로 그룹핑
            var statGroups = group.GroupBy(d => d.StatName);

            List<PassiveSkillLevelStat> statList = new List<PassiveSkillLevelStat>();

            foreach (var statGroup in statGroups)
            {
                StatName statName = statGroup.Key;
                int maxLevel = statGroup.Max(g => g.Level);

                float[] values = new float[maxLevel];
                foreach (var d in statGroup)
                {
                    int idx = d.Level - 1;
                    values[idx] = d.Value;
                }

                PassiveSkillLevelStat levelStat = new PassiveSkillLevelStat();
                typeof(PassiveSkillLevelStat).GetField("_statName", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(levelStat, statName);
                typeof(PassiveSkillLevelStat).GetField("_levelValues", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(levelStat, values);

                statList.Add(levelStat);
            }
            Sprite icon = null;
            if (!string.IsNullOrEmpty(first.IconPath))
                icon = ResourceLoader.LoadSprite(first.IconPath);

            var type = typeof(PassiveSkillData);
            type.GetField("_id", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(skillData, first.ID);
            type.GetField("_passiveSkillName", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(skillData, first.SkillName);
            type.GetField("_description", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(skillData, first.Description);
            type.GetField("_levelStats", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(skillData, statList.ToArray());
            type.GetField("_iconSprite", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(skillData, icon);

            PassiveSkillType parsedType = PassiveSkillDataReader.ParsePassiveSkillType(first.LocalizationKey);
            type.GetField("_skillType", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(skillData, parsedType);

            // maxLevel은 Initialize()에서 자동 계산됨
            skillData.Initialize();

            string assetPath = $"{SAVE_PATH}/{first.LocalizationKey}.asset";
            if (File.Exists(assetPath)) AssetDatabase.DeleteAsset(assetPath);
            AssetDatabase.CreateAsset(skillData, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("PassiveSkillData SO 생성 완료!");
    }
}
