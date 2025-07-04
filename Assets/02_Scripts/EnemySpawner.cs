using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적프리펩을 저장해놓고 생성하는 역할
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("----- 컴포넌트 참조 -----")]
    [SerializeField] Enemy _enemyPrefab;    // 생성할 적 프리팹
    
    [Header("----- 스폰 데이터 -----")]
    [SerializeField] float _spawnSpan = 2f; // 적 생성 간격
    [SerializeField] float _minSpawnRange = 15f; // 최소 스폰 범위
    [SerializeField] float _maxSpawnRange = 20f; // 최대 스폰 범위

    ItemDropper _itemDropper; // 아이템 드롭퍼
    Transform _heroTransform; // 영웅의 Transform 컴포넌트 변수
    Coroutine _spawnRoutine;  // 생성 루틴

    public event Action<Enemy> OnEnemySpawned;

    public void Initialize(Transform target, ItemDropper itemDropper)
    {
        // 영웅의 Transform 컴포넌트를 받아와서 _heroTransform에 저장
        _heroTransform = target;
        // 아이템 드롭퍼를 받아와서 _itemDropper에 저장
        _itemDropper = itemDropper;
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
            // 영웅의 위치를 기준으로 랜덤한 위치를 계산
            Vector3 randomPos = _heroTransform.position + UnityEngine.Random.insideUnitSphere.normalized * UnityEngine.Random.Range(_minSpawnRange, _maxSpawnRange);
            // 적 생성 후 enemy 지역변수에 할당
            Enemy enemy = Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
            // 생성된 적을 EnemySpawner의 자식으로 설정
            enemy.transform.position = randomPos;

            // 적 초기화
            enemy.Initialize();

            // 적이 죽었을 때를 관리하는 EnemyDeathManager가 필요한가?
            //enemy.OnDeath += _itemDropper.DropItem; // 적이 죽었을 때 아이템 드롭

            OnEnemySpawned?.Invoke(enemy);

            // 생성 간격만큼 대기
            yield return new WaitForSeconds(_spawnSpan);
        }
    }
}
