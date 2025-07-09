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
    //[Header("----- 드롭아이템 목록(읽기 전용) -----")]
    //[SerializeField]
    List<DropItem> _currentDropItems = new(); // 드롭 아이템 목록

    Coroutine _spawnEnemyRoutine;       // 적 생성 코루틴 변수
    Coroutine _magnetEffectRoutine; // 자석 아이템 효과 코루틴 변수
    DropItem _dropItem; // 드롭 아이템 변수
    Transform _hero; // 영웅의 Transform 컴포넌트 변수

    public List<DropItem> CurrentDropItems => _currentDropItems;

    public void Initialize(Transform hero)
    {
        _hero = hero;

        // 자석 아이템 사용 이벤트 연결
        DropItemManager.OnMagnetItemUsed += UseMagnetItem;
    }

    /// <summary>
    /// 적이 죽었을 때 아이템을 드롭하는 함수
    /// </summary>
    /// <param name="position"></param>
    public void DropItem(Enemy enemy)
    {
        // 게임매니저에서 적 타입에 따라 dropItemId를 가져온다.
        // 다만 지금 일반적'만' '랜덤'한 아이템을 '한 개' 가져오므로
        // 일반적을 기준으로 테스트 중
        int dropItemId = GameManager.Instance.DataManager.DropItemPool.GetRandomItemId(DropPoolType.Normal);

        // 가져온 아이템 ID로 딕셔너리에서 해당 아이템 데이터를 가져온다.
        DropItemData itemData = GameManager.Instance.DataManager.DropItemDataDict[dropItemId];

        // 풀에서 해당 아이템 데이터의 경로를 이용해 프리팹 꺼내오기
        GameObject itemGo = GameManager.Instance.PoolManager.GetFromPool(itemData.DropItemPrefabPath);
        if (itemGo == null)
        {
            Debug.LogError($"풀에서 {itemData.DropItemPrefabPath} 아이템을 가져올 수 없습니다.");
            return;
        }

        // 위치 설정 및 초기화
        itemGo.transform.position = enemy.transform.position;

        // 드롭 아이템 초기화 (필요하다면)
        DropItem dropItem = itemGo.GetComponent<DropItem>();

        // 드롭 아이템 리스트에 등록
        RegisterDropItem(dropItem);

        // 드롭 아이템 초기화
        dropItem.Initialize(itemData.ItemId);
    }

    public void UseMagnetItem()
    {
        _magnetEffectRoutine = StartCoroutine(MagnetEffectRoutine());
    }

    IEnumerator MagnetEffectRoutine()
    {
        float moveSpeed = 10f; // 자석 아이템의 이동 속도(임시)
        while (_currentDropItems.Count > 0)
        {
            foreach (var dropItem in _currentDropItems)
            {
                // 드롭 아이템이 영웅에게 가까워지도록 이동
                Vector3 dir = (_hero.position - dropItem.transform.position).normalized;
                dropItem.transform.position += dir * moveSpeed * Time.deltaTime;
            }
            yield return null; // 다음 프레임까지 대기
        }
    }

    void RegisterDropItem(DropItem dropItem)
    {
        _currentDropItems.Add(dropItem);
        dropItem.OnDropItemUsed += HandleDropItemUsed;
    }

    void HandleDropItemUsed(DropItem dropItem)
    {
        // 아이템 사용 이벤트 핸들러
        if (_currentDropItems.Contains(dropItem))
        {
            _currentDropItems.Remove(dropItem);
            dropItem.OnDropItemUsed -= HandleDropItemUsed; // 이벤트 구독 해제
        }
    }
}
