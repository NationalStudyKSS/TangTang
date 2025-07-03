using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적이 죽으면 드랍되는 아이템들의 추상 클래스
/// </summary>
public abstract class DropItem : MonoBehaviour
{
    protected int itemId; // 아이템 ID
    public event Action<int> OnItemUsed; // 아이템 사용 이벤트
    public virtual void Use()
    {
        OnItemUsed?.Invoke(itemId);
    }
}
