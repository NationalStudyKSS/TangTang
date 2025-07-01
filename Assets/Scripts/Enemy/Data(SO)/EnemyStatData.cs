using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적 기본 능력치를 포함하는 설정 데이터 클래스
/// </summary>
[CreateAssetMenu(fileName = "EnemyStatData", menuName = "GameSettings/Enemy/EnemyStatData")]
public class EnemyStatData : ScriptableObject
{
    [SerializeField] float _maxHp;      // 기본 최대 체력
    [SerializeField] float _damage;     // 기본 공격력
    [SerializeField] float _speed;      // 기본 이동 속력

    public float MaxHp => _maxHp;
    public float Damage => _damage;
    public float Speed => _speed;
}
