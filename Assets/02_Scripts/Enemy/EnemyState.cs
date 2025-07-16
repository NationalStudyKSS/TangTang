using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 상태 패턴(State Pattern)
// 어떤 객체가 상태에 따라 다르게 행동할 때
// 각 상태를 객체화하여 필요에 따라 다르게 행동하도록 위임하는 디자인 패턴
// -> 행동들은 본래 클래스에 함수로 정의
// -> 상태들은 별도 클래스로 분리해서 본래 객체의 행동 함수들을 실행

/// <summary>
/// 적 캐릭터의 상태를 나타내는 열거형
/// </summary>
public enum EnemyStateType
{
    Idle,       // 방치 상태
    Attack,     // 공격 상태
    Stagger,    // 피격 상태
    Rush,       // 돌진 상태
    RangedAttack,   // 원거리 공격
    Skill,      // 스킬
    Dead,       // 사망 상태
    Count,      // 상태 종류 수 카운트 용
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

/// <summary>
/// 적 캐릭터 상태 클래스들의 공통 부모 추상 클래스
/// </summary>
public abstract class EnemyState
{
    protected Enemy _enemy;

    /// <summary>
    /// 상태 종류를 반환
    /// </summary>
    public abstract EnemyStateType StateType { get; }

    /// <summary>
    /// Enemy 상태 객체 생성자
    /// </summary>
    /// <param name="enemy">상태를 적용할 Enemy 객체(컴포넌트)</param>
    public EnemyState(Enemy enemy)
    {
        _enemy = enemy;
    }

    /// <summary>
    /// 상태 진입 시 호출되는 함수
    /// </summary>
    public abstract void Enter();

    /// <summary>
    /// 상태 유지 시 매 프레임 호출되는 함수
    /// </summary>
    public abstract void Update();

    /// <summary>
    /// 상태 종료 시 호출되는 함수
    /// </summary>
    public abstract void Exit();
}

public class IdleState : EnemyState
{
    public override EnemyStateType StateType => EnemyStateType.Idle;

    public IdleState(Enemy enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        // 필요한 초기화
    }

    public override void Update()
    {
        _enemy.FollowTarget();
    }

    public override void Exit()
    {
        _enemy.Stop();
    }
}


public class StaggerState : EnemyState
{
    private float _timer;
    private float _duration;

    public override EnemyStateType StateType => EnemyStateType.Stagger;

    public StaggerState(Enemy enemy, float duration) : base(enemy)
    {
        _duration = duration;
    }

    public override void Enter()
    {
        _timer = 0;
    }

    public override void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > _duration)
        {
            _enemy.ChangeState(EnemyStateType.Idle);
        }
    }

    public override void Exit()
    {
        // 특별한 작업 필요없으면 빈칸
    }
}

/// <summary>
/// 적 캐릭터가 죽었을 때의 상태를 처리하는 역할
/// </summary>
public class DeadState : EnemyState
{
    float _timer;           // 사망 상태 타이머
    float _duration;        // 사망 상태 지속시간
    public override EnemyStateType StateType => EnemyStateType.Dead;

    public DeadState(Enemy enemy, float duration) : base(enemy)
    {
        _duration = duration;
    }
    
    public override void Enter()
    {
        _timer = 0;
    }
    public override void Exit()
    {
        _enemy.Stop();
    }
    public override void Update()
    {
        // 사망 상태 동안 타이머를 증가시키고
        _timer += Time.deltaTime;
        // 사망 상태가 지속시간이 지나면
        if (_timer > _duration)
        {
            _timer = 0;
            // 적 캐릭터를 제거하는 함수 호출
            _enemy.Remove();
        }
    }
}

public class RushState : EnemyState
{
    IRushable _rushable;
    private float _readyDuration;
    private float _rushDuration;
    private float _timer;
    private Vector2 _direction;
    private Transform _target;
    private bool _isRushing;
    Vector2 _rushDirection;

    public override EnemyStateType StateType => EnemyStateType.Rush;

    public RushState(RushEnemy enemy, float readyDuration, float duration) : base(enemy)
    {
        _rushable = enemy;
        _readyDuration = readyDuration;
        _rushDuration = duration;
        
    }

    public override void Enter()
    {
        _timer = 0f;
        _isRushing = false;
        _enemy.Stop();                 // 멈추기
        _rushDirection = _rushable.CalculateRushDirection();
        //_enemy.PlayPreRushAnimation(); // 예열 애니메이션 (선택)
    }

    public override void Update()
    {
        _timer += Time.deltaTime;

        if (!_isRushing && _timer >= _readyDuration)
        {
            _isRushing = true;
            _timer = 0f;
        }

        if (_isRushing)
        {
            _rushable.Rush(_rushDirection);

            if (_timer >= _rushDuration)
            {
                _enemy.ChangeState(EnemyStateType.Idle);
            }
        }
    }

    public override void Exit()
    {
        _rushable.StopRush();
    }
}

public class RangedAttackState : EnemyState
{
    public RangedAttackState(Enemy enemy) : base(enemy)
    {
    }

    public override EnemyStateType StateType => throw new System.NotImplementedException();

    public override void Enter()
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}

public class SkillState : EnemyState
{
    public SkillState(Enemy enemy) : base(enemy)
    {
    }

    public override EnemyStateType StateType => throw new System.NotImplementedException();

    public override void Enter()
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}
