using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    float _damage;
    float _duration;
    float _returnDuration;
    Vector3 _startScale;
    Vector3 _endScale;

    // 어차피 맞은 적은 데미지를 한번만 받으므로, HashSet을 사용하여 중복 방지
    HashSet<Enemy> _hitEnemies = new HashSet<Enemy>();

    public void Initialize(float distance, Vector3 scale, float duration, float returnDuration, float damage)
    {
        _startScale = Vector3.zero;
        _endScale = scale;
        _duration = duration;
        _returnDuration = returnDuration;
        _damage = damage;

        StartCoroutine(AnimatePunch());
    }

    IEnumerator AnimatePunch()
    {
        float t = 0;
        while (t < _duration)
        {
            float lerp = t / _duration;
            transform.localScale = Vector3.Lerp(_startScale, _endScale, lerp);
            t += Time.deltaTime;
            yield return null;
        }

        t = 0;
        while (t < _returnDuration)
        {
            float lerp = t / _returnDuration;
            transform.localScale = Vector3.Lerp(_endScale, _startScale, lerp);
            t += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null && !_hitEnemies.Contains(enemy))
        {
            _hitEnemies.Add(enemy);
            enemy.TakeHit(_damage);
        }
    }
}
