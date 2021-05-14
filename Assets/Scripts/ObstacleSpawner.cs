using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public float defaultObsCreateInSeconds = 5f;

    public GameObject mainPath;

    public GameObject[] levelList;

    private float timer;
    private int timeElapsedInSec;
    private List<float> levelsStartSec;

    // Start is called before the first frame update
    void Start()
    {
        levelsStartSec = new List<float>();
        foreach (GameObject level in levelList)
        {
            LevelProps levelProperties = level.GetComponent<LevelProps>();
            levelsStartSec.Add(levelProperties.levelStartSec);

        }
        levelsStartSec.Sort();

        StartCoroutine("ObstacleCreation");
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
        timeElapsedInSec = (int)(timer);
        //Debug.Log(timer.ToString());
        //Debug.Log(Time.realtimeSinceStartup.ToString());
        //Debug.Log(timeElapsedInSec.ToString());
    }

    IEnumerator ObstacleCreation()
    {
        Debug.Log("test0");
        WaitForSeconds waitForSeconds;
        while (true)
        {
            LevelProps currentLevel = levelList[GetCurrentLevel() - 1].GetComponent<LevelProps>();
            if (currentLevel)
                waitForSeconds = new WaitForSeconds(currentLevel.obstacleCreateInSec);
            else
                waitForSeconds = new WaitForSeconds(defaultObsCreateInSeconds);

            // Place your method calls
            yield return waitForSeconds;
            CreateObstacle(GetCurrentLevel());
        }
    }

    private void CreateObstacle(int currentLevelIndex)
    {
        int randomObstacleIndex;
        Debug.Log("CreateObsCurrLevel:" + currentLevelIndex.ToString());
        LevelProps  currentLevel= levelList[currentLevelIndex - 1].GetComponent<LevelProps>();
        randomObstacleIndex = Random.Range(0, currentLevel.levelObstacles.Length);
        Vector2 creationPosition = new Vector2(transform.position.x, currentLevel.obstaclePosY[randomObstacleIndex]);
        GameObject newObstacle = Instantiate(currentLevel.levelObstacles[randomObstacleIndex], creationPosition, Quaternion.identity);
        newObstacle.transform.parent = mainPath.transform;
        return;

    }

    public void SetMainPath(GameObject newPath)
    {
        mainPath = newPath;
    }

    private int GetCurrentLevel()
    {
        int currentLevel=1;

        foreach (float levelStartSec in levelsStartSec)
        {
            if (timeElapsedInSec >= levelStartSec)
                currentLevel++;
        }

        if (currentLevel > levelsStartSec.Count)
            currentLevel = levelsStartSec.Count;

        return currentLevel;

    }
}