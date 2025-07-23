using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 일시정지 UI를 관리하는 클래스
/// </summary>
public class PauseView : MonoBehaviour
{
    [Header("----- 컴포넌트 참조 -----")]
    [SerializeField] private SkillSlotView[] _activeSkillSlots;
    [SerializeField] private SkillSlotView[] _passiveSkillSlots;

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
}
