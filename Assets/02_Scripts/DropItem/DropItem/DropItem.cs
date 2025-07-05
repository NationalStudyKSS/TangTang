using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적이 죽으면 드랍되는 아이템들의 추상 클래스
/// 이대로는 못 쓰고 상속받아서 필요한거 알아서 추가해서 사용해야 함
/// </summary>
public class DropItem : MonoBehaviour
{
    [Header("----- 드롭 아이템 데이터(읽기 전용) -----")]
    [SerializeField] protected DropItemData _data;

    /// <summary>
    /// 아이템이 드롭될 때 id를 받아와서 _data에 넣어주는 역할
    /// </summary>
    /// <param name="itemId">드롭될 때 받아온 id</param>
    public void Initialize(int itemId)
    {
        // 받아온 아이템 ID로 딕셔너리에서 해당 아이템 데이터를 가져옴
        _data = GameManager.Instance.DataManager.DropItemDataDict[itemId];
        gameObject.SetActive(true); // 아이템 활성화
    }

    //public void ApplyEffect();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hero"))
        {
            //ApplyEffect();

            gameObject.SetActive(false);

            Poolable poolable = GetComponent<Poolable>();
            if (poolable != null)
            {
                poolable.ReturnToPool();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
