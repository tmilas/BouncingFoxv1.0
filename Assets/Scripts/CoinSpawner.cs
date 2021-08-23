using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{

    public GameObject[] levelList;
    public GameObject coin;
    public float defaultCoinCreateInSeconds = 0.5f;
    public GameObject mainPath;


    private List<float> levelsStartSec;
    private float timer;
    private int timeElapsedInSec;
    private Hashtable coinsCreated = new Hashtable();
    private bool coinsLimitReached = false;




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

        StartCoroutine("CoinCreation");
    }

    void Update()
    {

        timer += Time.deltaTime;
        timeElapsedInSec = (int)(timer);
        //Debug.Log(timer.ToString());
        //Debug.Log(Time.realtimeSinceStartup.ToString());
        //Debug.Log(timeElapsedInSec.ToString());
    }


    private int GetCurrentLevel()
    {
        int currentLevel = 0;

        foreach (float levelStartSec in levelsStartSec)
        {
            if (timeElapsedInSec >= levelStartSec)
                currentLevel++;
        }

        if (currentLevel > levelsStartSec.Count)
            currentLevel = levelsStartSec.Count;

        return currentLevel;
    }

    IEnumerator CoinCreation()
    {
        //Debug.Log("test0");
        WaitForSeconds waitForSeconds;
        while (true)
        {
            LevelProps currentLevel = levelList[GetCurrentLevel() - 1].GetComponent<LevelProps>();
            if (currentLevel)
            {
                float createSec = 0;
                //float randomSec;

                //randomSec = Random.Range(0, currentLevel.obstacleCreateInSec);
                createSec = currentLevel.coinCreateInSec;

                waitForSeconds = new WaitForSeconds(createSec);
            }
            else
                waitForSeconds = new WaitForSeconds(defaultCoinCreateInSeconds);

            // Place your method calls
            yield return waitForSeconds;
            CreateCoin(GetCurrentLevel());
        }
    }

    private void CreateCoin(int currentLevelIndex)
    {
        LevelProps currentLevel = levelList[currentLevelIndex - 1].GetComponent<LevelProps>();
        coinsLimitReached = false;
        if (coinsCreated.ContainsKey(currentLevelIndex))
        {
            if ((int)coinsCreated[currentLevelIndex] >= currentLevel.coinsCreateTotal)
                coinsLimitReached = true;

        }

        if (!coinsLimitReached && checkIfCoinCreatable())
        {
            int posYFactor = Random.Range(0, 2);
            Vector2 coinCreationPosition = new Vector2(transform.position.x, transform.position.y + (posYFactor * 3.5f));
            GameObject newCoin = Instantiate(coin, coinCreationPosition, Quaternion.identity);
            if (coinsCreated.ContainsKey(currentLevelIndex))
                coinsCreated[currentLevelIndex] = (int)coinsCreated[currentLevelIndex] + 1;
            else
                coinsCreated.Add(currentLevelIndex, (int)1);

            newCoin.transform.parent = mainPath.transform;
        }

    }

    private bool checkIfCoinCreatable()
    {
        bool leftHit = false;
        bool rightHit = false;
        ContactFilter2D contactFilter = new ContactFilter2D();

        RaycastHit2D[] leftHits = new RaycastHit2D[10];
        int totalObjectsHit = Physics2D.Raycast(transform.position, Vector2.left, contactFilter, leftHits, 4f);
        if (totalObjectsHit > 0)
        {
            for (int i = 0; i < totalObjectsHit; i++)
            {
                //Debug.Log("cccc222");
                //Debug.Log(leftHits[i].transform.parent.tag);
                if (leftHits[i].transform.parent.tag.Equals("SpawnedObject") || leftHits[i].transform.tag.Equals("SpawnedObject"))
                {
                    leftHit = true;
                    break;
                }
            }
        }

        RaycastHit2D[] rightHits = new RaycastHit2D[10];
        totalObjectsHit = Physics2D.Raycast(transform.position, Vector2.right, contactFilter, rightHits, 4f);
        if (totalObjectsHit > 0)
        {
            for (int i = 0; i < totalObjectsHit; i++)
            {
                if (rightHits[i].transform.parent.tag.Equals("SpawnedObject") || rightHits[i].transform.tag.Equals("SpawnedObject"))
                {
                    rightHit = true;
                    break;
                }
            }
        }

        if (leftHit || rightHit)
            return false;
        else
            return true;
    }

    public void SetMainPath(GameObject newPath)
    {
        mainPath = newPath;
    }

}
