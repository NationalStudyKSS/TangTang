using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 패시브스킬 하나의 레벨별 스텟 데이터들을 보관하는 ScriptableObject
/// 각 스텟은 StatType별로 구분되며, 패시브스킬 레벨에 따라 값이 달라진다.
/// </summary>
[CreateAssetMenu(fileName = "PassiveSkillData", menuName = "GameSettings/HeroSkill/PassiveSkillData")]
public class PassiveSkillData : ScriptableObject
{
    [SerializeField] int _id; // 패시브스킬 ID
    [SerializeField] string _passiveSkillName;   // 패시브스킬 이름

    //스텟 종류별로 레벨별 값을 설정한 배열
    [SerializeField] PassiveSkillLevelStat[] _levelStats;

    // 스킬 종류를 나타내는 열거형
    [SerializeField] PassiveSkillType _skillType;

    [TextArea(3, 5)][SerializeField] string _description; // 패시브스킬 설명
    [SerializeField] Sprite _iconSprite;  // 패시브스킬 아이콘 스프라이트

    // 스텟 종류별로 빠르게 조회할 수 있도록 만든 딕셔너리
    Dictionary<StatName, PassiveSkillLevelStat> _levelStatMap = new Dictionary<StatName, PassiveSkillLevelStat>();
    // = new();
    int _maxLevel; // 최대 레벨

    public int Id => _id; // 패시브스킬 ID
    public string PassiveSkillName => _passiveSkillName;
    public PassiveSkillType SkillType => _skillType;
    public string Description => _description;
    public Sprite IconSprite => _iconSprite;
    public int MaxLevel => _maxLevel; // 최대 레벨은 _levelStats 배열 중 최대 레벨을 갖는 PassiveSkillLevelStat의 MaxLevel
    public PassiveSkillLevelStat[] LevelStats => _levelStats; // 레벨별 스텟 정보 배열

    /// <summary>
    /// 주어진 스탯 타입과 레벨에 대한 값을 반환해 주는 함수
    /// </summary>
    /// <param name="statName">패시브스킬 스탯 종류</param>
    /// <param name="level">패시브스킬 레벨</param>
    /// <returns></returns>
    public float GetStat(StatName statName, int level)
    {
        // _levelStatMap이 statType에 해당하는 값을 가지고 있는 경우
        if (_levelStatMap.TryGetValue(statName, out var levelStat))
        {
            // statType에 해당하는 값을 반환
            return levelStat.GetValue(level);
        }

        // 없으면 경고 메시지를 출력하고 0을 반환
        Debug.LogWarning($"{_passiveSkillName} 패시브스킬은 {statName} 스텟이 없습니다.");
        return 0;
    }

    private void OnValidate()
    {
        Initialize();
    }

    private void OnEnable()
    {
        Initialize();
    }

    /// <summary>
    /// 에디터(인스펙터뷰)에서 변수(스텟 배열, 이름 등)가 변경될 때
    /// 자동으로 _levelStatMap 딕셔너리를 갱신한다.
    /// </summary>
    public void Initialize()
    {
        _levelStatMap.Clear();
        _maxLevel = 0;

        if (_levelStats == null || _levelStats.Length == 0)
            return;

        foreach (var levelStat in _levelStats)
        {
            if (levelStat == null) continue;
            _levelStatMap[levelStat.StatName] = levelStat;

            if (_maxLevel < levelStat.MaxLevel)
            {
                _maxLevel = levelStat.MaxLevel;
            }
        }
    }
}
