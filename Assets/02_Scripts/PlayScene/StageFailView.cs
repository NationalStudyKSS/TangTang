using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageFailView : MonoBehaviour
{
    [SerializeField] Button _okButton;

    public void Initialize()
    {
        gameObject.SetActive(true);
        _okButton.onClick.AddListener(OnOkButtonClicked);
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
}
