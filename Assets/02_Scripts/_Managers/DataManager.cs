using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 데이터 관리 매니저
/// </summary>
public class DataManager : MonoBehaviour
{
    [Header("----- 게임 데이터 -----")]
    [SerializeField] HeroStatData _heroStatData; // 주인공 능력치 데이터
    [SerializeField] EnemyStatData _enemyStatData; // 적 능력치 데이터

    public HeroStatData HeroStatData => _heroStatData; // 주인공 능력치 데이터 접근 프로퍼티
    public EnemyStatData EnemyStatData => _enemyStatData; // 적 능력치 데이터 접근 프로퍼티

    public static DataManager _instance;

    public static DataManager Instance
    {
        get
        {
            // 인스턴스가 없으면
            if (_instance == null)
            {
                // 씬에서 DataManager를 찾음
                _instance = FindObjectOfType<DataManager>();
                // 씬에서 찾았는데도 _instance가 없으면
                if (_instance == null)
                {
                    // 새로운 GameObject를 생성하고
                    GameObject dm = new GameObject("DataManager");
                    // DataManager 컴포넌트를 추가하여 인스턴스를 생성
                    _instance = dm.AddComponent<DataManager>();

                }
            }
            // 인스턴스 반환
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            // 현재 인스턴스를 _instance로 설정하고
            _instance = this;
            // 게임 오브젝트를 파괴하지 않도록 설정
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 이미 인스턴스가 존재하면 현재 게임 오브젝트를 파괴
            Destroy(gameObject);
        }
    }
}
