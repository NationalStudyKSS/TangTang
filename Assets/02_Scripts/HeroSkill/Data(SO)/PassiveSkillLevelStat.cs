using System;
using UnityEngine;

/// <summary>
/// 어떤 스탯을 증가시킬 것인지 정의하는 enum
/// </summary>
public enum PassiveSkillStatType
{
    ItemGetRange,
    MaxHp,
    Damage,
    MoveSpeed,
    Count,
}
/// <summary>
/// 패시브 스킬의 레벨별 스탯 정보를 담는 클래스
/// 레벨 0부터 레벨 4까지의 보너스 스탯 값을 담고 있다.
/// </summary>
[Serializable]
public class PassiveSkillLevelStat
{
    // 어떤 스탯을 증가시킬건지 
    [SerializeField] PassiveSkillStatType _statType;
    // 각 레벨별 수치 배열
    [SerializeField] float[] _levelValues;

    public PassiveSkillStatType StatType => _statType;
    public int MaxLevel => _levelValues.Length - 1; // 최대 레벨은 배열 길이 - 1
    public float[] LevelValues => _levelValues;

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
