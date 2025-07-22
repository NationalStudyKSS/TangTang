using UnityEngine;
using UnityEngine.UI;

public class EquipmentView : MonoBehaviour
{
    [SerializeField] Image _iconImage;

    public void SetEquipment(Equipment equipment)
    {
        if (equipment != null)
        {
            _iconImage.sprite = equipment.Model.Config.IconSprite;
            _iconImage.enabled = true;
        }
        else
        {
            _iconImage.enabled = false;
        }
    }
<<<<<<< HEAD
=======

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
        _invento

        if (_equipment == null) return;
        _dragController.ShowTooltip(_equipment.ItemModel);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _dragController.HideTooltip();
    }
>>>>>>> parent of 32a5505 (컴파일 에러제거)
}
