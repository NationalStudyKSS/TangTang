using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    /// <summary>
    /// 어떤 layer가 어떤 layerMask에 포함되는지 여부를 반환해 주는 함수
    /// </summary>
    /// <param name="layermask"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static bool Contains(LayerMask layerMask, int layer)
    {
        return ((1 << layer) & layerMask.value) != 0;
    }
}
