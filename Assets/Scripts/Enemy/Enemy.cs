using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적에게 필요한 부품들을 조립해 적을 구성하는 역할
/// 기본, 이동, 공격, 피격, 사망
/// </summary>
public class Enemy : MonoBehaviour
{
    [SerializeField] Mover _mover;

    [SerializeField] Transform _target;

    public void Initialize()
    {
        Transform target = GameObject.FindGameObjectWithTag("Hero")?.transform;
        _target = target;
    }


    private void Update()
    {
        // 적 캐릭터가 타겟(주인공)을 추적하는 로직
        if (_target != null)
        {
            FollowTarget();
        }
    }

    /// <summary>
    /// 추적 대상 방향으로 이동하는 함수
    /// </summary>
    void FollowTarget()
    {
        // 적 캐릭터에서 타겟(주인공) 위치로 향하는 방향 벡터 구하기
        Vector3 dir = (_target.position - transform.position).normalized;
        _mover.Move(dir);
    }

    public void Move(Vector3 direction)
    {
        _mover.Move(direction);
    }

    public void Attack()
    {

    }

    public void TakeDamage()
    {

    }

    public void Die()
    {

    }
}

public enum EnemyState
{
    Idle,
    Move,
    Attack,
    Hit,
    Death,
}

// 적 캐릭터의 상태 인터페이스
public interface IEnemyState
{
    // 인터페이스는 프로퍼티도 멤버로 가질 수 있다.
    EnemyState State { get; }

    // 이 상태가 시작했을 때 동작하는 함수
    void Enter();

    // 이 상태가 진행 중일 때 반복적으로 동작하는 함수
    // (유니티 Update()와는 상관 X)
    void Update();

    // 이 상태가 끝났을 때 동작하는 함수
    void Exit();
}

public abstract class IdleState : IEnemyState
{
    public EnemyState State => throw new System.NotImplementedException();

    public abstract void Enter();

    public void Exit()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        throw new System.NotImplementedException();
    }
}