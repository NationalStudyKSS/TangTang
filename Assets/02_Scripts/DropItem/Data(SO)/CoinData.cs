using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 코인 아이템의 데이터를 관리하는 클래스
/// </summary>
[CreateAssetMenu(fileName = "CoinData", menuName = "GameSettings/DropItem/CoinData")]
public class CoinData : DropItemData
{
    [SerializeField] int _coinAmount; // 코인 아이템의 지급량
}
