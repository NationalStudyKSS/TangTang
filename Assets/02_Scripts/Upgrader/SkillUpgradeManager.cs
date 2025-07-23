using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _pauseView.Initialize();
        OnActiveSkillUpgraded += _pauseView.UpdateSkillSlotView;
        OnPassiveSkillUpgraded += _pauseView.UpdateSkillSlotView;
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
}
