using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronHelmet : PassiveSkill
{
    protected override void Apply()
    {
        if (_heroModel == null)
            return;

        _heroModel.SetStat(_statName, StatType.Stage, _bonusValue);
    }

    private void OnValidate()
    {
        _passiveSkillType = PassiveSkillType.IronHelmet;
    }
}
