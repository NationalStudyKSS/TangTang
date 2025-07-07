using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적이 죽으면 드랍되는 아이템들의 클래스
/// </summary>
public class DropItem : MonoBehaviour
{
    [Header("----- 드롭 아이템 데이터(읽기 전용) -----")]
    [SerializeField] protected DropItemData _data;

    // 아이템 획득했냐고 이벤트 발행할때 필요
    public event Action<DropItem> OnDropItemUsed;

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

    /// <summary>
    /// 영웅과 충돌 시 아이템의 효과를 적용하고,
    /// 아이템을 비활성화한 후 풀로 반환하거나 파괴한다.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hero"))
        {
            gameObject.SetActive(false);

            // 아이템 효과 적용
            _data.ApplyEffect();

            Poolable poolable = GetComponent<Poolable>();
            if (poolable != null)
            {
                poolable.ReturnToPool();
            }
            else
            {
                Destroy(gameObject);
            }
            // 아이템 획득 이벤트 발행
            OnDropItemUsed?.Invoke(this);
        }
    }
}
