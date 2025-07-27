using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] string _enemyPrefabPath;

    Transform _heroTransform; // 영웅의 Transform 컴포넌트 변수
    Coroutine _spawnRoutine;  // 생성 루틴

    int _playTimeInt;

    public event Action<Enemy> OnEnemySpawned; // 적이 생성되었을 때 발생하는 이벤트

    public void Initialize(Transform target, int playTimeInt)
    {
        _playTimeInt = playTimeInt;
        // 영웅의 Transform 컴포넌트를 받아와서 _heroTransform에 저장
        _heroTransform = target;
        // 적 생성 루틴 시작
        _spawnRoutine = StartCoroutine(SpawnEnemyRoutine());
    }

    /// <summary>
    /// 적을 생성하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnEnemyRoutine()
    {
        // 뭔가 에러가 있어서 1초 대기
        yield return new WaitForSeconds(10f);

        SpawnEnemy();
    }

    /// <summary>
    /// 적을 Object Pooling을 이용해 생성하는 함수
    /// </summary>
    public void SpawnEnemy()
    {
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

        // 적 초기화
        enemy.Initialize(_playTimeInt);

        // 적이 생성되었을 때 이벤트를 발생시킴
        OnEnemySpawned?.Invoke(enemy);
    }
}
