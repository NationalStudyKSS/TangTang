using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 일정 방향을 향해 날아가 적 캐릭터에 닿으면
/// 데미지를 입히는 총알 클래스
/// </summary>
public class ProjectileBullet : HeroBullet
{
    // 이동 속력
    [SerializeField] float _speed;

    // 총알 지속 시간(총알 게임오브젝트가 일정 시간 뒤에 자동 파괴되게 하기 위함)
    [SerializeField] protected float _duration;

    // 이동 방향
    Vector3 _dir;

    // 타이머
    protected float _timer;

    /// <summary>
    /// 이동 속력을 설정하는 함수
    /// </summary>
    /// <param name="speed"></param>
    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    /// <summary>
    /// 총알의 지속 시간을 설정하는 함수
    /// </summary>
    /// <param name="duration"></param>
    public void SetDuration(float duration)
    {
        _duration = duration;
    }

    /// <summary>
    /// 이동 방향을 설정하는 함수
    /// </summary>
    /// <param name="dir"></param>
    public void SetDirection(Vector3 dir)
    {
        _dir = dir;

        // 총알이 날아가는 방향을 바라보도록 고개를 돌림
        transform.up = dir;
    }

    private void OnEnable()
    {
        _timer = 0f;
    }

    protected virtual void FixedUpdate()
    {
        transform.Translate(_dir * _speed * Time.fixedDeltaTime, Space.World);
        //transform.Translate(Vector3.up * _speed * Time.fixedDeltaTime, Space.World);

        // 타이머가 지속 시간보다 커지면 총알 게임오브젝트 파괴
        _timer += Time.fixedDeltaTime;
        if (_timer >= _duration)
        {
            Poolable poolable = GetComponent<Poolable>();

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
    }
}
