using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;

/// <summary>
/// 뷰 클릭시 나오는 툴팁창을 관리하는 클래스.
/// 인벤토리 뷰에서 클릭 됐을 경우 장착 버튼이 활성화 되고
/// 장비 뷰에서 클릭 됐을 경우 해제 버튼이 활성화 된다.
/// </summary>
public class EquipmentDescView : MonoBehaviour
{
    const string _priceTextFormat = "가격: {0} 골드";

    [Header("----- 컴포넌트 참조 -----")]
    [SerializeField] TextMeshProUGUI _nameText;     // 장비 이름 텍스트
    [SerializeField] TextMeshProUGUI _descText;     // 장비 설명 텍스트
    [SerializeField] TextMeshProUGUI _priceText;    // 장비 가격 텍스트
    [SerializeField] GameObject _equipButton;       // 장착 버튼
    [SerializeField] GameObject _unequipButton;     // 해제 버튼

    EquipmentModel _model;

    public event Action<EquipmentModel> OnEquipButtonClicked;
    public event Action<EquipSlotType> OnUnequippButtonClicked;

    /// <summary>
    /// 모델을 설정하는 함수.
    /// 툴팁 창을 열 때마다 호출되어야 하며,
    /// 실제 기능은 모델을 받아와 UI에 표시해야 할
    /// 정보들을 갱신하는 역할을 한다.
    /// </summary>
    /// <param name="model">이번에 선택된 모델</param>
    public void RefreshUI(EquipmentModel model)
    {
        _model = model;
        // UI 정보 갱신
        _nameText.text = model.Config.EquipmentName;
        _descText.text = model.Config.Description;
        _priceText.text = string.Format(_priceTextFormat, model.Config.Price);

        // 장착여부에 따라(어떤 뷰에서 클릭되었냐에 따라)
        // 장착된 장비였으면 해제버튼만 필요하므로
        if (model.IsEquipped == true)
        {
            _equipButton.SetActive(false);
            _unequipButton.SetActive(true);
        }

        // 장착된 장비가 아니라면(인벤토리에 있던 장비) 장착버튼만 필요하므로
        if (model.IsEquipped == false)
        {
            _equipButton.SetActive(true);
            _unequipButton.SetActive(false);
        }
    }

    /// <summary>
    /// 툴팁창을 보여주는 함수.
    /// 모델과 위치값을 받아와
    /// 그에 해당하는 정보를 이용하여 UI를 갱신하고
    /// 원하는 위치에 띄운다.
    /// </summary>
    /// <param name="model">이번에 보여줄 장비의 정보모델</param>
    /// <param name="pos">띄울 위치</param>
    public void Show(EquipmentModel model, Vector2 pos)
    {
        RefreshUI(model);
        // 아래쪽 아이템을 누르면 짤리는 경우를 대비
        //Debug.Log(pos.y);
        if (pos.y < 240f)
        {
            pos.y = 240f;
        }
        transform.position = pos;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 툴팁창을 닫는 함수
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 장비 장착 버튼을 눌렀을 때 실행될 함수
    /// </summary>
    public void OnEquipped()
    {
        Hide();
        // 아이템 장착 버튼을 눌렀을 때 발행될 이벤트
        // EquipController가 구독하여 처리할 것
        OnEquipButtonClicked?.Invoke(_model);
    }

    public void OnUnequipped()
    {
        Hide();
        // 아이템 해제 버튼을 눌렀을 때 발행될 이벤트
        // EquipController가 구독하여 처리할 것
        OnUnequippButtonClicked?.Invoke(_model.Config.EquipSlotType);
    }
}
