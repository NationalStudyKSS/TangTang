using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 아이템을 떨어뜨리는(PoolManager 이용해서 꺼내오는) 역할
/// 단순히 생각해서 EnemySpawner의 Item 버전이라고 생각하면된다.
/// </summary>
public class ItemDropper : MonoBehaviour
{
    [Header("----- 드롭아이템 경로(읽기 전용) -----")]
    [SerializeField] string[] _dropItemPrefabPath; // 드롭 아이템 데이터에 적어놔야함

    [Header("----- 드롭아이템 목록(읽기 전용) -----")]
    [SerializeField] List<GameObject> _dropItems = new(); // 드롭 아이템 목록
    [SerializeField] Dictionary<int, GameObject> _dropItemMap = new(); // 드롭 아이템 ID와 게임 오브젝트 매핑 딕셔너리

    Coroutine _spawnEnemyRoutine;       // 적 생성 코루틴 변수

    public void DropItem(Vector3 position)
    {
        int dropItemId = GameManager.Instance.DataManager.DropItemPool.GetRandomItemId(DropPoolType.Normal);
        DropItemData itemData = GameManager.Instance.DataManager.DropItemDataDict[dropItemId];

        // 풀에서 해당 아이템 프리팹 꺼내오기
        GameObject itemGo = GameManager.Instance.PoolManager.GetFromPool(itemData.DropItemPrefabPath);
        if (itemGo == null)
        {
            Debug.LogError($"풀에서 {itemData.DropItemPrefabPath} 아이템을 가져올 수 없습니다.");
            return;
        }

        // 위치 설정 및 초기화
        itemGo.transform.position = position;

        // 드롭 아이템 초기화 (필요하다면)
        DropItem dropItem = itemGo.GetComponent<DropItem>();
        dropItem?.Initialize(itemData.ItemId); // itemData를 넘기면 효과 등 처리 가능
    }
    
}
