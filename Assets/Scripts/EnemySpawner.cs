using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적프리펩을 저장해놓고 생성하는 역할
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("----- 적 생성 데이터 -----")]
    [SerializeField] GameObject _enemyPrefab; // 적 프리팹
    [SerializeField] Transform _spawnPoint; // 적 생성 위치
    [SerializeField] float _spawnSpan = 2.0f; // 적 생성 간격
    [SerializeField] bool _isSpawnEnabled = true; // 적 생성 활성화 여부
    
    private List<GameObject> _enemies = new List<GameObject>(); // 현재 생성된 적 목록
    private GameObject _enemy;


    void SpawnEnemy()
    {
        // null 체크
        if (_enemyPrefab == null || _spawnPoint == null)
        {
            Debug.LogError("적 프리팹 또는 생성 위치가 설정되지 않았습니다.");
            return;
        }
        


        // 적 프리팹을 생성
        GameObject enemy = Instantiate(_enemyPrefab, transform.position, Quaternion.identity);

        // 적 초기화
        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            enemyComponent.Initialize();
        }
    }
}
