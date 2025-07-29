using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 영웅의 HUD(Heads-Up Display) 뷰를 관리하는 클래스.
/// </summary>
public class HeroHudView : MonoBehaviour
{
    [SerializeField] Image _hpBar; // 체력 바 이미지

    public void ChangeHpBar(float currentHp, float MaxHp)
    {
        //Debug.Log(currentHp + " / " + MaxHp, gameObject);
        _hpBar.fillAmount = currentHp / MaxHp;
    }
}
