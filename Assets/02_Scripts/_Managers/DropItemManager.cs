using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 드롭  아이템을 관리하는 매니저 역할
/// </summary>
public class DropItemManager : MonoBehaviour
{
    [SerializeField] GameObject[] _dropItemPrefabs; // 드롭 아이템 프리팹 배열

    [SerializeField] float _chance1; // 드롭 아이템 확률 1
    [SerializeField] float _chance2; // 드롭 아이템 확률 2
    [SerializeField] float _chance3; // 드롭 아이템 확률 3
    [SerializeField] float _chance4; // 드롭 아이템 확률 4
    [SerializeField] float _chance5; // 드롭 아이템 확률 5
    [SerializeField] float _chance6; // 드롭 아이템 확률 6
    [SerializeField] float _chance7; // 드롭 아이템 확률 7
    [SerializeField] float _chance8; // 드롭 아이템 확률 8

    public void DropItem(int dropItemIndex, Vector3 position)
    {
        // 드롭 아이템 인덱스가 유효한지 확인
        if (dropItemIndex < 0 || dropItemIndex >= _dropItemPrefabs.Length)
        {
            Debug.LogError("Invalid drop item index: " + dropItemIndex);
            return;
        }
        // 드롭 아이템 프리팹을 해당 위치에 생성
        GameObject dropItem = Instantiate(_dropItemPrefabs[dropItemIndex], position, Quaternion.identity);
        dropItem.transform.SetParent(transform); // 매니저의 자식으로 설정
    }
}

[System.Serializable]
public class ItemDropData
{
    [SerializeField] int _id;
    [SerializeField] float _chance;
}