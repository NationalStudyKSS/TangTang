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
    [SerializeField] float _baseMaxHp;          // 기본 최대 체력
    [SerializeField] float _baseDamage;         // 기본 공격력
    [SerializeField] float _baseSpeed;          // 기본 이동 속력
    [SerializeField] float _baseExpRequired;    // 기본 레벨업에 필요한 경험치
    [SerializeField] int _baseMaxLevel;         // 기본 최대 레벨
    [SerializeField] float _baseExpIncrementRate;   // 기본 경험치 획득률

    [Header("----- 레벨당 증가하는 스탯 관련 -----")]
    // ex: 1.1이라면 1레벨마다 10% 증가
    [SerializeField] float _hpIncrementRate;             // 체력 배수
    [SerializeField] float _damageIncrementRate;         // 공격력 배수
    [SerializeField] float _speedIncrementRate;          // 이동 속력 배수
    [SerializeField] float _expRequiredIncrementRate;    // 레벨업에 필요한 경험치 증가 배수

    public int BaseMaxLevel => _baseMaxLevel;
    public float BaseExpIncrementRate => _baseExpIncrementRate;

    /// <summary>
    /// 레벨에 따른 최대 체력을 반환해 주는 함수
    /// </summary>
    /// <param name="level">레벨</param>
    /// <returns></returns>
    public float BaseMaxHp(int level)
    {
        if (level <= 0)
            return _baseMaxHp;
        return _baseMaxHp * Mathf.Pow(_hpIncrementRate, level - 1);
    }

    /// <summary>
    /// 레벨에 따른 공격력을 반환해 주는 함수
    /// </summary>
    /// <param name="level">레벨</param>
    /// <returns></returns>
    public float BaseDamage(int level)
    {
        if (level <= 0)
            return _baseDamage;
        return _baseDamage * Mathf.Pow(_damageIncrementRate, level - 1);
    }

    /// <summary>
    /// 레벨에 따른 이동 속력을 반환해 주는 함수
    /// </summary>
    /// <param name="level">레벨</param>
    /// <returns></returns>
    public float BaseSpeed(int level)
    {
        if (level <= 0)
            return _baseSpeed;
        return _baseSpeed * Mathf.Pow(_speedIncrementRate, level - 1);
    }

    /// <summary>
    /// 레벨에 따른 필요 경험치를 반환해 주는 함수
    /// (주인공 레벨은 1부터 시작한다고 가정)
    /// </summary>
    /// <param name="level">레벨</param>
    /// <returns></returns>
    public float BaseExpRequired(int level)
    {
        if (level <= 0)
            return _baseExpRequired;

        return _baseExpRequired * Mathf.Pow(_expRequiredIncrementRate, level - 1);
    }
}
