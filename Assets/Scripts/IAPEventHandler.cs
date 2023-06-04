using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IAPEventHandler : MonoBehaviour
{

    private LifeEngine lifeEngine;

    public void UnlockGameGranted()
    {
        Debug.Log("Unlock Game Granted!!!");
        lifeEngine = FindObjectOfType<LifeEngine>();
        DateTime endDate = new DateTime(2099, 1, 1);
        lifeEngine.NoLimitsUntil(endDate);

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
