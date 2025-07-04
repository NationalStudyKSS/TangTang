using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유니티 Resources를 활용해 게임의 리소스를 관리하는 매니저.
/// </summary>
public class ResourceManager : MonoBehaviour
{
    Dictionary<string, GameObject> _prefabCache = new();

    /// <summary>
    /// 지정 경로의 프리팹을 로드해 반환하는 함수
    /// </summary>
    /// <param name="path">Resources 폴더 안의 프리팹 경로</param>
    /// <returns></returns>
    public GameObject LoadPrefab(string path)
    {
        // 이미 캐시에 로드한 프리팹 참조가 저장되어 있으면
        if (_prefabCache.ContainsKey(path) == true)
        {
            return _prefabCache[path];
        }

        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab == null)
        {
            Debug.LogError($"{path} 경로 프리팹이 없습니다.");
        }
        else
        {
            _prefabCache[path] = prefab;
        }
        return prefab;
    }

    public void Initialize()
    {
        // Resources 폴더 안의 프리팹들을 미리 로드해 캐시에 저장할 수 있다.
        // 예를 들어, "Prefabs/Enemy" 경로에 있는 모든 적 프리팹을 로드할 수 있다.
        // 이 부분은 필요에 따라 구현할 수 있다.
    }


}
