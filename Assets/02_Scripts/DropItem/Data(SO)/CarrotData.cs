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
}
