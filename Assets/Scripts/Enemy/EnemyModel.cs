using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 적 캐릭터의 데이터 로직을 담당하는 역할
/// </summary>
public class EnemyModel : MonoBehaviour
{
    [Header("----- 확인용 스탯 -----")]
    // 공격력
    [SerializeField] float _damage;
    // 이동속도
    [SerializeField] float _moveSpeed;
    // 보상 경험치
    [SerializeField] float _expReward;
    // 보상 골드
    [SerializeField] int _goldReward;
    // 최대 체력
    [SerializeField] float _maxHp;
    // 현재 체력
    [SerializeField] float _currentHp;

    EnemyStatData _enemyStatData;

    // 이동속도 변경 이벤트
    public event Action<float> OnSpeedChanged;
    // 체력 변경 이벤트
    public event UnityAction<float, float> OnHpChanged;
    // 골드 변경 이벤트
    public event Action<int> OnGoldChanged;
    // 사망 이벤트
    public event Action OnDeath;

    public float Damage => _damage;
    public float MoveSpeed => _moveSpeed;
    public float MaxHp => _maxHp;
    public float CurrentHp => _currentHp;
    public float ExpReward => _expReward;
    public float GoldReward => _goldReward;

    public void Initialize(EnemyStatData enemyStatData)
    {
        _enemyStatData = enemyStatData;

        _currentHp = _maxHp;
    }

    public void TakeDamage(float amount)
    {
        if (_currentHp <= 0) return;

        _currentHp = Mathf.Min(_currentHp - amount, _maxHp);

        // 체력 변경 이벤트 발행
        OnHpChanged?.Invoke(_currentHp, _maxHp);

        if (_currentHp <= 0)
        {
            OnGoldChanged?.Invoke(_goldReward);
            // 사망 이벤트 발행
            OnDeath?.Invoke();

        }
    }

}
