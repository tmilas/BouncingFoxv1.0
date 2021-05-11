using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProps : MonoBehaviour
{

    [SerializeField] public GameObject[] levelObstacles;
    [SerializeField] public float[] obstaclePosY;

    [SerializeField] public float obstacleCreateInSec = 5f;
    [SerializeField] public float levelStartSec;
}
