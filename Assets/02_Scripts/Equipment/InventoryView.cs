using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 인벤토리 슬롯을 담당함
/// slotIndex로 각 슬롯을 구분함
/// 클릭 시 장비 설명
/// </summary>
public class InventoryView : MonoBehaviour, IPointerClickHandler
{
    Inventory _inventory;       
    int _slotIndex;

    EquipmentModel _model;
    [SerializeField] Image _iconImage;

    public EquipmentModel Model => _model;

    public event Action<EquipmentModel, Vector2> OnViewClicked;

    public void Initialize(Inventory inventory, int slotIndex)
    {
        _inventory = inventory;
        _slotIndex = slotIndex;
    }

    /// <summary>
    /// 모델을 설정해주는 함수
    /// 인벤토리에 표시해줄 정보들이 필요하기때문
    /// </summary>
    /// <param name="model">설정할 모델</param>
    public void SetModel(EquipmentModel model)
    {
        _model = model;

        if (model != null)
        {
            _iconImage.sprite = _model.Config.IconSprite;
            _iconImage.gameObject.SetActive(true);
        }
        else
        {
            _iconImage.gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnViewClicked?.Invoke(_model, eventData.position);
    }
}
