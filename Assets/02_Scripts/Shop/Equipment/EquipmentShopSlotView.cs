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

    string _heroId;

    public event Action<string> OnBuyButtonClicked;

    /// <summary>
    /// 초기화 함수
    /// </summary>
    /// <param name="heroId">받아온 영웅의 Id</param>
    public void Initialize(string heroId)
    {
        // 내부
        _heroId = heroId;

        EquipmentConfig config = GameManager.Instance.DataManager.EquipmentMap[_heroId];
        _equipmentIcon.sprite = config.IconSprite;
        _priceText.text = $"{config.Price} 골드";
        _DescText.text = config.Description;

        _buyButton.onClick.AddListener(BuyButtonClick);
    }

    public void BuyButtonClick()
    {
        OnBuyButtonClicked?.Invoke(_heroId);
        gameObject.SetActive(false);
    }
}
