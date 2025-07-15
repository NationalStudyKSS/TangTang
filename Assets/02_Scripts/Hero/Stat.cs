using System;
using UnityEngine;

public enum StatType
{
    Base,
    Bonus,
    Stage,
    Buff
}

/// <summary>
/// 스탯 종류별 이름 정의
/// </summary>
[System.Serializable]
public class Stat
{
    public float Base
    {
        get => _base;
        set
        {
            if (_base != value)
            {
                _base = value;
                OnStatChanged();
            }
        }
    }

    public float Bonus
    {
        get => _bonus;
        set
        {
            if (_bonus != value)
            {
                _bonus = value;
                OnStatChanged();
            }
        }
    }

    public float Stage
    {
        get => _stage;
        set
        {
            if (_stage != value)
            {
                _stage = value;
                OnStatChanged();
            }
        }
    }

    public float Buff
    {
        get => _buff;
        set
        {
            if (_buff != value)
            {
                _buff = value;
                OnStatChanged();
            }
        }
    }

    [SerializeField] float _base;   // 기본 스탯
    [SerializeField] float _bonus;  // 장비 등으로 증가될 보너스 스탯
    [SerializeField] float _stage;  // 스테이지 강화 비율
    [SerializeField] float _buff;   // 버프 비율

    public float Final => (_base + _bonus) * (1 + _stage) * (1 + _buff);

    public event Action<float> OnValueChanged;

    /// <summary>
    /// 스탯이 변경되었을 때 호출되는 함수
    /// </summary>
    void OnStatChanged()
    {
        OnValueChanged?.Invoke(Final);
    }
}
