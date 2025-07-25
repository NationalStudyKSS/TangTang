using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class PlayScene : MonoBehaviour
{
    [Header("----- 컴포넌트 참조 -----")]
    // Hero와 Enemy 는 둘다 스폰 되는 조건이 있으므로 임시
    [SerializeField] Hero _hero;
    [SerializeField] EnemyManager _enemyManager; // 현재 살아있는 적들을 관리하는 매니저
    [SerializeField] Upgrader _upgrader; // 업그레이드 선택 시스템을 관리하는 컴포넌트
    [SerializeField] InputHandler _inputHandler;
    [SerializeField] PlaySceneView _playSceneView; // 게임 진행 중인 UI를 관리하는 컴포넌트
    [SerializeField] CinemachineVirtualCamera _camera;
    [SerializeField] DeadView _deadView;
    [SerializeField] StageFailView _stageFailView;
    [SerializeField] GroundReposition[] _grounds;

    [Header("----- 게임 상태(읽기 전용) -----")]
    [SerializeField] int _enmeyKillCount;   // 적 처치 수
    [SerializeField] float _playTime;       // 게임이 시작된 후 경과한 시간
    [SerializeField] int _playTimeInt;      // 초 단위로 변환해서 저장하는 변수

    public event Action<int> OnPlayTimeChanged;

    private void Start()
    {
        string heroId;
        // PlayScene에서 바로 시작되는 경우용
        if (string.IsNullOrEmpty(GameManager.Instance.HeroManager.SelectedHeroName))
        {
            Debug.Log("선택된 영웅이 없어 기본 영웅을 생성합니다.");
            heroId = "BunnyHogirl";
            GameManager.Instance.DataManager.SetHeroData(heroId);
        }
        // 정상적으로 선택된 영웅을 받아오면
        else
        {
            heroId = GameManager.Instance.HeroManager.SelectedHeroName;
            GameManager.Instance.DataManager.SetHeroData(heroId);
        }

        // 영웅 생성
        GameObject heroObj = Resources.Load<GameObject>($"Prefabs/Hero/{heroId}");

        if (heroObj != null)
        {
            GameObject hero = Instantiate(heroObj);

            // 카메라 설정
            _camera.Follow = hero.transform;

            _hero = hero.GetComponent<Hero>();
            if (_hero == null)
                Debug.LogError("Hero 컴포넌트를 찾지 못했습니다.");

            if (_hero.Model == null)
                Debug.LogError("HeroModel이 연결되지 않았습니다.");
        }
        else
        {
            Debug.Log("영웅 생성 실패");
            return;
        }
        
        // 스테이지 UI 이벤트 연결
        OnPlayTimeChanged += _playSceneView.SetPlayTime; // 게임 시간 변경 이벤트를 UI에 연결
        _hero.OnDamageChanged += _playSceneView.SetDamage; // 영웅의 공격력 변경 이벤트를 UI에 연결
        _hero.RaiseOnHpChanged += _playSceneView.SetHp; // 영웅의 체력 변경 이벤트를 UI에 연결
        _hero.OnExpChanged += _playSceneView.SetExp; // 영웅의 경험치 변경 이벤트를 UI에 연결
        _hero.OnLevelChanged += _playSceneView.SetLevel; // 영웅의 레벨 변경 이벤트를 UI에 연결
        _enemyManager.OnDeath += UpdateEnemyKillCount; // 적이 죽었을 때 적 처치 수를 업데이트하는 이벤트를 연결

        _hero.OnLevelChanged += OnLevelUp;
        _hero.RaiseOnDead += _deadView.OnDead;

        // 죽었을 때 창 이벤트 구독 및 초기화
        _deadView.YesButtonClicked += _hero.Revive;
        _deadView.NoButtonClicked += ShowFailResult;
        _deadView.Initialize();

        // 게임 상태 초기화
        _enmeyKillCount = 0;
        _playTime = 0f;
        _playTimeInt = 0;

        // PlaySceneView 초기화(순서 중요함)
        _playSceneView.Initialize();
        // 임시(나중에 게임시작 시 영웅선택창 만들면 필요없을듯?)
        _hero.Initialize();

        _inputHandler.OnMoveInput += OnMoveInput;        // 이동 입력 이벤트를 연결

        // 업그레이드 시스템 초기화
        _upgrader.SetHero(_hero.gameObject);
        _upgrader.Initialize();
        
        // Spawner 초기화
        _enemyManager.Initialize(_hero.transform);

        // GroundRepositions 초기화
        foreach (var ground in _grounds)
        {
            ground.Initialize(_hero.transform);
        }
    }

    private void Update()
    {
        UpdatePlayTime();
    }

    /// <summary>
    /// 이동 입력이 들어왔을 때 실행하는 함수
    /// </summary>
    /// <param name="inputVec"></param>
    public void OnMoveInput(Vector2 inputVec)
    {
        _hero.Move(inputVec);
    }

    public void OnLevelUp(int preLevel, int CurLevel)
    {
        _upgrader.OnLevelUp(CurLevel - preLevel); // 업그레이드 선택을 시작
    }

    /// <summary>
    /// 게임이 시작된 후 경과한 시간을 업데이트하는 함수
    /// </summary>
    private void UpdatePlayTime()
    {
        _playTime += Time.deltaTime;

        // 초 단위로 시간을 계산(내림을 이용해서 시간 변화 체크인데 올림도 큰 차이없을듯?)
        int playTimeInt = Mathf.FloorToInt(_playTime);

        // 초 단위로 값이 바뀔 때만 이벤트 발행
        // 업데이트문에서 계속 호출되므로, 초 단위로 값이 바뀔 때만 이벤트를 발생시켜야 함
        if (playTimeInt != _playTimeInt)
        {
            _playTimeInt = playTimeInt;
            OnPlayTimeChanged?.Invoke(_playTimeInt);
        }
    }

    private void UpdateEnemyKillCount(Enemy _)
    {
        _enmeyKillCount++;
        _playSceneView.SetEnemyKillCount(_enmeyKillCount);
    }
    
    void ShowFailResult()
    {
        _stageFailView.Initialize();
    }
}
