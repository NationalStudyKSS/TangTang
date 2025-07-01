using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayScene : MonoBehaviour
{
    [Header("----- 컴포넌트 참조 -----")]
    [SerializeField] Hero _hero;
    [SerializeField] InputHandler _inputHandler;
    [SerializeField] HeroStatData _heroStatData;
    [SerializeField] Enemy _enemy;

    private void Start()
    {
        _hero.Initialize(_heroStatData);

        _inputHandler.OnMoveInput += OnMoveInput;
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
