using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public enum EnemyType
{
    Normal,      // 일반 적
    Elite,       // 정예 적
    Boss,        // 보스 적
}

public enum ElementType
{
    Fire,
    Water,
    Wind,
}

/// <summary>
/// 적에게 필요한 부품들을 조립해 적을 구성하는 역할
/// 기본상태(영웅쪽으로 이동),
/// 공격(부딪혀서),
/// 피격상태(데미지 입고 피격애니메이션), 
/// 사망상태(사망 애니메이션 후 콜라이더 리지드바디 끄고 사라짐)
/// </summary>
public abstract class Enemy : MonoBehaviour
{
    [Header("----- 컴포넌트 참조 -----")]
    [SerializeField] protected Mover _mover;             // 적 캐릭터의 이동을 담당하는 Mover 컴포넌트
    [SerializeField] protected EnemyModel _model;         // 적 캐릭터의 모델(데이터) 클래스
    [SerializeField] protected Animator _animator;        // 애니메이션을 담당하는 Animator 컴포넌트
    [SerializeField] protected Collider2D _collider;      // 적 캐릭터의 충돌을 담당하는 Collider2D 컴포넌트
    [SerializeField] protected Rigidbody2D _rigid;        // 적 캐릭터의 물리적 상호작용을 담당하는 Rigidbody2D 컴포넌트
    [SerializeField] protected Transform _spriteRoot;     // 스프라이트의 부모 오브젝트로, 스프라이트를 뒤집을 때 사용

    [Header("----- 공격 -----")]
    [SerializeField] protected LayerMask _targetLayerMask;   // 공격 타겟 레이어 마스크

    [Header("----- 임시 수치 -----")]
    [SerializeField] float _staggerDuration = 0.3f; // 피격 상태 지속시간
    [SerializeField] float _deathDuration = 0.7f;    // 죽음 상태 지속시간

    /// <summary>
    /// 적 캐릭터 상태 객체들
    /// </summary>
    protected EnemyState[] _states = new EnemyState[(int)EnemyStateType.Count];

    /// <summary>
    /// 현재 상태
    /// </summary>
    protected EnemyState _currentState;

    protected Coroutine _attackRoutine;       // 공격 코루틴 참조 변수
    protected Transform _target;              // 추적 대상

    public event Action<Enemy> OnDeath;       // 사망 이벤트

    //임시
    public virtual void Initialize()
    {
        _states = new EnemyState[(int)EnemyStateType.Count];

        _states[(int)EnemyStateType.Idle] = new IdleState(this);
        _states[(int)EnemyStateType.Stagger] = new StaggerState(this, _staggerDuration);
        _states[(int)EnemyStateType.Dead] = new DeadState(this, _deathDuration);

        // 물리와 충돌 다시 켜기
        _collider.enabled = true;
        _rigid.simulated = true;

        // 공격 코루틴 정리
        if (_attackRoutine != null)
        {
            StopCoroutine(_attackRoutine);
            _attackRoutine = null;
        }

        // 상태 초기화
        _currentState = null;

        // 구독 초기화 (중복 방지)
        _model.OnDead -= OnDead;
        _model.OnDead += OnDead;

        // 모델 초기화
        _model.Initialize();

        // 타겟 설정
        _target = GameObject.FindGameObjectWithTag("Hero")?.transform;

        // Mover 이벤트 중복 방지
        _mover.OnMoved -= OnMoved;
        _mover.OnMoved += OnMoved;

        // 상태 초기 진입
        ChangeState(EnemyStateType.Idle);
    }

    /// <summary>
    /// 상태를 변경하는 함수
    /// </summary>
    /// <param name="newState">새로운 상태</param>
    public virtual void ChangeState(EnemyStateType newState)
    {
        if (_currentState != null)
        {
            if (_currentState.StateType == EnemyStateType.Dead) return;
            if (_currentState.StateType == newState) return;

            _currentState.Exit();
        }

        _currentState = _states[(int)newState];
        _currentState.Enter();
    }

    protected virtual void FixedUpdate()
    {
        // 현재 상태 객체의 업데이트 함수를 호출한다.
        if (_currentState == null) return;

        _currentState.Update();
    }

    /// <summary>
    /// 추적 대상 방향으로 이동하는 함수
    /// </summary>
    public virtual void FollowTarget()
    {
        // 적 캐릭터에서 타겟(주인공) 위치로 향하는 방향 벡터 구하기
        Vector3 dir = (_target.position - transform.position).normalized;
        _mover.Move(dir);
    }

    /// <summary>
    /// Mover가 이동했을 때 자동으로 호출되는 함수
    /// </summary>
    /// <param name="moveVec"></param>
    protected virtual void OnMoved(Vector3 velocity)
    {
        // 이동 벡터의 x값에 따라 스프라이트를 뒤집기
        // 스프라이트를 뒤집는 방법은 여러 가지가 있지만,
        // 여기서는 스프라이트의 부모 오브젝트의 스케일을 조정하는 방법을 사용
        if (velocity.x > 0 && _spriteRoot.localScale.x > 0)
        {
            // 스프라이트가 오른쪽을 바라보도록 뒤집기
            _spriteRoot.localScale = new Vector3(-1, 1, 1);
        }
        if (velocity.x < 0 && _spriteRoot.localScale.x < 0)
        {
            // 스프라이트가 왼쪽을 바라보도록 뒤집기
            _spriteRoot.localScale = new Vector3(1, 1, 1);
        }
        // 애니메이터의 이동 속도 파라미터를 설정
        _animator.SetFloat(AnimatorParameters.MoveSpeed, velocity.magnitude);
    }

    /// <summary>
    /// 이동을 중단하는 함수
    /// </summary>
    public virtual void Stop()
    {
        // Mover에 (0,0,0)을 전달하여 이동을 중단
        _mover.Move(Vector3.zero);
    }

    // 1. 충돌 감지
    // 2. 주인공 캐릭터 확인
    // 3. 주인공 캐릭터 공격
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Hero")) return;
        // 1. 만들어 놓은 함수로 Layer 체크하는 방법
        if (_targetLayerMask.Contains(collision.gameObject.layer))
        {
            // 2. 주인공 캐릭터 확인
            Hero hero = collision.gameObject.GetComponent<Hero>();
            if (hero == null) return;
            if (hero != null)
            {
                // 3. 주인공 캐릭터 공격
                if (_attackRoutine == null)
                {
                    _attackRoutine = StartCoroutine(AttackRoutine(hero));
                }
            }
        }
    }

    /// <summary>
    /// 누구를 때릴거야?를 받아온 뒤
    /// 공격을 반복하는 코루틴
    /// </summary>
    /// <param name="hero"></param>
    /// <returns></returns>
    IEnumerator AttackRoutine(Hero hero)
    {
        while (true)
        {
            _animator.SetTrigger(AnimatorParameters.OnAttack);
            hero.TakeHit(_model.Damage, _model.Element);

            yield return null;

            yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0).Length);
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        // 공격 대상인 레이어마스크에 포함되는 레이어의 게임오브젝트와
        // 충돌이 끝나면
        if (_targetLayerMask.Contains(collision.gameObject.layer))
        {
            Hero hero = collision.gameObject.GetComponent<Hero>();
            if (hero != null)
            {
                // 코루틴 종료
                if (_attackRoutine != null)
                {
                    StopCoroutine(_attackRoutine);
                }
                _attackRoutine = null;
            }
        }
    }

    /// <summary>
    /// 물리적이던 뭐던 아무튼 피격을 당했을때 처리하는 함수
    /// </summary>
    /// <param name="damage"></param>
    public void TakeHit(float damage)
    {
        if (_model.CurrentHp <= 0) return;

        ChangeState(EnemyStateType.Stagger);
        _model.TakeDamage(damage, GameManager.Instance.HeroManager.Type);
    }

    /// <summary>
    /// 적 캐릭터가 죽었을 때 실행할 함수
    /// </summary>
    public void OnDead()
    {
        // 공격 코루틴 종료
        if (_attackRoutine != null)
        {
            StopCoroutine(_attackRoutine);
        }

        // 일단 죽었을 때 애니메이션 재생시키고
        _animator.SetTrigger(AnimatorParameters.OnDead);
        // 콜라이더 꺼주고
        _collider.enabled = false;
        // 리지드 바디도 꺼주고
        _rigid.simulated = false;

        // 현재 상태를 Death로 변경
        ChangeState(EnemyStateType.Dead);

        // 사망 이벤트 발행
        OnDeath?.Invoke(this);
    }

    /// <summary>
    /// 이 적 캐릭터 게임오브젝트를 제거하는 함수
    /// </summary>
    public virtual void Remove()
    {
        Poolable poolable = GetComponent<Poolable>();
        if (poolable != null)
        {
            poolable.ReturnToPool();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}