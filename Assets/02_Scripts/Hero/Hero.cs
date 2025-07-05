using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 주인공 캐릭터 담당 역할
/// 주인공 캐릭터 그 자체임.
/// 부품을 다 낀 상태의 로봇같은
/// </summary>
public class Hero : MonoBehaviour
{
    [Header("----- 컴포넌트 참조 -----")]
    [SerializeField] HeroModel _model;
    [SerializeField] Mover _mover;
    [SerializeField] Animator _animator;
    [SerializeField] HeroHudView _hudView; // 주인공 캐릭터의 HUD(Heads-Up Display) 뷰를 관리하는 컴포넌트
    [SerializeField] Transform _spriteRoot;     // flip대신 쓸 예정. sprite들이 모인 부모 오브젝트 선택하면됨.

    // event 변수를 프로퍼티처럼 쓰는 방법
    // 외부에서 Hero의 OnExpChanged 이벤트를 구독/해제하게 되면
    // 사실은 _model(HeroModel)의 OnExpChanged 이벤트를 구독/해제하게 되는 것과 같음
    // 중개해주는 역할임
    public event UnityAction<float, float> OnExpChanged
    {
        // 구독 동작 설정
        add => _model.OnExpChanged += value;
        // 구독 해제 동작 설정
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
        add => _model.OnSpeedChanged += value;
        remove => _model.OnSpeedChanged -= value;
    }

    public event UnityAction OnDeath
    {
        add => _model.OnDeath += value;
        remove => _model.OnDeath -= value;
    }

    public void Initialize()
    {
        _mover.OnMoved += OnMoved;
        _model.OnSpeedChanged += _mover.SetSpeed;
        _model.OnHpChanged += _hudView.ChangeHpBar;

        _model.Initialize();
    }

    /// <summary>
    /// 주인공 캐릭터를 지정된 방향으로 이동시키는 함수
    /// </summary>
    /// <param name="direction"></param>
    public void Move(Vector3 direction)
    {
        _mover.Move(direction);
    }

    /// <summary>
    /// 주인공 캐릭터가 이동했을 때 호출되는 함수
    /// </summary>
    /// <param name="velocity"></param>
    void OnMoved(Vector3 velocity)
    {
        if(velocity.x > 0)
        {
            _spriteRoot.localScale = new Vector3(-1, 1, 1);
        }
        if(velocity.x < 0)
        {
            _spriteRoot.localScale = new Vector3(1, 1, 1);
        }
        _animator.SetFloat(AnimatorParameters.MoveSpeed,velocity.magnitude);
    }

    /// <summary>
    /// 주인공 캐릭터가 공격을 받았을 때 호출되는 함수
    /// </summary>
    /// <param name="damage">받은 데미지 양</param>
    public void TakeHit(float amount)
    {
        _model.TakeDamage(amount);
    }

    /// <summary>
    /// 경험치를 획득하는 함수
    /// </summary>
    public void AddExp()
    {
        float amount = 0;
        _model.AddExp(amount);
    }
}
