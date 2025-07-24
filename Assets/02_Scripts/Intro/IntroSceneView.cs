using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인트로씬의 UI를 담당하는 역할
/// </summary>
public class IntroSceneView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _goldText;
    [SerializeField] Button _goldCheatButton;
    [SerializeField] RawImage _selectedHeroRawImage;
    [SerializeField] Button _gameStartButton;

    public event Action OnGoldCheatButtonClicked;
    public event Action GameStartButtonClicked;

    public void Initialize()
    {
        // CurrencyManager의 골드 변경 이벤트 구독
        GameManager.Instance.CurrencyManager.OnGoldChanged += SetGoldText;

        // 초기 골드 받아와서 설정
        SetGoldText(GameManager.Instance.CurrencyManager.Gold);

        _goldCheatButton.onClick.AddListener(GoldCheatButtonClick);
        _gameStartButton.onClick.AddListener(OnGameStartButtonClicked);
    }

    /// <summary>
    /// 골드 보유량 텍스트를 설정하는 함수
    /// </summary>
    /// <param name="gold">CurrencyManager의 골드</param>
    public void SetGoldText(int gold)
    {
        _goldText.text = gold.ToString();
    }

    public void GoldCheatButtonClick()
    {
        OnGoldCheatButtonClicked?.Invoke();
    }

    public void SetTexture(string heroId)
    {
        _selectedHeroRawImage.texture = GameManager.Instance.DataManager.HeroMetaDataMap[heroId].HeroTexture;
    }

    public void OnGameStartButtonClicked()
    {
        GameManager.Instance.SceneLoadManager.GameStart();
    }
}
