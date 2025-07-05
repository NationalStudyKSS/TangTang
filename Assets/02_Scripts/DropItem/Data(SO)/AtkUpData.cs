using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 공격력 증가 아이템의 데이터를 관리하는 클래스
/// </summary>
[CreateAssetMenu(fileName = "AtkUpData", menuName = "GameSettings/DropItem/AtkUpData")]
public class AtkUpData : DropItemData
{
    [SerializeField] float _atkUpRate; // 공격력 증가량 비율
    [SerializeField] float _duration; // 효과 지속 시간

    public float AtkUpRate => _atkUpRate; // 공격력 증가량 비율을 반환
    public float Duration => _duration; // 효과 지속 시간을 반환

}
