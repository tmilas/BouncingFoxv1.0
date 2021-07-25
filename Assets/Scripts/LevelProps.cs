using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProps : MonoBehaviour
{

    [SerializeField] public GameObject[] levelObstacles;

    [SerializeField] public float levelSpeedFactor = 1f;
    [SerializeField] public float obstacleCreateInSec = 5f;
    [SerializeField] public float coinCreateInSec = 0.5f;
    [SerializeField] public float levelStartSec;

    [SerializeField] public GameObject[] levelParachutes;
    [SerializeField] public float parachuteCreateInSec = 5f;

    [SerializeField] public GameObject[] levelPotions;
    [SerializeField] public int potionsCreateTotal = 2;
    [SerializeField] public int coinsCreateTotal = 10;


}
