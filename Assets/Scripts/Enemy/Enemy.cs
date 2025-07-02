using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 적에게 필요한 부품들을 조립해 적을 구성하는 역할
/// 기본, 이동, 공격, 피격, 사망
/// 애니메이션은 상태머신에서 처리할 예정
/// </summary>
public class Enemy : MonoBehaviour
{
    [Header("----- 컴포넌트 참조 -----")]
    [SerializeField] Mover _mover;            // 적 캐릭터의 이동을 담당하는 Mover 컴포넌트
    [SerializeField] EnemyModel _model;       // 적 캐릭터의 모델(데이터) 클래스
    [SerializeField] Animator _animator;      // 애니메이션을 담당하는 Animator 컴포넌트
    [SerializeField] Collider2D _collider;    // 적 캐릭터의 충돌을 담당하는 Collider2D 컴포넌트
    [SerializeField] Rigidbody2D _rigid;      // 적 캐릭터의 물리적 상호작용을 담당하는 Rigidbody2D 컴포넌트
    [SerializeField] Transform _spriteRoot;   // 스프라이트의 부모 오브젝트로, 스프라이트를 뒤집을 때 사용

    [Header("----- 공격 -----")]
    // 공격 타겟 레이어 마스크
    [SerializeField] LayerMask _targetLayerMask;
    // 공격 간격(초)
    [SerializeField] float _attackSpan;

    // 현재 상태 객체를 가리키는 인터페이스 변수
    IEnemyState _currentState;

    // 공격 코루틴 참조 변수
    Coroutine _attackRoutine;

    // 추적 대상
    Transform _target;

    //임시
    public void Initialize()
    {
        // Hero태그를 가진 게임 오브젝트를 찾아서 타겟으로 설정
        Transform target = GameObject.FindGameObjectWithTag("Hero")?.transform;
        _target = target;

        _mover.OnMoved += OnMoved;
        _model.OnDeath += OnDeath;

        _model.Initialize();

        ChangeState(EnemyState.Idle);
    }
    void ChangeState(EnemyState newState)
    {
        if (_currentState != null)
        {
            // 현재 상태가 죽음 상태이면 전환하지 않음
            if (_currentState.State == EnemyState.Death) return;

            // 현재 상태가 바꾸려는 상태와 같으면 전환하지 않음
            if (_currentState.State == newState) return;

            // 현재 상태 종료
            _currentState.Exit();
        }

        switch (newState)
        {
            case EnemyState.Stagger:
                // 피격 상태로 전환
                _currentState = new StaggerState(this, 0.5f); // 피격 상태 지속시간 0.5초
                break;
            case EnemyState.Death:
                // 죽음 상태로 전환
                _currentState = new DeathState(this, 1.0f);   // 죽음 상태 지속시간 1초
                break;
            default:
                // 기본 상태로 전환
                _currentState = new IdleState(this);
                break;
        }
        // 새 현재 상태 시작
        _currentState.Enter();
    }

    private void FixedUpdate()
    {
        // 현재 상태 객체의 업데이트 함수를 호출한다.
        if (_currentState == null) return;

        _currentState.Update();
    }

    /// <summary>
    /// 추적 대상 방향으로 이동하는 함수
    /// </summary>
    void FollowTarget()
    {
        // 적 캐릭터에서 타겟(주인공) 위치로 향하는 방향 벡터 구하기
        Vector3 dir = (_target.position - transform.position).normalized;
        _mover.Move(dir);
        //OnMoved(dir);
    }

    /// <summary>
    /// Mover가 이동했을 때 자동으로 호출되는 함수
    /// </summary>
    /// <param name="moveVec"></param>
    void OnMoved(Vector3 velocity)
    {
        // 이동 벡터의 x값에 따라 스프라이트를 뒤집기
        // 스프라이트를 뒤집는 방법은 여러 가지가 있지만,
        // 여기서는 스프라이트의 부모 오브젝트의 스케일을 조정하는 방법을 사용
        if (velocity.x > 0)
        {
            // 스프라이트가 오른쪽을 바라보도록 뒤집기
            _spriteRoot.localScale = new Vector3(-1, 1, 1);
        }
        if (velocity.x < 0)
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
    void Stop()
    {
        // Mover에 (0,0,0)을 전달하여 이동을 중단
        _mover.Move(Vector3.zero);
    }

    // 1. 충돌 감지
    // 2. 주인공 캐릭터 확인
    // 3. 주인공 캐릭터 공격
    private void OnCollisionEnter2D(Collision2D collision)
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
                _attackRoutine = StartCoroutine(AttackRoutine(hero));
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
            hero.TakeHit(_model.Damage);

            yield return new WaitForSeconds(_attackSpan);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
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
                    StopCoroutine(_attackRoutine);
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

        ChangeState(EnemyState.Stagger);
        _model.TakeDamage(damage);
    }

    /// <summary>
    /// 적 캐릭터가 죽었을 때 실행할 함수
    /// </summary>
    public void OnDeath()
    {
        // 현재 상태를 Death로 변경
        ChangeState(EnemyState.Death);
    }

    /// <summary>
    /// 이 적 캐릭터 게임오브젝트를 제거하는 함수
    /// </summary>
    void Remove()
    {
        Destroy(gameObject);
    }

    // 적 캐릭터의 상태 종류 enum
    public enum EnemyState
    {
        Idle,       // 기본 상태
        Stagger,    // 피격 상태(휘청거리다)
        Death,      // 죽은 상태
    }

    // 적 캐릭터의 상태 인터페이스
    public interface IEnemyState
    {
        // 인터페이스는 프로퍼티도 멤버로 가질 수 있다.
        EnemyState State { get; }

        // 이 상태가 시작했을 때 동작하는 함수
        void Enter();

        // 이 상태가 진행 중일 때 반복적으로 동작하는 함수
        // (유니티 Update()와는 상관 X)
        void Update();

        // 이 상태가 끝났을 때 동작하는 함수
        void Exit();
    }

    public class IdleState : IEnemyState
    {
        public EnemyState State => EnemyState.Idle;
        Enemy _enemy;

        public IdleState(Enemy enemy)
        {
            _enemy = enemy;
        }

        public void Enter()
        {

        }

        public void Exit()
        {
            _enemy.Stop();
        }

        // 일반 상태인 동안에는 반복적으로 타겟 추적 실행
        public void Update()
        {
            // IdleState가 Enemy 클래스의 내부 클래스기 때문에
            // private 함수도 사용 가능
            _enemy.FollowTarget();
        }
    }

    /// <summary>
    /// 적 캐릭터가 피격당했을 때의 상태
    /// </summary>
    public class StaggerState : IEnemyState
    {
        public EnemyState State => EnemyState.Stagger;
        Enemy _enemy;           // 피격당한 적 캐릭터 참조
        float _timer;           // 피격 상태 타이머
        float _duration;        // 피격 상태 지속시간

        /// <summary>
        /// 적 캐릭터가 피격당했을 때의 상태 생성자
        /// </summary>
        /// <param name="enemy">변수로 받아서 넣어줄 적</param>
        /// <param name="duration">변수로 받아서 넣어줄 지속시간</param>
        public StaggerState(Enemy enemy, float duration)
        {
            _enemy = enemy;
            _duration = duration;
        }

        public void Enter()
        {
            // 피격 상태에 들어갈 때 타이머 초기화
            _timer = 0;
            // 애니메이터에 피격 애니메이션 트리거 설정
            _enemy._animator.SetTrigger(AnimatorParameters.OnHit);
            // 적 캐릭터의 상태를 Stagger로 변경
            _enemy.ChangeState(EnemyState.Stagger);
        }

        public void Exit()
        {
            
        }

        public void Update()
        {
            // 피격 상태 동안 타이머를 증가시키고
            _timer += Time.deltaTime;
            // 지속시간이 지나면
            if (_timer > _duration)
            {
                // 타이머를 초기화 한 뒤
                _timer = 0;
                // 적 캐릭터의 상태를 Idle로 변경
                _enemy.ChangeState(EnemyState.Idle);
            }
        }
    }

    /// <summary>
    /// 적 캐릭터가 죽었을 때의 상태를 처리하는 역할
    /// </summary>
    public class DeathState : IEnemyState
    {
        public EnemyState State => EnemyState.Death;
        Enemy _enemy;           // 죽은 적 캐릭터 참조
        float _timer;           // 사망 상태 타이머
        float _duration;        // 사망 상태 지속시간
        /// <summary>
        /// 적 캐릭터가 죽었을 때의 상태 생성자
        /// </summary>
        /// <param name="enemy">참조할 적</param>
        /// <param name="duration">지속시간</param>
        public DeathState(Enemy enemy, float duration)
        {
            _enemy = enemy;       // 적 캐릭터 참조  
            _duration = duration; // 사망 상태 지속시간
        }
        public void Enter()
        {
            // 일단 죽었을 때 애니메이션 재생시키고
            _enemy._animator.SetTrigger(AnimatorParameters.OnDeath);
            // 콜라이더 꺼주고
            _enemy._collider.enabled = false;
            // 리지드 바디도 꺼주고
            _enemy._rigid.simulated = false;

            // 공격 코루틴 종료
            if (_enemy._attackRoutine != null)
            {
                _enemy.StopCoroutine(_enemy._attackRoutine);
            }

            _timer = 0;
        }
        public void Exit()
        {
            
        }
        public void Update()
        {
            // 사망 상태 동안 타이머를 증가시키고
            _timer += Time.deltaTime;
            // 사망 상태가 지속시간이 지나면
            if (_timer > _duration)
            {
                _timer = 0;
                // 적 캐릭터를 제거하는 함수 호출
                _enemy.Remove();
            }
        }
    }
}

