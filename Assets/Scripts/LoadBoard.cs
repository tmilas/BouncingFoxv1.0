using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadBoard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (LB_Controller.instance != null)
        {
            LB_Controller.OnUpdatedScores += OnLeaderboardUpdated;
            StartCoroutine(DownloadScores());
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnLeaderboardUpdated(LB_Entry[] entries)
    {
        GameObject newRow;
        int i = 1;
        int maxRow = 11;

        if (entries != null && entries.Length > 0)
        {
            foreach (LB_Entry entry in entries)
            {
                Debug.Log("Rank: " + entry.rank + "; Name: " + entry.name + "; Points: " + entry.points);
                newRow = GameObject.Find("LBRow" + i.ToString());
                newRow.transform.Find("Rank").GetComponent<Text>().text = entry.rank.ToString();
                newRow.transform.Find("Player").GetComponent<Text>().text = entry.name.ToString();
                newRow.transform.Find("Score").GetComponent<Text>().text = entry.points.ToString();
                i++;
                if (i == maxRow)
                    break;


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
