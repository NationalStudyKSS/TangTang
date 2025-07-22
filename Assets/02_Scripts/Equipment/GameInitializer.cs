using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] InventorySlotView[] inventorySlots;
    [SerializeField] EquipController equipController;

    InventoryModel inventoryModel;
    InventoryPresenter inventoryPresenter;

    void Start()
    {
        inventoryModel = new InventoryModel(inventorySlots.Length);
        inventoryPresenter = new InventoryPresenter(inventoryModel, inventorySlots, equipController);

        // 예시 아이템 추가
        EquipmentConfig swordConfig = Resources.Load<EquipmentConfig>("Configs/2_Weapon");
        var swordModel = new EquipmentModel(swordConfig);

        inventoryPresenter.AddItem(swordModel);
    }
}

