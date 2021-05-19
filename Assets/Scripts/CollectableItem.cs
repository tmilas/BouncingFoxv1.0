using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public enum CollectableType { Invincibility5s, Invincibility8s, JumpPower2x, JumpPower3x,
        SlowMotion2x, SlowMotion3x, FastMotion2x, FastMotion3x,
        GravityIncrease2x, GravityIncrease3x, ObstacleIncrease2x,
        ObstacleIncrease3x, ScoreIncrease2x, ScoreIncrease3x,
        BiggerCharacter2x, BiggerCharacter3x, SmallerCharacter2x, SmallerCharacter3x,
        RandomCollectable }

    public float itemFactor = 0f;
    public float itemDuration = 3f;
    public CollectableType collectableType;
    public GameObject collectableParticleEffect;

    private GameEngine gameEngine;

    private void Start()
    {
        gameEngine = FindObjectOfType<GameEngine>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameEngine.SetGameBonus(this);
        if (collectableParticleEffect)
        {
            Vector3 particlePos = new Vector3(transform.position.x,transform.position.y-0.7f, 0f);
            GameObject particleEffect=Instantiate(collectableParticleEffect,particlePos,Quaternion.identity);
            Object.Destroy(particleEffect, 3f);
        }

        Object.Destroy(gameObject);
    }
}