using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 용의 공 액티브 스킬
/// </summary>
public class DragonBallSkill : FiringActiveSkill
{
    //[SerializeField] int _attackCount;      // 총알 공격 횟수(적 관통 얼마나 할지)

    [Header(" ----- 총알 프리펩 ----- ")]
    [SerializeField] ProjectileBullet _bulletPrefab; // 발사할 총알 프리팹

    [Header(" ----- 타겟 감지 ----- ")]
    [SerializeField] LayerMask _targetLayerMask; // 감지할 타겟 설정

    public override ActiveSkillType ActiveSkillType => ActiveSkillType.DragonBall;

    protected override void CalculateStats()
    {
        // 일단 부모 클래스의 스텟들은 계산하고
        base.CalculateStats();

        //// GunActiveSkill에 추가로 필요한 스텟들을 계산
        //_attackCount = Mathf.RoundToInt(_data.GetStat(ActiveSkillStatType.AttackCount, _level));
    }

    // 부모 클래스의 SpawnBullet() 함수가 abstract로 선언되어 있으므로
    // GunSkill 클래스에서 반드시 구현해야 한다.
    protected override void SpawnBullet()
    {
        // 일단 총알을 생성하고
        ProjectileBullet bullet = Instantiate(_bulletPrefab);

        // bullet의 위치를 이 게임오브젝트 위치로 설정
        bullet.transform.position = transform.position;

        // 총알이 생성될 때 총알의 스텟들을 설정
        bullet.SetDamage(_damage);
        bullet.SetDirection(GetBulletDirection());
        bullet.SetDuration(_bulletDuration);
        bullet.SetSpeed(_bulletSpeed);
    }
}
