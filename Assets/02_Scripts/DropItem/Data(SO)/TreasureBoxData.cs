using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 보물 상자 아이템의 데이터를 관리하는 클래스
/// 랜덤한 업그레이드 횟수를 제공한다.
/// </summary>
[CreateAssetMenu(fileName = "TreasureBoxData", menuName = "GameSettings/DropItem/TreasureBoxData")]
public class TreasureBoxData : DropItemData
{

    public int GetRandomUpgradeCount()
    {
        // 1부터 5까지의 랜덤한 업그레이드 횟수를 반환합니다.
        return Random.Range(1, 6);
    }
}
