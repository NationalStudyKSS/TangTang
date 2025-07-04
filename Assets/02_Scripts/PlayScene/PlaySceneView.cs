using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 게임 진행 중인 UI를 관리하는 클래스
/// </summary>
public class PlaySceneView : MonoBehaviour
{
    [SerializeField] Image _enemyKillCountIcon;           // 적 처치 수 아이콘
    [SerializeField] TextMeshProUGUI _enemyKillCountText; // 적 처치 수를 표시하는 텍스트
    [SerializeField] Image _playTimeIcon;                 // 스테이지 진행시간 아이콘
    [SerializeField] TextMeshProUGUI _playTimeText;       // 스테이지 진행시간을 표시하는 텍스트
    [SerializeField] Image _goldIcon;                     // 골드 아이콘
    [SerializeField] TextMeshProUGUI _goldText;           // 획득한 골드 수를 표시하는 텍스트
    [SerializeField] Image _carrotIcon;                   // 당근 아이콘
    [SerializeField] TextMeshProUGUI _carrotText;         // 획득한 당근 수를 표시하는 텍스트
    [SerializeField] Image _hpIcon;                       // 영웅 Hp 아이콘
    [SerializeField] TextMeshProUGUI _hpText;             // Current Hp를 표시하는 텍스트
    [SerializeField] Image _damageIcon;                   // 영웅 Damage 아이콘
    [SerializeField] TextMeshProUGUI _damageText;         // Current Damage를 표시하는 텍스트
    [SerializeField] Image _pauseIcon;                    // 일시정지 아이콘
    [SerializeField] TextMeshProUGUI _heroLvText;         // 영웅 레벨을 표시하는 텍스트
    [SerializeField] Image _expBar;                       // 경험치 바 이미지

    public void Initialize()
    {
        SetEnemyKillCount(0);
        SetPlayTime(0);
    }

    public void SetEnemyKillCount(int enemyKillCount)
    {
        _enemyKillCountText.text = enemyKillCount.ToString();
    }

    public void SetPlayTime(int playTimeInt)
    {
        // 시간을 분:초 형식으로 변환하여 표시
        int minutes = Mathf.FloorToInt(playTimeInt / 60);
        int seconds = Mathf.FloorToInt(playTimeInt % 60);
        _playTimeText.text = $"{minutes:D2}:{seconds:D2}";
    }
}
