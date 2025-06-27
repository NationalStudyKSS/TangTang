using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// 유니티 Input System의 PlayerInput을 활용해 입력을 받아 알리는 역할
/// </summary>
public class PlayerInputHandler : InputHandler
{
    public override event UnityAction<Vector2> OnMoveInput;

    Vector2 _moveInput;

    void OnMove(InputValue inputValue)
    {
        _moveInput = inputValue.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        OnMoveInput?.Invoke(_moveInput);
    }
}
