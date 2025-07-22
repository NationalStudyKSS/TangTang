using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryModel
{
    private EquipmentModel[] _items;
    public int Capacity { get; private set; }

    public InventoryModel(int capacity)
    {
        Capacity = capacity;
        _items = new EquipmentModel[capacity];
    }

    public bool AddItem(EquipmentModel item)
    {
        for (int i = 0; i < Capacity; i++)
        {
            if (_items[i] == null)
            {
                _items[i] = item;
                item.SetSlotIndex(i);
                return true;
            }
        }
        return false;
    }

    public bool RemoveItem(int index)
    {
        if (index >= 0 && index < Capacity && _items[index] != null)
        {
            _items[index] = null;
            return true;
        }
        return false;
    }

    public EquipmentModel GetItem(int index)
    {
        if (index >= 0 && index < Capacity)
            return _items[index];
        return null;
    }
}
