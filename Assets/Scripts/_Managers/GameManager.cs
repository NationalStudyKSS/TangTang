using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 싱글톤으로 유지되어 게임 전역에서 데이터를 관리하는 역할
/// </summary>
public class GameManager : MonoBehaviour
{
    static GameManager _Instance;

    [SerializeField] int _gold;

    //DataManager _dataManager;

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

                    //// 기타 매니저들 자식으로 추가
                    //_Instance._dataManager = FindObjectOfType<DataManager>();
                    //if(_Instance._dataManager == null)
                    //{
                    //    GameObject dm = new GameObject("DataManager");
                    //    _Instance._dataManager = dm.AddComponent<DataManager>();
                    //    _Instance._dataManager.setparent
                    //}
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
    }

    private void Start()
    {
        
    }
}
