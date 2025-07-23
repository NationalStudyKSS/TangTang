using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour, IUpgradable
{
    public abstract string UpgradeName { get; }
    public abstract string Description { get; }
    public abstract Sprite IconSprite { get; }
    public abstract int Level { get; }
    public abstract bool IsMaxLevel { get; }
    public abstract bool CanUpgrade { get; }
    public abstract UpgradeType UpgradeType { get; }

    public event Action<IUpgradable> OnUpgraded;

    public abstract void Initialize();
    public abstract void Upgrade();

    // 이벤트 호출 메서드는 공통으로 둘 수도 있음
    protected void RaiseOnUpgraded()
    {
        OnUpgraded?.Invoke(this);
    }
}
