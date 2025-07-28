using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class HeroModel : MonoBehaviour
{
    [Header("----- 임시 무적관련 -----")]
    [SerializeField] float _invincibleDuration = 0.3f;
    [SerializeField] bool _isInvincible = false;

    [SerializeField]  // 인스펙터 노출 위해 추가
    private HeroStats _stats = new HeroStats();

    public HeroStats Stats => _stats;

    public float CurrentHp { get; private set; }
    public float CurrentExp { get; private set; }
    public int CurrentLevel { get; private set; }
    public ElementType ElementType { get; private set; }

    HeroStatData _data;
    Coroutine _invincibleCoroutine;
    Coroutine _buffCoroutine;

    // 이벤트 선언
    public event Action<float, float> OnHpChanged;           // (현재HP, 최대HP)
    public event Action<float, float> OnExpChanged;          // (현재Exp, 다음레벨Exp)
    public event Action<int, int> OnLevelChanged;            // (이전레벨, 현재레벨)
    public event Action<float> OnDamageChanged;              // (현재 공격력)
    public event Action<float> OnMoveSpeedChanged;           // (현재 이동 속력)
    public event Action<float> OnItemGetRangeChanged;        // (현재 아이템 획득 범위)
    public event Action OnDead;                             // 죽음 이벤트
    public event Action<float> OnExpAdded;

    public void Initialize()
    {
        DropItemManager.OnAtkUpItemUsed += ApplyDamageBuff;
        DropItemManager.OnExpItemUsed += AddExp;
        DropItemManager.OnHpPotionItemUsed += Heal;

        if (GameManager.Instance == null)
            Debug.LogError("GameManager.Instance가 Null입니다.");

        if (GameManager.Instance.DataManager == null)
            Debug.LogError("DataManager가 Null입니다.");

        if (GameManager.Instance.DataManager.HeroStatData == null)
            Debug.LogError("HeroStatData가 Null입니다.");

        _data = GameManager.Instance.DataManager.HeroStatData;
        CurrentLevel = 1;
        CurrentExp = 0;

        Stats.MaxHp.Base = _data.GetMaxHp(CurrentLevel);
        Stats.Damage.Base = _data.GetDamage(CurrentLevel);
        Stats.MoveSpeed.Base = _data.GetMoveSpeed(CurrentLevel);
        Stats.ItemGetRange.Base = _data.GetItemGetRange(CurrentLevel);
        Stats.ExpGainRate.Base = _data.BaseExpGainRate;

        CurrentHp = Stats.MaxHp.Final;

        // Stat 이벤트 구독
        Stats.Damage.OnValueChanged += value => OnDamageChanged?.Invoke(value);
        Stats.MoveSpeed.OnValueChanged += value => OnMoveSpeedChanged?.Invoke(value);
        Stats.ItemGetRange.OnValueChanged += value => OnItemGetRangeChanged?.Invoke(value);
        Stats.MaxHp.OnValueChanged += max =>
        {
            // MaxHp 변경 시 체력도 갱신 필요
            if (CurrentHp > max)
                CurrentHp = max;
            OnHpChanged?.Invoke(CurrentHp, max);
        };

        // 초기 이벤트 호출
        OnHpChanged?.Invoke(CurrentHp, Stats.MaxHp.Final);
        OnExpChanged?.Invoke(CurrentExp, _data.GetExpRequired(CurrentLevel));
        OnLevelChanged?.Invoke(CurrentLevel, CurrentLevel);
        OnDamageChanged?.Invoke(Stats.Damage.Final);
        OnMoveSpeedChanged?.Invoke(Stats.MoveSpeed.Final);
        OnItemGetRangeChanged?.Invoke(Stats.ItemGetRange.Final);
    }

    private void OnDestroy()
    {
        DropItemManager.OnAtkUpItemUsed -= ApplyDamageBuff;
        DropItemManager.OnExpItemUsed -= AddExp;
        DropItemManager.OnHpPotionItemUsed -= Heal;
    }

    /// <summary>
    /// 주인공의 경험치를 추가하는 함수
    /// </summary>
    /// <param name="amount">증가할 경험치 양</param>
    public void AddExp(float amount)
    {
        // 경험치 획득률을 적용하여 경험치를 계산
        amount *= Stats.ExpGainRate.Final;
        // 경험치를 증가시키고
        CurrentExp += amount;
        OnExpAdded?.Invoke(amount);
        // 현재 레벨에 필요한 경험치 이상이 되면 레벨업을 시도
        while (CurrentExp >= _data.GetExpRequired(CurrentLevel))
        {
            LevelUp();
        }
        OnExpChanged?.Invoke(CurrentExp, _data.GetExpRequired(CurrentLevel));
    }

    /// <summary>
    /// 레벨업을 처리하는 함수
    /// </summary>
    public void LevelUp()
    {
        int previousLevel = CurrentLevel;
        float oldMaxHp = Stats.MaxHp.Final;

        // 레벨업 시 현재 경험치에서 레벨업에 필요한 경험치를 차감하고
        CurrentExp -= _data.GetExpRequired(CurrentLevel);
        // 레벨 업
        CurrentLevel++;

        Stats.MaxHp.Base = _data.GetMaxHp(CurrentLevel);
        Stats.Damage.Base = _data.GetDamage(CurrentLevel);
        Stats.MoveSpeed.Base = _data.GetMoveSpeed(CurrentLevel);
        Stats.ItemGetRange.Base = _data.GetItemGetRange(CurrentLevel);

        // 레벨업 전후 최대 HP 차이만큼 현재 체력 증가
        float newMaxHp = Stats.MaxHp.Final;

        CurrentHp += (newMaxHp - oldMaxHp);
        if (CurrentHp > newMaxHp)
            CurrentHp = newMaxHp;

        OnLevelChanged?.Invoke(previousLevel, CurrentLevel);
        OnHpChanged?.Invoke(CurrentHp, newMaxHp);
        OnExpChanged?.Invoke(CurrentExp, _data.GetExpRequired(CurrentLevel));
    }

    public void TakeDamage(float amount, ElementType attackerElement)
    {
        if (_isInvincible == true) return;

        float multiplier = ElementalCalculator.GetMultiplier(attackerElement, GameManager.Instance.HeroManager.Type);
        amount *= multiplier; // 속성에 따른 배수 적용

        CurrentHp = Mathf.Max(CurrentHp - amount, 0);
        OnHpChanged?.Invoke(CurrentHp, Stats.MaxHp.Final);

        if (CurrentHp <= 0)
            OnDead?.Invoke();

        _invincibleCoroutine = StartCoroutine(InvincibilityRoutine());
    }

    /// <summary>
    /// 무적여부를 껐다가 무적시간만큼 지난 후 켜주는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator InvincibilityRoutine()
    {
        _isInvincible = true;
        yield return new WaitForSeconds(_invincibleDuration);
        _isInvincible = false;
    }

    public void Revive()
    {
        CurrentHp = Stats.MaxHp.Final;
        OnHpChanged?.Invoke(CurrentHp, Stats.MaxHp.Final);
    }

    public void Heal(float ratio)
    {
        float healAmount = Stats.MaxHp.Final * ratio;
        CurrentHp = Mathf.Min(CurrentHp + healAmount, Stats.MaxHp.Final);
        OnHpChanged?.Invoke(CurrentHp, Stats.MaxHp.Final);
    }

    public void AddCurrentHp(float amount)
    {
        CurrentHp = Mathf.Min(CurrentHp + amount, Stats.MaxHp.Final);
        OnHpChanged?.Invoke(CurrentHp, Stats.MaxHp.Final);
    }

    public void SetStat(StatName statName, StatType statType, float amount)
    {
        Stat stat = statName switch
        {
            StatName.MaxHp => Stats.MaxHp,
            StatName.Damage => Stats.Damage,
            StatName.MoveSpeed => Stats.MoveSpeed,
            StatName.ItemGetRange => Stats.ItemGetRange,
            StatName.ExpGainRate => Stats.ExpGainRate,
            _ => null
        };

        if (stat == null)
        {
            Debug.LogError("Unknown StatName: " + statName);
            return;
        }

        switch (statType)
        {
            case StatType.Base:
                stat.Base = 0; // 초기화
                stat.Base += amount;
                break;
            case StatType.Bonus:
                stat.Bonus = 0; // 초기화
                stat.Bonus += amount;
                break;
            case StatType.Stage:
                stat.Stage = 0; // 초기화
                stat.Stage += amount;
                break;
            case StatType.Buff:
                stat.Buff = 0; // 초기화
                stat.Buff += amount;
                break;
        }
    }

    public void ApplyDamageBuff(float buffRate, float duration)
    {
        if (_buffCoroutine != null)
            StopCoroutine(_buffCoroutine);

        _buffCoroutine = StartCoroutine(DamageBuffRoutine(buffRate, duration));
    }

    IEnumerator DamageBuffRoutine(float buffRate, float duration)
    {
        ApplyBuff(StatName.Damage, buffRate);

        yield return new WaitForSeconds(duration);

        RemoveBuff(StatName.Damage, buffRate);

        _buffCoroutine = null;
    }

    public void ApplyBuff(StatName statName, float rate)
    {
        SetStat(statName, StatType.Buff, rate);
    }

    public void RemoveBuff(StatName statName, float rate)
    {
        SetStat(statName, StatType.Buff, 0);
    }
}
