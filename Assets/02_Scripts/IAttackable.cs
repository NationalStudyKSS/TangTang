using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 공격력을 갖고 공격하는 대상이 되는 객체를 나타내는 인터페이스
/// </summary>
public interface IAttackable
{
    public void Attack();
}
