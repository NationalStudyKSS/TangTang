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

    public void NormalStageStart()
    {
        SceneManager.LoadScene("02_Play_Normal");
    }

    public void BossStageStart()
    {
        SceneManager.LoadScene("02_Play_Boss");
    }

    
}
