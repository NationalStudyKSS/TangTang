using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DropItemName
{
    AtkUp,
    Bomb,
    Carrot,
    Gold,
    Exp,
    HpPotion,
    Magnet,
    TreasureBox,
    Count
}

/// <summary>
/// 드롭 아이템의 데이터를 관리하는 추상 클래스
/// 단순히 데이터만 들고 있어야함
/// </summary>
[CreateAssetMenu(fileName = "DropItemData", menuName = "GameSettings/DropItem/DropItemData")]
public abstract class DropItemData : ScriptableObject
{
    [SerializeField] protected int _itemId; // 아이템 ID
    [SerializeField] protected float _chance; // 드롭 확률
    [SerializeField] protected DropItemName _name; // 아이템 이름 (enum으로 관리)
    [SerializeField] protected string _dropItemPrefabPath; // 드롭 아이템 프리팹 경로

    /// <summary>
    /// 아이템 효과를 적용하는 추상 메서드
    /// </summary>
    public abstract void ApplyEffect(); 

    public int ItemId => _itemId; // 아이템 ID를 반환
    public float Chance => _chance; // 드롭 확률을 반환
    public DropItemName Name => _name; // 아이템 이름을 반환
    public string DropItemPrefabPath => _dropItemPrefabPath; // 드롭 아이템 프리팹 경로를 반환
}
