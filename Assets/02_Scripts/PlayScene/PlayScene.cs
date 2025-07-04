using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayScene : MonoBehaviour
{
    [Header("----- 컴포넌트 참조 -----")]
    // Hero와 Enemy 는 둘다 스폰 되는 조건이 있으므로 임시
    [SerializeField] Hero _hero;
    [SerializeField] InputHandler _inputHandler;
    [SerializeField] EnemySpawner _enemySpawner;
    [SerializeField] ItemDropper _itemDropper; // 아이템 드롭퍼
    [SerializeField] PlaySceneView _playSceneView; // 게임 진행 중인 UI를 관리하는 컴포넌트

    [Header("----- 게임 상태(읽기 전용) -----")]
    [SerializeField] int _enmeyKillCount;   // 적 처치 수
    [SerializeField] float _playTime;       // 게임이 시작된 후 경과한 시간
    [SerializeField] int _playTimeInt;      // 이벤트를 업데이트문에서 돌리고 있기 때문에 초 단위로 변환해서 저장하는 변수

    public event UnityAction<int> OnPlayTimeChanged;

    private void Start()
    {
        _inputHandler.OnMoveInput += OnMoveInput;        // 이동 입력 이벤트를 연결
        _enemySpawner.OnEnemySpawned += OnEnemySpawned;  // 적이 생성되었을 때 이벤트를 연결(중개 역할)
        
        OnPlayTimeChanged += _playSceneView.SetPlayTime; // 게임 시간 변경 이벤트를 UI에 연결

        // 게임 상태 초기화
        _enmeyKillCount = 0;
        _playTime = 0f;

        // 임시(나중에 게임시작 시 영웅선택창 만들면 필요없을듯?)
        _hero.Initialize();
        // Spawner 초기화
        _enemySpawner.Initialize(_hero.transform);
        // PlaySceneView 초기화
        _playSceneView.Initialize();
    }

    

    private void Update()
    {
        _playTime += Time.deltaTime;

        // 초 단위로 시간을 계산(내림을 이용해서 시간 변화 체크인데 올림도 큰 차이없을듯?)
        int playTimeInt = Mathf.FloorToInt(_playTime);

        // 초 단위로 값이 바뀔 때만 이벤트 발행
        if (playTimeInt != _playTimeInt)
        {
            _playTimeInt = playTimeInt;
            OnPlayTimeChanged?.Invoke(_playTimeInt);
        }
    }

    /// <summary>
    /// 이동 입력이 들어왔을 때 실행하는 함수
    /// </summary>
    /// <param name="inputVec"></param>
    public void OnMoveInput(Vector2 inputVec)
    {
        _hero.Move(inputVec);
    }

    /// <summary>
    /// 적이 생성되었을 때 호출되는 함수
    /// </summary>
    /// <param name="enemy">Spawner에서 적 생성시 받아올 적</param>
    public void OnEnemySpawned(Enemy enemy)
    {
        // 적이 생성되면 Item Dropper에 이벤트 등록
        enemy.OnDeath += _itemDropper.DropItem;

        // 적이 죽었을 때 이벤트 등록
        enemy.OnDeath += UpdateEnemyKillCount;
    }

    void UpdateEnemyKillCount(Vector3 _)
    {
        _enmeyKillCount++;
        _playSceneView.SetEnemyKillCount(_enmeyKillCount);
    }
}
