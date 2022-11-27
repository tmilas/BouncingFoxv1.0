using System;
using UnityEngine;

public class LifeEngine : MonoBehaviour
{
    public static LifeEngine instance;

    private const int MAX_LIVES = 5;
    private static TimeSpan newLifeInterval = new TimeSpan(0, 1, 0);
    //private static TimeSpan newLifeInterval = new TimeSpan(0, 0, 30);

    private StorageEngine storageEngine;
    private NavigationHandler navHandler;
    private StartGameCanvas startGameCanvas;

    private DateTime lostLifeTimeStamp;
    private int livesLeft = MAX_LIVES;
    private bool isLimitless = false;
    private DateTime limitEndDate = DateTime.Now;
//    private bool isTimerActive = false;

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
        bool isIntervalPassed = false;

        if (livesLeft < MAX_LIVES || (isLimitless && DateTime.Now <= limitEndDate))
        {
            TimeSpan t = DateTime.Now - lostLifeTimeStamp;
            int amountOfIntervalsPassed;

            try
            {
                double intervalD = System.Math.Floor(t.TotalSeconds / newLifeInterval.TotalSeconds);
                amountOfIntervalsPassed = Convert.ToInt32(intervalD);

                if (amountOfIntervalsPassed > 0)
                {
                    //Debug.Log("On update : Lives left: " + livesLeft.ToString() + ", new lives: " + amountOfIntervalsPassed.ToString()+ " t= " + t.Seconds);
                    livesLeft = livesLeft + amountOfIntervalsPassed;

                    lostLifeTimeStamp = lostLifeTimeStamp.AddSeconds(amountOfIntervalsPassed * newLifeInterval.TotalSeconds);

                    isIntervalPassed = true;

                    if (livesLeft >= MAX_LIVES)
                    {
                        FullLives();
                    }

                    SaveLivesToStorage();
                }

                if (livesLeft < MAX_LIVES)
                {
                    TimeSpan tempTime = newLifeInterval - t;
                    //Debug.Log("timespans - " + tempTime.Seconds + "passed time - " + t.Seconds);
                    int tempSeconds = tempTime.Seconds;

                    if (startGameCanvas && (isIntervalPassed || !startGameCanvas.IsCounterActive()))
                    {
                        //Debug.Log("timespans in " + newLifeInterval.Seconds + " - " + tempTime.Seconds);

                        startGameCanvas.ShowLifeCounter(tempSeconds);
                    }
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
        //Debug.Log("TOLGA11");
        if(livesLeft>0)
        {
            //Debug.Log("TOLGA22");

            

            //Debug.Log(livesLeft + "1");
            if (livesLeft >= MAX_LIVES)
            {
                //Debug.Log(livesLeft + "2");
                if (startGameCanvas)
                {
                    //Debug.Log(livesLeft + "3");
                    lostLifeTimeStamp = DateTime.Now;
                }
            }

            livesLeft--;

            if (navHandler)
            {
                //Debug.Log("TOLGA33");
                Time.timeScale = 1;
                //isTimerActive = false;
                navHandler.StartGame();
            }

            //Debug.Log(livesLeft + "4");

            SaveLivesToStorage();
        }
    }

    private void FullLives()
    {
        livesLeft = MAX_LIVES;
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