using UnityEngine;

public class InventoryPresenter
{
    private InventoryModel _model;
    private InventorySlotView[] _slotViews;
    private EquipController _equipController;

    public InventoryPresenter(InventoryModel model, InventorySlotView[] slotViews, EquipController equipController)
    {
        _model = model;
        _slotViews = slotViews;
        _equipController = equipController;

        for (int i = 0; i < _slotViews.Length; i++)
        {
            int index = i; // 클로저 문제 방지용
            _slotViews[i].Initialize(index);
            _slotViews[i].OnClicked += (slotIndex) => OnSlotClicked(index);
        }

        RefreshView();
    }

    private void RefreshView()
    {
        for (int i = 0; i < _slotViews.Length; i++)
        {
            var item = _model.GetItem(i);
            _slotViews[i].SetIcon(item?.Config.IconSprite);
        }
    }

    private void OnSlotClicked(int slotIndex)
    {
        var item = _model.GetItem(slotIndex);
        if (item != null)
        {
            // 장비 장착 요청
            _equipController.Equip(item);
            // 선택한 아이템에 대한 UI 처리, 툴팁 등도 여기서 호출 가능
        }
    }

    public void AddItem(EquipmentModel item)
    {
        if (_model.AddItem(item))
        {
            RefreshView();
        }
        else
        {
            Debug.Log("인벤토리 가득 참");
        }
    }

    public void RemoveItem(int slotIndex)
    {
        if (_model.RemoveItem(slotIndex))
        {
            RefreshView();
        }
    }
}
