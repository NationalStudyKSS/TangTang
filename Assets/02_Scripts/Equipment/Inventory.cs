using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인벤토리 시스템의 모든 것을 담는 클래스
/// 아이템의 첫 생성이 이루어지고 이에 따라 아이템의 정보도 갖고있어야 한다.
/// </summary>
public class Inventory : MonoBehaviour
{
    const int _inventoryCount = 20;

    [Header("----- 장비 설정 데이터(ReadOnly) -----")]
    [SerializeField] EquipmentConfig[] _configs;
    
    Dictionary<string, EquipmentConfig> _configMap = new();
    // 유저가 보유중인 장비의 모델인데 Equipment Class를 받을지 고민해봐야함
    EquipmentModel[] _equipmentModels = new EquipmentModel[_inventoryCount];

    public EquipmentModel[] EquipmentModels => _equipmentModels;

    public event Action<int, EquipmentModel> OnSlotChanged;      // 자기 슬롯 번호와 장비 모델을 매개변수로 전달

    public void Initialize()
    {
        if(GameManager.Instance?.DataManager == null)
    {
            Debug.LogError("DataManager 초기화 안됨");
            return;
        }

        _configs = GameManager.Instance.DataManager.EquipmentConfigs;

        if (_configs == null || _configs.Length == 0)
        {
            Debug.LogWarning("장비 설정 데이터가 비어있음");
            return;
        }

        _configMap.Clear();
        foreach (var config in _configs)
        {
            _configMap[config.Id] = config;
        }
    }

    // 임시) 장비 추가 치트
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            AddEquipment("Helmet");
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            AddEquipment("Armor");
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            AddEquipment("Weapon");
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            AddEquipment("Gloves");
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            AddEquipment("Boots");
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            AddEquipment("Ring");
        }
    }

    /// <summary>
    /// EquipmentModel을 생성하여 반환하는 함수
    /// </summary>
    /// <param name="config">생성할 장비의 정보</param>
    /// <returns>생성된 장비 모델</returns>
    public EquipmentModel CreateEquipmentModel(EquipmentConfig config)
    {
        return new EquipmentModel(config);
    }

    /// <summary>
    /// 장비의 Id를 이용하여 장비를 하나 추가하는 함수
    /// </summary>
    /// <param name="id">추가할 장비의 Id</param>
    public void AddEquipment(string id)
    {
        // 만약 딕셔너리에 등록된 id가 아니라면 리턴
        if (_configMap.ContainsKey(id) == false)
        {
            Debug.Log($"존재하지 않는 아이템입니다. (id: {id})");
            return;
        }

        // 입력받은 Id를 통해 딕셔너리에서 아이템 정보 찾고
        EquipmentConfig config = _configMap[id];

        // 인벤토리 용량을 이용해 인벤토리 슬롯 다 돌면서
        for (int i = 0; i < _inventoryCount; i++)
        {
            // 만약 슬롯의 모델이 없다면(= 슬롯이 비어있다면)
            if (_equipmentModels[i] == null)
            {
                // 처음으로 장비가 등록되어야 하니까 생성해주고
                _equipmentModels[i] = CreateEquipmentModel(config);

                // 장비모델에 인덱스 등록해주고
                _equipmentModels[i].SetSlotIndex(i);

                // 인벤토리에 생성된거니까 장착여부 false로 해주고
                _equipmentModels[i].SetIsEquipped(false);

                // 슬롯 바뀌었다고 이벤트 알림
                // InventorySlotView가 구독하여 아이템 보여줄거임
                OnSlotChanged?.Invoke(i, _equipmentModels[i]);

                // 하나 추가했으면 끝
                return;
            }
        }

        Debug.Log("인벤토리가 가득 찼습니다.");
    }

    /// <summary>
    /// 인벤토리에 장비 추가를 시도하는 함수
    /// </summary>
    /// <param name="model">추가하려는 장비 모델</param>
    /// <returns>성공했나요?</returns>
    public bool TryAddEquipment(EquipmentModel model)
    {
        // 인벤토리 순회하면서
        for (int i = 0; i < _equipmentModels.Length; i++)
        {
            // 빈공간이 있으면(= 장비 모델이 비어있으면)
            if (_equipmentModels[i] == null)
            {
                // 그 자리에 지금 추가하려는 장비 모델 넣고
                _equipmentModels[i] = model;
                // 슬롯 인덱스 다시 부여해주고
                _equipmentModels[i].SetSlotIndex(i);
                // 인벤토리에 들어온거니까 장착여부 false로 해주고
                _equipmentModels[i].SetIsEquipped(false);
                // 슬롯 정보 바뀌었으니까 이벤트 알림
                OnSlotChanged?.Invoke(i, model);
                // 추가 되었으니까 true 반환
                return true;
            }
        }

        // 위 조건을 만족 못했으면 인벤토리가 가득찬거니까
        Debug.Log("아이템 슬롯이 가득 찼습니다.");
        // false 반환
        return false;
    }

    /// <summary>
    /// 장비를 제거하는 함수
    /// </summary>
    /// <param name="slotIndex"></param>
    public void RemoveEquipment(EquipmentModel model)
    {
        int slotIndex = model.SlotIndex;
        if (_equipmentModels[slotIndex] == null) return;

        _equipmentModels[slotIndex] = null;
        OnSlotChanged?.Invoke(slotIndex, _equipmentModels[slotIndex]);
    }
}
