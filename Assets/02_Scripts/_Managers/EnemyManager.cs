using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 현재 살아있는 적들 관리할 때 필요할거같아서 만든 매니저
/// </summary>
public class EnemyManager : MonoBehaviour
{
    [SerializeField] EnemySpawner _enemySpawner;
    [SerializeField] ItemDropper _itemDropper;

    [Header("----- 현재 살아있는 적들(읽기 전용) -----")]
    [SerializeField] List<Enemy> _currentEnemies = new();   // 현재 살아있는 적들

    Enemy _enemy;
    
    public List<Enemy> CurrentEnemies => _currentEnemies;

    public event Action<Enemy> OnDeath; // 외부에 적 사망 알림

    public void Initialize(Transform hero)
    {
        // 적 스폰 이벤트 연결
        _enemySpawner.OnEnemySpawned += RegisterEnemy;
        // 리스트 한번 비워주기
        _currentEnemies.Clear();

        _enemySpawner.Initialize(hero); // 적 스폰러 초기화
        _itemDropper.Initialize(); // 아이템 드롭퍼 초기화
    }

    public void KillAllEnemies()
    {
        // 현재 살아있는 적들을 모두 죽인다.
        foreach (Enemy enemy in _currentEnemies)
        {
            if (enemy != null)
            {
                enemy.TakeHit(9999f);
            }
        }

        // 현재 살아있는 적들을 비운다.
        _currentEnemies.Clear();
    }

    /// <summary>
    /// 각각의 적이 스폰될때마다 적을 리스트에 등록하고
    /// 이벤트를 연결해주는 함수
    /// </summary>
    /// <param name="enemy">이번에 스폰된 적</param>
    public void RegisterEnemy(Enemy enemy)
    {
        _currentEnemies.Add(enemy);
        enemy.OnDeath += HandleEnemyDeath;
    }

    /// <summary>
    /// 적이 사망했을 때 호출되는 함수
    /// 적이 죽었을 때 장례식을 치뤄주는
    /// 즉, 후처리를 해주는 함수
    /// </summary>
    /// <param name="enemy"></param>
    void HandleEnemyDeath(Enemy enemy)
    {
        // 아이템 드롭 등 처리
        _itemDropper.DropItem(enemy);

        // 리스트에서 제거
        _currentEnemies.Remove(enemy);
        // 이벤트 연결 해제
        enemy.OnDeath -= HandleEnemyDeath;

        // 외부에 적 사망 알림
        OnDeath?.Invoke(enemy);
    }
}
