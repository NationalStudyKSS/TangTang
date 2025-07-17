using UnityEngine;

public class RangedAttackEnemy : Enemy, IRangedAttackable
{
    [Header("----- Enemy가 가져야 할 스탯 -----")]
    [SerializeField] float _attackRange;
    [SerializeField] float _attackSpan;

    [Header("----- Bullet에게 전달해줘야 할 스탯 -----")]
    [SerializeField] float _bulletSpeed;
    [SerializeField] float _bulletDuration;

    public float AttackRange => _attackRange;

    public float AttackSpan => _attackSpan;

    public float BulletSpeed => _bulletSpeed; 

    public float BulletDuration => _bulletDuration;

    public override void Initialize()
    {
        base.Initialize();

        _states[(int)EnemyStateType.RangedAttack] = new RangedAttackState(this, this, _attackSpan);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        Collider2D hit = Physics2D.OverlapCircle(transform.position, _attackRange, _targetLayerMask);

        // 사거리 내에 대상이 있다면 RangedAttack 상태로 전환
        if (hit != null)
        {
            if (_currentState.StateType != EnemyStateType.RangedAttack)
            {
                ChangeState(EnemyStateType.RangedAttack);
            }
        }
        else
        {
            if (_currentState.StateType == EnemyStateType.RangedAttack)
            {
                ChangeState(EnemyStateType.Idle);
            }
        }
    }

    public void SpawnBullet()
    {
        // 일단 총알을 생성하고
        GameObject go = GameManager.Instance.PoolManager.GetFromPool("Bullet/Arrow");
        if (go == null)
        {
            Debug.LogError("Arrow 프리팹을 찾을 수 없습니다.");
            return;
        }

        EnemyArrow bullet = go.GetComponent<EnemyArrow>();
        if (bullet == null)
        {
            Debug.LogError("EnemyArrow 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        // bullet의 위치를 이 게임오브젝트 위치로 설정
        bullet.transform.position = transform.position;

        // 총알이 생성될 때 총알의 스텟들을 설정
        bullet.SetDamage(Model.Damage);
        bullet.SetDirection(GetDirection());
        bullet.SetDuration(_bulletDuration);
        bullet.SetSpeed(_bulletSpeed);
    }

    public Vector3 GetDirection()
    {
        Debug.Log(_target.transform.position);
        Debug.Log(transform.position);
        Vector3 direction = (_target.transform.position - transform.position).normalized;
        return direction;
    }
}
