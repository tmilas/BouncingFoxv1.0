using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IAPEventHandler : MonoBehaviour
{
    public void UnlockGameGranted()
    {
        Debug.Log("Unlock Game Granted!!!");
        GameObject highScoreText = GameObject.Find("HighScore Text");
        highScoreText.GetComponent<Text>().text = "Granted";
    }

    public void UnlockGameNotGranted()
    {
        Debug.Log("Unlock Game Not Granted!!!");
        GameObject highScoreText = GameObject.Find("HighScore Text");
        highScoreText.GetComponent<Text>().text = "Not Granted";
    }
}
