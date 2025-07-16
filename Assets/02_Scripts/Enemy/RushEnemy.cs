using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 돌진하는 적을 구현하는 클래스
/// </summary>
public class RushEnemy : Enemy, IRushable
{
    [SerializeField] float _rushCoolTime;
    [SerializeField] float _readyDuration;
    [SerializeField] float _rushDuration;
    [SerializeField] float _rushSpeed;
    [SerializeField] Vector2 _rushDirection;

    float _rushTimer;

    public float RushCoolTime => _rushCoolTime;
    public float ReadyDuration => _readyDuration;
    public float RushDuration => _rushDuration;
    public float RushSpeed => _rushSpeed;
    public Vector2 RushDirection => _rushDirection;

    public override void Initialize()
    {
        base.Initialize();

        _rushTimer = 0f;
        // Rush 상태도 상태 배열에 추가
        _states[(int)EnemyStateType.Rush] = new RushState(this, _readyDuration, _rushDuration);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_currentState.StateType == EnemyStateType.Idle)
        {
            _rushTimer += Time.deltaTime;
            if (_rushTimer >= RushCoolTime)
            {
                _rushTimer = 0f;
                ChangeState(EnemyStateType.Rush);
            }
        }
    }

    public Vector2 CalculateRushDirection()
    {
        if (_target != null)
            _rushDirection = (_target.position - transform.position).normalized;
        else
            _rushDirection = Vector2.zero;
        return _rushDirection;
    }

    public void Rush(Vector2 rushDirection)
    {
        _mover.SetSpeed(_rushSpeed);
        _mover.Move(rushDirection);
    }

    public void StopRush()
    {
        _mover.SetSpeed(_model.MoveSpeed);
        _rushTimer = 0f;
    }
}
