using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 영웅의 모든 데이터번들을 리스트로 갖고 있는 데이터베이스
/// </summary>
[CreateAssetMenu(fileName = "HeroDatabase", menuName = "GameSettings/HeroDatabase")]
public class HeroDatabase : ScriptableObject
{
    public List<HeroDataBundle> heroDataBundleList;
}
