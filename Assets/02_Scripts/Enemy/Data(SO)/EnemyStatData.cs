using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적 기본 능력치를 포함하는 설정 데이터 클래스
/// </summary>
[CreateAssetMenu(fileName = "EnemyStatData", menuName = "GameSettings/Enemy/EnemyStatData")]
public class EnemyStatData : ScriptableObject
{
    [Header("기본 정보")]
    public int id;                     // ID (엑셀 Index)
    public string localizationKey;     // 로컬라이징용 키값
    public string enemyName;           // 이름

    [Header("속성 및 타입")]
    public ElementType element;        // 속성 (Fire, Water, Wind, 등)
    public EnemyType enemyType;        // 타입 (Normal, Elite, Boss 등)

    [Header("기본 능력치")]
    public float baseMaxHp;            // 기본 최대 체력
    public float baseDamage;           // 기본 공격력
    public float moveSpeed;            // 이동 속도

    [Header("돌진 관련")]
    public float rushWarmupTime;       // 돌진 예열 시간 (초)
    public float rushDuration;         // 돌진 지속 시간 (초)

    [Header("탐지 및 스킬")]
    public float heroDetectRange;      // 영웅 감지 거리
    public float skillCooldown;        // 스킬 쿨타임 (초)
    public float skillCastTime;        // 스킬 시전 시간 (초)

}
