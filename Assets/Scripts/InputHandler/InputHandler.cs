using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 사용자의 입력을 받아 알리는 역할
/// </summary>
public abstract class InputHandler : MonoBehaviour
{
    // 이동 입력 이벤트 변수
    public abstract event UnityAction<Vector2> OnMoveInput;
}
