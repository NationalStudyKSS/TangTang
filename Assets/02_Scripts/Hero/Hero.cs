using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 주인공 캐릭터 담당 역할
/// 주인공 캐릭터 그 자체임.
/// 부품을 다 낀 상태의 로봇같은 역할.
/// </summary>
public class Hero : MonoBehaviour
{
    [Header("----- 컴포넌트 참조 -----")]
    [SerializeField] HeroModel _model;
    [SerializeField] Mover _mover;
    [SerializeField] Animator _animator;
    [SerializeField] HeroHudView _hudView;  // HUD 관리 컴포넌트
    [SerializeField] Transform _spriteRoot; // 캐릭터 스프라이트 부모 오브젝트 (좌우 반전 용)

    public float CurrentDamage => _model.CurrentDamage; // 현재 공격력 전달용(임시)

    // 이벤트 중개 - 외부에서 Hero의 이벤트를 구독하면 내부 _model의 이벤트 구독과 동일하게 처리
    public event UnityAction<float, float> OnExpChanged
    {
        add => _model.OnExpChanged += value;
        remove => _model.OnExpChanged -= value;
    }

    public event UnityAction<int, int> OnLevelChanged
    {
        add => _model.OnLevelChanged += value;
        remove => _model.OnLevelChanged -= value;
    }

    public event UnityAction<float, float> OnHpChanged
    {
        add => _model.OnHpChanged += value;
        remove => _model.OnHpChanged -= value;
    }

    public event UnityAction<float> OnDamageChanged
    {
        add => _model.OnDamageChanged += value;
        remove => _model.OnDamageChanged -= value;
    }

    public event UnityAction<float> OnSpeedChanged
    {
        add => _model.OnMoveSpeedChanged += value;  // HeroModel에서 이벤트명 변경됨
        remove => _model.OnMoveSpeedChanged -= value;
    }

    public event UnityAction OnDeath
    {
        add => _model.OnDeath += value;
        remove => _model.OnDeath -= value;
    }

    public void Initialize()
    {
        _mover.OnMoved += OnMoved;
        _model.OnMoveSpeedChanged += _mover.SetSpeed;  // 이벤트명 수정
        _model.OnHpChanged += _hudView.ChangeHpBar;

        _model.Initialize();
    }

    /// <summary>
    /// 주인공 캐릭터를 지정된 방향으로 이동시키는 함수
    /// </summary>
    public void Move(Vector3 direction)
    {
        _mover.Move(direction);
    }

    /// <summary>
    /// 주인공 캐릭터가 이동했을 때 호출되는 함수
    /// </summary>
    void OnMoved(Vector3 velocity)
    {
        if (velocity.x > 0)
            _spriteRoot.localScale = new Vector3(-1, 1, 1);
        else if (velocity.x < 0)
            _spriteRoot.localScale = new Vector3(1, 1, 1);

        _animator.SetFloat(AnimatorParameters.MoveSpeed, velocity.magnitude);
    }

    /// <summary>
    /// 주인공 캐릭터가 공격을 받았을 때 호출되는 함수
    /// </summary>
    public void TakeHit(float damageAmount)
    {
        _model.TakeDamage(damageAmount);
    }

    /// <summary>
    /// 경험치를 획득하는 함수
    /// </summary>
    /// <param name="amount">획득할 경험치량</param>
    public void AddExp(float amount)
    {
        _model.AddExp(amount);
    }
}
