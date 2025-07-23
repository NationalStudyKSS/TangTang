using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class HeroStatSOGenerator : MonoBehaviour
{
    private const string CSV_PATH = "Data/CSV/HeroStatData";
    private const string SAVE_PATH = "Assets/Resources/Data/Hero"; // 저장될 폴더

    [MenuItem("Tools/Generate HeroStatData SOs")]
    public static void GenerateSOs()
    {
        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }

        List<HeroDataRaw> heroList = HeroStatDataReader.ReadHeroData(CSV_PATH);

        foreach (var hero in heroList)
        {
            HeroStatData asset = ScriptableObject.CreateInstance<HeroStatData>();

            asset.name = hero.Name;
            SetStats(asset, hero);

            string assetPath = $"{SAVE_PATH}/{hero.LocalizationKey}.asset";
            // 기존 SO가 있으면 삭제
            if (File.Exists(assetPath))
            {
                AssetDatabase.DeleteAsset(assetPath);
            }
            AssetDatabase.CreateAsset(asset, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("HeroStatData SO 생성 완료!");
    }

    private static void SetStats(HeroStatData asset, HeroDataRaw data)
    {
        var soType = typeof(HeroStatData);
        soType.GetField("_baseMaxHp", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(asset, data.BaseMaxHp);
        soType.GetField("_hpGrowthRate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(asset, data.HpGrowthRate);

        soType.GetField("_baseDamage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(asset, data.BaseDamage);
        soType.GetField("_damageGrowthRate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(asset, data.DamageGrowthRate);

        soType.GetField("_baseMoveSpeed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(asset, data.BaseMoveSpeed);
        soType.GetField("_moveSpeedGrowthRate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(asset, data.MoveSpeedGrowthRate);

        soType.GetField("_baseItemGetRange", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(asset, data.BaseItemGetRange);
        soType.GetField("_itemGetRangeGrowthRate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(asset, data.ItemGetRangeGrowthRate);

        soType.GetField("_baseExpToLevelUp", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(asset, data.BaseExpToLevelUp);
        soType.GetField("_expRequiredGrowthRate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(asset, data.ExpRequiredGrowthRate);

        // 기본값으로 설정할 수 있는 부분들
        soType.GetField("_maxLevel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(asset, 99);
        soType.GetField("_baseExpGainRate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(asset, 1f);
    }
}
