using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class SkillSlotView : MonoBehaviour
{
    [Header("----- 컴포넌트 참조 -----")]
    [SerializeField] Sprite fullStarSprite;
    [SerializeField] Sprite emptyStarSprite;
    [SerializeField] Image _icon;
    [SerializeField] Image[] _stars;

    /// <summary>
    /// 초기화 함수
    /// </summary>
    public void Initialize()
    {
        // 처음에는 null 넣어서 초기화
        SetSlotView(null);
    }

    /// <summary>
    /// 슬롯뷰 하나를 설정하는 함수
    /// </summary>
    /// <param name="skill">업그레이드 가능한 스킬</param>
    public void SetSlotView(IUpgradable skill)
    {
        // 만약 스킬이 없다면
        if (skill == null)
        {
            // 아이콘 꺼주고
            _icon.gameObject.SetActive(false);
            foreach (var star in _stars)
            {
                // 별도 꺼주고
                star.gameObject.SetActive(false);
            }
            return;
        }
        // 스킬이 있으면
        // 아이콘 켜주고
        _icon.gameObject.SetActive(true);
        // 아이콘에 스킬 아이콘 넣어주고
        _icon.sprite = skill.IconSprite;
        foreach (var star in _stars)
        {
            // 별도 켜주고
            star.gameObject.SetActive(true);
        }
        for (int i = 0; i < _stars.Length; i++)
        {
            _stars[i].sprite = (i <= skill.Level) ? fullStarSprite : emptyStarSprite;
        }
    }
}

