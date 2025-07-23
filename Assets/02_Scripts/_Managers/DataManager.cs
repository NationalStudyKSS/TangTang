using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

/// <summary>
/// 게임에서 사용하는 데이터들을 관리하는 매니저
/// </summary>
public class DataManager : MonoBehaviour
{
    [Header("----- 게임 데이터 (ReadOnly) -----")]
    [SerializeField] Dictionary<string, HeroStatData> _heroStatDataMap; // 주인공 능력치 데이터 딕셔너리
    [SerializeField] Dictionary<string, HeroMetaData> _heroMetaDataMap; // 주인공 메타 데이터
    [SerializeField] Dictionary<int, ActiveSkillData> _activeSkillDataDict; // 액티브 스킬 데이터를 Id로 관리하는 딕셔너리
    [SerializeField] Dictionary<PassiveSkillType, PassiveSkillData> _passiveSkillDataDict; // 패시브 스킬 데이터를 Id로 관리하는 딕셔너리

    [SerializeField] EnemyStatData _enemyStatData; // 적 능력치 데이터
    
    [SerializeField] DropItemPoolData _dropItemPool;    // 드롭 아이템 풀 데이터

    [SerializeField] Dictionary<int, DropItemData> _dropItemDataDict;    // 드롭 아이템 데이터를 ID로 관리하는 딕셔너리

    [SerializeField] EquipmentConfig[] _equipmentConfigs;

    HeroStatData _heroStatData;
    ResourceManager _resourceManager; // 리소스 매니저

    public Dictionary<string, HeroStatData> HeroStatDataMap => _heroStatDataMap;
    public Dictionary<string, HeroMetaData> HeroMetaDataMap => _heroMetaDataMap;
    public Dictionary<int, ActiveSkillData> ActiveSkillDataDict => _activeSkillDataDict; // 액티브 스킬 데이터 딕셔너리 접근 프로퍼티
    public Dictionary<PassiveSkillType, PassiveSkillData> PassiveSkillDataDict => _passiveSkillDataDict; // 패시브 스킬 데이터 딕셔너리 접근 프로퍼티
    public EnemyStatData EnemyStatData => _enemyStatData; // 적 능력치 데이터 접근 프로퍼티
    public DropItemPoolData DropItemPool => _dropItemPool; // 드롭 아이템 풀 데이터 접근 프로퍼티
    public Dictionary<int, DropItemData> DropItemDataDict => _dropItemDataDict; // 드롭 아이템 데이터 딕셔너리 접근 프로퍼티
    public EquipmentConfig[] EquipmentConfigs => _equipmentConfigs;
    public HeroStatData HeroStatData => _heroStatData; // 주인공 능력치 데이터 접근 프로퍼티

    public static DataManager _instance;

    public void Initialize(ResourceManager resourceManager)
    {
        // 리소스매니저 연결해주고
        _resourceManager = resourceManager;

        // 각종 데이터들을 리소스 매니저를 통해 로드
        _enemyStatData = Resources.Load<EnemyStatData>("Data/Enemy/EnemyStatData");
        _dropItemPool = Resources.Load<DropItemPoolData>("Data/DropItem/DropItemPoolData");

        // 스탯 데이터 배열 로드
        HeroStatData[] heroStatDataArray = Resources.LoadAll<HeroStatData>("Data/Hero");
        // 딕셔너리 초기화
        _heroStatDataMap = new Dictionary<string, HeroStatData>();
        // 딕셔너리 순회하면서 매핑
        foreach (var data in heroStatDataArray)
        {
            _heroStatDataMap[data.HeroId] = data;
        }
        
        HeroMetaData[] heroMetaDataArray = Resources.LoadAll<HeroMetaData>("Data/Hero");
        _heroMetaDataMap = new Dictionary<string, HeroMetaData>();
        foreach (var data in heroMetaDataArray)
        {
            _heroMetaDataMap[data.HeroId] = data;
        }

        // 액티브 스킬 데이터 배열 로드 후 딕셔너리로 변환
        ActiveSkillData[] activeSkillArray = Resources.LoadAll<ActiveSkillData>("Data/HeroSkill/ActiveSkill");
        // 액티브 스킬 데이터 딕셔너리 초기화(리스트 순회하면서 매핑해줌)
        _activeSkillDataDict = new Dictionary<int, ActiveSkillData>();
        foreach (var data in activeSkillArray)
        {
            // 액티브 스킬 데이터의 ID를 키로 사용하여 딕셔너리에 추가
            _activeSkillDataDict[data.Id-1] = data;
        }

        // 패시브 스킬 데이터 배열 로드 후 딕셔너리로 변환
        PassiveSkillData[] passiveSkillArray = Resources.LoadAll<PassiveSkillData>("Data/HeroSkill/PassiveSkill");
        // 패시브 스킬 데이터 딕셔너리 초기화(리스트 순회하면서 매핑해줌)
        _passiveSkillDataDict = new Dictionary<PassiveSkillType, PassiveSkillData>();
        foreach (var data in passiveSkillArray)
        {
            // 패시브 스킬 데이터의 ID를 키로 사용하여 딕셔너리에 추가
            _passiveSkillDataDict[data.SkillType] = data;
        }

        // 드롭 아이템 데이터 배열 로드 후 리스트로 변환
        DropItemData[] dropItemArray = Resources.LoadAll<DropItemData>("Data/DropItem");
        // 드롭 아이템 데이터 딕셔너리 초기화(리스트 순회하면서 매핑해줌)
        _dropItemDataDict = new Dictionary<int, DropItemData>();
        foreach (var data in dropItemArray)
            _dropItemDataDict[data.ItemId] = data;

        _equipmentConfigs = Resources.LoadAll<EquipmentConfig>("Data/Equipment");
    }

    public void SetHeroData(string heroId)
    {
        _heroStatData = _heroStatDataMap[heroId];
    }
}
