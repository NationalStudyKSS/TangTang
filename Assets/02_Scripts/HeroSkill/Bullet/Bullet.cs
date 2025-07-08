using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적과 충돌하여 데미지를 입히는 역할
/// </summary>
public class Bullet : MonoBehaviour
{
    // 총알 데미지
    [SerializeField] float _damage;
    // 충돌할 레이어 마스크
    [SerializeField] protected LayerMask _targetLayerMask;

    /// <summary>
    /// 데미지를 설정하는 함수
    /// </summary>
    /// <param name="damage"></param>
    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 상대 게임오브젝트의 레이어가
        // _targetLayerMask에 포함되면
        if (_targetLayerMask.Contains(collision.gameObject.layer))
        {
            // 충돌한 상대 게임오브젝트의 Enemy 컴포넌트를 가져온다.
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            // Enemy 컴포넌트가 실제로 있으면
            if (enemy != null)
            {
                Attack(enemy);
            }
        }
    }

    /// <summary>
    /// 적을 공격하는 함수
    /// </summary>
    /// <param name="enemy"></param>
    protected virtual void Attack(Enemy enemy)
    {
        enemy.TakeHit(_damage);
    }
}
