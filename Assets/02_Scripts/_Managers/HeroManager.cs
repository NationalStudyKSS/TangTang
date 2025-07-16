using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 시작전을 포함해서 갖고있어야 할 영웅의 정보를 담는 매니저
/// </summary>
public class HeroManager : MonoBehaviour
{
    ElementType _type;

    public ElementType Type => _type;

    public void Initialize()
    {
        // 임시
        _type = ElementType.Fire;
    }

    /// <summary>
    /// 영웅의 속성을 정해줄 함수
    /// </summary>
    /// <param name="type">영웅 속성</param>
    public void SetHeroElementType(ElementType type)
    {
        _type = type;
    }
}
