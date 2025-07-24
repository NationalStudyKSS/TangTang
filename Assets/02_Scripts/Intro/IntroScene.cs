using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인트로 씬을 관리하는 클래스
/// </summary>
public class IntroScene : MonoBehaviour
{
    
    [SerializeField] HeroSelectController _heroSelectController;
    [SerializeField] IntroSceneView _introView;
    [SerializeField] EquipmentShopController _equipmentShopController;

    private void Start()
    {
        _heroSelectController.OnHeroIdChanged += _introView.SetTexture;
        _introView.OnGoldCheatButtonClicked += GoldCheatUsed;

        _heroSelectController.Initialize();
        _introView.Initialize();
        _equipmentShopController.Initialize();
    }

    public void GoldCheatUsed()
    {
        GameManager.Instance.CurrencyManager.ChangeGold(1000);
    }
}
