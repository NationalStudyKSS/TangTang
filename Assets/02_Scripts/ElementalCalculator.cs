using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 속성ㅅ에 따른 데미지 배수 계산
/// </summary>
public static class ElementalCalculator
{
    private static readonly float[,] multiplierTable = new float[3, 3]
    {
        //불   물    바람 
        {1.0f, 0.8f, 1.2f,},   // 불이 불,물,바람 공격시
        {1.2f, 1.0f, 0.8f,},   // 물이 불,물,바람 공격시
        {0.8f, 1.2f, 1.0f,},   // 바람이 불,물,바람 공격시
    };

    /// <summary>
    /// 공격한놈이랑 맞는놈 속성에 따라 배수 반환
    /// </summary>
    /// <param name="attacker">공격하는놈</param>
    /// <param name="defender">맞는놈</param>
    /// <returns>배수</returns>
    public static float GetMultiplier(ElementType attacker, ElementType defender)
    {
        if (attacker ==0) return 1.0f;
        if (defender == 0) return 1.0f;
        return multiplierTable[(int)attacker, (int)defender];
    }
}