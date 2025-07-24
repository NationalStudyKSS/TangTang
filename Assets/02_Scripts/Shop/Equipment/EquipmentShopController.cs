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
            _views[index].Initialize(heroId);
            _views[index].OnBuyButtonClicked += GameManager.Instance.CurrencyManager.ChangeGold;
            index++;
        }
    }

    public void BuyEquipment(int price)
    {
        GameManager.Instance.CurrencyManager.ChangeGold(-price);
        OnEquipmentBought?.Invoke()
    }
}
