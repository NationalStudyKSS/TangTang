using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 데이터 관리 매니저
/// </summary>
public class DataManager : MonoBehaviour
{
    [Header("----- 게임 데이터 -----")]
    [SerializeField] HeroStatData _heroStatData; // 주인공 능력치 데이터
    [SerializeField] EnemyStatData _enemyStatData; // 적 능력치 데이터


}
