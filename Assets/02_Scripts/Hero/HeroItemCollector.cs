using System.Collections.Generic;
using UnityEngine;

public class HeroItemCollector : MonoBehaviour
{
    [SerializeField] float _pullSpeed = 10f;
    [SerializeField] CircleCollider2D _collider;
    Transform _heroTransform;
    List<Transform> _pullingItems = new List<Transform>();

    public void Initialize(Transform heroTransform)
    {
        _heroTransform = heroTransform;
    }

    private void Update()
    {
        // 매 프레임마다 모든 아이템을 끌어당김
        for (int i = _pullingItems.Count - 1; i >= 0; i--)
        {
            Transform item = _pullingItems[i];
            if (item == null)
            {
                _pullingItems.RemoveAt(i);
                continue;
            }

            Vector3 dir = (_heroTransform.position - item.position).normalized;
            item.position += dir * _pullSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<DropItem>(out var dropItem))
        {
            // 이미 리스트에 있지 않다면 추가
            if (!_pullingItems.Contains(dropItem.transform))
            {
                _pullingItems.Add(dropItem.transform);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<DropItem>(out var dropItem))
        {
            _pullingItems.Remove(dropItem.transform);
        }
    }

    public void SetRange(float range)
    {
        _collider.radius = range;
    }
}
