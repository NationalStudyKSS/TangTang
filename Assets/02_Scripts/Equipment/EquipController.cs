using UnityEngine;
using System.Collections.Generic;

public class EquipController : MonoBehaviour
{
    Dictionary<EquipSlotType, Equipment> _equipped = new();

    [SerializeField] Transform[] _slotTransforms;
    [SerializeField] EquipmentView[] _equipmentViews;

<<<<<<< HEAD
=======
    /// <summary>
    /// 무기 장착 이벤트. HitPoint 전달
    /// </summary>
    public event Action<Transform> OnWeaponEquipped;

    public void Initialize()
    {
        foreach (var equipmentView in _equipmentViews)
        {
            equipmentView.Initialize(this, _inventory);
            equipmentView.SetEquipment(null);
        }
    }

    /// <summary>
    /// 장비를 장착하는 함수
    /// 기존 장비가 있으면 자동으로 해제 후 새 장비를 장착한다.
    /// </summary>
    /// <param name="model"></param>
>>>>>>> parent of 32a5505 (컴파일 에러제거)
    public void Equip(EquipmentModel model)
    {
        EquipSlotType slotType = model.Config.EquipSlotType;
        int slotIndex = (int)slotType;

        // 기존 장비 해제
        UnEquip(slotType);

<<<<<<< HEAD
        var prefab = model.Config.EquipmentPrefab;
        if (prefab == null)
        {
            Debug.LogError("프리팹 없음");
            return;
        }

        var equipGO = GameObject.Instantiate(prefab, _slotTransforms[slotIndex]);
        var equipment = equipGO.GetComponent<Equipment>();
        equipment.SetModel(model);
        _equipped[slotType] = equipment;

        _equipmentViews[slotIndex].SetEquipment(equipment);
=======
        // 장비 프리펩 생성
        Transform slotTransform = _slotTransforms[slotIndex];
        Equipment equipment = Instantiate(model.EquipmentPrefab, slotTransform);
        equipment.SetEquipmentModel(model);
        _equipmentMap[slotType] = equipment;

        // 장비 스탯 적용
        GameManager.Instance.HeroManager.AddBonusHp(equipment.BonusMaxHp);
        GameManager.Instance.HeroManager.AddBonusDamage(equipment.BonusDamage);
        GameManager.Instance.HeroManager.AddBonusMoveSpeed(equipment.BonusMoveSpeed);
        GameManager.Instance.HeroManager.AddBonusItemGetRange(equipment.BonusItemGetRange);

        _equipmentViews[(int)slotType].Initialize(this, _inventory);
        _equipmentViews[(int)slotType].SetEquipment(equipment);
        equipment.EquipmentModel.SetSlotIndex((int)slotType);
        
>>>>>>> parent of 32a5505 (컴파일 에러제거)
    }

    public void UnEquip(EquipSlotType slotType)
    {
        if (_equipped.TryGetValue(slotType, out var equipment))
        {
<<<<<<< HEAD
            GameObject.Destroy(equipment.gameObject);
            _equipped.Remove(slotType);
            _equipmentViews[(int)slotType].SetEquipment(null);
        }
    }
=======
            Equipment equipment = _equipmentMap[slotType];

            // 1. 해당 장비와 연결된 아이템 모델을 인벤토리에 추가
            // -> 인벤토리에 아이템 추가 실패 시 장비 해제 불가
            if (_inventory.TryAddEquipment(equipment.EquipmentModel) == false) return;

            // 2. 장비로 인한 능력치 변화 해제
            _heroModel.AddMaxHp(-equipment.BonusMaxHp);
            _heroModel.AddArmor(-equipment.BonusArmor);
            _heroModel.AddDamage(-equipment.BonusDamage);

            // 3. 장비 제거
            Destroy(equipment.gameObject);

            // 4. 장비 맵에서 키 제거
            _equipmentMap.Remove(slotType);

            // 5. 무기의 경우 무기 제거 이벤트 알림
            if (slotType == EquipSlotType.Weapon)
            {
                OnWeaponEquipped?.Invoke(null);
            }
        }
    }

    public void ShowEquipmentView(EquipmentModel model, Transform slotTransform)
    {
        _itemDescView.SetEquipmentModel(model);
        _itemDescView.transform.position = slotTransform.position;
        _itemDescView.gameObject.SetActive(true);
    }

    /// <summary>
    /// 아이템 설명 뷰(툴팁)를 숨기는 함수
    /// </summary>
    public void HideEquipmentDescView()
    {
        _itemDescView.gameObject.SetActive(false);
    }
>>>>>>> parent of 32a5505 (컴파일 에러제거)
}
