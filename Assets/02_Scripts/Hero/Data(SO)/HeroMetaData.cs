using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 주인공 메타 데이터를 포함하는 설정 데이터 클래스
/// </summary>
[CreateAssetMenu(fileName = "HeroMetaData", menuName = "GameSettings/Hero/HeroMetaData")]
public class HeroMetaData : ScriptableObject
{
    [Header("----- 주인공 캐릭터 메타데이터 -----")]
    [SerializeField] string _heroId;                // 주인공 ID
    [SerializeField] string _heroName;           // 주인공 표시 이름
    [SerializeField] string _heroSkillname;     // 주인공 전용 스킬 이름
    [SerializeField] Sprite _skillIcon;                  // 주인공 전용스킬 아이콘
    [SerializeField] RenderTexture _heroTexture;    // 주인공 보여주기 텍스쳐
    [SerializeField] GameObject _heroPrefab;        // 주인공 프리팹

    public string HeroId => _heroId;
    public string HeroName => _heroName;
    public string HeroSkillname => _heroSkillname;
    public Sprite SkillIcon => _skillIcon;
    public RenderTexture HeroTexture => _heroTexture;
    public GameObject HeroPrefab => _heroPrefab;
}
