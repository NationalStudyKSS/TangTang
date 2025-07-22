using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 장비 클래스
/// </summary>
public class Equipment : MonoBehaviour
{
    // 이 장비가 어떤 아이템에서 비롯된 것인지
    EquipmentModel _model;

    public EquipmentModel Model => _model;

    public void SetModel(EquipmentModel model)
    {
        _model = model;
    }
}
