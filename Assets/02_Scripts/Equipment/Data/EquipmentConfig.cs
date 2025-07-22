using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipSlotType
{
    Helmet,
    Armor,
    Weapon,
    Gloves,
    Boots,
    Ring
}
/// <summary>
/// 장비의 설정 데이터 클래스
/// </summary>
[CreateAssetMenu(fileName = "EquipmentConfig", menuName = "GameSettings/EquipmentConfig")]
public class EquipmentConfig : ScriptableObject
{
    [Header("----- 장비 설정 데이터 -----")]
    [SerializeField] string _id;                            // 장비 ID
    [SerializeField] string _equipmentName;                 // 장비 이름
    [TextArea(3, 5)][SerializeField] string _description;   // 장비 설명
    [SerializeField] int _price;                            // 장비 가격
    [SerializeField] Sprite _iconSprite;                    // 장비 아이콘
    [SerializeField] Equipment _equipmentPrefab;            // 장비

    [Header("----- 장비 스탯 -----")]
    [SerializeField] float _bonusMaxHp;
    [SerializeField] float _bonusDamage;
    [SerializeField] float _bonusMoveSpeed;
    [SerializeField] float _bonusItemGetRange;

    [Header("----- 장비 슬롯 -----")]
    [SerializeField] EquipSlotType _equipSlotType;

    public string Id => _id;
    public string EquipmentName => _equipmentName;
    public string Description => _description;
    public int Price => _price;
    public Sprite IconSprite => _iconSprite;
    public Equipment EquipmentPrefab => _equipmentPrefab;
    public float BonusMaxHp => _bonusMaxHp;
    public float BonusDamage => _bonusDamage;
    public float BonusMoveSpeed => _bonusMoveSpeed;
    public float BonusItemGetRange => _bonusItemGetRange;
    public EquipSlotType EquipSlotType => _equipSlotType;
}
