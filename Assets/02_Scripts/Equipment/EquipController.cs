using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// 장비 장착/해제를 담당하는 Controller (MVP의 C 역할)
/// </summary>
public class EquipController : MonoBehaviour
{
    Inventory _inventory;      // 아이템을 뺄 때 필요함.
    const int _equipSlotCount = 6;

    // 장비 슬롯 딕셔너리 => 장비 장착 시 슬롯타입을 키로 장비 모델을 등록해놓음
    // 장비 스탯 데이터만 필요하고 실물은 필요없어서 장비 모델을 사용함.
    // 추후에 실제 프리펩 형태의 게임오브젝트가 필요하다면 Equipment 클래스를 사용하여
    // 딕셔너리를 만드는게 좋을듯.
    Dictionary<EquipSlotType, EquipmentModel> _equipmentMap = new();
    
    public void Initialize(Inventory inventory)
    {
        _inventory = inventory;
    }

    /// <summary>
    /// 장비를 장착하는 함수.
    /// 기존에 장착된 장비가 있으면 자동으로 해제 후 새 장비를 장착한다.
    /// </summary>
    /// <param name="model">받아온 장비 모델</param>
    public void Equip(EquipmentModel model)
    {
        EquipSlotType slotType = model.Config.EquipSlotType;
        int slotIndex = (int)slotType;

        if (slotIndex < 0 || slotIndex >= _equipSlotCount) return;

        // 기존 장비가 있었다면 해제
        Unequip(slotType);

        // 장비 슬롯 딕셔너리에 등록
        _equipmentMap[slotType] = model;

        // 장비 스탯 적용
        GameManager.Instance.HeroManager.AddBonusHp(model.Config.BonusMaxHp);
        GameManager.Instance.HeroManager.AddBonusDamage(model.Config.BonusDamage);
        GameManager.Instance.HeroManager.AddBonusMoveSpeed(model.Config.BonusMoveSpeed);
        GameManager.Instance.HeroManager.AddBonusItemGetRange(model.Config.BonusItemGetRange);

        // 장비 모델 세팅
        model.SetIsEquipped(true);
    }

    /// <summary>
    /// 장비장착 슬롯의 타입을 받아서 장비 모델을 딕셔너리에서 검색 후
    /// 장비를 해제하는 함수.
    /// </summary>
    /// <param name="slotType">해제하려는 슬롯의 타입</param>
    public void Unequip(EquipSlotType slotType)
    {
        // 만약 장비 슬롯 딕셔너리에서 슬롯타입에 Value가 있으면(= 장비 모델이 있으면)
        if (_equipmentMap.ContainsKey(slotType))
        {
            // slotType을 키값으로 하는 Value, 즉 EquipmentModel을 찾는다.
            EquipmentModel model = _equipmentMap[slotType];

            // 혹시 장비를 해제하려고 했는데 장비창이 꽉 차서 Try함수가 실패하면 리턴
            if (_inventory.TryAddEquipment(model) == false)
            {
                Debug.LogWarning("인벤토리에 공간이 없어 장비 해제에 실패했습니다.");
                return;
            }

            // 장비 스탯 해제(스탯 증가에서 -부호를 붙여서 스탯 감소 = 해제)
            GameManager.Instance.HeroManager.AddBonusHp(-model.Config.BonusMaxHp);
            GameManager.Instance.HeroManager.AddBonusDamage(-model.Config.BonusDamage);
            GameManager.Instance.HeroManager.AddBonusMoveSpeed(-model.Config.BonusMoveSpeed);
            GameManager.Instance.HeroManager.AddBonusItemGetRange(-model.Config.BonusItemGetRange);

            // 딕셔너리에서 키 제거
            _equipmentMap.Remove(slotType);

            // 장비 모델 세팅
            model.SetIsEquipped(false);
        }
    }
}
