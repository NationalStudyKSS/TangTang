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
}
