using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 호걸 펀치 스킬 클래스
/// </summary>
public class HogirlPunchSkill : FiringActiveSkill
{
    [SerializeField] float _bulletRange; // 총알이 날아갈 거리

    [Header(" ----- 총알 프리펩 ----- ")]
    [SerializeField] HogirlPunchBullet _bulletPrefab; // 발사할 총알 프리팹

    [Header(" ----- 타겟 감지 ----- ")]
    [SerializeField] LayerMask _targetLayerMask; // 감지할 타겟 설정

    public override ActiveSkillType ActiveSkillType => ActiveSkillType.HogirlPunch;

    protected override void CalculateStats()
    {
        // 일단 부모 클래스의 스텟들은 계산하고
        base.CalculateStats();

        // HogirlPunchSkill에 추가로 필요한 스텟들을 계산
        _bulletRange = _data.GetStat(ActiveSkillStatType.BulletRange, _level);
    }

    protected override void SpawnBullet()
    {
        // 일단 총알을 생성하고
        HogirlPunchBullet bullet = Instantiate(_bulletPrefab);

        // bullet의 위치를 이 게임오브젝트 위치로 설정
        bullet.transform.position = transform.position;

        // 총알이 생성될 때 총알의 스텟들을 설정
        bullet.SetDamage(_damage);
        bullet.SetDirection(GetBulletDirection());
        bullet.SetDuration(_bulletDuration);
        bullet.SetPunchDistance(_bulletRange);
        bullet.SetHero(transform);
        bullet.Initialize();
    }
}
