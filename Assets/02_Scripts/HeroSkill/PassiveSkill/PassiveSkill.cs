using UnityEngine;



public abstract class PassiveSkill : MonoBehaviour, IUpgradable
{
    [Header("----- 대상 모델 -----")]
    [SerializeField] protected HeroModel _heroModel;

    [Header("----- 설정 데이터 -----")]
    [SerializeField] protected PassiveSkillData _data;

    [Header("----- 스탯 -----")]
    [SerializeField] protected int _level;        // 기어 레벨
    [SerializeField] protected float _bonusValue; // 현재 레벨 보너스 값
    [SerializeField] protected float _previousBonusValue;          // 이전 레벨 보너스 값

    public abstract PassiveSkillStatType PassiveSkillType { get; }
    public string UpgradeName => _data.PassiveSkillName;
    public string Description => _data.Description;
    public Sprite IconSprite => _data.IconSprite;
    public int Level => _level;
    public bool IsMaxLevel => _level >= _data.MaxLevel;

    public void Start()
    {
        _heroModel = GetComponentInParent<HeroModel>();
    }

    /// <summary>
    /// 현재 레벨에 따른 보너스 값 계산 및 갱신
    /// </summary>
    void CalculateStats()
    {
        _bonusValue = _data.GetStat(PassiveSkillType, _level);
    }

    /// <summary>
    /// 장비 업그레이드 함수
    /// </summary>
    public void Upgrade()
    {
        if (_data == null)
            _data = GameManager.Instance.DataManager.PassiveSkillDataDict[(int)PassiveSkillType];
        // 이전 보너스 저장
        _previousBonusValue = _bonusValue;

        // 레벨 올리고
        _level++;

        // 보너스 재계산
        CalculateStats();

        // 보너스 적용 (이전 보너스 빼고 새 보너스 더하기)
        Apply();
    }

    /// <summary>
    /// 장비 효과 적용 함수 (이전 보너스 빼고 새 보너스 더하기)
    /// </summary>
    protected abstract void Apply();
}
