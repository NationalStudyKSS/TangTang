using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 획득한 장비를 보관하는 인벤토리
/// </summary>
public class Inventory : MonoBehaviour
{
    public const int _inventoryCount = 20;

    [Header("----- 아이템 설정 데이터 -----")]
    [SerializeField] EquipmentConfig[] _EquipmentConfigs;      // 아이템 설정 데이터 배열
    Dictionary<string, EquipmentConfig> _EquipmentConfigMap = new();  // 아이템 설정 데이터 맵

    [Header("----- 컴포넌트 참조 -----")]
    [SerializeField] InventoryView[] _inventoryViews;         // 아이템 뷰 배열
    [SerializeField] EquipmentDescView _equipmentDescView;          // 아이템 툴팁 창
    [SerializeField] EquipController _equipController;              // 장비 장착 컨트롤러

    public EquipController EquipController => _equipController;

    // 유저가 보유하고 있는 아이템 배열
    EquipmentModel[] _equipmentModels = new EquipmentModel[_inventoryCount];

    // 현재 선택 중인 슬롯 번호
    int _selectedSlotIndex = -1;

    private void Awake()
    {
        // 장비 설정 데이터를 맵에 저장
        foreach (var equipmentConfig in _EquipmentConfigs)
        {
            _EquipmentConfigMap[equipmentConfig.Id] = equipmentConfig;
        }
        // 아이템 뷰 초기화
        for (int i = 0; i < _inventoryCount; i++)
        {
            _inventoryViews[i].SetModel(null);
            _inventoryViews[i].OnViewClicked += _equipmentDescView.Toggle;
        }
    }

    private void Start()
    {
        for (int i = 0; i < _inventoryViews.Length; i++)
        {
            _inventoryViews[i].SetModel(_equipmentModels[i]);
            _inventoryViews[i].Initialize(this, i);
        }

        //_equipController.Initialize();
    }

    /// <summary>
    /// 아이템 설정 데이터로 아이템 모델을 만들어 반환해 주는 함수
    /// </summary>
    /// <param name="itemConfig"></param>
    /// <returns></returns>
    public EquipmentModel CreateEquipmentModel(EquipmentConfig itemConfig)
    {
        return new EquipmentModel(itemConfig);
    }

    /// <summary>
    /// 인덱스 번호로 모델 반환을 시도하는 함수
    /// </summary>
    /// <param name="slotIndex">슬롯의 인덱스 번호</param>
    /// <param name="model">반환할 모델</param>
    /// <returns>아이템 모델 존재 여부</returns>
    public bool TryGetEquipmentModel(int slotIndex, out EquipmentModel model)
    {
        model = null;

        if (slotIndex < 0 || slotIndex >= _inventoryViews.Length) return false;

        model = _equipmentModels[slotIndex];
        return model != null;
    }

    /// <summary>
    /// 최초로 인벤토리에 장비를 추가하는 함수
    /// 아이템 추가 시 인벤토리를 순회하여 빈자리를 찾아보고
    /// 빈자리가 있으면 로직이 실행된다.
    /// </summary>
    /// <param name="id">추가할 장비의 Id</param>
    public void AddEquipment(string id)
    {
        if (_EquipmentConfigMap.ContainsKey(id) == false)
        {
            Debug.Log($"존재하지 않는 아이템입니다. (id: {id})");
            return;
        }

        // 설정 데이터 검색
        EquipmentConfig config = _EquipmentConfigMap[id];

        // 인벤토리 순회
        for (int i = 0; i < _inventoryCount; i++)
        {
            // 만약 빈자리라면(모델이 존재하지 않기때문)
            // 빈자리가 있는지 판단하는 조건문으로도 작용함
            if (_equipmentModels[i] == null)
            {
                // 모델을 하나 만들어주고
                _equipmentModels[i] = CreateEquipmentModel(config);
                // View 초기화 해주고
                _inventoryViews[i].Initialize(this, i);
                // View에 모델 할당해주기
                _inventoryViews[i].SetModel(_equipmentModels[i]);
                // 아이템 하나 추가에 한번만 실행해야되니까 실행 후 리턴
                return;
            }
        }

        // 만약 위 for문을 거치지 않았을 경우(아이템이 꽉찼다는 소리)
        Debug.Log("인벤토리가 가득 찼습니다.");
    }

    /// <summary>
    /// 이미 존재하는 장비를 인벤토리에 추가 시도하는 함수
    /// </summary>
    /// <param name="model"></param>
    public bool TryAddEquipment(EquipmentModel model)
    {
        for (int i = 0; i < _inventoryCount; i++)
        {
            if (_equipmentModels[i] == null)
            {
                _equipmentModels[i] = model;
                model.SetSlotIndex(i);
                _inventoryViews[i].SetModel(model);
                return true;
            }
        }

        Debug.Log("인벤토리가 가득 찼습니다.");
        return false;
    }

    /// <summary>
    /// 인벤토리에서 아이템을 제거하는 함수
    /// </summary>
    /// <param name="slotIndex">제거하려는 인덱스 번호</param>
    public void RemoveEquipment(int slotIndex)
    {
        // 제거하려는 슬롯에 모델이 있는경우
        if (TryGetEquipmentModel(slotIndex, out EquipmentModel model) == true)
        {
            // 모델의 삭제 함수를 실행시켜주고
            model.Remove();
            // 지금 슬롯의 모델자리 청소해서 비워주고
            _equipmentModels[slotIndex] = null;
            // View에도 모델 비었다고 말해줘야함
            _inventoryViews[slotIndex].SetModel(_equipmentModels[slotIndex]);
        }
    }

    /// <summary>
    /// 장비 장착 버튼을 눌렀을 때 실행되야 하는 함수
    /// </summary>
    /// <param name="slotIndex"></param>
    public void OnEquipped(int slotIndex)
    {

    }

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
}
