using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRushable
{
    public float RushCoolTime { get; }
    public float ReadyDuration { get; }
    public float RushDuration { get; }
    public float RushSpeed { get; }
    Vector2 CalculateRushDirection();
    void Rush(Vector2 rushDirection);
    void StopRush();
}
