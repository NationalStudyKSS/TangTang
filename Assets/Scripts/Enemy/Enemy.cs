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

    public void FollowTarget()
    {

    }

    public void Move()
    {

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