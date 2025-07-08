using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 무기 스탯 종류(타입) 정의
/// </summary>
public enum ActiveSkillStatType
{
    DamageRate,      // 영웅으로부터 받을 데미지 계수
    BulletSpeed,     // 총알 속력(무기 회전 속력, 날아가는 속력)
    ShootingRange,   // 사정거리(회전 반경, 타겟팅 범위)
    BulletCount,     // 총알 수(발사 시 생성되는 총알의 개수)
    BulletDuration,  // 총알 지속 시간(계속 남아있으면 안되니까)
    CoolTime,        // 총알 발사 간격(쿨타임)
    FireDelay,       // 연발 시 총알 사이 간격 시간
    AttackCount,     // 한 총알의 공격 횟수(적 관통을 얼마나 할지)
    BulletRange,     // 총알 범위
    RotSpeed,        // 총알 자체의 회전 속력}
    DamageDelay,     // 데미지 입히는 시간 간격
}

/// <summary>
/// 하나의 스탯 종류(타입)에 대해 레벨별 수치를 저장하는 클래스
/// 예: Damage {10, 20, 30, 40, 50}
/// </summary>
[System.Serializable]   // System.Serializable은 객체의 변수들을 인스펙터뷰에서 설정하게 해 주는 기능
public class ActiveSkillLevelStat
{
    // 어떤 스텟에 대한 데이터인지(ex. Damage, BulletSpeed 등)
    [SerializeField] ActiveSkillStatType _statType;

    // 각 레벨별 수치 배열
    [SerializeField] float[] _levelValues;

    public ActiveSkillStatType StatType => _statType;
    public int MaxLevel => _levelValues.Length - 1; // 최대 레벨은 배열 길이 - 1

    /// <summary>
    /// 특정 레벨에 해당하는 값을 반환하는 함수
    /// 음수 레벨이면 0, 최대레벨을 초과하는 레벨이면 최대 레벨의 값을 반환한다.
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public float GetValue(int level)
    {
        if (level < 0)
        {
            // 음수 레벨이면 0 반환
            return 0f;
        }
        // 레벨이 최대 레벨을 초과하면 배열 범위 끝번호로 한정
        if (level >= _levelValues.Length)
        {
            level = _levelValues.Length - 1; // 최대 레벨로 조정
            // 최대 레벨을 초과하는 레벨이면 최대 레벨의 값을 반환
            return _levelValues[level];
        }

        // 해당 레벨의 수치 반환
        return _levelValues[level];
    }
}
