using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class ElectricFieldSkill : ActiveSkill
{
    [SerializeField] float _radius; // 전기장 범위
    [SerializeField] float _damageInterval;

    List<Enemy> _enemiesInRange = new List<Enemy>();
    float _timer;
    Coroutine _attackRoutine;

    public override ActiveSkillType ActiveSkillType => ActiveSkillType.ElectricField;

    protected override void CalculateStats()
    {
        base.CalculateStats();

        _radius = _data.GetStat(ActiveSkillStatType.BulletRange, _level);
        _damageInterval = _data.GetStat(ActiveSkillStatType.FireDelay, _level);

        transform.localScale = new Vector3(_radius, _radius, 1);
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

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            foreach (Enemy enemy in _enemiesInRange.ToArray())
            {
                if (enemy != null)
                    enemy.TakeHit(_damage);
            }
            yield return new WaitForSeconds(_damageInterval);
        }
    }

    public override void Upgrade()
    {
        base.Upgrade();

        // AttackRoutine() 코루틴이 실행 중이 아니면
        if (_attackRoutine == null)
        {
            _attackRoutine = StartCoroutine(AttackRoutine());
        }
    }
}
