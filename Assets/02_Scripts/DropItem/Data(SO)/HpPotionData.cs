using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 체력 회복 아이템의 데이터를 관리하는 클래스
/// </summary>
[CreateAssetMenu(fileName = "HpPotionData", menuName = "GameSettings/DropItem/HpPotionData")]
public class HpPotionData : DropItemData
{
    [SerializeField] float _hpHealRate; // 체력 회복 아이템의 회복비율량
    
}
