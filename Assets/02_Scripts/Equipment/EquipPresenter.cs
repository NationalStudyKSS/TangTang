using UnityEngine;

public class EquipPresenter : MonoBehaviour 
{
    [Header("----- 컴포넌트 참조 -----")]
    [SerializeField] EquipController _equipController;
    [SerializeField] EquipmentView[] _equipmentViews;
    [SerializeField] EquipmentDescView _equipmentDescView;

    EquipmentModel[] _models;

    public void Initialize(Inventory inventory, EquipmentModel[] models)
    {
        _models = models;

        for (int i = 0; i < _equipmentViews.Length; i++)
        {
            int index = i;
            _equipmentViews[i].Initialize();

            _equipmentViews[i].OnClicked += _equipmentDescView.Show;
            _equipmentDescView.OnEquipButtonClicked += (model) =>
            {
                foreach (var view in _equipmentViews)
                {
                    if (view.SlotType == model.Config.EquipSlotType)
                    {
                        view.SetIcon(model);
                        break;
                    }
                }
            };
            _equipmentDescView.OnUnequippButtonClicked += (slotType) =>
            {
                foreach (var view in _equipmentViews)
                {
                    if (view.SlotType == slotType)
                    {
                        view.SetIcon(null);
                        break;
                    }
                }
            };
        }

        _equipController.Initialize(inventory);
        _equipmentDescView.OnEquipButtonClicked += _equipController.Equip;
        _equipmentDescView.OnEquipButtonClicked += inventory.RemoveEquipment;
        
        _equipmentDescView.OnUnequippButtonClicked += _equipController.Unequip;
    }
}
