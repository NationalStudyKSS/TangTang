using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 장비상점을 담당할 클래스
/// </summary>
public class EquipmentShopController : MonoBehaviour
{
    [SerializeField] EquipmentShopSlotView[] _views;

    public event Action<string> OnEquipmentBought;

    public void Initialize()
    {
        int index = 0;
        foreach (var heroId in GameManager.Instance.DataManager.EquipmentMap.Keys)
        {
            _views[index].Initialize(index, heroId);
            _views[index].OnBuyButtonClicked += BuyEquipment;
            index++;
        }
    }

    public void BuyEquipment(int slotIndex, string equipmentId)
    {
        var currency = GameManager.Instance.CurrencyManager;
        int price = GameManager.Instance.DataManager.EquipmentMap[equipmentId].Price;

        if (currency.Gold < price)
            return;

        currency.ChangeGold(-price);
        _views[slotIndex].gameObject.SetActive(false);
        OnEquipmentBought?.Invoke(equipmentId);
    }
}
