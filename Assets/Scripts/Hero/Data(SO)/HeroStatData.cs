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
    [SerializeField] float _maxHp;      // 기본 최대 체력
    [SerializeField] float _damage;     // 기본 공격력
    [SerializeField] float _speed;      // 기본 이동 속력
    [SerializeField] float _baseExp;    // 기본 경험치
    [SerializeField] int _maxLevel;     // 최대 레벨

    [Header("----- 레벨당 증가하는 스탯 관련 -----")]
    // ex: 1.1이라면 1레벨마다 10% 증가
    [SerializeField] float _hpIncrementRate; // 체력 배수
    [SerializeField] float _damageIncrementRate; // 공격력 배수
    [SerializeField] float _speedIncrementRate; // 이동 속력 배수
    [SerializeField] float _expIncrementRate; // 경험치 배수

    public int MaxLevel => _maxLevel;

    /// <summary>
    /// 레벨에 따른 최대 체력을 반환해 주는 함수
    /// </summary>
    /// <param name="level">레벨</param>
    /// <returns></returns>
    public float MaxHp(int level)
    {
        if (level <= 0)
            return _maxHp;
        return _maxHp * Mathf.Pow(_hpIncrementRate, level - 1);
    }

    /// <summary>
    /// 레벨에 따른 공격력을 반환해 주는 함수
    /// </summary>
    /// <param name="level">레벨</param>
    /// <returns></returns>
    public float Damage(int level)
    {
        if (level <= 0)
            return _damage;
        return _damage * Mathf.Pow(_damageIncrementRate, level - 1);
    }

    /// <summary>
    /// 레벨에 따른 이동 속력을 반환해 주는 함수
    /// </summary>
    /// <param name="level">레벨</param>
    /// <returns></returns>
    public float Speed(int level)
    {
        if (level <= 0)
            return _speed;
        return _speed * Mathf.Pow(_speedIncrementRate, level - 1);
    }

    /// <summary>
    /// 레벨에 따른 필요 경험치를 반환해 주는 함수
    /// (주인공 레벨은 1부터 시작한다고 가정)
    /// </summary>
    /// <param name="level">레벨</param>
    /// <returns></returns>
    public float GetExp(int level)
    {
        if (level <= 0)
            return _baseExp;

        return _baseExp * Mathf.Pow(_expIncrementRate, level - 1);
    }
}
