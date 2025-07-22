using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipBlocker : MonoBehaviour
{
    [SerializeField] InventoryPresenter _presenter;

    public void OnClickOutside()
    {
        //_presenter.Toggle(null, Vector2.zero);
        gameObject.SetActive(false);
    }
}
