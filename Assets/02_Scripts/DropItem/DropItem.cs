using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적이 죽으면 드랍되는 아이템들의 추상 클래스
/// </summary>
public class DropItem : MonoBehaviour
{
    DropItemData _data;

    public void Initialize(int itemId)
    {
        _data = GameManager.Instance.DataManager.DropItemDataDict[itemId];
        gameObject.SetActive(true); // 아이템 활성화
        // 아이콘, 색상 등도 여기서 초기화 가능
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hero"))
        {
            ApplyEffect();

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

    void ApplyEffect()
    {
        switch (_data.Name)
        {
            case DropItemName.AtkUp:
                Debug.Log("공격력업!");
                break;
            case DropItemName.Bomb:
                Debug.Log("폭탄 받아라!");
                break;
            case DropItemName.Carrot:
                Debug.Log("당근이죠!");
                break;
            case DropItemName.Coin:
                Debug.Log("야호 돈이다!");
                break;
            case DropItemName.Exp:
                Debug.Log("경험치업!");
                break;
            case DropItemName.HpPotion:
                Debug.Log("체력 회복!");
                break;
            case DropItemName.Magnet:
                Debug.Log("자석 효과!");
                break;
            case DropItemName.TreasureBox:
                Debug.Log("보물상자 열기!");
                break;
            default:
                Debug.LogWarning($"알 수 없는 드롭 아이템: {_data.Name}");
                break;
        }
    }
    
}
