using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkillRaw
{
    public int ID;
    public PassiveSkillType SkillType;
    public string LocalizationKey;
    public string SkillName;
    public int Level;
    public StatName StatName;
    public float Value;
    public string Description;
    public string IconPath;
}

public static class PassiveSkillDataReader
{
    public static List<PassiveSkillRaw> ReadPassiveSkillData(string filePath)
    {
        var list = new List<PassiveSkillRaw>();

        List<Dictionary<string, object>> data = CSVReader.Read(filePath);

        foreach (var row in data)
        {
            var skill = new PassiveSkillRaw();

            skill.ID = GetInt(row, "ID");
            skill.LocalizationKey = GetString(row, "LocalizationKey");
            skill.SkillType = ParsePassiveSkillType(skill.LocalizationKey);
            skill.SkillName = GetString(row, "SkillName");
            skill.Level = GetInt(row, "Level");
            skill.StatName = ParseStatType(GetString(row, "StatName"));
            skill.Value = GetFloat(row, "Value");
            skill.Description = GetString(row, "Description");
            skill.IconPath = GetString(row, "IconPath");

            list.Add(skill);
        }

        return list;
    }

    static int GetInt(Dictionary<string, object> row, string key) => row.ContainsKey(key) ? System.Convert.ToInt32(row[key]) : 0;
    static float GetFloat(Dictionary<string, object> row, string key) => row.ContainsKey(key) ? System.Convert.ToSingle(row[key]) : 0f;
    static string GetString(Dictionary<string, object> row, string key) => row.ContainsKey(key) ? row[key].ToString() : string.Empty;
    static StatName ParseStatType(string raw)
    {
        if (System.Enum.TryParse(raw, true, out StatName result))
            return result;

        Debug.LogWarning($"[PassiveSkillDataReader] Unknown EffectType: {raw}");
        return StatName.ItemGetRange; // 기본값 또는 예외 처리
    }
    public static PassiveSkillType ParsePassiveSkillType(string key)
    {
        // 예: "_heroPassiveSkill_ironHelmet" → "ironHelmet" → "IronHelmet"
        string raw = key.Replace("_heroPassiveSkill_", "");
        raw = char.ToUpper(raw[0]) + raw.Substring(1); // 첫 글자 대문자

        if (Enum.TryParse(raw, out PassiveSkillType result))
            return result;

        Debug.LogWarning($"[SOGenerator] PassiveSkillType 변환 실패: {key}");
        return default;
    }
}
