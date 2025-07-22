using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class InventorySlotView : MonoBehaviour, IPointerClickHandler
{
    public event Action<int> OnClicked;  // 슬롯 클릭 이벤트

    [SerializeField] private Image _iconImage;

    private int _slotIndex;

    public void Initialize(int slotIndex)
    {
        _slotIndex = slotIndex;
        SetIcon(null);
    }

    public void SetIcon(Sprite icon)
    {
        if (icon != null)
        {
            _iconImage.sprite = icon;
            _iconImage.enabled = true;
        }
        else
        {
            _iconImage.enabled = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClicked?.Invoke(_slotIndex);
    }
}
