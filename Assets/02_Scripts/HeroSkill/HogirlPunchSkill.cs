using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 호걸 펀치 스킬을 구현하는 클래스
/// </summary>
public class HogirlPunchSkill : MonoBehaviour
{
    [Header("----- 호걸 펀치 스킬 데이터 -----")]
    [SerializeField] Transform _fist;             // 주먹의 크기를 조절할 오브젝트(Renderer 달린거)
    [SerializeField] Vector3 _startPos;           // 처음 시작했을 때의 주먹 위치
    [SerializeField] Vector3 _startScale;         // 처음 시작했을 때의 주먹 크기
    [SerializeField] Vector3 _endPos;             // 끝에 도달했을 때의 주먹 위치
    [SerializeField] Vector3 _endScale;           // 끝에 도달했을 때의 주먹 크기
    [SerializeField] int _punchNum;               // 주먹이 나갈 개수
    [SerializeField] float _punchDistance;        // 주먹이 나가는 거리 = 적 감지 범위
    [SerializeField] float _punchDuration;        // 주먹이 나가는 시간

    public void Initialize()
    {

    }

    void DetectEnemy()
    {
        //Physics2D.OverlapCircleAll()

    }
}
