using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 자석 아이템의 데이터를 관리하는 클래스
/// </summary>
[CreateAssetMenu(fileName = "MagnetData", menuName = "GameSettings/DropItem/MagnetData")]
public class MagnetData : DropItemData
{
    public override void ApplyEffect()
    {
        // 자석 아이템 사용 이벤트 발생
        DropItemManager.RaiseMagnetItemUsed();
    }
}
