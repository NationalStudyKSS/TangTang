using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HogirlPunchBullet : Bullet
{
    float _returnDuration; // 쓸지말지 고민중

    // 받아오는 변수들
    [SerializeField] float _punchDistance; // 주먹이 얼마나 멀리까지 뻗는지
    [SerializeField] float _duration; // 주먹이 앞으로 뻗었다가 되돌아오는 시간
    [SerializeField] Transform _hero;
    [SerializeField] Vector3 _dir;

    // 하드코딩중...
    [SerializeField] Vector3 _startScale;
    [SerializeField] Vector3 _endScale;

    Coroutine _punchCoroutine;

    public void Initialize()
    {
        // (임시) 크기를 받아올 수 있으면 좋을듯
        _startScale = new Vector3(0.01f, 0.01f, 0.01f);
        _endScale = new Vector3(2f, 2f, 2f);

        _punchCoroutine = StartCoroutine(PunchRoutine());
    }

    IEnumerator PunchRoutine()
    {
        float t = 0;
        

        // 앞으로 이동 + 커짐
        while (t < _duration / 2f)
        {
            Vector3 startPos = _hero.position;
            Vector3 endPos = _hero.position + _dir * _punchDistance; // 원하는 거리만큼
            float lerp = t / (_duration / 2f);
            transform.position = Vector3.Lerp(startPos, endPos, lerp);
            transform.localScale = Vector3.Lerp(_startScale, _endScale, lerp);
            t += Time.deltaTime;
            yield return null;
        }

        t = 0;
        // 되돌아오기 + 작아짐
        while (t < _duration / 2f)
        {
            Vector3 startPos = _hero.position;
            Vector3 endPos = _hero.position + _dir * _punchDistance; // 원하는 거리만큼
            float lerp = t / (_duration / 2f);
            transform.position = Vector3.Lerp(endPos, startPos, lerp);
            transform.localScale = Vector3.Lerp(_endScale, _startScale, lerp);
            t += Time.deltaTime;
            yield return null;
        }

        Poolable poolable = GetComponent<Poolable>();
        if (poolable != null)
        {
            // Object Pooling을 사용하여 총알 게임오브젝트를 비활성화
            poolable.ReturnToPool();
        }
        else
        {
            // Object Pooling을 사용하지 않는 경우, Destroy로 게임오브젝트 파괴
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 총알의 지속 시간을 설정하는 함수
    /// </summary>
    /// <param name="duration"></param>
    public void SetDuration(float duration)
    {
        _duration = duration;
    }

    /// <summary>
    /// 이동 방향을 설정하는 함수
    /// </summary>
    /// <param name="dir"></param>
    public void SetDirection(Vector3 dir)
    {
        _dir = dir;

        // 총알이 날아가는 방향을 바라보도록 고개를 돌림
        transform.right = dir;
    }

    public void SetPunchDistance(float distance)
    {
        _punchDistance = distance;
    }

    public void SetHero(Transform hero)
    {
        _hero = hero;
    }
}
