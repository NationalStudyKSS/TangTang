using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 주먹 스킬을 구현하는 클래스.
/// </summary>
public class FistSkill : MonoBehaviour
{
    [SerializeField] Transform _fist;     // 주먹 오브젝트
    [SerializeField] float _punchDistance = 1.5f; // 나갈 거리
    [SerializeField] float _punchDuration = 0.3f; // 나가는 시간
    [SerializeField] float _returnDuration = 0.2f; // 돌아오는 시간
    [SerializeField] Vector3 _punchScale = new Vector3(1.5f, 1.5f, 1.5f);

    Vector3 _originalLocalPosition;
    Vector3 _originalLocalScale;

    private void Awake()
    {
        _originalLocalPosition = _fist.localPosition;
        _originalLocalScale = _fist.localScale;
    }

    private void Start()
    {
        StopAllCoroutines();
        StartCoroutine(PunchRoutine());
    }

    IEnumerator PunchRoutine()
    {
        Vector3 startPos = _originalLocalPosition;
        Vector3 endPos = startPos + Vector3.right * _punchDistance;
        Vector3 startScale = _originalLocalScale;
        Vector3 endScale = _punchScale;

        while (true)
        {
            // 나가기
            float t = 0f;
            while (t < _punchDuration)
            {
                float lerp = t / _punchDuration;
                _fist.localPosition = Vector3.Lerp(startPos, endPos, lerp);
                _fist.localScale = Vector3.Lerp(startScale, endScale, lerp);
                t += Time.deltaTime;
                yield return null;
            }

            _fist.localPosition = endPos;
            _fist.localScale = endScale;

            // 돌아오기
            t = 0f;
            while (t < _returnDuration)
            {
                float lerp = t / _returnDuration;
                _fist.localPosition = Vector3.Lerp(endPos, startPos, lerp);
                _fist.localScale = Vector3.Lerp(endScale, startScale, lerp);
                t += Time.deltaTime;
                yield return null;
            }

            _fist.localPosition = startPos;
            _fist.localScale = startScale;

            yield return new WaitForSeconds(2f); // 다음 공격까지 대기
        }
    }

}
