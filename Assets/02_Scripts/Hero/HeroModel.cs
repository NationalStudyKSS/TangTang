using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 주인공 캐릭터의 데이터 로직을 담당하는 역할
/// </summary>
public class HeroModel : MonoBehaviour
{
    [Header("----- 기본 스탯(Base) -----")]
    [SerializeField] float _baseMaxHp;                // 기본 최대 체력
    [SerializeField] float _baseDamage;               // 기본 공격력
    [SerializeField] float _baseSpeed;                // 기본 이동 속력
    [SerializeField] float _baseExpRequired;          // 기본 레벨업에 필요한 경험치
    [SerializeField] int _baseMaxLevel;               // 기본 최대 레벨
    [SerializeField] float _baseExpIncrementRate;     // 기본 경험치 획득률

    [Header("----- 적용된 최종 스탯(Final) -----")]
    [SerializeField] float _maxHp;            // 최종 최대 체력
    [SerializeField] float _damage;           // 최종 공격력
    [SerializeField] float _speed;            // 최종 이동 속력
    [SerializeField] float _expRequired;      // 최종 레벨업에 필요한 경험치
    [SerializeField] int _maxLevel;           // 최종 최대 레벨
    [SerializeField] float _expIncrementRate; // 최종 경험치 획득률

    [Header("----- 현재 스탯(current) -----")]
    [SerializeField] float _currentHp;     // 현재 체력
    [SerializeField] float _currentDamage; // 현재 공격력
    [SerializeField] float _currentSpeed;  // 현재 이동 속력
    [SerializeField] float _currentExp;    // 현재 경험치량, 레벨업에 필요한 경험치랑 관련없음
    [SerializeField] int _currentLevel;    // 현재 레벨
    [SerializeField] float _currentExpIncrementRate; // 현재 경험치 획득률

    // 장비나 기타로 시작시 받아올 스탯들
    [Header(" ----- 보너스 스탯(bonus) -----")]
    [SerializeField] float _bonusMaxHp;              // 보너스 최대 체력
    [SerializeField] float _bonusDamage;             // 보너스 공격력
    [SerializeField] float _bonusSpeed;              // 보너스 이동 속력
    [SerializeField] float _bounusExpIncrementRate;   // 보너스 경험치 획득률

    HeroStatData _heroStatData;
    Coroutine _attackBuffCoroutine; // 공격력 버프 코루틴

    // 체력 변경 이벤트
    public event UnityAction<float, float> OnHpChanged;
    // 공격력 변경 이벤트
    public event UnityAction<float> OnDamageChanged;
    // 이동 속력 변경 이벤트
    public event UnityAction<float> OnSpeedChanged;
    // 사망 이벤트
    public event UnityAction OnDeath;
    // 경험치 변화 이벤트
    public event UnityAction<float, float> OnExpChanged;    // 현재 경험치, 레벨업에 필요한 경험치
    // 레벨 변화 이벤트
    public event UnityAction<int, int> OnLevelChanged;      // 이전 레벨, 현재 레벨

    public float MaxHp => _maxHp;           
    public float Damage => _damage;         
    public float Speed => _speed;          
    public float ExpRequired => _expRequired;
    public int MaxLevel => _maxLevel;
    public float ExpIncrementRate => _expIncrementRate;
    public float CurrentHp => _currentHp;  
    public float CurrentDamage => _currentDamage; 
    public float CurrentExp => _currentExp;
    public int CurrentLevel => _currentLevel;
    public float CurrentExpIncrementRate => _currentExpIncrementRate;

    public void Initialize()
    {
        // 공격력 버프 이벤트 연결
        DropItemManager.OnAtkUpItemUsed += ApplyAttackBuff;

        // 경험치 획득 이벤트 연결
        DropItemManager.OnExpItemUsed += AddExp;

        // 체력포션 이벤트 연결
        DropItemManager.OnHpPotionItemUsed += HpHeal;

        // 영웅 레벨과 경험치 초기화
        _currentLevel = 1;
        _currentExp = 0f;

        // HeroStatData를 DataManager에서 가져옴
        _heroStatData = GameManager.Instance.DataManager.HeroStatData;

        // Base스탯들 HeroStatData에 있는 값들로 초기화
        _baseMaxHp = _heroStatData.BaseMaxHp(_currentLevel);
        _baseDamage = _heroStatData.BaseDamage(_currentLevel);
        _baseSpeed = _heroStatData.BaseSpeed(_currentLevel);
        _baseExpRequired = _heroStatData.BaseExpRequired(_currentLevel);
        _baseMaxLevel = _heroStatData.BaseMaxLevel;
        _baseExpIncrementRate = _heroStatData.BaseExpIncrementRate;

        // Bonus스탯들 받아오기
        // 받아오는 코드

        // 최종 스탯들 Base랑 Bonus랑 합쳐서 초기화
        _maxHp = _baseMaxHp + _bonusMaxHp;
        _damage = _baseDamage + _bonusDamage;
        _speed = _baseSpeed + _bonusSpeed;
        _expRequired = _baseExpRequired;
        _maxLevel = _baseMaxLevel;
        _expIncrementRate = _baseExpIncrementRate + _bounusExpIncrementRate;

        // 현재 스탯들 최종 스탯들로 초기화
        _currentHp = _maxHp;
        _currentDamage = _damage;
        _currentSpeed = _speed;
        _currentExpIncrementRate = _expIncrementRate;

        // 초기화용 이벤트 발행
        OnHpChanged?.Invoke(_currentHp, _maxHp);
        OnDamageChanged?.Invoke(_damage);
        OnSpeedChanged?.Invoke(_speed);

        OnExpChanged?.Invoke(_currentExp, _expRequired);
        OnLevelChanged?.Invoke(_currentLevel, _currentLevel);

        // 공격력 버프 코루틴 초기화
        _attackBuffCoroutine = null; 
    }

    private void OnDestroy()
    {
        DropItemManager.OnAtkUpItemUsed -= ApplyAttackBuff;
        DropItemManager.OnHpPotionItemUsed -= HpHeal;
        DropItemManager.OnExpItemUsed -= AddExp;
    }

    /// <summary>
    /// 공격당했을 때 데미지만큼 체력을 깎는 함수
    /// </summary>
    /// <param name="amount"></param>
    public void TakeDamage(float amount)
    {
        // 현재 체력이 0 이하인 경우, 더 이상 데미지를 받지 않음
        if (_currentHp <= 0) return;

        // 체력 감소가 마이너스로 들어와서(회복처럼 될 경우)
        // 최대 체력을 초과하지 않도록 제한
        _currentHp = Mathf.Min(_currentHp - amount, _maxHp);

        // 체력 변경 이벤트 발행
        OnHpChanged?.Invoke(_currentHp, _maxHp);

        // 현재 체력이 0 이하가 되면
        if (_currentHp <= 0)
        {
            // 사망 이벤트 발행
            OnDeath?.Invoke();
        }
    }

    /// <summary>
    /// 경험치를 추가하는 함수
    /// </summary>
    /// <param name="amount">경험치 획득량</param>
    public void AddExp(float amount)
    {
        // 경험치가 0 이하인 경우, 추가하지 않음
        if (amount <= 0) return;

        // 현재 경험치에 추가 경험치를 더함
        _currentExp += amount;

        // 경험치가 최대 경험치 이상인 경우 반복
        while (_currentExp >= _expRequired)
        {
            // 레벨업 처리
            LevelUp();
        }

        // 경험치 변경 이벤트 발행
        OnExpChanged?.Invoke(_currentExp, _expRequired);
    }

    /// <summary>
    /// 레벨업을 처리하는 함수
    /// </summary>
    private void LevelUp()
    {
        int preLevel = _currentLevel;

        if (_currentLevel >= _heroStatData.BaseMaxLevel)
        {
            _currentExp = _expRequired;
            return;
        }

        _currentExp -= _expRequired; // 현재 경험치에서 레벨업에 필요한 경험치를 뺌
        _currentLevel++;    // 레벨 증가

        // Base 스탯 계산
        float preBaseMaxHp = _baseMaxHp;
        _baseMaxHp = _heroStatData.BaseMaxHp(_currentLevel);
        _baseDamage = _heroStatData.BaseDamage(_currentLevel);
        _baseSpeed = _heroStatData.BaseSpeed(_currentLevel);
        _baseExpRequired = _heroStatData.BaseExpRequired(_currentLevel);

        // Final 스탯 재계산
        _maxHp = _baseMaxHp + _bonusMaxHp;
        _damage = _baseDamage + _bonusDamage;
        _speed = _baseSpeed + _bonusSpeed;
        _expRequired = _baseExpRequired;

        // 현재 스탯 갱신
        _currentHp += (_maxHp - preBaseMaxHp); // 레벨업으로 인한 체력 증가량을 현재 체력에 반영
        _currentDamage = _damage;
        _currentSpeed = _speed;

        // 이벤트 갱신
        OnLevelChanged?.Invoke(preLevel, _currentLevel);
        OnHpChanged?.Invoke(_currentHp, _maxHp);
        OnDamageChanged?.Invoke(_currentDamage);
        OnSpeedChanged?.Invoke(_currentSpeed);
    }

    // 영웅 버프 관련
    /// <summary>
    /// 영웅에게 공격력 버프를 적용하는 함수
    /// </summary>
    /// <param name="atkUpRate"></param>
    /// <param name="duration"></param>
    public void ApplyAttackBuff(float atkUpRate, float duration)
    {
        // 기존 버프 코루틴이 돌고 있다면 중단
        if (_attackBuffCoroutine != null)
            StopCoroutine(_attackBuffCoroutine);

        _attackBuffCoroutine = StartCoroutine(AttackBuffRoutine(atkUpRate, duration));
    }

    /// <summary>
    /// 공격력 버프를 적용하는 코루틴
    /// </summary>
    /// <param name="atkUpRate"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    IEnumerator AttackBuffRoutine(float atkUpRate, float duration)
    {
        float originalDamage = _damage; // 기준값은 항상 원래 공격력

        _currentDamage = originalDamage*(1 + atkUpRate);
        OnDamageChanged?.Invoke(_currentDamage);

        yield return new WaitForSeconds(duration);

        _currentDamage = originalDamage;
        OnDamageChanged?.Invoke(_currentDamage);
        _attackBuffCoroutine = null;
    }

    public void HpHeal(float hpHealRate)
    {
        // 체력 회복량이 0 이하인 경우, 회복하지 않음
        if (hpHealRate <= 0) return;
        // 현재 체력을 최대 체력을 초과하지 않도록 제한
        _currentHp = Mathf.Min(_currentHp + (_maxHp * hpHealRate), _maxHp);
        // 체력 변경 이벤트 발행
        OnHpChanged?.Invoke(_currentHp, _maxHp);
    }
}
