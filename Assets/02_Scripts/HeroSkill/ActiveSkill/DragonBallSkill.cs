using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 용의 공 액티브 스킬
/// </summary>
public class DragonBallSkill : FiringActiveSkill
{
    public override ActiveSkillType ActiveSkillType => ActiveSkillType.DragonBall;

    protected override void SpawnBullet()
    {
        
    }
}
