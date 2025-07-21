using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 씬끼리의 전환을 담당하는 매니저
/// </summary>
public class SceneLoadManager : MonoBehaviour
{
    public void Initialize()
    {

    }

    public void GameStart()
    {
        SceneManager.LoadScene("02_Play");
    }
}
