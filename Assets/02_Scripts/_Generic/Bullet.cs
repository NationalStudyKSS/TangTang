using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// _damage와 _element를 갖고 _targetLayerMask를 가진 게임오브젝트와 충돌을 감지하여 데미지를 준다.
/// 충돌했을 때 파괴된다던지 풀로 돌아간다던지는 모르는 클래스
/// </summary>
/// <typeparam name="TTarget">이걸 사용하는 입장에서 적으로 간주하는 대상</typeparam>
public class Bullet<TTarget> : MonoBehaviour where TTarget : MonoBehaviour, IDamageable
{
    [SerializeField] protected float _damage;
    [SerializeField] protected ElementType _type;
    [SerializeField] protected LayerMask _targetLayerMask;

    public float Damage => _damage;
    public ElementType Type => _type;
    public LayerMask TargetLayerMask => _targetLayerMask;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (_targetLayerMask.Contains(collision.gameObject.layer))
        {
            TTarget target = collision.gameObject.GetComponent<TTarget>();

            if (target != null)
            {
                Attack(target);
            }
        }
    }

    /// <summary>
    /// 데미지를 설정하는 함수
    /// </summary>
    /// <param name="damage"></param>
    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    protected virtual void Attack(TTarget target)
    {
        target.TakeHit(_damage, _type);
    }
}
