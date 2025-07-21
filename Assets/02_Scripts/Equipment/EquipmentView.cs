using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 장비 장착 슬롯을 담당함
/// EquipmentType으로 구분함
/// 클릭 시 장비 설명
/// </summary>
public class EquipmentView : MonoBehaviour, IPointerClickHandler
{
    EquipController _equipController;

    Equipment _equipment;

    [SerializeField] EquipSlotType _slotType;
    [SerializeField] Image _iconImage;

    public void Initialize(EquipController equipController)
    {
        _equipController = equipController;
    }

    public void SetEquipment(Equipment equipment)
    {
        _equipment = equipment;

        if (_equipment != null)
        {
            _iconImage.sprite = _equipment.Model.Config.IconSprite;
            _iconImage.gameObject.SetActive(true);
        }
        else
        {
            _iconImage.gameObject.SetActive(false);
        }
    }

    public void Hide(bool isHidden)
    {
        _iconImage.enabled = !isHidden;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right) return;

        _equipController.UnEquip(_slotType);
        SetEquipment(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        if (_equipment == null) return;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }
}
