using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : Bullet<Hero>
{
    [SerializeField] float _duration;
    [SerializeField] Mover _mover;

    float _timer;
    Vector3 _dir;

    private void OnEnable()
    {
        _timer = 0f;
    }

    protected virtual void FixedUpdate()
    {
        _mover.Move(_dir);

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

    /// <summary>
    /// 이동 속력을 설정하는 함수
    /// </summary>
    /// <param name="speed"></param>
    public void SetSpeed(float speed)
    {
        _mover.SetSpeed(speed);
    }

    /// <summary>
    /// 총알의 지속 시간을 설정하는 함수
    /// </summary>
    /// <param name="duration"></param>
    public void SetDuration(float duration)
    {
        _duration = duration;
    }

    public void SetDirection(Vector3 dir)
    {
        _dir = dir;
    }

    protected override void Attack(Hero target)
    {
        // 일단 부모처럼 공격하고
        base.Attack(target);

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
