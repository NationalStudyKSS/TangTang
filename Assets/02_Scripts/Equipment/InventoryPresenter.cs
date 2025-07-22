using System;
using UnityEngine;

/// <summary>
/// 인벤토리뷰와 장비 모델 사이의 상호작용에 대한 처리를 담당할 클래스
/// 의존성 주입, 상호작용, 함수 실행 등등 여러가지를 처리한다.
/// MVP 패턴의 P에 해당한다.
/// </summary>
public class InventoryPresenter : MonoBehaviour
{
    [Header("----- 컴포넌트 참조 -----")]
    [SerializeField] InventorySlotView[] _slotViews;
    [SerializeField] EquipmentDescView _equipmentDescView;

    EquipmentModel[] _models;
    
    public void Initialize(Inventory inventory, EquipmentModel[] models)
    {
        _models = models;

        for (int i = 0; i < _slotViews.Length; i++)
        {
            // 슬롯 각각에 인덱스 번호 부여 및 아이콘 초기화
            int index = i;
            _slotViews[i].Initialize(index);
            inventory.OnSlotChanged += _slotViews[i].SetIcon;

            _slotViews[i].OnClicked += (slotIndex, pos) =>
            {
                var model = GetModelByIndex(slotIndex, models);
                if (model != null)
                    _equipmentDescView.Show(model, pos);
            };
        }
    }

    EquipmentModel GetModelByIndex(int slotIndex, EquipmentModel[] models)
    {
        if (slotIndex >= 0 && slotIndex < models.Length)
            return models[slotIndex];
        return null;
    }
}
