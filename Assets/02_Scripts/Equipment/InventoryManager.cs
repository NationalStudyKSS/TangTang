using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인벤토리 시스템을 총괄하는 역할을 할 매니저
/// </summary>
public class InventoryManager : MonoBehaviour
{
    [Header("----- 컴포넌트 참조 -----")]
    [SerializeField] InventoryPresenter _inventoryPresenter;
    [SerializeField] EquipPresenter _equipPresenter;
    [SerializeField] Inventory _inventory;
    [SerializeField] BonusStatUI _bonusStatUI;

    public Inventory Inventory => _inventory;

    public void Initialize()
    {
        _inventoryPresenter.Initialize(_inventory, _inventory.EquipmentModels);
        _equipPresenter.Initialize(_inventory, _inventory.EquipmentModels);
        _inventory.Initialize();
        _bonusStatUI.Initialize();
    }
}
