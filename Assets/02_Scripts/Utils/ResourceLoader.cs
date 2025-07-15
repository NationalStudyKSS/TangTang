using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceLoader
{
    public static Sprite LoadSprite(string path)
    {
        var sprite = Resources.Load<Sprite>(path);
        if (sprite == null)
            Debug.LogWarning($"[ResourceLoader] 스프라이트 경로 오류: {path}");
        return sprite;
    }

    public static GameObject LoadPrefab(string path)
    {
        var prefab = Resources.Load<GameObject>(path);
        if (prefab == null)
            Debug.LogWarning($"[ResourceLoader] 프리팹 경로 오류: {path}");
        return prefab;
    }
}
