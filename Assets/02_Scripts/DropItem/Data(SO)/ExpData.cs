using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 경험치 아이템의 데이터를 관리하는 클래스
/// 적의 종류에 따라 지급되는 경험치 아이템이 달라질 수 있어서
/// 상속을 통해 경험치 아이템의 지급량을 다르게 할 수 있음
/// </summary>
[CreateAssetMenu(fileName = "ExpData", menuName = "GameSettings/DropItem/ExpData")]
public class ExpData : DropItemData
{
    [SerializeField] int _expAmount; // 경험치 아이템의 지급량
    public int ExpAmount => _expAmount; // 경험치 아이템의 지급량을 반환
    
    public override void ApplyEffect()
    {
        // 경험치 아이템 사용 이벤트 발생
        DropItemManager.RaiseExpItemUsed(_expAmount);
    }
}
