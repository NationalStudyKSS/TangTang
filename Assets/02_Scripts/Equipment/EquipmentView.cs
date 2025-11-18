using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentView : MonoBehaviour, IPointerClickHandler
{
    [Header("----- 컴포넌트 참조 -----")]
    [SerializeField] Image _iconImage;  // 아이콘 이미지
    [SerializeField] EquipSlotType _slotType;
    
    EquipmentModel _model;
    Sprite _basicSprite; // 장비창이 비어있을 때의 기본 스프라이트

    public EquipSlotType SlotType => _slotType;

    public event Action<EquipmentModel, Vector2> OnClicked;  

    void Awake()
    {
        _basicSprite = _iconImage.sprite;
        if(_basicSprite == null)
        {
            Debug.LogWarning($"{gameObject.name}의 기본 스프라이트가 설정되지 않았습니다.");
        }
    }

    /// <summary>
    /// 초기화 함수.
    /// 아이콘을 비워준다.
    /// </summary>
    public void Initialize()
    {
        SetIcon(null);
    }

    /// <summary>
    /// 아이콘 설정 함수
    /// </summary>
    /// <param name="model"></param>
    public void SetIcon(EquipmentModel model)
    {
        if (model != null && model.Config.EquipSlotType != _slotType)
            return; // 슬롯 타입 불일치 시 무시

        _model = model;

        if (model == null)
        {
            _iconImage.sprite = null;
            // Todo : 슬롯별 기본 스프라이트로 설정하기

            return;
        }

        Sprite icon = model.Config.IconSprite;

        if (icon != null)
        {
            _iconImage.sprite = icon;
            _iconImage.enabled = true;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 만약 슬롯 타입이 없다면 리턴
        if (_slotType == EquipSlotType.None) return;

        // 뷰 클릭 시 자신의 슬롯인덱스를 매개변수로 이벤트를 발행한다.
        // 이 이벤트는 EquipmentDescView가 구독하여 툴팁 창을 띄울 것이다.
        OnClicked?.Invoke(_model, eventData.position);
    }
}