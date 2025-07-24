using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 장비상점 아이템 슬롯의 각 뷰를 담당할 클래스
/// </summary>
public class EquipmentShopSlotView : MonoBehaviour
{
    [SerializeField] Image _equipmentIcon;
    [SerializeField] Button _buyButton;
    [SerializeField] TextMeshProUGUI _priceText;
    [SerializeField] TextMeshProUGUI _DescText;

    int _slotIndex;
    string _equipmentId;

    public event Action<int, string> OnBuyButtonClicked;

    /// <summary>
    /// 초기화 함수
    /// </summary>
    /// <param name="EquipmentId">받아온 장비의 Id</param>
    public void Initialize(int slotIndex, string equipmentId)
    {
        _slotIndex = slotIndex;
        _equipmentId = equipmentId;

        EquipmentConfig config = GameManager.Instance.DataManager.EquipmentMap[_equipmentId];
        _equipmentIcon.sprite = config.IconSprite;
        _priceText.text = $"{config.Price} 골드";
        _DescText.text = config.Description;

        _buyButton.onClick.AddListener(BuyButtonClick);
    }

    public void BuyButtonClick()
    {
        OnBuyButtonClicked?.Invoke(_slotIndex, _equipmentId);
    }
}
