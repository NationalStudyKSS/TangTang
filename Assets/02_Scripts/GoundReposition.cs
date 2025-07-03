using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 타겟(주인공, 카메라)의 위치에 따라 게임오브젝트(지면, 배경)를
/// 반복적으로 일정 간격으로 재배치하는 역할
/// </summary>
public class GoundReposition : MonoBehaviour
{
    // 재배치 기준이 되는 대상의 트랜스폼 컴포넌트
    [SerializeField] Transform _target;

    // 재배치 감도 거리(타겟으로부터 이 거리보다 멀어지면 재배치)
    [SerializeField] float _senseDistance;

    // 재배치 시 이동 거리
    [SerializeField] float _reposDistance;

    private void FixedUpdate()
    {
        if (_target == null) return;

        // X 방향 거리 차이 계산
        float distX = _target.position.x - transform.position.x;
        
        // 감도보다 거리가 크면 오른쪽/왼쪽으로 재배치
        if (Mathf.Abs(distX) > _senseDistance)
        {
            // Mathf.Sign(distX): 거리의 부호 -> +면 오른쪽, -면 왼쪽
            transform.position += Vector3.right * _reposDistance * Mathf.Sign(distX);
        }

        // Y 방향 거리 차이 계산
        float distY = _target.position.y - transform.position.y;

        // 감도보다 거리가 크면 위/아래로 재배치
        if (Mathf.Abs(distY) > _senseDistance)
        {
            transform.position += Vector3.up * _reposDistance * Mathf.Sign(distY);
        }
    }
}
