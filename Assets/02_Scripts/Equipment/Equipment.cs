using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 장비에 실제 장착되는 역할을 한다.
/// 모델을 부여받아 자신이 어떤 장비인지 알 수 있다.
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
