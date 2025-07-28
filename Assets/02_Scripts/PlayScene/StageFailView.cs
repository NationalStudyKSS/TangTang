using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageFailView : MonoBehaviour
{
    [SerializeField] Button _okButton;
    [SerializeField] TextMeshProUGUI _playTimeText;
    [SerializeField] TextMeshProUGUI _enemyKillCountText;
    [SerializeField] TextMeshProUGUI _playerExpGetText;
    [SerializeField] TextMeshProUGUI _goldGetText;

    int _playTimeInt;
    int _enemyKillCount;
    int _playerExpGet;
    int _currentGold;
    int _startGold;

    public void Initialize(PlayScene playScene)
    {
        _playTimeInt = playScene.PlayTimeInt;
        _enemyKillCount = playScene.EnemyKillCount;
        _playerExpGet = playScene.ExpTotal;
        _currentGold = GameManager.Instance.CurrencyManager.Gold;
        _startGold = playScene.StartGold;

        gameObject.SetActive(true);
        _okButton.onClick.AddListener(OnOkButtonClicked);
        SetUI();
    }

    /// <summary>
    /// 확인버튼 눌렀을 때 호출되는 함수
    /// </summary>
    public void OnOkButtonClicked()
    {
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
        SceneManager.LoadScene("01_Main");
    }

    public void SetUI()
    {
        _playTimeText.text = $"도전 시간: {_playTimeInt / 60}분 {_playTimeInt % 60}초";
        _enemyKillCountText.text = $"처치한 적: {_enemyKillCount}마리";
        _playerExpGetText.text = $"획득한 경험치: {_playerExpGet}점";
        _goldGetText.text = $"획득한 골드: {_currentGold-_startGold}";
    }
}
