using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DropItemName
{
    AtkUp,
    Bomb,
    Carrot,
    Coin,
    Exp,
    HpPotion,
    Magnet,
    TreasureBox,
    Count
}

/// <summary>
/// 드롭 아이템의 데이터를 관리하는 클래스
/// 지금은 임시로 ID와 확률, 프리펩만 있음
/// </summary>
[CreateAssetMenu(fileName = "DropItemData", menuName = "GameSettings/DropItem/DropItemData")]
public class DropItemData : ScriptableObject
{
    [SerializeField] int _itemId; // 아이템 ID
    [SerializeField] float _chance; // 드롭 확률
    [SerializeField] DropItemName _name;
    [SerializeField] string _dropItemPrefabPath; // 드롭 아이템 프리팹 경로
    // 상속을 이용해서 얼마나 증가하거나 몇초동안 지속할지 등을 추가할 수 있음

    public int ItemId => _itemId; // 아이템 ID를 반환
    public float Chance => _chance; // 드롭 확률을 반환
    public DropItemName Name => _name; // 아이템 이름을 반환
    public string DropItemPrefabPath => _dropItemPrefabPath; // 드롭 아이템 프리팹 경로를 반환
}
