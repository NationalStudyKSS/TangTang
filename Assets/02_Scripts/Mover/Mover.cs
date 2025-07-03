using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 게임오브젝트를 일정 속력으로 원하는 방향으로 이동시키는 역할
/// </summary>
public abstract class Mover : MonoBehaviour
{
    // 이동 속력
    [SerializeField] protected float _speed;
    public abstract event UnityAction<Vector3> OnMoved;
    public virtual float Speed => _speed;

    /// <summary>
    /// direction 방향으로 게임오브젝트를 이동시키는 함수
    /// </summary>
    /// <param name="direction"></param>
    public abstract void Move(Vector3 direction);
    public virtual void SetSpeed(float speed)
    {
        _speed = speed;
    }
}
