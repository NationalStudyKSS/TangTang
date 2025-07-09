using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System;

public class HeroPassiveSkillSOGenerator
{
    private const string CSV_PATH = "Data/CSV/PassiveSkillData"; // Resources 기준, 확장자 제외
    private const string SAVE_PATH = "Assets/Resources/Data/HeroSkill/PassiveSkill";

    [MenuItem("Tools/Generate PassiveSkillData SOs")]
    public static void GeneratePassiveSkillSOs()
    {
        if (!Directory.Exists(SAVE_PATH))
            Directory.CreateDirectory(SAVE_PATH);

        var rawDataList = ReadCSVWithCSVReader(CSV_PATH);
        var groupedBySkill = rawDataList.GroupBy(d => d.ID);

        foreach (var group in groupedBySkill)
        {
            var first = group.First();

            PassiveSkillData skillData = ScriptableObject.CreateInstance<PassiveSkillData>();
            skillData.name = first.SkillName;

            // 스탯 타입별로 그룹핑
            var statGroups = group.GroupBy(d => d.EffectType);

            List<PassiveSkillLevelStat> statList = new List<PassiveSkillLevelStat>();

            foreach (var statGroup in statGroups)
            {
                PassiveSkillStatType statType = statGroup.Key;
                int maxLevel = statGroup.Max(g => g.Level);

                float[] values = new float[maxLevel];
                foreach (var d in statGroup)
                {
                    int idx = d.Level - 1;
                    values[idx] = d.Value;
                }

                PassiveSkillLevelStat levelStat = new PassiveSkillLevelStat();
                typeof(PassiveSkillLevelStat).GetField("_statType", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(levelStat, statType);
                typeof(PassiveSkillLevelStat).GetField("_levelValues", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(levelStat, values);

                statList.Add(levelStat);
            }

            var type = typeof(PassiveSkillData);
            type.GetField("_id", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(skillData, first.ID);
            type.GetField("_passiveSkillName", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(skillData, first.SkillName);
            type.GetField("_description", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(skillData, first.Description);
            type.GetField("_levelStats", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(skillData, statList.ToArray());

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

    private static List<PassiveSkillRaw> ReadCSVWithCSVReader(string path)
    {
        var result = new List<PassiveSkillRaw>();
        List<Dictionary<string, object>> rawData = CSVReader.Read(path);

        foreach (var row in rawData)
        {
            var data = new PassiveSkillRaw
            {
                ID = GetInt(row, "ID"),
                LocalizationKey = GetString(row, "LocalizationKey"),
                SkillName = GetString(row, "SkillName"),
                Level = GetInt(row, "Level"),
                EffectType = ParseStatType(GetString(row, "EffectType")),
                Value = GetFloat(row, "Value"),
                Description = GetString(row, "Description")
            };

            result.Add(data);
        }

        return result;
    }

    private static int GetInt(Dictionary<string, object> row, string key)
    {
        return row.ContainsKey(key) ? Convert.ToInt32(row[key]) : 0;
    }

    private static float GetFloat(Dictionary<string, object> row, string key)
    {
        return row.ContainsKey(key) ? Convert.ToSingle(row[key]) : 0f;
    }

    private static string GetString(Dictionary<string, object> row, string key)
    {
        return row.ContainsKey(key) ? row[key].ToString() : string.Empty;
    }

    private static PassiveSkillStatType ParseStatType(string raw)
    {
        if (Enum.TryParse(raw, true, out PassiveSkillStatType result))
            return result;

        Debug.LogWarning($"[PassiveSkillSOGenerator] Unknown EffectType: {raw}");
        return PassiveSkillStatType.ItemGetRange; // 기본값
    }
}
