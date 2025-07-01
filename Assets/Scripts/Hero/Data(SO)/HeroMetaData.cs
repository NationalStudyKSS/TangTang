using UnityEngine;

/// <summary>
/// 주인공 캐릭터의 메타데이터를 포함하는 설정 데이터 클래스
/// </summary>
[CreateAssetMenu(fileName = "HeroMetaData", menuName = "GameSettings/Hero/HeroMetaData")]
public class HeroMetaData : ScriptableObject
{
    [Header("----- 주인공 캐릭터 메타데이터 -----")]
    [SerializeField] GameObject _heroPrefab;        // 주인공 프리팹
    [SerializeField] Sprite _icon;                  // 주인공 아이콘
    
    [SerializeField] string _heroId;                // 주인공 ID
    [SerializeField] string _displayName;           // 주인공 표시 이름
    [SerializeField] string _description;           // 주인공 설명

    // 외부 접근용 프로퍼티 (읽기 전용)
    public GameObject HeroPrefab => _heroPrefab;
    public Sprite Icon => _icon;

    public string HeroId => _heroId;
    public string DisplayName => _displayName;

    public string Description => _description;
}