using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;

/// <summary>
/// 장비 클릭시 나오는 설명창을 관리하는 클래스
/// </summary>
public class EquipmentDescView : MonoBehaviour
{
    const string _priceTextFormat = "가격: {0} 골드";

    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] TextMeshProUGUI _descText;
    [SerializeField] TextMeshProUGUI _priceText;
    [SerializeField] GameObject _blocker; // TooltipBlocker 오브젝트

    EquipmentModel _currentModel;
    bool isShowing = false;

    public void SetModel(EquipmentModel model)
    {
        _nameText.text = model.Config.EquipmentName;
        _descText.text = model.Config.Description;
        _priceText.text = string.Format(_priceTextFormat, model.Config.Price);
    }

    public void Toggle(EquipmentModel model, Vector2 pos)
    {
        if (model == null)
        {
            Hide();
            return;
        }


        if (isShowing && _currentModel == model)
        {
            Hide();
            return;
        }

        Show(model, pos);
    }

    public void Show(EquipmentModel model, Vector2 pos)
    {
        isShowing = true;
        _currentModel = model;
        SetModel(model);
        // 아래쪽 아이템을 누르면 짤리는 경우를 대비
        //Debug.Log(pos.y);
        if (pos.y < 240f)
        {
            pos.y = 240f;
        }
        transform.position = pos;
        gameObject.SetActive(true);
        _blocker?.SetActive(true); // blocker 활성화
    }

    public void Hide()
    {
        isShowing = false;
        _currentModel = null;
        gameObject.SetActive(false);
        _blocker?.SetActive(false); // blocker도 함께 닫기
    }
}
