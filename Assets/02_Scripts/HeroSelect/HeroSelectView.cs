using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 영웅 선택창에서 하나의 슬롯을 관리하는 클래스
/// </summary>
public class HeroSelectView : MonoBehaviour
{
    [Header("----- 컴포넌트 참조 -----")]
    [SerializeField] RawImage _heroImage;
    [SerializeField] Image _skillIcon;
    [SerializeField] TextMeshProUGUI _heroNameText;
    [SerializeField] TextMeshProUGUI _heroSkillNameText;
    [SerializeField] TextMeshProUGUI _heroHpText;
    [SerializeField] TextMeshProUGUI _heroDamageText;
    [SerializeField] TextMeshProUGUI _heroMoveSpeedText;
    [SerializeField] Button _selectButton;
    [SerializeField] TextMeshProUGUI _selectButtonText;

    string _heroId;

    public string HeroId => _heroId;

    public event Action<HeroSelectView, string> OnSelected;

    public void SetHero(string heroId)
    {
        DataManager data = GameManager.Instance.DataManager;
        _heroImage.texture = data.HeroMetaDataMap[heroId].HeroTexture;
        _skillIcon.sprite = data.HeroMetaDataMap[heroId].SkillIcon;
        _heroNameText.text = $"바니 이름 : {data.HeroMetaDataMap[heroId].HeroName}";
        _heroSkillNameText.text = $"전용 스킬 : {data.HeroMetaDataMap[heroId].HeroSkillname}";
        _heroHpText.text = $"기본 체력 : {data.HeroStatDataMap[heroId].GetMaxHp(0)}";
        _heroDamageText.text = $"기본 공격력 : {data.HeroStatDataMap[heroId].GetDamage(0)}";
        _heroMoveSpeedText.text = $"기본 이동속도 : {data.HeroStatDataMap[heroId].GetMoveSpeed(0)}";

        _heroId = data.HeroMetaDataMap[heroId].HeroId;

        _selectButton.onClick.AddListener(OnButtonClicked);
    }

    public void OnButtonClicked()
    {
        OnSelected?.Invoke(this, _heroId);
    }

    public void SetSelected(bool isSelected)
    {
        _selectButton.interactable = !isSelected;
        _selectButtonText.text = isSelected ? "선택 중" : "선택하기";
    }
}
