using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 업그레이드 된 스킬들을 관리하는 클래스
/// </summary>
public class SkillUpgradeManager : MonoBehaviour
{
    public static SkillUpgradeManager Instance { get; private set; }

    [SerializeField] PauseView _pauseView;

    List<IUpgradable> _activeSkills = new();
    List<IUpgradable> _passiveSkills = new();

    int _activeSkillCount = 0;
    int _passiveSkillCount = 0;

    public event Action<IUpgradable, int> OnActiveSkillUpgraded;
    public event Action<IUpgradable, int> OnPassiveSkillUpgraded;
    public event Action MainButtonClicked;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _pauseView.Initialize();
        OnActiveSkillUpgraded += _pauseView.UpdateSkillSlotView;
        OnPassiveSkillUpgraded += _pauseView.UpdateSkillSlotView;
        _pauseView.MainButtonClicked += OnMainButtonClicked;
        _pauseView.MuteButtonClicked += OnMuteButtonClicked;
    }

    public void RegisterUpgrade(IUpgradable upgradable)
    {
        int typeIndex = (int)upgradable.UpgradeType;
        if (typeIndex < 0 || typeIndex >= (int)UpgradeType.Count) return;

        if (upgradable.UpgradeType == UpgradeType.ActiveSkill)
        {
            if (_activeSkills.Contains(upgradable) == false)
            {
                _activeSkills.Add(upgradable);
                int index = _activeSkillCount++;

                upgradable.OnUpgraded += (skill) =>
                {
                    OnActiveSkillUpgraded?.Invoke(skill, index);
                };
            }
        }
        else if (upgradable.UpgradeType == UpgradeType.PassiveSkill)
        {
            if (_passiveSkills.Contains(upgradable) == false)
            {
                _passiveSkills.Add(upgradable);
                int index = _passiveSkillCount++;

                upgradable.OnUpgraded += (skill) =>
                {
                    OnPassiveSkillUpgraded?.Invoke(skill, index);
                };
            }
        }
    }

    /// <summary>
    /// 메인화면 버튼이 눌렸을 때 실행되어야 할 함수(전달용)
    /// </summary>
    public void OnMainButtonClicked()
    {
        MainButtonClicked?.Invoke();
    }

    // 임시) 게임시작시 항상 음소거가 아니라고 가정
    bool isMuted = false;
    /// <summary>
    /// 음소거 버튼을 눌렀을 때 처리할 함수인데
    /// 여기서 처리하면 이상하긴 하지만.. 임시로..
    /// </summary>
    public void OnMuteButtonClicked()
    {
        // 음소거 상태 뒤집어주고
        isMuted = !isMuted;
        _pauseView.SetMuteText(isMuted);
    }
}
