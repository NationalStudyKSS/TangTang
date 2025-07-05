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
/// 드롭 아이템의 데이터를 관리하는 추상 클래스
/// 해당 효과를 적용하는 기능까지 자식들이 구현해야함.
/// 이대로는 못 쓰고 상속받아서 필요한거 알아서 추가해서 사용해야 함
/// </summary>
[CreateAssetMenu(fileName = "DropItemData", menuName = "GameSettings/DropItem/DropItemData")]
public abstract class DropItemData : ScriptableObject
{
    [SerializeField] protected int _itemId; // 아이템 ID
    [SerializeField] protected float _chance; // 드롭 확률
    [SerializeField] protected DropItemName _name; // 아이템 이름 (enum으로 관리)
    [SerializeField] protected string _dropItemPrefabPath; // 드롭 아이템 프리팹 경로

    /// <summary>
    /// 각각의 아이템들 데이터 클래스에서 변수를 바꾸던 효과를 바꾸던
    /// 알아서 그 기능을 구현해주세요~
    /// 단, 효과를 직접 바꾸는 것이 아니라
    /// 이미 있는 함수를 호출하는 정도
    /// </summary>
    public virtual void ApplyEffect()
    {

    }

    public int ItemId => _itemId; // 아이템 ID를 반환
    public float Chance => _chance; // 드롭 확률을 반환
    public DropItemName Name => _name; // 아이템 이름을 반환
    public string DropItemPrefabPath => _dropItemPrefabPath; // 드롭 아이템 프리팹 경로를 반환
}
