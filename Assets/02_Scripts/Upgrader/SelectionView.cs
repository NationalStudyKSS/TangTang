using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 하나의 업그레이드 대상의 UI 요소를 담당하는 뷰 클래스
/// </summary>
public class SelectionView : MonoBehaviour
{
    [Header("----- 컴포넌트 참조 -----")]
    [SerializeField] Image _icon;                   // 업그레이드 아이콘 이미지
    [SerializeField] TextMeshProUGUI _nameText;     // 업그레이드 이름 텍스트
    [SerializeField] TextMeshProUGUI _descText;     // 업그레이드 설명 텍스트
    [SerializeField] TextMeshProUGUI _levelText;    // 업그레이드 레벨 텍스트

    Upgrader _upgrader;             // 업그레이더(Upgrader) 참조 변수
    IUpgradable _upgradable;        // 연결된 업그레이드 대상 객체

    public void Initialize(Upgrader upgrader, IUpgradable upgradable)
    {
        _upgrader = upgrader;
        _upgradable = upgradable;  // 업그레이드 대상 객체를 저장
        // UI 요소 초기화
        _icon.sprite = _upgradable.IconSprite;
        _nameText.text = _upgradable.UpgradeName;
        _descText.text = _upgradable.Description;
        _levelText.text = $"- Level {_upgradable.Level + 2}";
    }

    /// <summary>
    /// 클릭되었을 때 호출되는 함수
    /// </summary>
    public void OnClikced()
    {
        if (_upgradable != null)
        {
            _upgradable.Upgrade();
        }
        // 업그레이드 창 닫는 작업
        _upgrader.EndSelection();
    }
}
