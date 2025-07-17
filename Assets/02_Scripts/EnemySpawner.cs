using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적을 단순히'스폰'하는 역할
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] string _enemyPrefabPath;

    [Header("----- 스폰 데이터(나중에 데이터로 정리) -----")]
    [SerializeField] float _spawnSpan = 2f; // 적 생성 간격
    [SerializeField] float _minSpawnRange = 15f; // 최소 스폰 범위
    [SerializeField] float _maxSpawnRange = 20f; // 최대 스폰 범위

    Transform _heroTransform; // 영웅의 Transform 컴포넌트 변수
    Coroutine _spawnRoutine;  // 생성 루틴

    public event Action<Enemy> OnEnemySpawned; // 적이 생성되었을 때 발생하는 이벤트

    public void Initialize(Transform target)
    {
        // 영웅의 Transform 컴포넌트를 받아와서 _heroTransform에 저장
        _heroTransform = target;
        // 적 생성 루틴 시작
        _spawnRoutine = StartCoroutine(SpawnEnemyRoutine());
    }

    private void OnDrawGizmosSelected()
    {
        // Initialize()에서 _heroTransform을 받아오고 있으므로
        // Play하기 전에는 _heroTransform이 null이니까 return시킴
        if (_heroTransform == null) return;

        // Gizmos를 사용하여 스폰 범위를 시각적으로 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_heroTransform.position, _minSpawnRange);
        Gizmos.DrawWireSphere(_heroTransform.position, _maxSpawnRange);
    }

    /// <summary>
    /// 적을 생성하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnEnemyRoutine()
    {
        // 뭔가 에러가 있어서 1초 대기
        yield return new WaitForSeconds(1f);

        // 무한 루프를 돌면서 적을 계속 생성
        while (true)
        {
            SpawnEnemy();
            // 생성 간격만큼 대기
            yield return new WaitForSeconds(_spawnSpan);
        }
    }

    /// <summary>
    /// 적을 Object Pooling을 이용해 생성하는 함수
    /// </summary>
    public void SpawnEnemy()
    {
        // 이거보다 더 확실하게 Vector2로 구하는게 나음

        //// 영웅의 위치를 기준으로 랜덤한 위치를 계산
        //Vector3 randomPos = _heroTransform.position + UnityEngine.Random.insideUnitSphere.normalized * UnityEngine.Random.Range(_minSpawnRange, _maxSpawnRange);
        //// 이거 안해서 적이 z가 이상해져서 안보이는 적이 생겼음...
        //randomPos.z = 0f; // 2D 게임이므로 z축은 0으로 설정

        Vector2 dir = UnityEngine.Random.insideUnitCircle.normalized;
        float dist = UnityEngine.Random.Range(_minSpawnRange, _maxSpawnRange);
        Vector3 randomPos = _heroTransform.position + new Vector3(dir.x * dist, dir.y * dist, 0f);

        // 적 생성 후 enemy 지역변수에 할당
        GameObject go = GameManager.Instance.PoolManager.GetFromPool(_enemyPrefabPath);
        if (go == null)
        {
            Debug.LogError("Enemy 프리팹을 찾을 수 없습니다.");
            return;
        }

        Enemy enemy = go.GetComponent<Enemy>();
        if (enemy == null)
        {
            Debug.LogError("Enemy 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        // 생성된 적을 EnemySpawner의 자식으로 설정
        enemy.transform.position = randomPos;

        // 적 초기화
        enemy.Initialize();

        // 적이 생성되었을 때 이벤트를 발생시킴
        OnEnemySpawned?.Invoke(enemy);
    }
}
