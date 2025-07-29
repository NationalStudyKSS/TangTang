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
    [SerializeField] private FixedJoystick _joystick;

    public override event UnityAction<Vector2> OnMoveInput;

    Vector2 _moveInput;

    void Start()
    {
        var playerInput = GetComponent<PlayerInput>();
        Debug.Log($"현재 입력 방식: {playerInput.currentControlScheme}");
    }

    //void OnMove(InputValue inputValue)
    //{
    //    _moveInput = inputValue.Get<Vector2>();
    //    Debug.Log($"MoveInput: {_moveInput}");
    //    OnMoveInput?.Invoke(_moveInput);
    //}

    void OnMove(InputValue inputValue)
    {
        Vector2 newInput = inputValue.Get<Vector2>();
        _moveInput = newInput;

        // PlayerInput을 통해 현재 사용 중인 장치 확인
        var playerInput = GetComponent<PlayerInput>();
        string controlScheme = playerInput.currentControlScheme;

        // 등록된 장치 목록 출력
        foreach (var device in playerInput.devices)
        {
            //Debug.Log($"Device: {device.displayName} (Layout: {device.layout})");
        }

        //Debug.Log($"MoveInput: {_moveInput} | ControlScheme: {controlScheme}");

        OnMoveInput?.Invoke(_moveInput);
    }


    private void FixedUpdate()
    {
        Vector2 finalInput = _moveInput;

        if (_joystick != null)
        {
            Vector2 joystickInput = new Vector2(_joystick.Horizontal, _joystick.Vertical);

            // 조이스틱이 약간이라도 움직였으면 조이스틱 입력 사용
            if (joystickInput.magnitude > 0.1f)
            {
                finalInput = joystickInput;
            }
        }

        OnMoveInput?.Invoke(finalInput);
    }
}
