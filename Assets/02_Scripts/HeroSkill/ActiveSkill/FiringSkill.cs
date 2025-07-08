using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 일정 시간마다 총알을 발사하는 액티브스킬 추상 클래스
/// 단독으로 FiringActiveSkill 클래스로는 객체를 생성하지 않겠다.
/// </summary>
public abstract class FiringActiveSkill : ActiveSkill
{
    [SerializeField] protected float _bulletSpeed;  // 총알 속력
    [SerializeField] protected int _bulletCount;    // 한 번에 발사하는 총알 수
    [SerializeField] protected float _coolTime;     // 총알 발사 간격(쿨타임)
    [SerializeField] protected float _fireDelay;    // 연발 시 총알 사이 간격 시간
    [SerializeField] protected float _bulletDuration; // 총알 지속 시간

    // 총알을 주기적으로 발사하기 위한 코루틴 변수
    Coroutine _attackRoutine;

    protected override void CalculateStats()
    {
        base.CalculateStats();
        _bulletSpeed = _data.GetStat(ActiveSkillStatType.BulletSpeed, _level);
        _bulletCount = Mathf.RoundToInt(_data.GetStat(ActiveSkillStatType.BulletCount, _level));
        _coolTime = _data.GetStat(ActiveSkillStatType.CoolTime, _level);
        _fireDelay = _data.GetStat(ActiveSkillStatType.FireDelay, _level);
        _bulletDuration = _data.GetStat(ActiveSkillStatType.BulletDuration, _level);
    }

    /// <summary>
    /// 일정 주기로 발사 루틴을 호출하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackRoutine()
    {
        while (true)
        {
            // 쿨타임 대기
            yield return new WaitForSeconds(_coolTime);

            // 발사 루틴 실행
            StartCoroutine(FireRoutine());
        }
    }

    /// <summary>
    /// 설정된 수만큼 총알을 연발로 발사하는 코루틴
    /// 총알이 날아간다는 것은 Bullet에서 이미 구현되어있음
    /// </summary>
    /// <returns></returns>
    IEnumerator FireRoutine()
    {
        for (int i = 0; i < _bulletCount; i++)
        {
            // 총알 하나 생성
            SpawnBullet();

            // 연발 사이 시간 간격만큼 대기
            yield return new WaitForSeconds(_fireDelay);
        }
    }

    /// <summary>
    /// 총알을 한 발 or 몇 발 생성하는 함수
    /// </summary>
    protected abstract void SpawnBullet();

    /// <summary>
    /// 총알이 날아갈 방향을 계산하는 함수
    /// </summary>
    /// <returns></returns>
    protected virtual Vector3 GetBulletDirection()
    {
        // 2차원에서 랜덤방향으로 발사
        return Random.insideUnitCircle.normalized;
    }

    /// <summary>
    /// 최초로 업그레이드 = 해당 무기 루틴의 시작
    /// 그 후의 업그레이드 = 지금처럼 공격해
    /// </summary>
    public override void Upgrade()
    {
        base.Upgrade();

        // AttackRoutine() 코루틴이 실행 중이 아니면
        if (_attackRoutine == null)
        {
            _attackRoutine = StartCoroutine(AttackRoutine());
        }
    }
}
