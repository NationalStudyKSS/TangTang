using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 영웅의 모든 데이터들을 리스트로 갖고 있는 데이터베이스
/// </summary>
[CreateAssetMenu(fileName = "HeroDatabase", menuName = "GameSettings/Hero/HeroDatabase")]
public class HeroDatabase : ScriptableObject
{
    [SerializeField] List<HeroData> allHeroes = new();

    private Dictionary<string, HeroData> _dict;

    public void Initialize()
    {
        _dict = allHeroes.ToDictionary(h => h.HeroMetaData.HeroId, h => h);
    }

    public HeroData GetHero(string id)
    {
        return _dict[id]; // id가 존재하지 않을 경우 예외 발생 가능 → 안전처리 추가 가능
    }
}
