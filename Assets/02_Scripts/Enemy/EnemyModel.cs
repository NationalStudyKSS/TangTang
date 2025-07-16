using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 적 캐릭터의 '데이터 로직'을 담당하는 역할
/// 겉으로 보이는 상호작용이 아닌 수치를 갖고 노는 곳
/// </summary>
public class EnemyModel : MonoBehaviour, IDamageable
{
    [Header("----- 확인용 스탯 -----")]
    // 공격력
    [SerializeField] float _damage;
    // 이동속도
    [SerializeField] float _moveSpeed;
    // 최대 체력
    [SerializeField] float _maxHp;
    // 현재 체력
    [SerializeField] float _currentHp;
    // 속성
    [SerializeField] ElementType _element;
    // 타입
    [SerializeField] EnemyType _enemyType;
    
    EnemyStatData _enemyStatData;

    // 이동속도 변경 이벤트
    public event Action<float> OnSpeedChanged;
    // 체력 변경 이벤트
    public event Action<float, float> OnHpChanged;
    // 사망 이벤트
    public event Action OnDead;

    public float Damage => _damage;
    public float MoveSpeed => _moveSpeed;
    public float MaxHp => _maxHp;
    public float CurrentHp => _currentHp;
    public ElementType Element => _element;
    public EnemyType EnemyType => _enemyType;

    public void Initialize()
    {
        // 적 스탯 데이터 가져오기
        _enemyStatData = GameManager.Instance.DataManager.EnemyStatData;
        // 가져온 데이터에 있는 값들 매칭해서 초기화
        // 임시(EnemyStatData 리팩토링중)
        _damage = 10;
        _moveSpeed = 5;
        _maxHp = 100;
        //_damage = _enemyStatData.Damage;
        //_moveSpeed = _enemyStatData.Speed;
        //_maxHp = _enemyStatData.MaxHp;
        
        // 초기화 필요한 변수들
        _currentHp = _maxHp;
    }

    /// <summary>
    /// 공격당했을 때 데미지만큼 체력을 깎는 함수
    /// 죽으면 사망 이벤트도 발행
    /// </summary>
    /// <param name="amount">공격받은 데미지 양</param>
    public void TakeDamage(float amount, ElementType attackerElement)
    {
        if (_currentHp <= 0) return;

        float multiplier = ElementalCalculator.GetMultiplier(attackerElement, _element);
        amount *= multiplier; // 속성에 따른 배수 적용

        _currentHp = Mathf.Min(_currentHp - amount, _maxHp);

        // 체력 변경 이벤트 발행
        OnHpChanged?.Invoke(_currentHp, _maxHp);

        // 현재 체력이 0 이하가 되면 사망 처리
        if (_currentHp <= 0)
        {
            // 사망 이벤트 발행
            OnDead?.Invoke();
        }
    }
}
