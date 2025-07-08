using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

public class ActiveSkillSOGenerator
{
    private const string CSV_PATH = "Data/CSV/ActiveSkillData"; // Resources 폴더 기준 (확장자 제외)
    private const string SAVE_PATH = "Assets/Resources/Data/HeroSkill/ActiveSkill";

    [MenuItem("Tools/Generate ActiveSkillData SOs")]
    public static void GenerateActiveSkillSOs()
    {
        if (!Directory.Exists(SAVE_PATH))
            Directory.CreateDirectory(SAVE_PATH);

        var rawDataList = ReadCSVWithCSVReader(CSV_PATH);

        var groupedBySkill = rawDataList.GroupBy(d => d.ID);

        foreach (var group in groupedBySkill)
        {
            var first = group.First();

            ActiveSkillData skillData = ScriptableObject.CreateInstance<ActiveSkillData>();
            
            skillData.name = first.SkillName;

            int maxLevel = group.Max(g => g.Level);

            int id = first.ID;
            float[] damageRates = new float[maxLevel];
            float[] bulletSpeeds = new float[maxLevel];
            float[] shootingRanges = new float[maxLevel];
            float[] bulletCounts = new float[maxLevel];
            float[] bulletDurations = new float[maxLevel];
            float[] coolTimes = new float[maxLevel];
            float[] fireDelays = new float[maxLevel];
            float[] attackCounts = new float[maxLevel];
            float[] bulletRanges = new float[maxLevel];
            float[] rotSpeeds = new float[maxLevel];
            float[] damageDelays = new float[maxLevel];

            foreach (var d in group)
            {
                int idx = d.Level - 1;

                damageRates[idx] = d.DamageRate;
                bulletSpeeds[idx] = d.BulletSpeed;
                shootingRanges[idx] = d.ShootingRange;
                bulletCounts[idx] = d.BulletCount;
                bulletDurations[idx] = d.BulletDuration;
                coolTimes[idx] = d.CoolTime;
                fireDelays[idx] = d.FireDelay;
                attackCounts[idx] = d.AttackCount;
                bulletRanges[idx] = d.BulletRange;
                rotSpeeds[idx] = d.RotSpeed;
                damageDelays[idx] = d.DamageDelay;
            }

            List<ActiveSkillLevelStat> levelStats = new List<ActiveSkillLevelStat>
            {
                CreateLevelStat(ActiveSkillStatType.DamageRate, damageRates),
                CreateLevelStat(ActiveSkillStatType.BulletSpeed, bulletSpeeds),
                CreateLevelStat(ActiveSkillStatType.ShootingRange, shootingRanges),
                CreateLevelStat(ActiveSkillStatType.BulletCount, bulletCounts),
                CreateLevelStat(ActiveSkillStatType.BulletDuration, bulletDurations),
                CreateLevelStat(ActiveSkillStatType.CoolTime, coolTimes),
                CreateLevelStat(ActiveSkillStatType.FireDelay, fireDelays),
                CreateLevelStat(ActiveSkillStatType.AttackCount, attackCounts),
                CreateLevelStat(ActiveSkillStatType.BulletRange, bulletRanges),
                CreateLevelStat(ActiveSkillStatType.RotSpeed, rotSpeeds),
                CreateLevelStat(ActiveSkillStatType.DamageDelay, damageDelays)
            };

            var skillType = typeof(ActiveSkillData);
            skillType.GetField("_activeSkillName", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(skillData, first.SkillName);
            skillType.GetField("_description", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(skillData, first.Description);
            skillType.GetField("_id", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(skillData, id);
            skillType.GetField("_levelStats", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(skillData, levelStats.ToArray());
            skillType.GetField("_maxLevel", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(skillData, maxLevel);

            skillData.Initialize();

            string assetPath = $"{SAVE_PATH}/{first.LocalizationKey}.asset";
            if (File.Exists(assetPath)) AssetDatabase.DeleteAsset(assetPath);
            AssetDatabase.CreateAsset(skillData, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("ActiveSkillData SO 생성 완료!");
    }

    private static ActiveSkillLevelStat CreateLevelStat(ActiveSkillStatType type, float[] values)
    {
        var stat = new ActiveSkillLevelStat();
        typeof(ActiveSkillLevelStat).GetField("_statType", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(stat, type);
        typeof(ActiveSkillLevelStat).GetField("_levelValues", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(stat, values);
        return stat;
    }

    private static List<ActiveSkillRaw> ReadCSVWithCSVReader(string path)
    {
        var result = new List<ActiveSkillRaw>();
        List<Dictionary<string, object>> rawData = CSVReader.Read(path);

        foreach (var row in rawData)
        {
            var data = new ActiveSkillRaw();

            data.ID = GetInt(row, "ID");
            data.LocalizationKey = GetString(row, "LocalizationKey");
            data.SkillName = GetString(row, "SkillName");
            data.Level = GetInt(row, "Level");

            data.DamageRate = GetFloat(row, "DamageRate");
            data.BulletSpeed = GetFloat(row, "BulletSpeed");
            data.ShootingRange = GetFloat(row, "ShootingRange");
            data.BulletCount = GetFloat(row, "BulletCount");
            data.BulletDuration = GetFloat(row, "BulletDuration");
            data.CoolTime = GetFloat(row, "CoolTime");
            data.FireDelay = GetFloat(row, "FireDelay");
            data.AttackCount = GetFloat(row, "AttackCount");
            data.BulletRange = GetFloat(row, "BulletRange");
            data.RotSpeed = GetFloat(row, "RotSpeed");
            data.DamageDelay = GetFloat(row, "DamageDelay");

            data.IsUnique = GetString(row, "IsUnique").ToLower() == "true";
            data.SpawnPosition = GetString(row, "SpawnPosition");
            data.Description = GetString(row, "Description");

            result.Add(data);
        }

        return result;
    }

    private static int GetInt(Dictionary<string, object> row, string key)
    {
        return row.ContainsKey(key) ? System.Convert.ToInt32(row[key]) : 0;
    }

    private static float GetFloat(Dictionary<string, object> row, string key)
    {
        return row.ContainsKey(key) ? System.Convert.ToSingle(row[key]) : 0f;
    }

    private static string GetString(Dictionary<string, object> row, string key)
    {
        return row.ContainsKey(key) ? row[key].ToString() : string.Empty;
    }
}
