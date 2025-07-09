using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkillRaw
{
    public int ID;
    public string LocalizationKey;
    public string SkillName;
    public int Level;
    public PassiveSkillStatType EffectType;
    public float Value;
    public string Description;
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
            skill.SkillName = GetString(row, "SkillName");
            skill.Level = GetInt(row, "Level");
            skill.EffectType = ParseStatType(GetString(row, "EffectType"));
            skill.Value = GetFloat(row, "Value");
            skill.Description = GetString(row, "Description");

            list.Add(skill);
        }

        return list;
    }

    static int GetInt(Dictionary<string, object> row, string key) => row.ContainsKey(key) ? System.Convert.ToInt32(row[key]) : 0;
    static float GetFloat(Dictionary<string, object> row, string key) => row.ContainsKey(key) ? System.Convert.ToSingle(row[key]) : 0f;
    static string GetString(Dictionary<string, object> row, string key) => row.ContainsKey(key) ? row[key].ToString() : string.Empty;
    static PassiveSkillStatType ParseStatType(string raw)
    {
        if (System.Enum.TryParse(raw, true, out PassiveSkillStatType result))
            return result;

        Debug.LogWarning($"[PassiveSkillDataReader] Unknown EffectType: {raw}");
        return PassiveSkillStatType.ItemGetRange; // 기본값 또는 예외 처리
    }

}
