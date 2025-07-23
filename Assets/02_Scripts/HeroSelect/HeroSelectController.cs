using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 영웅 선택을 제어하는 클래스
/// </summary>
public class HeroSelectController : MonoBehaviour
{
    [SerializeField] HeroSelectView[] _views;

    private void Start()
    {
        int index = 0;
        foreach (var heroId in GameManager.Instance.DataManager.HeroStatDataMap.Keys)
        {
            _views[index].SetHero(heroId);
            index++;
        }
    }
}
