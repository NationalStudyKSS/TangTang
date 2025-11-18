using UnityEngine;
using UnityEngine.UI;

public class IntroSceneView : MonoBehaviour
{
    [SerializeField] Button _startButton; // 시작 버튼

    private void Awake()
    {
        // 시작 버튼에 클릭 이벤트 리스너 추가
        _startButton.onClick.AddListener(OnStartButtonClicked);
    }

    void OnStartButtonClicked()
    {
        // 시작 버튼 클릭 시 게임 씬으로 전환
        //GameManager.Instance.SceneLoadManager.LoadScene("GameScene");
    }
}