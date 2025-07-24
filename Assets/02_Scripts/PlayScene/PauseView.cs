using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 일시정지 UI를 관리하는 클래스
/// </summary>
public class PauseView : MonoBehaviour
{
    [Header("----- 컴포넌트 참조 -----")]
    [SerializeField] SkillSlotView[] _activeSkillSlots;
    [SerializeField] SkillSlotView[] _passiveSkillSlots;
    [SerializeField] Button _mainButton;
    [SerializeField] Button _muteButton;
    [SerializeField] TextMeshProUGUI _muteText;

    public event Action MainButtonClicked;
    public event Action MuteButtonClicked;

    /// <summary>
    /// 초기화 함수
    /// </summary>
    public void Initialize()
    {
        // 액티브스킬 슬롯 전부 초기화
        for (int i = 0; i < _activeSkillSlots.Length; i++)
        {
            _activeSkillSlots[i].Initialize();
        }
        // 패시브슼리 슬롯 전부 초기화
        for (int i = 0; i < _passiveSkillSlots.Length; i++)
        {
            _passiveSkillSlots[i].Initialize();
        }

        _mainButton.onClick.AddListener(MainButtonClick);
        _muteButton.onClick.AddListener(MuteButtonClick);
    }

    /// <summary>
    /// 스킬 슬롯뷰를 업데이트 하는 함수
    /// Iupgradable과 (해당하는 스킬의) 슬롯인덱스를 받아서
    /// 액티브스킬인지 패시브스킬인지 구분하고
    /// 매칭되는 스킬 슬롯의 정보를 업데이트 한다.
    /// </summary>
    /// <param name="upgradable">업그레이드 한 스킬</param>
    /// <param name="slotIndex">바꿔야 할 슬롯인덱스</param>
    public void UpdateSkillSlotView(IUpgradable upgradable, int slotIndex)
    {
        if (upgradable.UpgradeType == UpgradeType.ActiveSkill)
        {
            _activeSkillSlots[slotIndex].SetSlotView(upgradable);
        }
        if (upgradable.UpgradeType == UpgradeType.PassiveSkill)
        {
            _passiveSkillSlots[slotIndex].SetSlotView(upgradable);
        }
    }

    /// <summary>
    /// 메인화면 버튼을 누르면 실행할 함수
    /// </summary>
    public void MainButtonClick()
    {
        // 메인화면 버튼 눌렸어! 이벤트 발행
        MainButtonClicked?.Invoke();
    }

    /// <summary>
    /// 음소거 버튼을 누르면 실행할 함수
    /// </summary>
    public void MuteButtonClick()
    {
        // 음소거 버튼 눌렸어! 이벤트 발행
        MuteButtonClicked?.Invoke();
    }

    /// <summary>
    /// 음소거 버튼에 달린 텍스트를 설정할 함수
    /// </summary>
    /// <param name="isMuted">지금 음소거인지 여부</param>
    public void SetMuteText(bool isMuted)
    {
        _muteText.text = isMuted ? "음소거 해제" : "음소거";
    }
}
