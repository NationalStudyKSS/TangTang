using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HeroDataRaw
{
    public int ID;
    public string LocalizationKey;
    public string Name;

    public float BaseMaxHp;
    public float HpGrowthRate;

    public float BaseDamage;
    public float DamageGrowthRate;

    public float BaseMoveSpeed;
    public float MoveSpeedGrowthRate;

    public float BaseItemGetRange;
    public float ItemGetRangeGrowthRate;

    public float BaseExpToLevelUp;
    public float ExpRequiredGrowthRate;
}

public static class HeroStatDataReader
{
    public static List<HeroDataRaw> ReadHeroData(string filePath)
    {
        var rawList = new List<HeroDataRaw>();

        List<Dictionary<string, object>> data = CSVReader.Read(filePath);

        foreach (var row in data)
        {
            HeroDataRaw hero = new HeroDataRaw();

            hero.ID = GetInt(row, "ID");
            hero.LocalizationKey = GetString(row, "LocalizationKey");
            hero.Name = GetString(row, "Name");

            hero.BaseMaxHp = GetFloat(row, "BaseMaxHp");
            hero.HpGrowthRate = GetFloat(row, "HpGrowthRate");

            hero.BaseDamage = GetFloat(row, "BaseDamage");
            hero.DamageGrowthRate = GetFloat(row, "DamageGrowthRate");

            hero.BaseMoveSpeed = GetFloat(row, "BaseMoveSpeed");
            hero.MoveSpeedGrowthRate = GetFloat(row, "MoveSpeedGrowthRate");

            hero.BaseItemGetRange = GetFloat(row, "BaseItemGetRange");
            hero.ItemGetRangeGrowthRate = GetFloat(row, "ItemGetRangeGrowthRate");

            hero.BaseExpToLevelUp = GetFloat(row, "BaseExpToLevelUp");
            hero.ExpRequiredGrowthRate = GetFloat(row, "ExpRequiredGrowthRate");

            rawList.Add(hero);
        }

        return rawList;
    }

    static int GetInt(Dictionary<string, object> row, string key)
    {
        return row.ContainsKey(key) ? System.Convert.ToInt32(row[key]) : 0;
    }

    static float GetFloat(Dictionary<string, object> row, string key)
    {
        return row.ContainsKey(key) ? System.Convert.ToSingle(row[key]) : 0f;
    }

    static string GetString(Dictionary<string, object> row, string key)
    {
        return row.ContainsKey(key) ? row[key].ToString() : string.Empty;
    }
}
