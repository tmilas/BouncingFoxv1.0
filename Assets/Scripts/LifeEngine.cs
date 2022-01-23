using System;
using UnityEngine;
using UnityEngine.UI;

public class LifeEngine : MonoBehaviour
{
    public Text lifeText;
    public Text timesLeftTexT;

    private const int MAX_LIVES = 5;

    private StorageEngine storageEngine;
    private NavigationHandler navHandler;

    private static TimeSpan newLifeInterval = new TimeSpan(0, 1, 0);

    private DateTime lostLifeTimeStamp;
    private int livesLeft = MAX_LIVES;

    void Start()
    {
        storageEngine = FindObjectOfType<StorageEngine>();

        navHandler = FindObjectOfType<NavigationHandler>();

        GetLivesFromStorage();
    }

    void Update()
    {
        if (livesLeft < MAX_LIVES)
        {
            TimeSpan t = DateTime.Now - lostLifeTimeStamp;
            int amountOfIntervalsPassed = 0;

            try
            {
                double intervalD = System.Math.Floor(t.TotalSeconds / newLifeInterval.TotalSeconds);
                amountOfIntervalsPassed = Convert.ToInt32(intervalD);

                if (amountOfIntervalsPassed > 0)
                {
                    //Debug.Log("On update : Lives left: " + livesLeft.ToString() + ", new lives: " + amountOfIntervalsPassed.ToString());
                    livesLeft = livesLeft + amountOfIntervalsPassed;
                    lostLifeTimeStamp = DateTime.Now;

                    if (livesLeft >= MAX_LIVES)
                    {
                        FullLives();
                    }

                    SaveLivesToStorage();
                }

                if (livesLeft < MAX_LIVES)
                {
                    TimeSpan tempTime = newLifeInterval - t + TimeSpan.FromSeconds(1);
                    ShowTime(tempTime);
                }
            }
            catch (OverflowException)
            {
                FullLives();
            }
        }

        lifeText.text = livesLeft.ToString();
    }

    public void DecideStart()
    {
        if(livesLeft>0)
        {
            livesLeft--;

            if (livesLeft < MAX_LIVES && !timesLeftTexT.IsActive())
            {
                lostLifeTimeStamp = DateTime.Now;
            }

            if (navHandler)
            {
                navHandler.StartGame();
            }

            SaveLivesToStorage();
        }
    }

    private void ShowTime(TimeSpan timeSpan)
    {
        if(!timesLeftTexT.IsActive())
        { 
            timesLeftTexT.gameObject.SetActive(true);
        }

        timesLeftTexT.text = timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00");
    }

    private void FullLives()
    {
        livesLeft = MAX_LIVES;

        timesLeftTexT.gameObject.SetActive(false);
    }

    private void GetLivesFromStorage()
    {
        string livesTempText = storageEngine.LoadLifeCount();
        if (livesTempText != "")
        {
            string[] livesArray = livesTempText.Split(';');

            //Debug.Log("lives array length: " + livesArray.Length);

            try
            {
                livesLeft = Int32.Parse(livesArray[0]);

                lostLifeTimeStamp = DateTime.Parse(livesArray[1]);

                //Debug.Log("Load from storage : Lives left: " + livesLeft.ToString() + "time left: " + lostLifeTimeStamp.ToString());
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }
    }

    private void SaveLivesToStorage()
    {
        string livesTempText = livesLeft.ToString() + ";" + lostLifeTimeStamp.ToString();

        storageEngine.SaveLifeCount(livesTempText);

        //Debug.Log("Save to storage : Lives left: " + livesLeft.ToString() + "time left: " + lostLifeTimeStamp.ToString());
    }
}