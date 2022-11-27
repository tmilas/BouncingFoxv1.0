using System;
using UnityEngine;
using UnityEngine.UI;
using Lovatto.Countdown;

public class StartGameCanvas : MonoBehaviour
{
    //Countdown
    public bl_Countdown countdown;

    public GameObject[] lifeImages;

    #region life
    public Text lifeText;
    //public Text timesLeftTexT;
    public Button startButton;

    private LifeEngine lifeEngine;

    void Start()
    {
        
        lifeEngine = FindObjectOfType<LifeEngine>();

        lifeEngine.SetActiveScene("start");
    }

    public void ShowLife(string livesLeft)
    {
        lifeText.text = livesLeft;

        ShowLifeImages();
    }

    public void ShowLifeImages()
    {
        int totalLives = 1;
        Int32.TryParse(lifeText.text,out totalLives);

        for(int i=0;i<5;i++)
        {
            if(i<totalLives)
            {
                if(lifeImages[i]!=null)
                    lifeImages[i].SetActive(true);
            }
            else
            {
                if (lifeImages[i] != null)
                    lifeImages[i].SetActive(false);
            }
        }
    }

    public void ShowLifeCounter(int startFrom)
    {
        //timesLeftTexT.text = timesLeft.Minutes.ToString("00") + ":" + timesLeft.Seconds.ToString("00");

        //Debug.Log("ShowLifeCounter" + startFrom + " - " + countdown.CurrentCountValue);
        if (countdown != null)
        {
            countdown.startTime = startFrom;

            if(!countdown.countdownUI.gameObject.activeInHierarchy)
            {
                countdown.countdownUI.gameObject.SetActive(true);
            }
            countdown.StartCountdown(startFrom);

            //Debug.Log("ShowLifeCounter started");
        }

    }

    public bool IsCounterActive()
    {
        if(!countdown)
        {
            return false;
        }

       // Debug.Log("Iscounting: " + countdown.IsCounting + " enabled " + countdown.countdownUI.enabled + " " + countdown.countdownUI.gameObject.activeInHierarchy);

        return countdown.IsCounting && countdown.countdownUI.gameObject.activeInHierarchy;
    }

    public void StartGame()
    {
        if(lifeEngine)
        {
            lifeEngine.DecideStart();
        }
    }

    public void EnableStartButton(bool status)
    {
        startButton.gameObject.SetActive(status);
    }
    #endregion
}
