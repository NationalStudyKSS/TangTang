using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 주인공 기본 능력치를 포함하는 설정 데이터 클래스
/// </summary>
[CreateAssetMenu(fileName = "HeroStatData", menuName = "GameSettings/Hero/HeroStatData")]
public class HeroStatData : ScriptableObject
{
    [SerializeField] float _maxHp;      // 기본 최대 체력
    [SerializeField] float _speed;      // 기본 이동 속력
    [SerializeField] float _baseExp;    // 기본 경험치
    [SerializeField] float _expIncrementRate; // 경험치 배수

    [Header("----- 해금 정보 -----")]
    [SerializeField] int _unlockCost;

    public int UnlockCost => _unlockCost;

    public float MaxHp => _maxHp;
    public float Speed => _speed;

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
