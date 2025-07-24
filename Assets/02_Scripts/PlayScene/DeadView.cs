using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 영웅이 죽었을 때 나오는 뷰를 관리하는 클래스
/// 버튼을 누르면 이벤트를 발행한다.
/// </summary>
public class DeadView : MonoBehaviour
{
    [SerializeField] Button _yesButton;
    [SerializeField] Button _noButton;

    public event Action YesButtonClicked;
    public event Action NoButtonClicked;

    public void Initialize()
    {
        _yesButton.onClick.AddListener(OnYesButtonClicked);
        _noButton.onClick.AddListener(OnNoButtonClicked);
    }

    public void OnDead(GameObject _)
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 예 버튼을 눌렀을 때 실행될 함수
    /// </summary>
    public void OnYesButtonClicked()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        YesButtonClicked?.Invoke();
    }

    /// <summary>
    /// 아니오 버튼을 눌렀을 때 실행될 함수
    /// </summary>
    public void OnNoButtonClicked()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        NoButtonClicked?.Invoke();
    }
}
