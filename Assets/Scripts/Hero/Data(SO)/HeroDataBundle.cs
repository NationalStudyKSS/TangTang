using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

/// <summary>
/// 영웅의 모든 데이터들을 묶음으로 관리하는 SO 클래스
/// 각 데이터들을 묶어 한번에 보기 편하게 만들어 놓은 것으로
/// 이 번들을 거쳐야만 Data들에 접근이 가능하다.
/// </summary>
[CreateAssetMenu(fileName = "HeroDataBundle", menuName = "GameSettings/HeroDataBundle")]
public class HeroDataBundle : ScriptableObject
{
    [SerializeField] int _heroId;

    [SerializeField] HeroStatData _heroStatData;
    [SerializeField] HeroUIData _heroUIData;

    [SerializeField] GameObject heroPrefab;

    public int HeroId => _heroId;
    public HeroStatData HeroStatData => _heroStatData;
    public HeroUIData HeroUIData => _heroUIData;
    public GameObject HeroPrefab => heroPrefab;
}
