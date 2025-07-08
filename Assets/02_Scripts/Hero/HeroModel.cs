using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 주인공 캐릭터의 데이터 로직을 담당하는 클래스
/// </summary>
public class HeroModel : MonoBehaviour
{
    [Header("----- 기본 스탯(Base) -----")]
    [SerializeField] float _baseMaxHp;              // 기본 최대 체력
    [SerializeField] float _baseDamage;             // 기본 공격력
    [SerializeField] float _baseMoveSpeed;          // 기본 이동 속력
    [SerializeField] float _baseItemGetRange;       // 기본 아이템 획득 범위
    [SerializeField] float _baseExpToLevelUp;       // 기본 레벨업 필요 경험치
    [SerializeField] int _maxLevel;                  // 최대 레벨
    [SerializeField] float _baseExpGainRate;        // 경험치 획득률

    [Header("----- 보너스 스탯(Bonus) -----")]
    [SerializeField] float _bonusMaxHp;              // 보너스 최대 체력
    [SerializeField] float _bonusDamage;             // 보너스 공격력
    [SerializeField] float _bonusMoveSpeed;          // 보너스 이동 속력
    [SerializeField] float _bonusItemGetRange;       // 보너스 아이템 획득 범위
    [SerializeField] float _bonusExpGainRate;        // 보너스 경험치 획득률

    [Header("----- 최종 스탯(Final) -----")]
    [SerializeField] float _maxHp;                  // 최종 최대 체력
    [SerializeField] float _damage;                  // 최종 공격력
    [SerializeField] float _moveSpeed;               // 최종 이동 속력
    [SerializeField] float _itemGetRange;            // 최종 아이템 획득 범위
    [SerializeField] float _expToLevelUp;            // 최종 레벨업 필요 경험치
    [SerializeField] float _expGainRate;             // 최종 경험치 획득률

    [Header("----- 현재 스탯(Current) -----")]
    [SerializeField] float _currentHp;               // 현재 체력
    [SerializeField] float _currentDamage;           // 현재 공격력
    [SerializeField] float _currentMoveSpeed;        // 현재 이동 속력
    [SerializeField] float _currentItemGetRange;     // 현재 아이템 획득 범위
    [SerializeField] float _currentExp;               // 현재 경험치
    [SerializeField] int _currentLevel;               // 현재 레벨
    [SerializeField] float _currentExpGainRate;      // 현재 경험치 획득률

    HeroStatData _heroStatData;
    Coroutine _attackBuffCoroutine;

    // 이벤트 선언
    public event UnityAction<float, float> OnHpChanged;
    public event UnityAction<float> OnDamageChanged;
    public event UnityAction<float> OnMoveSpeedChanged;
    public event UnityAction<float> OnItemGetRangeChanged;
    public event UnityAction<float, float> OnExpChanged;
    public event UnityAction<int, int> OnLevelChanged;
    public event UnityAction OnDeath;

    // 프로퍼티
    public float MaxHp => _maxHp;
    public float Damage => _damage;
    public float MoveSpeed => _moveSpeed;
    public float ItemGetRange => _itemGetRange;
    public float ExpToLevelUp => _expToLevelUp;
    public float ExpGainRate => _expGainRate;
    public float CurrentHp => _currentHp;
    public float CurrentDamage => _currentDamage;
    public float CurrentMoveSpeed => _currentMoveSpeed;
    public float CurrentItemGetRange => _currentItemGetRange;
    public float CurrentExp => _currentExp;
    public int CurrentLevel => _currentLevel;
    public float CurrentExpGainRate => _currentExpGainRate;

    public void Initialize()
    {
        // 이벤트 연결 (필요시)
        DropItemManager.OnAtkUpItemUsed += ApplyAttackBuff;
        DropItemManager.OnExpItemUsed += AddExp;
        DropItemManager.OnHpPotionItemUsed += HpHeal;

        _currentLevel = 1;
        _currentExp = 0f;

        _heroStatData = GameManager.Instance.DataManager.HeroStatData;

        // Base 스탯 초기화 (레벨에 따른 값)
        _baseMaxHp = _heroStatData.GetMaxHp(_currentLevel);
        _baseDamage = _heroStatData.GetDamage(_currentLevel);
        _baseMoveSpeed = _heroStatData.GetMoveSpeed(_currentLevel);
        _baseItemGetRange = _heroStatData.GetItemGetRange(_currentLevel);
        _baseExpToLevelUp = _heroStatData.GetExpRequired(_currentLevel);
        _maxLevel = _heroStatData.MaxLevel;
        _baseExpGainRate = _heroStatData.BaseExpGainRate;

        // 보너스 스탯 받아오기 (코드 추가 필요)
        // 예: _bonusMaxHp = ...;

        // 최종 스탯 계산 (Base + Bonus)
        _maxHp = _baseMaxHp + _bonusMaxHp;
        _damage = _baseDamage + _bonusDamage;
        _moveSpeed = _baseMoveSpeed + _bonusMoveSpeed;
        _itemGetRange = _baseItemGetRange + _bonusItemGetRange;
        _expToLevelUp = _baseExpToLevelUp;
        _expGainRate = _baseExpGainRate + _bonusExpGainRate;

        // 현재 스탯 초기화
        _currentHp = _maxHp;
        _currentDamage = _damage;
        _currentMoveSpeed = _moveSpeed;
        _currentItemGetRange = _itemGetRange;
        _currentExpGainRate = _expGainRate;

        // 이벤트 호출
        OnHpChanged?.Invoke(_currentHp, _maxHp);
        OnDamageChanged?.Invoke(_damage);
        OnMoveSpeedChanged?.Invoke(_moveSpeed);
        OnItemGetRangeChanged?.Invoke(_itemGetRange);
        OnExpChanged?.Invoke(_currentExp, _expToLevelUp);
        OnLevelChanged?.Invoke(_currentLevel, _currentLevel);

        _attackBuffCoroutine = null;
    }

    private void OnDestroy()
    {
        DropItemManager.OnAtkUpItemUsed -= ApplyAttackBuff;
        DropItemManager.OnHpPotionItemUsed -= HpHeal;
        DropItemManager.OnExpItemUsed -= AddExp;
    }

    public void TakeDamage(float amount)
    {
        if (_currentHp <= 0) return;

        _currentHp = Mathf.Min(_currentHp - amount, _maxHp);
        _currentHp = Mathf.Max(_currentHp, 0f);

        OnHpChanged?.Invoke(_currentHp, _maxHp);

        if (_currentHp <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    public void AddExp(float amount)
    {
        if (amount <= 0) return;

        _currentExp += amount;

        while (_currentExp >= _expToLevelUp)
        {
            LevelUp();
        }

        OnExpChanged?.Invoke(_currentExp, _expToLevelUp);
    }

    private void LevelUp()
    {
        int previousLevel = _currentLevel;

        if (_currentLevel >= _maxLevel)
        {
            _currentExp = _expToLevelUp;
            return;
        }

        _currentExp -= _expToLevelUp;
        _currentLevel++;

        // Base 스탯 재계산
        _baseMaxHp = _heroStatData.GetMaxHp(_currentLevel);
        _baseDamage = _heroStatData.GetDamage(_currentLevel);
        _baseMoveSpeed = _heroStatData.GetMoveSpeed(_currentLevel);
        _baseItemGetRange = _heroStatData.GetItemGetRange(_currentLevel);
        _baseExpToLevelUp = _heroStatData.GetExpRequired(_currentLevel);

        // 최종 스탯 재계산 (Base + Bonus)
        _maxHp = _baseMaxHp + _bonusMaxHp;
        _damage = _baseDamage + _bonusDamage;
        _moveSpeed = _baseMoveSpeed + _bonusMoveSpeed;
        _itemGetRange = _baseItemGetRange + _bonusItemGetRange;
        _expToLevelUp = _baseExpToLevelUp;

        // 현재 체력은 레벨업으로 증가한 체력만큼 더해줌
        _currentHp += (_maxHp - _baseMaxHp);

        _currentDamage = _damage;
        _currentMoveSpeed = _moveSpeed;
        _currentItemGetRange = _itemGetRange;

        OnLevelChanged?.Invoke(previousLevel, _currentLevel);
        OnHpChanged?.Invoke(_currentHp, _maxHp);
        OnDamageChanged?.Invoke(_currentDamage);
        OnMoveSpeedChanged?.Invoke(_currentMoveSpeed);
        OnItemGetRangeChanged?.Invoke(_currentItemGetRange);
    }

    public void ApplyAttackBuff(float atkUpRate, float duration)
    {
        if (_attackBuffCoroutine != null)
            StopCoroutine(_attackBuffCoroutine);

        _attackBuffCoroutine = StartCoroutine(AttackBuffRoutine(atkUpRate, duration));
    }

    IEnumerator AttackBuffRoutine(float atkUpRate, float duration)
    {
        float originalDamage = _damage;

        _currentDamage = originalDamage * (1 + atkUpRate);
        OnDamageChanged?.Invoke(_currentDamage);

        yield return new WaitForSeconds(duration);

        _currentDamage = originalDamage;
        OnDamageChanged?.Invoke(_currentDamage);

        _attackBuffCoroutine = null;
    }

    public void HpHeal(float hpHealRate)
    {
        if (hpHealRate <= 0) return;

        _currentHp = Mathf.Min(_currentHp + (_maxHp * hpHealRate), _maxHp);
        OnHpChanged?.Invoke(_currentHp, _maxHp);
    }
}
