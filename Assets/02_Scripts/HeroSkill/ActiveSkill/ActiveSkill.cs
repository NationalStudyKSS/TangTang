using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

// ActiveSkill의 역할에 대해 항상 정확하게 생각하고 인지해야함.
// ActiveSkill은 Bullet에 담을 데이터 값만 갖고있고
// Bullet을 생성하는 역할까지만 한다.
// (날아가던 회전을 하던 이런건 Bullet에서 할 일이다.)
// Bullet을 생성할 때에 필요한 데이터 뿐만 아니라
// 적을 탐지할지? 아니면 일정 주기마다 할지에 대한
// '판단 여부'도 갖고 있어야 한다.
// abstract를 붙여놨으므로 이 클래스로는 객체를 생성하지 않겠다.

/// <summary>
/// 액티브스킬 타입을 enum 형으로 정의
/// 나중에 추가될 액티브스킬의 타입을 추가할 수 있음.
/// </summary>
public enum ActiveSkillType
{
    HogirlPunch,    // 호컬펀치
    DragonBall,     // 용의 공
    SpinBlade,      // 회전 칼날
    ElectricField,  // 전기장
    Gun,            // 총
    Count           // enum 크기 측정용
}

/// <summary>
/// 액티브스킬의 공통 기능을 포함하는 추상 클래스.
/// </summary>
public abstract class ActiveSkill : SkillBase
{
    [Header("----- 대상 모델(인스펙터뷰 연결해아함) -----")]
    [SerializeField] protected HeroModel _model;
    [SerializeField] PassiveSkill _somePassiveSkill;

    [Header("----- 스탯 데이터 -----")]
    // 액티브스킬 데이터
    [SerializeField] protected ActiveSkillData _data;

    // 현재 액티브스킬 레벨
    [SerializeField] protected int _level;

    // 데미지 배율
    [SerializeField] protected float _damageRate;

    // 스킬 실제 데미지
    [SerializeField] protected float _damage;

    // 영웅 속성 받아올 변수
    [SerializeField] protected ElementType _type;

    // 영웅의 현재 공격력
    protected float _heroCurrentDamage;

    // 액티브스킬 타입을 받을 변수
    [SerializeField] protected ActiveSkillType _activeSkillType;

    // 자식들에게 너의 액티브스킬타입을 ActiveSkillType이라는 변수로 공개하라라고 명령
    public abstract ActiveSkillType ActiveSkillType { get; }

    public override string UpgradeName => _data.ActiveSkillName;
    public override string Description => _data.Description;
    public override Sprite IconSprite => _data.IconSprite;
    public override int Level => _level;
    public override bool IsMaxLevel => _data != null && _level >= _data.MaxLevel;
    public override bool CanUpgrade
    {
        get
        {
            if (!IsMaxLevel)
                return true; // 일반 업그레이드 가능

            // 초월 조건: 특정 패시브가 1레벨 이상인지 체크
            if (_somePassiveSkill.Level >= 0)
                return true; // 초월 강화 가능

            return false;
        }
    }
    public float DamageRate => _damageRate;
    public float Damage => _damage;
    public ElementType Type => _type;
    public override UpgradeType UpgradeType => UpgradeType.ActiveSkill;

    public override void Initialize()
    {
        // 스킬데이터를 데이터매니저에서 가져옴
        if (_data == null)
            _data = GameManager.Instance.DataManager.ActiveSkillDataDict[(int)ActiveSkillType];

        if (_model == null)
        {
            _model = GetComponentInParent<HeroModel>();
        }

        _model.OnDamageChanged += SetDamage;
        if (_data != null)
        {
            SetDamage(_model.Stats.Damage.Final);
        }
        SetType(_model.ElementType);
    }

    /// <summary>
    /// 레벨에 따른 액티브스킬 스텟을 계산하는 함수
    /// </summary>
    protected virtual void CalculateStats()
    {
        // 액티브스킬 레벨에 따른 데미지 계산
        _damageRate = _data.GetStat(ActiveSkillStatType.DamageRate, _level);
    }

    public override void Upgrade()
    {
        _level++;  // 액티브스킬 레벨을 하나 올리고
        CalculateStats();  // 스텟을 다시 계산한다.
        
        _model.OnDamageChanged += SetDamage;
        if (_data != null)
        {
            SetDamage(_model.Stats.Damage.Final);
        }

        SkillUpgradeManager.Instance.RegisterUpgrade(this);
        RaiseOnUpgraded();
    }

    /// <summary>
    /// 액티브스킬데미지를 증가시키는 함수
    /// 혹시 감소될 수도 있을까봐 Set이라고 했음
    /// </summary>
    /// <param name="additionalDamage">증가 혹은 감소될 데미지퍼센트</param>
    public void SetDamage(float heroCurrentDamage)
    {
        _damage = _damageRate * heroCurrentDamage;
    }

    /// <summary>
    /// 무기가 데미지를 줄 때 필요한 속성을 Set하는 함수
    /// </summary>
    /// <param name="type">넣어줄 속성타입</param>
    public void SetType(ElementType type)
    {
        _type = type;
    }
}
