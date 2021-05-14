using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public enum CollectableType { Invincibility5s, Invincibility8s, JumpPower2x, JumpPower3x,
        SlowMotion2x, SlowMotion3x, FastMotion2x, FastMotion3x,
        GravityIncrease2x, GravityIncrease3x, ObstacleIncrease2x,
        ObstacleIncrease3x,RandomCollectable }

    public float bonusPoints = 0f;
    public CollectableType collectableType;
}
