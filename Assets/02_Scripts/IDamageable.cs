using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 공격을 받을 수 있는 객체를 나타내는 인터페이스
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// 체력이 변경될 때 호출되는 이벤트(현재 체력, 최대 체력)
    /// </summary>
    public event Action<float, float> RaiseOnHpChanged;

    /// <summary>
    /// 캐릭터가 사망할 때 호출되는 이벤트
    /// </summary>
    public event Action <GameObject> RaiseOnDead;

    /// <summary>
    /// 피해를 입히는 함수
    /// </summary>
    /// <param name="damage">데미지 양</param>
    public void TakeHit(float amount, ElementType attackerElement);
}
