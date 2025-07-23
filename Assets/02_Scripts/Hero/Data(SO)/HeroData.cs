using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroData", menuName = "GameSettings/Hero/HeroData")]
public class HeroData : ScriptableObject
{
    [SerializeField] HeroStatData _heroStatData;
    [SerializeField] HeroMetaData _heroMetaData;

    public HeroStatData HeroStatData => _heroStatData;
    public HeroMetaData HeroMetaData => _heroMetaData;
}
