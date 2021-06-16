using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public float defaultObsCreateInSeconds = 5f;
    public float defaultParachuteCreateInSeconds = 5f;
    public float firstObsCreateInSeconds = 1f;

    public GameObject mainPath;

    public GameObject[] levelList;

    private float timer;
    private int timeElapsedInSec;
    private List<float> levelsStartSec;

    private bool isFirstRun = true;

    private Hashtable potionsCreated = new Hashtable();
    
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
        StartCoroutine("ParachuteCreation");

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
        //Debug.Log("test0");
        WaitForSeconds waitForSeconds;
        while (true)
        {
            LevelProps currentLevel = levelList[GetCurrentLevel() - 1].GetComponent<LevelProps>();
            if (currentLevel)
            {
                float createSec = 0;
                float randomSec;

                randomSec = Random.Range(0, currentLevel.obstacleCreateInSec);

                
                if (isFirstRun)
                {
                    createSec = firstObsCreateInSeconds;
                    isFirstRun = false;
                }
                else
                {
                    if (Mathf.CeilToInt(randomSec) % 2 == 0)
                        createSec = currentLevel.obstacleCreateInSec + randomSec * 0.2f;
                    else
                        createSec = currentLevel.obstacleCreateInSec - randomSec * 0.2f;

                }

                //Debug.Log("create sec: " + createSec);

                waitForSeconds = new WaitForSeconds(createSec);
            }
            else
                waitForSeconds = new WaitForSeconds(defaultObsCreateInSeconds);

            // Place your method calls
            yield return waitForSeconds;
            if (currentLevel.levelObstacles.Length>0)
                CreateObstacle(GetCurrentLevel());
        }
    }

    private void CreateObstacle(int currentLevelIndex)
    {
        int randomObstacleIndex;
        Debug.Log("CreateObsCurrLevel:" + currentLevelIndex.ToString());
        LevelProps  currentLevel= levelList[currentLevelIndex - 1].GetComponent<LevelProps>();
        randomObstacleIndex = Random.Range(0, currentLevel.levelObstacles.Length);
        //Vector2 creationPosition = new Vector2(transform.position.x, currentLevel.obstaclePosY[randomObstacleIndex]);
        Vector2 creationPosition = new Vector2(transform.position.x, currentLevel.levelObstacles[randomObstacleIndex].transform.position.y);
        GameObject newObstacle = Instantiate(currentLevel.levelObstacles[randomObstacleIndex], creationPosition, Quaternion.identity);

        bool createObstacleWithPotion = false;
        Transform collectableItemPos = null;

        if (currentLevel.levelPotions.Length>0 && currentLevel.potionsCreateTotal>0)
        {
            collectableItemPos = newObstacle.transform.Find("collectablepos");

            if (collectableItemPos)
            {
                if (potionsCreated.ContainsKey(currentLevelIndex))
                {
                    if ((int)potionsCreated[currentLevelIndex] < currentLevel.potionsCreateTotal)
                        createObstacleWithPotion = true;

                }
                else
                {
                    createObstacleWithPotion = true;
                }

            }

        }

        if (createObstacleWithPotion)
        {
            int randomPotionIndex = Random.Range(0, currentLevel.levelPotions.Length);
            Vector2 potionCreationPosition = new Vector2(collectableItemPos.position.x, collectableItemPos.position.y);
            GameObject newPotion = Instantiate(currentLevel.levelPotions[randomPotionIndex], potionCreationPosition, Quaternion.identity);
            newPotion.transform.parent = newObstacle.transform;
            if (potionsCreated.ContainsKey(currentLevelIndex))
                potionsCreated[currentLevelIndex] = (int) potionsCreated[currentLevelIndex] + 1;
            else
                potionsCreated.Add(currentLevelIndex, (int) 1);
            Debug.Log("xxxxx:" + potionsCreated[currentLevelIndex].ToString());
        }

        newObstacle.transform.parent = mainPath.transform;
        return;

    }

    IEnumerator ParachuteCreation()
    {
        WaitForSeconds waitForSeconds;
        while (true)
        {
            LevelProps currentLevel = levelList[GetCurrentLevel() - 1].GetComponent<LevelProps>();
            if (currentLevel)
            {
                float createSec = 0;
                float randomSec = Random.Range(0, currentLevel.parachuteCreateInSec);

                if (Mathf.CeilToInt(randomSec) % 2 == 0)
                    createSec = currentLevel.parachuteCreateInSec + randomSec * 0.2f;
                else
                    createSec = currentLevel.parachuteCreateInSec - randomSec * 0.2f;

                waitForSeconds = new WaitForSeconds(createSec);
            }
            else
                waitForSeconds = new WaitForSeconds(defaultParachuteCreateInSeconds);

            // Place your method calls
            yield return waitForSeconds;
            if (currentLevel.levelParachutes.Length>0)
                CreateParachute(GetCurrentLevel());
        }
    }

    private void CreateParachute(int currentLevelIndex)
    {
        int randomParachuteIndex;
        LevelProps currentLevel = levelList[currentLevelIndex - 1].GetComponent<LevelProps>();
        randomParachuteIndex = Random.Range(0, currentLevel.levelParachutes.Length);
        GameObject newParachute = Instantiate(currentLevel.levelParachutes[randomParachuteIndex]);
        Object.Destroy(newParachute, 5f);
        //newParachute.transform.parent = mainPath.transform;
        return;

    }

    public LevelProps GetLevelProps()
    {
        return levelList[GetCurrentLevel() - 1].GetComponent<LevelProps>();
    }

    public void SetMainPath(GameObject newPath)
    {
        mainPath = newPath;
    }

    private int GetCurrentLevel()
    {
        int currentLevel=0;

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
