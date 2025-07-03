using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 게임오브젝트의 Transform을 조절해서 일정 속력으로 이동시키는 역할
/// </summary>
public class TransformMover : Mover
{
    public override event UnityAction<Vector3> OnMoved;
    Vector3 _moveVector;
    public override void Move(Vector3 direction)
    {
        _moveVector = direction * _speed;
        transform.Translate(_moveVector * Time.deltaTime);

        OnMoved?.Invoke(_moveVector);
    }
}
