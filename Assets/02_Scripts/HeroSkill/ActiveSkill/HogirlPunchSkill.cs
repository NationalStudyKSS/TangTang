using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 호걸 펀치 스킬을 구현하는 클래스
/// </summary>
public class HogirlPunchSkill : ActiveSkill
{
    [SerializeField] float _shootingRange;      // 사정거리(사실상 적 감지 범위)
    [SerializeField] int _bulletCount;          // 총알 수(한 번에 몇 개의 펀치를 날릴지)

    public override ActiveSkillType ActiveSkillType => ActiveSkillType.HogirlPunch;
}
