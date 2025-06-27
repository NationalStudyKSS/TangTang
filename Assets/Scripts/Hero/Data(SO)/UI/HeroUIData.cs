using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 영웅의 정보들 중에 UI에 보여질 때 필요한 데이터
/// </summary>
[CreateAssetMenu(fileName = "HeroUIData", menuName = "GameSettings/HeroUIData")]
public class HeroUIData : ScriptableObject
{
    [Header("----- UI용 -----")]
    [SerializeField] Sprite _heroIcon;                         // 영웅 아이콘
    [SerializeField] string _name;                             // 영웅 이름 텍스트

    public Sprite HeroIcon => _heroIcon;
    public string Name => _name;
}
