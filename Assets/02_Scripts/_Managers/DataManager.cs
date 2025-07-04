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
    [SerializeField] HeroStatData _heroStatData; // 주인공 능력치 데이터
    [SerializeField] EnemyStatData _enemyStatData; // 적 능력치 데이터
    [SerializeField] DropItemPoolData _dropItemPool;    // 드롭 아이템 풀 데이터
    [SerializeField] List<DropItemData> _dropItemList;  // 드롭 아이템 데이터 리스트
    [SerializeField] Dictionary<int, DropItemData> _dropItemDataDict;    // 드롭 아이템 데이터를 ID로 관리하는 딕셔너리

    ResourceManager _resourceManager; // 리소스 매니저

    public HeroStatData HeroStatData => _heroStatData; // 주인공 능력치 데이터 접근 프로퍼티
    public EnemyStatData EnemyStatData => _enemyStatData; // 적 능력치 데이터 접근 프로퍼티
    public DropItemPoolData DropItemPool => _dropItemPool; // 드롭 아이템 풀 데이터 접근 프로퍼티
    public List<DropItemData> DropItemDataList => _dropItemList; // 드롭 아이템 데이터 리스트 접근 프로퍼티
    public Dictionary<int, DropItemData> DropItemDataDict => _dropItemDataDict; // 드롭 아이템 데이터 딕셔너리 접근 프로퍼티

    public static DataManager _instance;

    public void Initialize(ResourceManager resourceManager)
    {
        // 리소스매니저 연결해주고
        _resourceManager = resourceManager;

        // 각종 데이터들을 리소스 매니저를 통해 로드
        _heroStatData = Resources.Load<HeroStatData>("Data/Hero/HeroStatData");
        _enemyStatData = Resources.Load<EnemyStatData>("Data/Enemy/EnemyStatData");
        _dropItemPool = Resources.Load<DropItemPoolData>("Data/DropItem/DropItemPoolData");

        // 드롭 아이템 데이터 배열 로드 후 리스트로 변환
        DropItemData[] dropItemArray = Resources.LoadAll<DropItemData>("Data/DropItem");
        _dropItemList = new List<DropItemData>(dropItemArray);

        // 드롭 아이템 데이터 딕셔너리 초기화(리스트 순회하면서 매핑해줌)
        _dropItemDataDict = new Dictionary<int, DropItemData>();
        foreach (var data in _dropItemList)
            _dropItemDataDict[data.ItemId] = data;
    }
}
