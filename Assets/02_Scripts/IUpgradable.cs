using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 업그레이드 가능한 대상들의 공통 기능 인터페이스
/// </summary>
public interface IUpgradable
{
    // 프로퍼티(Iupgradable을 갖는 클래스는 반드시 이 프로퍼티들을 구현해야 함)
    /// <summary>
    /// 업그레이드 항목명
    /// </summary>
    /// 
    string UpgradeName { get; }

    /// <summary>
    /// 업그레이드 설명
    /// </summary>
    string Description { get; }

    /// <summary>
    /// 업그레이드 아이콘 스프라이트
    /// </summary>
    Sprite IconSprite { get; }

    /// <summary>
    /// 현재 업그레이드 레벨
    /// </summary>
    int Level { get; }

    /// <summary>
    /// 업그레이드 레벨이 최대치에 도달해 있는지 여부
    /// </summary>
    bool IsMaxLevel { get; }

    /// <summary>
    /// 업그레이드를 실행하는 함수
    /// </summary>
    void Upgrade();
}
