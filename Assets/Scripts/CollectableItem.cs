using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public enum CollectableType { Invincibility5s, Invincibility8s, JumpPower2x, JumpPower3x,
        SlowMotion2x, SlowMotion3x, FastMotion2x, FastMotion3x,
        GravityIncrease2x, GravityIncrease3x, ObstacleIncrease2x,
        ObstacleIncrease3x,RandomCollectable }

    public float itemFactor = 0f;
    public float itemDuration = 3f;
    public CollectableType collectableType;

    private GameEngine gameEngine;

    private void Start()
    {
        gameEngine = FindObjectOfType<GameEngine>();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameEngine.SetGameBonus(this);
    }

}
