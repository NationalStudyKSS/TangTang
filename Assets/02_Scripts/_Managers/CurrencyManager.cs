using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrencyType
{
    Gold, // 골드
    Carrot // 당근
}

/// <summary>
/// 골드랑 당근을 관리하는 매니저
/// </summary>
public class CurrencyManager : MonoBehaviour
{
    [Header("----- 재화 보유량(ReadOnly) -----")]
    [SerializeField] int _gold;
    [SerializeField] int _carrot;

    public int Gold => _gold;
    public int Carrot => _carrot;

    public event Action<int> OnGoldChanged; // 골드 변경 이벤트
    public event Action<int> OnCarrotChanged; // 당근 변경 이벤트
    
    public void Initialize()
    {
        // 재화 불러오기
        LoadCurrency();

        // 이벤트 연결
        DropItemManager.OnGoldItemUsed += ChangeGold; // 골드 아이템 사용 이벤트
        DropItemManager.OnCarrotItemUsed += ChangeCarrot; // 당근 아이템 사용 이벤트

        // 초기 골드와 당근 설정
        OnGoldChanged?.Invoke(_gold);
        OnCarrotChanged?.Invoke(_carrot);
    }

    /// <summary>
    /// 보유중인 골드를 변경하는 함수
    /// </summary>
    /// <param name="amount">변경될 골드량</param>
    public void ChangeGold(int amount)
    {
        _gold += amount;
        // 골드 변경 이벤트 발생
        OnGoldChanged?.Invoke(_gold);
        SaveCurrency();
    }

    /// <summary>
    /// 보유중인 당근을 변경하는 함수
    /// </summary>
    /// <param name="amount">변경될 당근량</param>
    public void ChangeCarrot(int amount)
    {
        _carrot += amount;
        // 당근 변경 이벤트 발생
        OnCarrotChanged?.Invoke(_carrot);
        SaveCurrency();
    }

    void LoadCurrency()
    {
        _gold = PlayerPrefs.GetInt("Gold", 0);
        _carrot = PlayerPrefs.GetInt("Carrot", 0);
    }

    void SaveCurrency()
    {
        PlayerPrefs.SetInt("Gold", _gold);
        PlayerPrefs.SetInt("Carrot", _carrot);
        PlayerPrefs.Save();
    }
}
