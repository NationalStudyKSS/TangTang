using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayScene : MonoBehaviour
{
    [Header("----- 컴포넌트 참조 -----")]
    // Hero와 Enemy 는 둘다 스폰 되는 조건이 있으므로 임시
    [SerializeField] Hero _hero;
    [SerializeField] InputHandler _inputHandler;
    [SerializeField] EnemySpawner _enemySpawner;

    private void Start()
    {
        _inputHandler.OnMoveInput += OnMoveInput;

        // 임시(나중에 게임시작 시 영웅선택창 만들면 필요없을듯?)
        _hero.Initialize();
        // Spawner 초기화
        _enemySpawner.Initialize(_hero.transform);
    }

    /// <summary>
    /// 이동 입력이 들어왔을 때 실행하는 함수
    /// </summary>
    /// <param name="inputVec"></param>
    public void OnMoveInput(Vector2 inputVec)
    {
        _hero.Move(inputVec);
    }
}
