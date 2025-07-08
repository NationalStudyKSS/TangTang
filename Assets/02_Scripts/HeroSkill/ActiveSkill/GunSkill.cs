using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 일정 시간마다 가까운 적 캐릭터를 향해 GunBullet을 발사하는 액티브스킬 클래스
/// </summary>
public class GunSkill : FiringActiveSkill
{
    [SerializeField] float _shootingRange;  // 사정 거리(적 탐지 범위)
    [SerializeField] int _attackCount;      // 총알 공격 횟수(적 관통 얼마나 할지)

    [Header(" ----- 총알 프리펩 ----- ")]
    [SerializeField] GunBullet _bulletPrefab; // 발사할 총알 프리팹

    [Header(" ----- 타겟 감지 ----- ")]
    [SerializeField] LayerMask _targetLayerMask; // 감지할 타겟 설정

    /// <summary>
    /// 감지된 타겟 정보를 저장할 배열
    /// </summary>
    Collider2D[] _colliders = new Collider2D[10];

    public override ActiveSkillType ActiveSkillType => ActiveSkillType.Gun;

    protected override void CalculateStats()
    {
        // 일단 부모 클래스의 스텟들은 계산하고
        base.CalculateStats();

        // GunActiveSkill에 추가로 필요한 스텟들을 계산
        _shootingRange = _data.GetStat(ActiveSkillStatType.ShootingRange, _level);
        _attackCount = Mathf.RoundToInt(_data.GetStat(ActiveSkillStatType.AttackCount, _level));
    }

    // 부모 클래스의 SpawnBullet() 함수가 abstract로 선언되어 있으므로
    // GunSkill 클래스에서 반드시 구현해야 한다.
    protected override void SpawnBullet()
    {
        // 일단 총알을 생성하고
        GunBullet bullet = Instantiate(_bulletPrefab);

        // bullet의 위치를 이 게임오브젝트 위치로 설정
        bullet.transform.position = transform.position;

        // 총알이 생성될 때 총알의 스텟들을 설정
        bullet.SetCount(_attackCount);
        bullet.SetDamage(_damage);
        bullet.SetDirection(GetBulletDirection());
        bullet.SetDuration(_bulletDuration);
        bullet.SetSpeed(_bulletSpeed);
    }

    /// <summary>
    /// 가장 가까운 적 방향으로 총알 방향을 계산
    /// </summary>
    /// <returns></returns>
    protected override Vector3 GetBulletDirection()
    {
        // 감지된 가장 가까운 녀석을 target에 저장
        Transform target = GetNearestTarget();

        // 만약 감지된 타겟이 null, 즉 없다면
        if (target == null)
        {
            // 타겟이 없다면 기존 방향으로 발사
            return base.GetBulletDirection();
        }
        else
        {
            // 타겟이 있다면 타겟 방향으로 총알을 발사
            return (target.position - transform.position).normalized;
        }
    }

    /// <summary>
    /// 사정거리 내에서 가장 가까운 적 타겟을 찾는 함수
    /// </summary>
    /// <returns>가장 가까운 타겟의 Transform. 없으면 null</returns>
    Transform GetNearestTarget()
    {
        // OverlapCircleNonAlloc(Vector2 center, float radius, Collider2D[] results, int layerMask)
        // 어떤 중점(transform.position)을 기준으로
        // 사정거리(_shootingRange) 내에 있는
        // (_targetLayerMask)에 해당하는 타겟들을 감지한다.
        // 감지된 타겟들은 (_colliders) 배열에 저장된다.
        if (Physics2D.OverlapCircleNonAlloc(transform.position, _shootingRange, _colliders, _targetLayerMask.value) > 0)
        {
            // 0번 타겟이 감지된 타겟 중 가장 가까운 타겟이라고 가정
            Transform target = _colliders[0].transform;

            // 가장 가까운 거리 계산
            float minDistance = Vector3.Distance(transform.position, target.position);

            foreach (var collider in _colliders)
            {
                // 일단 collider 앞에서부터 확인하면서 더 없으면 있는놈들만 데리고 continue
                if (collider == null) continue; // null 체크

                // 현재 타겟과의 거리 계산
                float distance = Vector3.Distance(transform.position, collider.transform.position);

                // 현재 타겟이 더 가까우면 갱신
                if (distance < minDistance)
                {
                    minDistance = distance;
                    target = collider.transform;
                }
            }
            // 가장 가까운 타겟 반환
            return target;
        }

        // 만약 사정거리 내에 타겟이 없다면 null 반환
        else
        {
            return null;
        }
    }
}
