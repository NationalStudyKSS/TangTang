using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class ElectricFieldSkill : ActiveSkill
{
    public float radius = 5f;
    public float damagePerSecond = 10f;
    public float damageInterval = 1f;

    private CircleCollider2D _collider;
    private List<Enemy> _enemiesInRange = new List<Enemy>();
    private float _timer;

    public Transform hero; // 영웅 위치 추적용

    public override ActiveSkillType ActiveSkillType => ActiveSkillType.ElectricField;

    private void Start()
    {
        _collider = GetComponent<CircleCollider2D>();
        _collider.isTrigger = true;
        _collider.radius = radius;
    }

    private void Update()
    {
        // 영웅 위치 따라가기
        if (hero != null)
            transform.position = hero.position;

        // 지속 피해 주기용 타이머
        _timer += Time.deltaTime;
        if (_timer >= damageInterval)
        {
            _timer = 0f;
            Attack();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null && !_enemiesInRange.Contains(enemy))
        {
            _enemiesInRange.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null && _enemiesInRange.Contains(enemy))
        {
            _enemiesInRange.Remove(enemy);
        }
    }

    public void Attack()
    {
        foreach (Enemy enemy in _enemiesInRange.ToArray())
        {
            if (enemy != null)
                enemy.TakeHit(damagePerSecond);
        }
    }
}
