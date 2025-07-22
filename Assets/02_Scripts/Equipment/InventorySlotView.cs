using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// 인벤토리에서 하나의 슬롯에 해당하는 클래스.
/// 장비 아이콘과 슬롯번호를 갖고있다.
/// 클릭 시 툴팁창을 띄우는 역할까지 한다.
/// </summary>
public class InventorySlotView : MonoBehaviour, IPointerClickHandler
{
    [Header("----- 컴포넌트 참조 -----")]
    [SerializeField] Image _iconImage;

    [Header("----- 읽기 전용 -----")]
    [SerializeField] int _slotIndex;

    public event Action<int, Vector2> OnClicked;     // 클릭 시 자기의 슬롯 번호와 함께 이벤트발행

    /// <summary>
    /// 초기화 함수.
    /// 각 슬롯에 번호를 부여해주고 장비 아이콘을 비워준다.
    /// </summary>
    /// <param name="slotIndex"></param>
    public void Initialize(int slotIndex)
    {
        _slotIndex = slotIndex;
        
        SetIcon(_slotIndex, null);
    }

    /// <summary>
    /// 아이콘 설정 함수.
    /// 아이콘이 있다면 활성화 후 아이콘을 보여주고
    /// 아이콘이 없다면 비활성화 한다.
    /// </summary>
    /// <param name="slotIndex">인덱스 일치 여부 확인용</param>
    /// <param name="model">아이콘으로 설정할 장비 모델</param>
    public void SetIcon(int slotIndex, EquipmentModel model)
    {
        if (_slotIndex != slotIndex) return;

        if (model == null)
        {
            _iconImage.sprite = null;
            _iconImage.enabled = false;
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
        // 만약 슬롯인덱스가 부여되지 않았다면 리턴
        if (_slotIndex == -1) return;

        // 뷰 클릭 시 자신의 슬롯인덱스를 매개변수로 이벤트를 발행한다.
        // 이 이벤트는 EquipmentDescView가 구독하여 툴팁 창을 띄울 것이다.
        OnClicked?.Invoke(_slotIndex, transform.position);
    }
}
