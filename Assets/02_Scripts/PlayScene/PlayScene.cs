using System.Collections;
using System.Collections.Generic;
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

    [Header("----- 게임 상태(읽기 전용) -----")]
    [SerializeField] int _enmeyKillCount;   // 적 처치 수
    [SerializeField] float _playTime;       // 게임이 시작된 후 경과한 시간
    [SerializeField] int _playTimeInt;      // 초 단위로 변환해서 저장하는 변수

    public event UnityAction<int> OnPlayTimeChanged;

    private void Start()
    {
        _inputHandler.OnMoveInput += OnMoveInput;        // 이동 입력 이벤트를 연결
        
        // 스테이지 UI 이벤트 연결
        OnPlayTimeChanged += _playSceneView.SetPlayTime; // 게임 시간 변경 이벤트를 UI에 연결
        _hero.OnDamageChanged += _playSceneView.SetDamage; // 영웅의 공격력 변경 이벤트를 UI에 연결
        _hero.OnHpChanged += _playSceneView.SetHp; // 영웅의 체력 변경 이벤트를 UI에 연결
        _hero.OnExpChanged += _playSceneView.SetExp; // 영웅의 경험치 변경 이벤트를 UI에 연결
        _hero.OnLevelChanged += _playSceneView.SetLevel; // 영웅의 레벨 변경 이벤트를 UI에 연결
        _enemyManager.OnDeath += UpdateEnemyKillCount; // 적이 죽었을 때 적 처치 수를 업데이트하는 이벤트를 연결

        _hero.OnLevelChanged += OnLevelUp;

        // 게임 상태 초기화
        _enmeyKillCount = 0;
        _playTime = 0f;
        _playTimeInt = 0;

        // PlaySceneView 초기화(순서 중요함)
        _playSceneView.Initialize();
        // 임시(나중에 게임시작 시 영웅선택창 만들면 필요없을듯?)
        _hero.Initialize();
        // 업그레이드 시스템 초기화
        _upgrader.SetHero(_hero.gameObject);
        _upgrader.Initialize();

        // 구독 시점이 _hero.Initialize() 이후여야 하는데
        // 문제는 hero가 초기화할때 데미지를 한번 발행해서 스킬에 전달하려면 이걸 추가해야함..
        _hero.OnDamageChanged += _upgrader.GiveHeroCurrentDamage;
        _upgrader.GiveHeroCurrentDamage(_hero.CurrentDamage);    
        
        // Spawner 초기화
        _enemyManager.Initialize(_hero.transform);
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
}
