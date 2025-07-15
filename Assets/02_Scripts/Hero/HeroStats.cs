using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatName
{
    MaxHp,
    Damage,
    MoveSpeed,
    ItemGetRange,
    ExpGainRate
}

/// <summary>
/// Hero가 보유한 모든 스탯을 모아놓은 컨테이너
/// </summary>
[System.Serializable]
public class HeroStats
{
    public Stat MaxHp = new Stat();
    public Stat Damage = new Stat();
    public Stat MoveSpeed = new Stat();
    public Stat ItemGetRange = new Stat();
    public Stat ExpGainRate = new Stat();
}
