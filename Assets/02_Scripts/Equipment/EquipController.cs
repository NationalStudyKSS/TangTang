using UnityEngine;
using System.Collections.Generic;

public class EquipController : MonoBehaviour
{
    Dictionary<EquipSlotType, Equipment> _equipped = new();

    [SerializeField] Transform[] _slotTransforms;
    [SerializeField] EquipmentView[] _equipmentViews;

    public void Equip(EquipmentModel model)
    {
        EquipSlotType slotType = model.Config.EquipSlotType;
        int slotIndex = (int)slotType;

        // 기존 장비 해제
        UnEquip(slotType);

        var prefab = model.Config.EquipmentPrefab;
        if (prefab == null)
        {
            Debug.LogError("프리팹 없음");
            return;
        }

        var equipGO = GameObject.Instantiate(prefab, _slotTransforms[slotIndex]);
        var equipment = equipGO.GetComponent<Equipment>();
        equipment.SetModel(model);
        _equipped[slotType] = equipment;

        _equipmentViews[slotIndex].SetEquipment(equipment);
    }

    public void UnEquip(EquipSlotType slotType)
    {
        if (_equipped.TryGetValue(slotType, out var equipment))
        {
            GameObject.Destroy(equipment.gameObject);
            _equipped.Remove(slotType);
            _equipmentViews[(int)slotType].SetEquipment(null);
        }
    }
}
