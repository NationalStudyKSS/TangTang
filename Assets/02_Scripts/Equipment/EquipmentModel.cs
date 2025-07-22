using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 모델(런타임 데이터 + 비즈니스 로직)
/// 인벤토리에 포함되기 전에는 설정 데이터만 갖는다.
/// </summary>
public class EquipmentModel
{
    EquipmentConfig _config;
    int _slotIndex = -1;
    bool _isEquipped = false;

    public EquipmentConfig Config => _config;
    public int SlotIndex => _slotIndex;
    public bool IsEquipped => _isEquipped;

    public EquipmentModel(EquipmentConfig config)
    {
        _config = config;
    }

    public void SetSlotIndex(int index)
    {
        _slotIndex = index;
    }

    public void SetIsEquipped(bool isEquipped) => _isEquipped = isEquipped;
}
