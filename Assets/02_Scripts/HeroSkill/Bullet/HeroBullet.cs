using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBullet : Bullet<Enemy>
{
    [SerializeField] SpriteRenderer _spriteRenderer;

    public void SetColor(int index)
    {
        switch (index)
        {
            case 0:
                _spriteRenderer.color = Color.red;
                break;
            case 1:
                _spriteRenderer.color = Color.yellow;
                break;
            case 2:
                _spriteRenderer.color = Color.green;
                break;
            case 3:
                _spriteRenderer.color = Color.blue;
                break;
            case 4:
                _spriteRenderer.color = Color.black;
                break;
            default:
                break;
        }
    }
}
