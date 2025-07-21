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
/// 장비 클래스
/// </summary>
public class Equipment : MonoBehaviour
{
    [Header("----- 장비 슬롯 -----")]
    [SerializeField] EquipSlotType _equipSlotType;

    [Header("----- 장비 스탯 -----")]
    [SerializeField] float _bonusMaxHp;
    [SerializeField] float _bonusDamage;
    [SerializeField] float _bonusMoveSpeed;
    [SerializeField] float _bonusItemGetRange;

    // 이 장비가 어떤 아이템에서 비롯된 것인지
    EquipmentModel _model;

    public EquipSlotType EquipSlotType => _equipSlotType;

    public float BonusMaxHp => _bonusMaxHp;
    public float BonusDamage => _bonusDamage;
    public float BonusMoveSpeed => _bonusMoveSpeed;
    public float BonusItemGetRange => _bonusItemGetRange;

    public EquipmentModel Model => _model;

    public void SetModel(EquipmentModel model)
    {
        _model = model;
    }
}
