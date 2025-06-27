using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    /// <summary>
    /// 어떤 layer가 어떤 layerMask에 포함되는지 여부를 반환해 주는 함수
    /// </summary>
    /// <param name="layerMask"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static bool Contains(this LayerMask layerMask, int layer)
    {
        return Util.Contains(layerMask, layer);
    }
}
