using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 장비 장착등으로 생긴 보너스 스탯을 보여주는 UI
/// </summary>
public class BonusStatUI : MonoBehaviour
{
    [Header("----- 컴포넌트 참조 -----")]
    [SerializeField] TextMeshProUGUI _bonusHpText;
    [SerializeField] TextMeshProUGUI _bonusDamageText;
    [SerializeField] TextMeshProUGUI _bonusMoveSpeedText;
    [SerializeField] TextMeshProUGUI _bonusItemGetRangeText;

    HeroManager _heroManager;

    public void Initialize()
    {
        _heroManager = GameManager.Instance.HeroManager;
        _heroManager.OnBonusStatChanged += RefreshUI;
        RefreshUI();
    }

    void RefreshUI()
    {
        _bonusHpText.text = $"최대체력 +{_heroManager.BonusHp}";
        _bonusDamageText.text = $"공격력 +{_heroManager.BonusDamage}";
        _bonusMoveSpeedText.text = $"이동속도 +{_heroManager.BonusMoveSpeed}";
        _bonusItemGetRangeText.text = $"아이템획득범위 +{_heroManager.BonusItemGetRange}";
    }
}
