using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipBlocker : MonoBehaviour
{
    [SerializeField] EquipmentDescView _descView;

    public void OnClickOutside()
    {
        _descView.Toggle(null, Vector2.zero);
        gameObject.SetActive(false);
    }
}
