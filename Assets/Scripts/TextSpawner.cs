using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;

public class TextSpawner : MonoBehaviour
{
    public DamageNumber dmgnumPrefab;

    public void spawnText(string text)
    {
        DamageNumber spawnedText = dmgnumPrefab.CreateNew(Random.Range(1f, 10f), transform.position);
        spawnedText.prefix = text;
   
    }
}
