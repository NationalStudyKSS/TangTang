using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 주인공 기본 능력치를 포함하는 설정 데이터 클래스
/// </summary>
[CreateAssetMenu(fileName = "HeroStatData", menuName = "GameSettings/Hero/HeroStatData")]
public class HeroStatData : ScriptableObject
{
    [Header("----- 주인공 기본 스탯 -----")]
    [SerializeField] float _baseMaxHp;                // 기본 최대 체력
    [SerializeField] float _baseDamage;               // 기본 공격력
    [SerializeField] float _baseMoveSpeed;            // 기본 이동 속력
    [SerializeField] float _baseItemGetRange;         // 기본 아이템 획득 범위
    [SerializeField] float _baseExpToLevelUp;         // 기본 레벨업 필요 경험치
    [SerializeField] int _maxLevel;                    // 최대 레벨
    [SerializeField] float _baseExpGainRate;          // 경험치 획득률

    [Header("----- 레벨당 증가하는 스탯 관련 -----")]
    [SerializeField] float _hpGrowthRate;             // 체력 성장률 (예: 1.1 = 10% 증가)
    [SerializeField] float _damageGrowthRate;         // 공격력 성장률
    [SerializeField] float _moveSpeedGrowthRate;      // 이동 속도 성장률
    [SerializeField] float _itemGetRangeGrowthRate;   // 아이템 획득 범위 성장률
    [SerializeField] float _expRequiredGrowthRate;    // 레벨업 경험치 증가율

    public int MaxLevel => _maxLevel;
    public float BaseExpGainRate => _baseExpGainRate;

    /// <summary>
    /// 레벨에 따른 최대 체력을 반환해 주는 함수
    /// </summary>
    public float GetMaxHp(int level)
    {
        if (level <= 0)
            return _baseMaxHp;
        return _baseMaxHp * Mathf.Pow(_hpGrowthRate, level - 1);
    }

    /// <summary>
    /// 레벨에 따른 공격력을 반환해 주는 함수
    /// </summary>
    public float GetDamage(int level)
    {
        if (level <= 0)
            return _baseDamage;
        return _baseDamage * Mathf.Pow(_damageGrowthRate, level - 1);
    }

    /// <summary>
    /// 레벨에 따른 이동 속력을 반환해 주는 함수
    /// </summary>
    public float GetMoveSpeed(int level)
    {
        if (level <= 0)
            return _baseMoveSpeed;
        return _baseMoveSpeed * Mathf.Pow(_moveSpeedGrowthRate, level - 1);
    }

    /// <summary>
    /// 레벨에 따른 아이템 획득 범위를 반환해 주는 함수
    /// </summary>
    public float GetItemGetRange(int level)
    {
        if (level <= 0)
            return _baseItemGetRange;
        return _baseItemGetRange * Mathf.Pow(_itemGetRangeGrowthRate, level - 1);
    }

    /// <summary>
    /// 레벨에 따른 필요 경험치를 반환해 주는 함수
    /// </summary>
    public float GetExpRequired(int level)
    {
        if (level <= 0)
            return _baseExpToLevelUp;
        return _baseExpToLevelUp * Mathf.Pow(_expRequiredGrowthRate, level - 1);
    }
}
