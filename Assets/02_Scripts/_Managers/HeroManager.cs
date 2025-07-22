using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 시작전을 포함해서 갖고있어야 할 영웅의 정보를 담는 매니저
/// </summary>
public class HeroManager : MonoBehaviour
{
    [SerializeField] ElementType _type;
    [SerializeField] float _bonusHp;
    [SerializeField] float _bonusDamage;
    [SerializeField] float _bonusMoveSpeed;
    [SerializeField] float _bonusItemGetRange;

    public float BonusHp => _bonusHp;
    public float BonusDamage => _bonusDamage;
    public float BonusMoveSpeed => _bonusMoveSpeed;
    public float BonusItemGetRange => _bonusItemGetRange;

    public ElementType Type => _type;

    public event Action OnBonusStatChanged;

    public void Initialize()
    {
        // 임시
        _type = ElementType.Fire;

        // 초기화
        _bonusHp = 0;
        _bonusDamage = 0;
        _bonusMoveSpeed = 0;
        _bonusItemGetRange = 0;
    }

    /// <summary>
    /// 영웅의 속성을 정해줄 함수
    /// </summary>
    /// <param name="type">영웅 속성</param>
    public void SetHeroElementType(ElementType type)
    {
        _type = type;
    }

    public void AddBonusHp(float amount)
    {
        _bonusHp += amount;
        OnBonusStatChanged?.Invoke();
    }
    public void AddBonusDamage(float amount)
    {
        _bonusDamage += amount;
        OnBonusStatChanged?.Invoke();
    }
    public void AddBonusMoveSpeed(float amount)
    {
        _bonusMoveSpeed += amount;
        OnBonusStatChanged?.Invoke();
    }
    public void AddBonusItemGetRange(float amount)
    {
        _bonusItemGetRange += amount;
        OnBonusStatChanged?.Invoke();
    }

    /// <summary>
    /// 게임 시작 시 호출되어 영웅에게 보너스 스탯을 제공할 함수
    /// </summary>
    /// <param name="model">영웅의 모델</param>
    public void ApplyStatsToHero(HeroModel model)
    {
        model.SetStat(StatName.MaxHp, StatType.Bonus, _bonusHp);
        model.SetStat(StatName.Damage, StatType.Bonus, _bonusDamage);
        model.SetStat(StatName.MoveSpeed, StatType.Bonus, _bonusMoveSpeed);
        model.SetStat(StatName.ItemGetRange, StatType.Bonus, _bonusItemGetRange);
    }
}
