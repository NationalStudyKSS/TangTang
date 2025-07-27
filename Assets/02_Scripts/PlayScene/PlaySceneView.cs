using System;
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
    [SerializeField] TextMeshProUGUI _hpText;             // Current Hp/Max Hp를 표시하는 텍스트
    [SerializeField] Image _damageIcon;                   // 영웅 Damage 아이콘
    [SerializeField] TextMeshProUGUI _damageText;         // Current Damage를 표시하는 텍스트
    [SerializeField] Image _pauseIcon;                    // 일시정지 아이콘
    [SerializeField] TextMeshProUGUI _heroLvText;         // 영웅 레벨을 표시하는 텍스트
    [SerializeField] Image _expBar;                       // 경험치 바 이미지
    [SerializeField] GameObject _pausePanel;              // 일시정지 패널
    [SerializeField] TextMeshProUGUI _normalStageIntroText; // 일반 스테이지 진입 시 출력되는 텍스트
    [SerializeField] TextMeshProUGUI _bossStageIntroText; // 보스 스테이지 진입 시 출력되는 텍스트
    [SerializeField] TextMeshProUGUI _bossClearText;
    [SerializeField] Image _bossHpBar;
    [SerializeField] TextMeshProUGUI _bossHpText;
    [SerializeField] GameObject _clearPanel;
    //[SerializeField] Button _refreshButton;               // 새로고침 버튼
    //[SerializeField] TextMeshProUGUI _refreshCountText;   // 새로고침 가능한 횟수 텍스트

    bool _isPlaying = true;

    public event Action OnRefreshClicked;

    public void Initialize()
    {
        if (_bossHpBar != null && _bossHpText != null)
        {
            _bossHpBar.enabled = false;
            _bossHpText.enabled = false;
        }
        
        // 골드 변경 이벤트 구독
        GameManager.Instance.CurrencyManager.OnGoldChanged += SetGold;
        // 당근 변경 이벤트 구독
        GameManager.Instance.CurrencyManager.OnCarrotChanged += SetCarrot;

        // 골드 초기화
        SetGold(GameManager.Instance.CurrencyManager.Gold);
        // 당근 초기화
        SetCarrot(GameManager.Instance.CurrencyManager.Carrot);

        // 적 처치 수 초기화
        SetEnemyKillCount(0);
        SetPlayTime(0);
        //_refreshButton.onClick.AddListener(RefreshClicked);
    }

    public void SetEnemyKillCount(int enemyKillCount)
    {
        // 아이콘 받아오기
        _enemyKillCountText.text = enemyKillCount.ToString();
    }

    public void SetPlayTime(int playTimeInt)
    {
        // 아이콘 받아오기
        // 시간을 분:초 형식으로 변환하여 표시
        int minutes = Mathf.FloorToInt(playTimeInt / 60);
        int seconds = Mathf.FloorToInt(playTimeInt % 60);
        _playTimeText.text = $"{minutes:D2}:{seconds:D2}";
    }

    public void SetGold(int gold)
    {
        // 아이콘 받아오기
        _goldText.text = gold.ToString();
    }

    public void SetCarrot(int carrot)
    {
        // 아이콘 받아오기
        _carrotText.text = carrot.ToString();
    }

    public void SetHp(float currentHp, float maxHp)
    {
        // 아이콘 받아오기
        _hpText.text = $"{currentHp:F0}/{maxHp:F0}";
    }

    public void SetDamage(float currentDamage)
    {
        // 아이콘 받아오기
        _damageText.text = $"{currentDamage:F0}";
    }

    public void SetLevel(int preLevel, int currentLevel)
    {
        // 아이콘 받아오기
        _heroLvText.text = $"Lv. {currentLevel}";
    }

    public void SetExp(float currentExp, float expRequired)
    {
        _expBar.fillAmount = currentExp / expRequired; // fillAmount를 사용하여 경험치 바를 채움
    }

    public void Pause()
    {
        _isPlaying = !_isPlaying;

        if (_isPlaying)
        {
            Time.timeScale = 1f; // 게임 재개
            _pausePanel.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f; // 게임 일시정지
            _pausePanel.SetActive(true);
        }
    }

    public void NormalStageIntro()
    {
        StartCoroutine(NormalStageIntroRoutine());
    }

    IEnumerator NormalStageIntroRoutine()
    {
        Color color = _normalStageIntroText.color;

        float t = 0;
        while (t < 1.5f)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / 1.5f);
            _normalStageIntroText.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        t = 0;
        while (t < 1.5f)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / 1.5f);
            _normalStageIntroText.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
    }

    public void BossStageIntro()
    {
        StartCoroutine(BossStageIntroRoutine());
    }

    IEnumerator BossStageIntroRoutine()
    {
        Color color = _bossStageIntroText.color;

        float t = 0;
        while (t < 1.5f)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / 1.5f);
            _bossStageIntroText.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        t = 0;
        while (t < 1.5f)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / 1.5f);
            _bossStageIntroText.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
    }

    public void SetBossUI(float curHp, float maxHp)
    {
        _bossHpBar.enabled = true;
        _bossHpText.enabled = true;
        _bossHpBar.fillAmount = curHp / maxHp;
        _bossHpText.text = $"{(int)curHp}/{maxHp}";
    }
    
    public void OpenClearPanel()
    {
        _clearPanel.SetActive(true);
    }

    public void BossCleared(GameObject bossObj)
    {
        StartCoroutine(BossStageClearRoutine());
    }

    IEnumerator BossStageClearRoutine()
    {
        Color color = _bossClearText.color;

        float t = 0;
        while (t < 1.5f)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / 1.5f);
            _bossClearText.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        yield return new WaitForSeconds(5f);

        OpenClearPanel();
    }

    //public void RefreshClicked()
    //{
    //    OnRefreshClicked?.Invoke();
    //}

    //public void SetRefreshCountText(int refreshCount)
    //{
    //    _refreshCountText.text = $"새로고침\n({refreshCount}/3)";
    //}
}
