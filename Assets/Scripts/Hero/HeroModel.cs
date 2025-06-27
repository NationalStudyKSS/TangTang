using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 주인공 캐릭터의 데이터 로직을 담당하는 역할
/// </summary>
public class HeroModel : MonoBehaviour
{
    [Header("----- 스탯(인스펙터 뷰에서 보기위해만듬) -----")]
    [SerializeField] float _maxHp;      // 최대 체력
    [SerializeField] float _speed;      // 이동 속력
    [SerializeField] float _currentHp;  // 현재 체력
    [SerializeField] float _currentExp; // 현재 경험치
    [SerializeField] float _maxExp;     // 최대 경험치(필요 경험치)
    [SerializeField] int _level;        // 현재 레벨

    HeroStatData _heroStatData;

    // 체력 변경 이벤트
    public event UnityAction<float, float> OnHpChanged;
    // 이동 속력 변경 이벤트
    public event UnityAction<float> OnSpeedChanged;
    // 사망 이벤트
    public event UnityAction OnDeath;
    // 경험치 변화 이벤트
    public event UnityAction<float, float> OnExpChanged;
    // 레벨 변화 이벤트
    public event UnityAction<int, int> OnLevelChanged;

    // 프로퍼티 (읽기 전용)
    public float MaxHp => _maxHp;
    public float CurrentHp => _currentHp;
    public float Speed => _speed;
    public float MaxExp => _maxExp;
    public float CurrentExp => _currentExp;
    public int Level => _level;

    // 초기화
    public void Initialize(HeroStatData heroStatData)
    {
        _heroStatData = heroStatData;

        _maxHp = _heroStatData.MaxHp;
        _speed = _heroStatData.Speed;
        _maxExp = _heroStatData.GetExp(_level);
        _level = 0;

        _currentHp = _maxHp;

        // 초기 체력과 이동 속력 변경 이벤트 발행
        OnSpeedChanged?.Invoke(_speed);
        OnHpChanged?.Invoke(_currentHp, _maxHp);

        // 경험치 변경과 레벨 변경 이벤트 발행
        OnExpChanged?.Invoke(_currentExp, _maxHp);
        OnLevelChanged?.Invoke(_level, _level);
    }

    public void TakeDamage(float amount)
    {
        if (_currentHp <= 0) return;

        _currentHp = Mathf.Min(_currentHp - amount, _maxHp);

        // 체력 변경 이벤트 발행
        OnHpChanged?.Invoke(_currentHp, _maxHp);

        if (_currentHp <= 0)
        {
            // 사망 이벤트 발행
            OnDeath?.Invoke();
        }
    }

    /// <summary>
    /// 추가 체력을 적용하는 함수
    /// </summary>
    /// <param name="bounusHp"></param>
    public void SetBonusMaxHp(float bounusHp)
    {
        float ratio = _currentHp / _maxHp;
        _maxHp = _heroStatData.MaxHp + bounusHp;
        _currentHp = _maxHp * ratio;

        OnHpChanged?.Invoke(_currentHp, _maxHp);
    }

    /// <summary>
    /// 추가 속력 증가율을 적용하는 함수
    /// </summary>
    /// <param name="speedRatio"></param>
    public void SetBonusSpeedRation(float speedRatio)
    {
        _speed = (1 + speedRatio) * _heroStatData.Speed;
        OnSpeedChanged?.Invoke(_speed);
    }

    /// <summary>
    /// 경험치를 획득하는 함수
    /// </summary>
    /// <param name="amount">획득한 경험치 양</param>
    public void AddExp(float amount)
    {
        // 레벨 업 전 이전 레벨 변수
        int preLevel = _level;

        // 현재 경험치를 이번에 획득한 경험치만큼 증가
        _currentExp += amount;

        // 현재 경험치가 최대 경험치보다 큰 동안
        while(_currentExp >= _maxExp)
        {
            // 현재 경험치를 이번 레벨 업 필요치만큼 감소
            _currentExp -= _maxExp;

            // 레벨 1 증가
            _level++;

            // 최대 경험치를 변화된 레벨에 따라 갱신
            _maxExp = _heroStatData.GetExp(_level);

            // 레벨업을 여러번하면 count로 세서 하면안되나??
        }

        // 경험치 변화 이벤트 발행
        OnExpChanged?.Invoke(_currentExp, _maxExp);

        if(preLevel != _level)
        {
            // 레벨 변화 이벤트 발행
            OnLevelChanged?.Invoke(preLevel, _level);
        }
    }

    /// <summary>
    /// 현재경험치량이 최대 경험치량보다 크거나 같을 때 레벨업을 하는 함수
    /// </summary>
    public void LevelUp()
    {
        // 현재 경험치량이 최대 경험치량보다 작으면 레벨업하지 않음
        if (_currentExp < _maxExp) return;

        // 레벨이 최대 레벨보다 크거나 같으면 레벨업하지 않음

        _currentExp -= _maxExp;
        // 경험치통 늘리는 함수
        // SetMaxExp(_maxExp * 0.1f); // 레벨업시 최대 경험치량을 10% 증가
        _level++;

        OnExpChanged?.Invoke(_currentExp, _maxExp);
    }
}
