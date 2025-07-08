using System.Collections;
using System.Collections.Generic;
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
public abstract class ActiveSkill : MonoBehaviour, IUpgradable
{
    [Header("----- 스탯 데이터 -----")]
    // 액티브스킬 데이터
    [SerializeField] protected ActiveSkillData _data;

    // 현재 액티브스킬 레벨
    [SerializeField] protected int _level;

    // 데미지 배율
    [SerializeField] protected float _damageRate;

    // 스킬 실제 데미지
    [SerializeField] protected float _damage;
    // 영웅의 현재 공격력
    protected float _heroCurrentDamage;

    // 액티브스킬 타입을 받을 변수
    [SerializeField] protected ActiveSkillType _ActiveSkillType;

    // 자식들에게 너의 액티브스킬타입을 ActiveSkillType이라는 변수로 공개하라라고 명령
    public abstract ActiveSkillType ActiveSkillType { get; }

    public string UpgradeName => _data.ActiveSkillName;
    public string Description => _data.Description;
    public Sprite IconSprite => _data.IconSprite;
    public int Level => _level;
    public bool IsMaxLevel => _level >= _data.MaxLevel;
    public float DamageRate => _damageRate;

    /// <summary>
    /// 레벨에 따른 액티브스킬 스텟을 계산하는 함수
    /// </summary>
    protected virtual void CalculateStats()
    {
        // 액티브스킬 레벨에 따른 데미지 계산
        _damageRate = _data.GetStat(ActiveSkillStatType.DamageRate, _level);
    }

    public virtual void Upgrade()
    {
        // 스킬데이터를 데이터매니저에서 가져옴
        _data = GameManager.Instance.DataManager.ActiveSkillDataDict[(int)_ActiveSkillType];

        _level++;  // 액티브스킬 레벨을 하나 올리고
        CalculateStats();  // 스텟을 다시 계산한다.
    }

    /// <summary>
    /// 액티브스킬데미지를 증가시키는 함수
    /// 혹시 감소될 수도 있을까봐 Set이라고 했음
    /// </summary>
    /// <param name="additionalDamage">증가 혹은 감소될 데미지퍼센트</param>
    public void SetDamage(float heroCurrendDamage)
    {
        _damage = _damageRate * heroCurrendDamage;
    }
}
