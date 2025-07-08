using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gun(총) 무기의 총알 클래스
/// Count만큼 적 캐릭터를 관통하여 데미지를 입히고 Count가 모두 소모되면 자동으로 파괴된다.
/// </summary>
public class GunBullet : ProjectileBullet
{
    // 공격 횟수
    [SerializeField] int _count;

    // 공격 횟수를 설정하는 함수
    public void SetCount(int count)
    {
        _count = count;
    }

    protected override void Attack(Enemy enemy)
    {
        // 일단 부모처럼 공격하고
        base.Attack(enemy);

        // 공격 횟수를 차감하고
        _count--;

        // 공격 횟수가 모두 소모되었으면 게임오브젝트를 파괴
        if (_count <= 0)
        {
            Destroy(gameObject);
        }
    }
}
