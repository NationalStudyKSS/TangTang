using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEditor.EditorTools;
using UnityEngine;

/// <summary>
/// 싱글톤으로 유지되어 게임 전역에서 데이터를 관리하는 역할
/// </summary>
public class GameManager : MonoBehaviour
{
    static GameManager _Instance;

    [Header("----- 각종 매니저들 (ReadOnly) -----")]
    [SerializeField] ResourceManager _resourceManager; // 리소스 매니저
    [SerializeField] PoolManager _poolManager; // 오브젝트 풀 매니저
    [SerializeField] DataManager _dataManager; // 데이터 매니저

    public ResourceManager ResourceManager => _resourceManager; // 리소스 매니저 접근 프로퍼티
    public PoolManager PoolManager => _poolManager; // 오브젝트 풀 매니저 접근 프로퍼티
    public DataManager DataManager => _dataManager; // 데이터 매니저 접근 프로퍼티


    /// <summary>
    /// 게임 매니저 인스턴스에 접근하는 프로퍼티
    /// </summary>
    public static GameManager Instance
    {
        get
        {
            // 인스턴스가 없으면
            if (_Instance == null)
            {
                // 씬에서 GameManager를 찾음
                _Instance = FindObjectOfType<GameManager>();
                // 씬에서 찾았는데도 _instance가 없으면
                if (_Instance == null)
                {
                    // 새로운 GameObject를 생성하고
                    GameObject go = new GameObject("GameManager");
                    // GameManager 컴포넌트를 추가하여 인스턴스를 생성
                    _Instance = go.AddComponent<GameManager>();
                }
            }
            // 인스턴스 반환
            return _Instance;
        }
    }

    private void Awake()
    {
        // _Instance가 null이면
        if (_Instance == null)
        {
            // 현재 인스턴스를 _Instance로 설정하고
            _Instance = this;
            // 게임 오브젝트를 파괴하지 않도록 설정
            DontDestroyOnLoad(gameObject);
        }
        // _Instance가 null이 아니면
        else
        {
            // 중복된 인스턴스는 파괴
            Destroy(gameObject);
        }

        // ResourceManager 컴포넌트 가져오기
        _resourceManager = GetComponent<ResourceManager>();
        // ResourceManager 컴포넌트가 없으면
        if (_resourceManager == null)
        {
            // ResourceManager 추가
            _resourceManager = gameObject.AddComponent<ResourceManager>();
        }

        // PoolManager 컴포넌트 가져오기
        _poolManager = GetComponent<PoolManager>();
        // PoolManager 컴포넌트가 없으면
        if (_poolManager == null)
        {
            // PoolManager 추가
            _poolManager = gameObject.AddComponent<PoolManager>();
        }
        // ResourceManager 초기화
        _resourceManager.Initialize();
        // PoolManager 초기화
        _poolManager.Initialize(_resourceManager);

        // DataManager 컴포넌트 가져오기
        _dataManager = GetComponent<DataManager>();
        // DataManager 컴포넌트가 없으면
        if (_dataManager == null)
        {
            // DataManager 추가
            _dataManager = gameObject.AddComponent<DataManager>();
        }
        // DataManager 초기화
        _dataManager.Initialize(_resourceManager);
    }

    private void Start()
    {
        
    }
}
