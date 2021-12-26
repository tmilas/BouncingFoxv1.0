using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadBoard : MonoBehaviour
{

    private StorageEngine storageEngine;
    private string nickName;
    private bool userInTop10 = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject userRow;

        storageEngine = FindObjectOfType<StorageEngine>();
        string scoreText = storageEngine.LoadDataScore();
        nickName= storageEngine.LoadDataNick();

        if (scoreText == "")
           scoreText = "0";

        if (nickName == "")
            nickName = "User";

        userRow = GameObject.Find("LBRow99");
        userRow.transform.Find("Rank").GetComponent<Text>().text = "Next Time";
        userRow.transform.Find("Player").GetComponent<Text>().text = nickName;
        userRow.transform.Find("Score").GetComponent<Text>().text = scoreText;

       
        if (LB_Controller.instance != null)
        {
            if (LB_Controller.OnUpdatedScores==null)
            {
                LB_Controller.OnUpdatedScores += OnLeaderboardUpdated;
            }
            StartCoroutine(DownloadScores());
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnLeaderboardUpdated(LB_Entry[] entries)
    {
        Debug.Log("test1");
        GameObject newRow;
        GameObject userRow;

        int i = 1;
        int maxRow = 11;
        userInTop10 = false;
        nickName = storageEngine.LoadDataNick();

        if (entries != null && entries.Length > 0)
        {
            foreach (LB_Entry entry in entries)
            {
                Debug.Log("Rank: " + entry.rank + "; Name: " + entry.name + "; Points: " + entry.points);
                newRow = GameObject.Find("LBRow" + i.ToString());
                newRow.transform.Find("Rank").GetComponent<Text>().text = entry.rank.ToString();
                newRow.transform.Find("Player").GetComponent<Text>().text = entry.name.ToString();
                if (nickName.Equals(newRow.transform.Find("Player").GetComponent<Text>().text))
                {
                    newRow.GetComponent<Animator>().enabled = true;
                    userInTop10 = true;
                    Debug.Log("test2");
                }
                    
                newRow.transform.Find("Score").GetComponent<Text>().text = entry.points.ToString();
                i++;
                if (i == maxRow)
                    break;


            }

            if (userInTop10==false)
            {
                userRow = GameObject.Find("LBRow99");
                userRow.GetComponent<Animator>().enabled = true;
                Debug.Log("test3");

            }

        }
        else if (entries == null)
        {
            Debug.Log("ups something went wrong");
        }
    }

    IEnumerator DownloadScores()
    {
        yield return new WaitForSeconds(5);
        LB_Controller.instance.ReloadLeaderboard();
    }

}
