using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 영웅의 모든 데이터들을 묶음으로 관리하는 SO 클래스
/// </summary>
[CreateAssetMenu(fileName = "HeroData", menuName = "GameSettings/Hero/HeroData")]
public class HeroData : ScriptableObject
{
    [SerializeField] HeroStatData _heroStatData;
    [SerializeField] HeroMetaData _heroMetaData;

    public HeroStatData HeroStatData => _heroStatData;
    public HeroMetaData HeroMetaData => _heroMetaData;
}
