using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class LoadBoard : MonoBehaviour
{

    private StorageEngine storageEngine;
    private string nickNameWithId;
    private bool userInTop10 = false;
    private RestAPIClient restAPIClient;

    private void Start()
    {
    }

    // Start is called before the first frame update
    void Awake()
    {
        GameObject userRow;

        Debug.Log("LoadBoard - awake");
        storageEngine = FindObjectOfType<StorageEngine>();
        string scoreText = storageEngine.LoadDataScore();
        nickNameWithId= storageEngine.LoadDataNick(false);

        if (scoreText == "")
           scoreText = "0";

        if (nickNameWithId == "")
            nickNameWithId = "User";

        userRow = GameObject.Find("LBRow99");
        userRow.transform.Find("Rank").GetComponent<Text>().text = "Player Score";
        userRow.transform.Find("Player").GetComponent<Text>().text = nickNameWithId;
        userRow.transform.Find("Score").GetComponent<Text>().text = scoreText;

        
        /* if (LB_Controller.instance != null)
         {
             Debug.Log("LoadBoard - lbcontroller not null");
             if (LB_Controller.OnUpdatedScores==null)
             {
                 LB_Controller.OnUpdatedScores += OnLeaderboardUpdated;
             }
             //LB_Controller.instance.ReloadLeaderboard();

             StartCoroutine(DownloadScores());
         }*/

        restAPIClient = GameObject.FindObjectOfType<RestAPIClient>();
        if (restAPIClient!=null)
        {
            Debug.Log("rest not null");
            RestAPIClient.OnSuccessLBGet += OnLBUpdated;
            restAPIClient.GetLeaderBoard();
       
        }
        else
        {
            Debug.Log("rest null");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnLBUpdated(string test)
    {
        Debug.Log("yupppi" + test);
        //LeaderBoardEntry[] leaderBoardEntries = JsonConvert.DeserializeObject(test);

        List<LeaderBoardEntry> ll = JsonConvert.DeserializeObject<List<LeaderBoardEntry>>(test);


        Debug.Log(ll.Count.ToString());

        Debug.Log("OnLBUpdated");
        GameObject newRow;
        GameObject userRow;

        int i = 1;
        int maxRow = 11;
        userInTop10 = false;
        nickNameWithId = storageEngine.LoadDataNick(true);

        if (ll != null && ll.Count > 0)
        {
            foreach (LeaderBoardEntry entry in ll)
            {
                Debug.Log("Name: " + entry.playerid + "; Points: " + entry.playerscore);
                newRow = GameObject.Find("LBRow" + i.ToString());
                newRow.transform.Find("Rank").GetComponent<Text>().text = i.ToString();
                newRow.transform.Find("PlayerWithId").GetComponent<Text>().text = entry.playerid;
                //newRow.transform.Find("Player").GetComponent<Text>().text = entry.playerid.Substring(0, entry.playerid.ToString().IndexOf(StorageEngine.userIdSeperator));
                newRow.transform.Find("Player").GetComponent<Text>().text = entry.playerid;
                if (nickNameWithId.Equals(newRow.transform.Find("PlayerWithId").GetComponent<Text>().text))
                {
                    newRow.GetComponent<Animator>().enabled = true;
                    userInTop10 = true;
                    Debug.Log("test2");
                }

                newRow.transform.Find("Score").GetComponent<Text>().text = entry.playerscore.ToString();
                i++;
                if (i == maxRow)
                    break;


            }

            if (userInTop10 == false)
            {
                userRow = GameObject.Find("LBRow99");
                userRow.GetComponent<Animator>().enabled = true;
                Debug.Log("test3");

            }

        }
        else if (ll == null)
        {
            Debug.Log("ups something went wrong");
        }

    }

    private void OnLeaderboardUpdated(LB_Entry[] entries)
    {
        Debug.Log("OnLeaderboardUpdated");
        GameObject newRow;
        GameObject userRow;

        int i = 1;
        int maxRow = 11;
        userInTop10 = false;
        nickNameWithId = storageEngine.LoadDataNick(true);

        if (entries != null && entries.Length > 0)
        {
            foreach (LB_Entry entry in entries)
            {
                Debug.Log("Rank: " + entry.rank + "; Name: " + entry.name + "; Points: " + entry.points);
                newRow = GameObject.Find("LBRow" + i.ToString());
                newRow.transform.Find("Rank").GetComponent<Text>().text = entry.rank.ToString();
                newRow.transform.Find("PlayerWithId").GetComponent<Text>().text = entry.name.ToString();
                newRow.transform.Find("Player").GetComponent<Text>().text = entry.name.ToString().Substring(0, entry.name.ToString().IndexOf(StorageEngine.userIdSeperator));
                if (nickNameWithId.Equals(newRow.transform.Find("PlayerWithId").GetComponent<Text>().text))
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
        Debug.Log("LoadBoard - downloadscores1");
        yield return new WaitForSeconds(7);
        LB_Controller.instance.ReloadLeaderboard();
        Debug.Log("LoadBoard - downloadscores2");

    }

}
