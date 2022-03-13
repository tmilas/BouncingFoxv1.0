using System;
using UnityEngine;

public class LifeEngine : MonoBehaviour
{
    public static LifeEngine instance;

    private const int MAX_LIVES = 5;
    private static TimeSpan newLifeInterval = new TimeSpan(0, 2, 0);

    private StorageEngine storageEngine;
    private NavigationHandler navHandler;
    private StartGameCanvas startGameCanvas;

    private DateTime lostLifeTimeStamp;
    private int livesLeft = MAX_LIVES;
    private bool isLimitless = false;
    private DateTime limitEndDate = DateTime.Now;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        storageEngine = FindObjectOfType<StorageEngine>();

        GetLivesFromStorage();
    }

    void Update()
    {
        if (livesLeft < MAX_LIVES || (isLimitless && DateTime.Now <= limitEndDate))
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

            ShowLives();    
        }
    }

    public void SetActiveScene(string scene)
    {
        if(scene.Equals("start"))
        {
            startGameCanvas = FindObjectOfType<StartGameCanvas>();

            navHandler = FindObjectOfType<NavigationHandler>();
        }
    }

    private void ShowLives()
    {
        if(startGameCanvas)
        {
            startGameCanvas.ShowLife(livesLeft.ToString());

            startGameCanvas.EnableStartButton(true);

            if (livesLeft <= 0)
            {
                startGameCanvas.EnableStartButton(false);
            }
        }
    }

    public void DecideStart()
    {
        Debug.Log("TOLGA11");
        if(livesLeft>0)
        {
            Debug.Log("TOLGA22");

            livesLeft--;

            Debug.Log(livesLeft + "1");
            if (livesLeft < MAX_LIVES)
            {
                Debug.Log(livesLeft + "2");
                if (startGameCanvas && !startGameCanvas.IsLifeTimesLeftActive())
                {
                    Debug.Log(livesLeft + "3");
                    lostLifeTimeStamp = DateTime.Now;
                }
            }

            if (navHandler)
            {
                Debug.Log("TOLGA33");
                Time.timeScale = 1;
                navHandler.StartGame();
            }

            Debug.Log(livesLeft + "4");

            SaveLivesToStorage();
        }
    }

    private void ShowTime(TimeSpan timeSpan)
    {
        if(startGameCanvas)
        {
            if (!startGameCanvas.IsLifeTimesLeftActive())
            {
                startGameCanvas.SetLifeTimesLeftActive(true);
            }

            startGameCanvas.ShowLifeCounter(timeSpan);
        }
    }

    private void FullLives()
    {
        livesLeft = MAX_LIVES;

        if(startGameCanvas)
        {
            startGameCanvas.SetLifeTimesLeftActive(false);
        }
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

                isLimitless = Boolean.Parse(livesArray[2]);

                limitEndDate = DateTime.Parse(livesArray[3]);

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
        string livesTempText = livesLeft.ToString() + ";" + lostLifeTimeStamp.ToString() + ";" + isLimitless + ";" + limitEndDate.ToShortDateString();

        storageEngine.SaveLifeCount(livesTempText);

        //Debug.Log("Save to storage : Lives left: " + livesLeft.ToString() + "time left: " + lostLifeTimeStamp.ToString());
    }

    public void NoLimitsUntil(DateTime endDate)
    {
        isLimitless = true;

        limitEndDate = endDate;

        SaveLivesToStorage();
    }
}