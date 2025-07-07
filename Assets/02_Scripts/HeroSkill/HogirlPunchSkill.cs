using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 호걸 펀치 스킬을 구현하는 클래스
/// </summary>
public class HogirlPunchSkill : MonoBehaviour
{
    [Header("----- 호걸 펀치 스킬 데이터(나중에 받아올거임) -----")]
    [SerializeField] Transform _fist;             // 주먹의 크기를 조절할 오브젝트(Renderer 달린거)
    [SerializeField] Vector3 _startPos;           // 처음 시작했을 때의 주먹 위치
    [SerializeField] Vector3 _startScale;         // 처음 시작했을 때의 주먹 크기
    [SerializeField] Vector3 _endPos;             // 끝에 도달했을 때의 주먹 위치
    [SerializeField] Vector3 _endScale;           // 끝에 도달했을 때의 주먹 크기
    [SerializeField] LayerMask _detectingLayer;   // 감지할 레이어 마스크
    [SerializeField] int _punchNum;               // 주먹이 나갈 개수
    [SerializeField] float _punchDistance;        // 주먹이 나가는 거리 = 적 감지 범위
    [SerializeField] float _punchDuration;        // 주먹이 나가는 시간
    [SerializeField] float _punchCoolTime;        // 주먹 스킬 쿨타임

    float _timer;
    Coroutine _attackRoutine;

    public void Initialize()
    {

    }

    public void Attack()
    {
        for (int i = 0; i < _punchNum; i++)
        {
            // 주먹을 이동시키는 코루틴 시작
            _attackRoutine = StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            _fist.transform.position = _startPos; // 주먹의 위치를 시작 위치로 초기화
            _fist.transform.localScale = _startScale; // 주먹의 크기를 시작 크기로 초기화

            _timer = 0f;
            if (_timer < _punchDuration)
            {
                _timer = 0f; // 타이머 초기화
                // 주먹의 위치와 크기를 보간하여 이동
            }

            yield return new WaitForSeconds(_punchCoolTime); // 잠시 대기
        }
        

    }

    /// <summary>
    /// 가장 가까운 적들을 감지하는 함수
    /// </summary>
    /// <returns></returns>
    Enemy[] DetectNearestEnemies(int punchNum)
    {
        // 최대 20개의 적을 감지할 수 있는 배열
        Collider2D[] colliders = new Collider2D[20];    
        // 감지범위 내에 있는 적들의 수를 저장
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, _punchDistance, colliders, _detectingLayer);
        
        // 적을 저장할 리스트 생성
        List<Enemy> enemies = new List<Enemy>();

        // 감지된 적 수만큼 돌면서
        for (int i = 0; i < count; i++)
        {
            // 적이 있는지 확인
            if (colliders[i] != null)
            {
                // Enemy 컴포넌트를 가져와서
                Enemy enemy = colliders[i].GetComponent<Enemy>();
                if (enemy != null)
                {
                    // 리스트에 추가
                    enemies.Add(enemy);
                }
            }
        }

        // 거리가 가까운 순서대로 정렬
        enemies.Sort((a, b) =>
        Vector2.Distance(transform.position, a.transform.position)
        .CompareTo(Vector2.Distance(transform.position, b.transform.position)));

        return enemies.Take(punchNum).ToArray();
    }
}
