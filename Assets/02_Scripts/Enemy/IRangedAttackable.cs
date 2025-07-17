using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRangedAttackable 
{
    public float AttackRange { get; }
    public float AttackSpan { get; }
    public float BulletSpeed { get; }
    public float BulletDuration { get; }

    public void SpawnBullet();
    public Vector3 GetDirection();
}
