using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DropPoolType
{
    Normal,
    Elite,
    Boss
}

/// <summary>
/// 드롭 아이템 풀 데이터
/// </summary>
[CreateAssetMenu(fileName = "DropItemPoolData", menuName = "GameSettings/DropItem/DropItemPoolData")]
public class DropItemPoolData : ScriptableObject
{
    [Header("----- 드롭 아이템 풀 데이터(직접 할당 해줘야함) -----")]
    [SerializeField] List<DropItemData> _normalItems = new List<DropItemData>();    // 일반 적 드롭 아이템 풀
    [SerializeField] List<DropItemData> _eliteItems = new List<DropItemData>();     // 정예 적 드롭 아이템 풀
    [SerializeField] List<DropItemData> _bossItems = new List<DropItemData>();      // 보스 적 드롭 아이템 풀

    /// <summary>
    /// 드롭 아이템 풀을 타입에 따라 반환합니다.
    /// </summary>
    /// <param name="type">드롭 아이템 풀 타입(적 타입)</param>
    /// <returns></returns>
    public List<DropItemData> GetPoolByType(DropPoolType type)
    {
        switch (type)
        {
            case DropPoolType.Normal: return _normalItems;
            case DropPoolType.Elite: return _eliteItems;
            case DropPoolType.Boss: return _bossItems;
            default: return _normalItems;
        }
    }

    /// <summary>
    /// 드롭 아이템 풀에서 확률에 따라 랜덤으로 아이템 ID 하나를 반환합니다.
    /// 근데 일반, 정예, 보스마다 드롭 메커니즘이 달라서 의미가없어졌다...
    /// </summary>
    /// <param name="type">드롭 아이템 풀 타입(적 타입)</param>
    /// <returns></returns>
    public int GetRandomItemId(DropPoolType type)
    {
        float total = 0;

        // 드롭 아이템 풀을 타입에 따라 가져오기
        List<DropItemData> dropItemList = GetPoolByType(type);

        // 드롭 아이템 리스트가 비어있거나 null인 경우
        if (dropItemList == null || dropItemList.Count == 0)
            return -1; // 에러 처리

        // 확률 합계 계산
        for (int i = 0; i < dropItemList.Count; i++)
        {
            total += dropItemList[i].Chance;
        }

        // 드롭 아이템 리스트의 확률 합계 계산이 0보다 작거나 같으면
        if (total <= 0f)
            return -1; // 에러 처리

        // 랜덤한 확률 값 생성
        float randomPoint = Random.value * total;

        // 유니티 랜덤 게임플레이 요소 참고
        // https://docs.unity3d.com/kr/2019.3/Manual/RandomNumbers.html
        for (int i = 0; i < dropItemList.Count; i++)
        {
            // 랜덤으로 뽑은 값이 현재 아이템의 확률보다 작으면
            if (randomPoint < dropItemList[i].Chance)
            {
                // 해당 아이템 ID 반환
                return dropItemList[i].ItemId;
            }
            // 랜덤으로 뽑은 값이 현재 아이템의 확률보다 크면
            else
            {
                // 랜덤 값에서 현재 아이템의 확률을 빼기(이미 이번 아이템 확률은 지나갔음)
                randomPoint -= dropItemList[i].Chance;
            }
        }
        // 모든 아이템의 확률을 다 지나갔으면 마지막 아이템 ID 반환
        return dropItemList[dropItemList.Count - 1].ItemId;
    }
}

