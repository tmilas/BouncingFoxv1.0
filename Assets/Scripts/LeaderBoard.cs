using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    public GameObject boardRaw;

    // Start is called before the first frame update
    void Start()
    {
        GameObject newRow;

        if (LB_Controller.instance != null)
        {
            LB_Controller.OnUpdatedScores += OnLeaderboardUpdated;
            StartCoroutine(DownloadScores());
        }

        /*for (int i=0;i<10;i++)
        {
            newRow = Instantiate(boardRaw,this.transform);
            RectTransform xx= newRow.GetComponent<RectTransform>();
            xx.localPosition= new Vector3(0f, -60f * i, 0f);
            //xx. anchoredPosition = new Vector2(0f, -60f*i);
            //xx.position = new Vector3(0f, -60f*i, 0f);
            //newRow.transform.position = new Vector3(0f, 0f, 0f);
            newRow.transform.Find("Rank").GetComponent<Text>().text=(i+1).ToString();


        }*/

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnLeaderboardUpdated(LB_Entry[] entries)
    {
        GameObject newRow;
        int i=1;

        if (entries != null && entries.Length > 0)
        {
            foreach (LB_Entry entry in entries)
            {
                Debug.Log("Rank: " + entry.rank + "; Name: " + entry.name + "; Points: " + entry.points);
                newRow = Instantiate(boardRaw, this.transform);
                RectTransform xx = newRow.GetComponent<RectTransform>();
                xx.localPosition = new Vector3(0f, -140f * i, 0f);
                newRow.transform.Find("Rank").GetComponent<Text>().text = entry.rank.ToString();
                newRow.transform.Find("Player").GetComponent<Text>().text = entry.name.ToString();
                newRow.transform.Find("Score").GetComponent<Text>().text = entry.points.ToString();
                i++;


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
