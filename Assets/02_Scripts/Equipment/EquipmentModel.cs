using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 모델(런타임 데이터 + 비즈니스 로직)
/// 인벤토리에 포함되기 전에는 설정 데이터만 갖는다.
/// </summary>
public class EquipmentModel
{
    EquipmentConfig _config;  // 아이템 설정 데이터
    Inventory _inventory;
    int _slotIndex = -1;

    public EquipmentConfig Config => _config;  // 아이템 설정 데이터 접근자
    public int SlotIndex => _slotIndex;
    public EquipmentModel(EquipmentConfig config)
    {
        _config = config;
    }

    public void SetSlotIndex(int slotIndex)
    {
        _slotIndex = slotIndex;
    }

    /// <summary>
    /// 아이템이 인벤토리에 추가될 때 자동으로 호출되어야 하는 함수
    /// 비소모성 아이템의 경우 패시브 효과를 적용한다.
    /// </summary>
    /// <param name="inventory"></param>
    public void Acquire(Inventory inventory, int slotIndex)
    {
        _inventory = inventory;
        _slotIndex = slotIndex;
    }

    /// <summary>
    /// 아이템이 인벤토리에서 제거될 때 자동으로 호출되어야 하는 함수
    /// 비소모성 아이템의 경우 패시브 효과를 해제한다.
    /// </summary>
    public void Remove()
    {
        _slotIndex = -1;
        if (_inventory == null) return;
    }

    Equipment _equipmentPrefab;
    public Equipment EquipmentPrefab => _equipmentPrefab;

    public void Use()
    {
        if (_inventory == null) return;

        // 장비 장착
        //_inventory.EquipController.Equip(this);
    }
}
