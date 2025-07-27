using System;
using UnityEngine;

public enum PassiveSkillType
{
    AnalogClock,    // 아이템 획득 범위 증가
    IronHelmet,     // 최대 체력 증가
    ChargePlug,     // 공격력 증가
    WingShoes,      // 이동속도 증가
}

public abstract class PassiveSkill : SkillBase
{
    [Header("----- 대상 모델 -----")]
    [SerializeField] protected HeroModel _heroModel;

    [Header("----- 설정 데이터 -----")]
    [SerializeField] protected PassiveSkillData _data;

    [Header("----- 스탯 -----")]
    [SerializeField] protected int _level;        // 기어 레벨
    [SerializeField] protected float _bonusValue; // 현재 레벨 보너스 값
    [SerializeField] protected StatName _statName;
    [SerializeField] protected PassiveSkillType _passiveSkillType;

    public StatName StatName => _statName;
    public PassiveSkillType PassiveSkillType => _passiveSkillType;

    public override string UpgradeName => _data.PassiveSkillName;
    public override string Description => _data.Description;
    public override Sprite IconSprite => _data.IconSprite;
    public override int Level => _level;
    public override bool IsMaxLevel => _data != null && _level >= _data.MaxLevel;
    public override UpgradeType UpgradeType => UpgradeType.PassiveSkill;

    public override void Initialize()
    {
        _heroModel = GetComponentInParent<HeroModel>();

        if (_data == null)
        {
            if (!GameManager.Instance.DataManager.PassiveSkillDataDict.TryGetValue(_passiveSkillType, out _data))
            {
                Debug.LogError($"[PassiveSkill] SkillType {_passiveSkillType} 데이터 로드 실패");
                return;
            }
        }

        if (_data.LevelStats != null && _data.LevelStats.Length > 0)
            _statName = _data.LevelStats[0].StatName;

        CalculateStats();
        Apply();
    }

    /// <summary>
    /// 현재 레벨에 따른 보너스 값 계산 및 갱신
    /// </summary>
    void CalculateStats()
    {
        _bonusValue = _data.GetStat(StatName, _level);
    }

    /// <summary>
    /// 장비 업그레이드 함수
    /// </summary>
    public override void Upgrade()
    {
        _level++;
        CalculateStats();
        Apply();

        SkillUpgradeManager.Instance.RegisterUpgrade(this);
        RaiseOnUpgraded();
    }

    /// <summary>
    /// 장비 효과 적용 함수 (이전 보너스 빼고 새 보너스 더하기)
    /// </summary>
    protected abstract void Apply();
}