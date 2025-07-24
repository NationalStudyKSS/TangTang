using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 영웅 선택을 제어하는 클래스
/// </summary>
public class HeroSelectController : MonoBehaviour
{
    [SerializeField] HeroSelectView[] _views;

    HeroSelectView _currentView;

    public event Action<string> OnHeroIdChanged;

    public void Initialize()
    {
        int index = 0;
        foreach (var heroId in GameManager.Instance.DataManager.HeroStatDataMap.Keys)
        {
            _views[index].SetHero(heroId);
            _views[index].OnSelected += OnHeroSelected;
            _views[index].SetSelected(false); // 초기 상태
            if (index == 0)
            {
                _currentView = _views[index];
                _currentView.SetSelected(true);
                GameManager.Instance.HeroManager.SetHero(_currentView.HeroId);

                OnHeroIdChanged?.Invoke(_currentView.HeroId);
            }
            index++;
        }
    }

    void OnHeroSelected(HeroSelectView view, string heroId)
    {
        GameManager.Instance.HeroManager.SetHero(heroId);

        if (_currentView != null)
        {
            _currentView.SetSelected(false);
        }

        view.SetSelected(true);
        _currentView = view;

        OnHeroIdChanged?.Invoke(_currentView.HeroId);
    }
}
