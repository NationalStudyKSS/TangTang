using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 게임오브젝트의 Rigidbody2D를 조절해 일정 속력으로 이동시키는 역할
/// </summary>
[RequireComponent(typeof(Rigidbody2D))] // Rigidbody2D가 있는 것을 강제시키는 코드
public class Rigidbody2DMover : Mover
{
    public override event UnityAction<Vector3> OnMoved;

    Rigidbody2D _rigid;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    public override void Move(Vector3 direction)
    {
        _rigid.velocity = direction * _speed;
        OnMoved?.Invoke(_rigid.velocity);
    }
}
