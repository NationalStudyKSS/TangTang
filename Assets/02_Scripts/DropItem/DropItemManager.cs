using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 드롭아이템 효과를 연결해주는 매니저
/// </summary>
public static class DropItemManager
{
    // 공격력 업 아이템
    public static event Action<float, float> OnAtkUpItemUsed;
    public static void RaiseAtkUpItemUsed(float atkUpRate, float duration)
    {
        OnAtkUpItemUsed?.Invoke(atkUpRate, duration);
    }

    // 폭탄 아이템
    public static event Action OnBombItemUsed;
    public static void RaiseBombItemUsed()
    {
        OnBombItemUsed?.Invoke();
    }

    // 당근 아이템
    public static event Action<int> OnCarrotItemUsed;
    public static void RaiseCarrotItemUsed(int carrotAmount)
    {
        OnCarrotItemUsed?.Invoke(carrotAmount);
    }

    // 코인 아이템
    public static event Action<int> OnGoldItemUsed;
    public static void RaiseGoldItemUsed(int coinAmount)
    {
        OnGoldItemUsed?.Invoke(coinAmount);
    }

    // 경험치 아이템
    public static event Action<float> OnExpItemUsed;
    public static void RaiseExpItemUsed(float expAmount)
    {
        OnExpItemUsed?.Invoke(expAmount);
    }

    // HP 포션 아이템
    public static event Action<float> OnHpPotionItemUsed;
    public static void RaiseHpPotionItemUsed(float hpHealRate)
    {
        OnHpPotionItemUsed?.Invoke(hpHealRate);
    }

    // 자석 아이템
    public static event Action OnMagnetItemUsed;
    public static void RaiseMagnetItemUsed()
    {
        OnMagnetItemUsed?.Invoke();
    }

    // 보물상자 아이템
    public static event Action<int> OnTreasureBoxItemUsed;
    public static void RaiseTreasureBoxItemUsed(int upgradeCount)
    {
        OnTreasureBoxItemUsed?.Invoke(upgradeCount);
    }
}
