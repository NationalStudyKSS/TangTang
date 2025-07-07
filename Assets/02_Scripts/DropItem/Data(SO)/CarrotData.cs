using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 당근 아이템의 데이터를 관리하는 클래스
/// </summary>
[CreateAssetMenu(fileName = "CarrotData", menuName = "GameSettings/DropItem/CarrotData")]
public class CarrotData : DropItemData
{
    [SerializeField] int _carrotAmount; // 당근 아이템의 지급량

    public int CarrotAmount => _carrotAmount; // 당근 아이템의 지급량을 반환

    public override void ApplyEffect()
    {
        // 당근 아이템 사용 이벤트 발생
        DropItemManager.RaiseCarrotItemUsed(_carrotAmount);
    }
}
