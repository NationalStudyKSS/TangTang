using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkillRaw
{
    public int ID;
    public string LocalizationKey;
    public string SkillName;
    public int Level;
    public float DamageRate;
    public float BulletSpeed;
    public float ShootingRange;
    public float BulletCount;
    public float BulletDuration;
    public float CoolTime;
    public float FireDelay;
    public float AttackCount;
    public float BulletRange;
    public float RotSpeed;
    public float DamageDelay;
    public bool IsUnique;
    public string SpawnPosition;
    public string Description;
    public string IconPath;
    public string PrefabPath;
}

public static class ActiveSkillDataReader
{
    public static List<ActiveSkillRaw> ReadActiveSkillData(string filePath)
    {
        var list = new List<ActiveSkillRaw>();

        List<Dictionary<string, object>> data = CSVReader.Read(filePath);

        foreach (var row in data)
        {
            var skill = new ActiveSkillRaw();

            skill.ID = GetInt(row, "ID");
            skill.LocalizationKey = GetString(row, "LocalizationKey");
            skill.SkillName = GetString(row, "SkillName");
            skill.Level = GetInt(row, "Level");

            skill.DamageRate = GetFloat(row, "DamageRate");
            skill.BulletSpeed = GetFloat(row, "BulletSpeed");
            skill.ShootingRange = GetFloat(row, "ShootingRange");
            skill.BulletCount = GetFloat(row, "BulletCount");
            skill.BulletDuration = GetFloat(row, "BulletDuration");
            skill.CoolTime = GetFloat(row, "CoolTime");
            skill.FireDelay = GetFloat(row, "FireDelay");
            skill.AttackCount = GetFloat(row, "AttackCount");
            skill.BulletRange = GetFloat(row, "BulletRange");
            skill.RotSpeed = GetFloat(row, "RotSpeed");
            skill.DamageDelay = GetFloat(row, "DamageDelay");

            skill.IsUnique = GetBool(row, "IsUnique");
            skill.SpawnPosition = GetString(row, "SpawnPosition");
            skill.Description = GetString(row, "Description");
            skill.IconPath = GetString(row, "IconPath");
            skill.PrefabPath = GetString(row, "PrefabPath");

            list.Add(skill);
        }

        return list;
    }

    static int GetInt(Dictionary<string, object> row, string key) => row.ContainsKey(key) ? System.Convert.ToInt32(row[key]) : 0;
    static float GetFloat(Dictionary<string, object> row, string key) => row.ContainsKey(key) ? System.Convert.ToSingle(row[key]) : 0f;
    static string GetString(Dictionary<string, object> row, string key) => row.ContainsKey(key) ? row[key].ToString() : string.Empty;
    static bool GetBool(Dictionary<string, object> row, string key)
    {
        if (!row.ContainsKey(key)) return false;
        string val = row[key].ToString().ToLower();
        return val == "true" || val == "1" || val == "yes";
    }
}
