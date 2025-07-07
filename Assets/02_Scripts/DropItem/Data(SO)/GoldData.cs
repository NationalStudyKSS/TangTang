using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 코인 아이템의 데이터를 관리하는 클래스
/// </summary>
[CreateAssetMenu(fileName = "GoldData", menuName = "GameSettings/DropItem/GoldData")]
public class GoldData : DropItemData
{
    [SerializeField] int _goldAmount; // 코인 아이템의 지급량
    public int GoldAmount => _goldAmount; // 코인 아이템의 지급량을 반환

    public override void ApplyEffect()
    {
        // 코인 아이템 사용 이벤트 발생
        DropItemManager.RaiseGoldItemUsed(_goldAmount);
    }
}
