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
        // Space.World로 하는 것에 대해 이유 알아?
        transform.Translate(_moveVector * Time.deltaTime, Space.World);

        OnMoved?.Invoke(_moveVector);
    }
}
