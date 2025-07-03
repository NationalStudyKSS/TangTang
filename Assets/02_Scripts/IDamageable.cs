using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 공격을 받을 수 있는 객체를 나타내는 인터페이스
/// </summary>
public interface IDamageable
{
    void TakeDamage(float amount);
}
