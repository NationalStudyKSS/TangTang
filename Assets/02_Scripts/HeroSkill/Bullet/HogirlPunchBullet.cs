using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HogirlPunchBullet : Bullet
{
    float _duration;
    float _returnDuration;
    Vector3 _startScale;
    Vector3 _endScale;

    Coroutine _punchCoroutine;

    public void Start()
    {
        _punchCoroutine = StartCoroutine(PunchRoutine());
    }

    IEnumerator PunchRoutine()
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
}
