using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 회전 칼날 스킬 클래스
/// </summary>
public class SpinBladeSkill : ActiveSkill
{
    [SerializeField] float _shootingRange;      // 사정거리(총알 위치 반지름)
    [SerializeField] float _bulletSpeed;        // 총알 속력(총알이 회전하는 속력)
    [SerializeField] int _bulletCount;          // 총알 수(총알이 동시에 몇 개 배치되어 있을지)
    [SerializeField] float _bulletDuration;     // 총알 지속 시간(총알이 사라지기까지의 시간)
    [SerializeField] float _coolTime;           // 총알 발사 간격(쿨타임)

    // 생성된 총알 리스트
    List<Bullet> _bullets = new List<Bullet>();

    Coroutine _attackRoutine; // 공격 루틴 코루틴 변수

    public override ActiveSkillType ActiveSkillType => ActiveSkillType.SpinBlade;

    

    /// <summary>
    /// 레벨에 따른 액티브스킬의 현재 스텟(런타임 데이터)을 계산하는 함수
    /// </summary>
    protected override void CalculateStats()
    {
        base.CalculateStats();  // _damage = _data.GetStat(ActiveSkillStatType.Damage, _level);
        _bulletSpeed = _data.GetStat(ActiveSkillStatType.BulletSpeed, _level);
        _shootingRange = _data.GetStat(ActiveSkillStatType.ShootingRange, _level);
        _bulletDuration = _data.GetStat(ActiveSkillStatType.BulletDuration, _level);
        _coolTime = _data.GetStat(ActiveSkillStatType.CoolTime, _level);

        float bulletCount = (int)_data.GetStat(ActiveSkillStatType.BulletCount, _level);
        // float 값을 반올림해서 int로 전환
        _bulletCount = Mathf.RoundToInt(bulletCount);
    }

    private void FixedUpdate()
    {
        HandleRotation();
    }

    /// <summary>
    /// FixedUpdate()마다 호출
    /// 총알이 캐릭터 주위를 일정속력으로 돌기때문에(행성의 공전같은 느낌) 
    /// 액티브스킬의 총알이 계속 존재해야 하므로 필요한 함수이다.
    /// </summary>
    void HandleRotation()
    {
        transform.Rotate(0, 0, _bulletSpeed * Time.fixedDeltaTime);
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            SpawnBullets(); // 총알 배치
            yield return new WaitForSeconds(_bulletDuration);

            RemoveBullets(); // 총알 제거
            yield return new WaitForSeconds(_coolTime); // 쿨타임 대기
        }
    }

    /// <summary>
    /// 원형으로 총알들을 배치하는 함수.
    /// 각도 기준으로 일정간격 총알을 배치하는 역할.
    /// </summary>
    void SpawnBullets()
    {
        // 기존에 있던 총알들 제거
        RemoveBullets();

        // 총알들 사이 간격 각도 계산
        float angle = 360.0f / _bulletCount;

        // bulletCount만큼 총알 생성
        for (int i = 0; i < _bulletCount; i++)
        {
            // 일단 총알을 생성하고
            GameObject go = GameManager.Instance.PoolManager.GetFromPool("Bullet/SpinBlade");
            if (go == null)
            {
                Debug.LogError("Enemy 프리팹을 찾을 수 없습니다.");
                return;
            }

            Bullet bullet = go.GetComponent<Bullet>();
            if (bullet == null)
            {
                Debug.LogError("Enemy 컴포넌트를 찾을 수 없습니다.");
                return;
            }

            // 각 Bullet 게임오브젝트가 배치될 방향
            // Mathf.Cos(): 코사인(각도) -> x좌표
            // Mathf.Sin(): 사인(각도) -> y좌표
            // 각도 단위: degree(0도 ~ 360도) 라디안(2 * Pi = 360도)
            Vector3 dir = new Vector3(Mathf.Cos(i * angle * Mathf.Deg2Rad), Mathf.Sin(i * angle * Mathf.Deg2Rad), 0);

            // localPosition: 부모 게임오브젝트에 대한 상대 위치
            // 인스펙터뷰에 표시되는 Position은 사실 localPosition
            bullet.transform.localPosition = dir * _shootingRange;

            // 생성된 bullet 게임오브젝트의 위쪽 방향을
            // dir 방향으로 설정
            bullet.transform.up = dir;

            // 생성한 bullet의 데미지를 설정
            bullet.SetDamage(_damage);

            // 생성한 bullet 게임오브젝트의 부모 게임오브젝트를
            // ShovelActiveSkill 컴포넌트가 붙어 있는 게임오브젝트로 설정
            bullet.transform.SetParent(transform, false);

            // 생성된 bullet 게임오브젝트를 리스트에 저장
            _bullets.Add(bullet);
        }
    }

    /// <summary>
    /// 기존에 생성되어 있던 총알들을 제거하는 함수
    /// </summary>
    void RemoveBullets()
    {
        foreach (Bullet bullet in _bullets)
        {
            Poolable poolable = bullet.GetComponent<Poolable>();
            if (poolable != null)
            {
                // Object Pooling을 사용하여 총알 게임오브젝트를 비활성화
                poolable.ReturnToPool();
            }
            else
            {
                // Object Pooling을 사용하지 않는 경우, Destroy로 게임오브젝트 파괴
                Destroy(gameObject);
            }
        }

        // 리스트 비우기
        _bullets.Clear();
    }

    public override void Upgrade()
    {
        base.Upgrade();
        if (_model != null)
        {
            SetDamage(_model.Stats.Damage.Final);
        }

        if (_attackRoutine != null)
            StopCoroutine(_attackRoutine);
        _attackRoutine = StartCoroutine(AttackRoutine());
    }
}
