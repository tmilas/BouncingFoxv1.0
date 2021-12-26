using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEngine : MonoBehaviour
{
    [Header("Game Parameters")]
    public int totalPoints = 0;
    public int totalTimePoints = 0;
    public int totalCoins = 0;
    public int coinBasePoint = 25;
    public float jumpConstantSpeed = 7f;
    public float keyPressedMaxValue = 0.6f;
    public float keyPressedMinValue = 0.1f;
    public float maxJumpLimit = 30f;
    public float jumpSpeedFactor = 40f;
    public int scoreUnitFactor = 100;
    public int remainingLives = 1;
    public bool isPaused = false;

    [Header("Bonus Effects")]
    public float jumpFactor = 1;
    public int scoreFactor = 1;
    public float speedFactor = 1.5f;
    public float scaleFactor = 1;
    public float obstacleGenerationFactor = 1;
    public float bonusDuration = 0;
    public bool isBonusActive = false;

    public int highScore = 0;
    private float bonusBeginTime = 0f;

    private UIHandler uiHandler;
    private OptionsUIHandler optionsUIHandler;
    private StorageEngine storageEngine;
    private ObstacleSpawner obstacleSpawner;
    private Fox fox;

    private bool isGameOver = false;

    void Start()
    {
        uiHandler = FindObjectOfType<UIHandler>();
        optionsUIHandler = FindObjectOfType<OptionsUIHandler>();
        storageEngine = FindObjectOfType<StorageEngine>();
        obstacleSpawner = FindObjectOfType<ObstacleSpawner>();

        fox = FindObjectOfType<Fox>();

        SetDefaultFactors();

        GetHighScore();

        //test için
        highScore = 0;

        //Show help screen on first play
        if(highScore == 0)
        {
            StartCoroutine(ShowHelpOnStart());
        }
    }

    void Update()
    {
        if(!isGameOver)
        {
            IsBonusActive();
            CalculateScore();
        }
    }

    private void GetHighScore()
    {
        string highScoreText = storageEngine.LoadData();
        if (highScoreText != "")
        {
            highScore = Int32.Parse(highScoreText);
        }
    }

    private void SetHighScore()
    {
        if (totalPoints > highScore)
        {
            highScore = totalPoints;
            storageEngine.SaveData(totalPoints.ToString());
        }
    }

    private void CalculateScore()
    {
        int newTimePoints = Mathf.FloorToInt(Time.timeSinceLevelLoad) - totalTimePoints;
        totalTimePoints += newTimePoints;
        totalPoints += newTimePoints * scoreFactor * scoreUnitFactor;
        
        if (uiHandler)
            uiHandler.writeScore(totalPoints);
    }

    public void PauseGame()
    {
        isPaused = true;

        Time.timeScale = 0;

        if (uiHandler)
            uiHandler.ShowContinueGame(true);
    }

    IEnumerator ShowHelpOnStart()
    {
        yield return new WaitForSeconds(1f);
        ShowHelp();
    }

    public void ShowHelp()
    {
        isPaused = true;

        Time.timeScale = 0;

        if (optionsUIHandler)
            optionsUIHandler.ShowHelp();
    }

    public void ShowOptions()
    {
        isPaused = true;

        Time.timeScale = 0;

        if (optionsUIHandler)
            optionsUIHandler.ShowOptions();
    }

    public void SetGameOver(bool status)
    {
        isGameOver = status;
        speedFactor = 0;

        if(remainingLives>0)
        {
            Time.timeScale = 0;

            if (uiHandler)
                uiHandler.ShowContinueGame(true);

            remainingLives--;
        }
        else
        {
            CalculateFinalScore();

            SetHighScore();

            StartCoroutine(GoToGameOverScene());
        }
    }

    public void CalculateFinalScore()
    {
        totalPoints += totalCoins * coinBasePoint;
    }

    public void ContinueGame()
    {
        if(isPaused)
        {
            isPaused = false;
        }
        else
        {
            //Continue game after die
            isGameOver = false;
            SetDefaultFactors();

            CollectableItem collectableItem = new CollectableItem();
            collectableItem.itemDuration = 3;
            collectableItem.itemFactor = 3;
            collectableItem.collectableType = CollectableItem.CollectableType.Invincibility5s;

            SetGameBonus(collectableItem);

            fox.ContinueFox();
        }
        
        Time.timeScale = 1;

        if (uiHandler)
            uiHandler.ShowContinueGame(false);
    }

    IEnumerator GoToGameOverScene()
    {
        yield return new WaitForSeconds(1.6f);
        SceneManager.LoadScene("Game Over Screen");
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public float GetSpeedBonusFactor()
    {
        return speedFactor;
    }

    public float GetJumpBonusFactor()
    {
        return jumpFactor;
    }

    public float GetObstacleGenerationBonusFactor()
    {
        return obstacleGenerationFactor;
    }

    public void SetPower(float power, bool keyEnded)
    {
        float sliderValue = power / keyPressedMaxValue;

        if (uiHandler)
        {
            if(keyEnded)
            {
                uiHandler.writePowerSlider(sliderValue);
            }
            else
            {
                uiHandler.updatePowerSlider(sliderValue);
            }
        }
    }

    public void SetGameBonus(CollectableItem collectableItem)
    {
        if (fox && collectableItem.collectableType == CollectableItem.CollectableType.CoinScore)
        {
            totalCoins++;

            if (uiHandler)
            {
                uiHandler.WriteCoin(totalCoins);
                uiHandler.WriteBonus(collectableItem);
            }

            return;
        }

        if (fox && fox.isFoxInvincible())
        {
            return;
        }

        //first turn to default
        SetDefaultFactors();

        //get type of bonus, amount, duration
        bonusDuration = collectableItem.itemDuration;
        bonusBeginTime = Time.time;
        isBonusActive = true;

        if (collectableItem.collectableType == CollectableItem.CollectableType.RandomCollectable)
        {
            collectableItem = GetRandomBonus();
        }

        if(collectableItem.collectableType == CollectableItem.CollectableType.FastMotion2x ||
           collectableItem.collectableType == CollectableItem.CollectableType.FastMotion3x ||
           collectableItem.collectableType == CollectableItem.CollectableType.SlowMotion2x ||
           collectableItem.collectableType == CollectableItem.CollectableType.SlowMotion3x)
        {
            speedFactor = collectableItem.itemFactor;

            //Fast run animation
            if(collectableItem.collectableType == CollectableItem.CollectableType.FastMotion2x ||
               collectableItem.collectableType == CollectableItem.CollectableType.FastMotion3x)
            {
                fox.RunFast();
                scoreFactor = 2;
            }
        }
        else if (collectableItem.collectableType == CollectableItem.CollectableType.JumpPower2x ||
                 collectableItem.collectableType == CollectableItem.CollectableType.JumpPower3x ||
                 collectableItem.collectableType == CollectableItem.CollectableType.GravityIncrease2x ||
                 collectableItem.collectableType == CollectableItem.CollectableType.GravityIncrease3x)
        {
            jumpFactor = collectableItem.itemFactor;
        }
        else if (collectableItem.collectableType == CollectableItem.CollectableType.ScoreIncrease2x ||
                 collectableItem.collectableType == CollectableItem.CollectableType.ScoreIncrease3x)
        {
            scoreFactor = (int)collectableItem.itemFactor;
        }
        else if (collectableItem.collectableType == CollectableItem.CollectableType.ObstacleIncrease2x ||
                 collectableItem.collectableType == CollectableItem.CollectableType.ObstacleIncrease3x)
        {
            obstacleGenerationFactor = collectableItem.itemFactor;
        }
        else if (collectableItem.collectableType == CollectableItem.CollectableType.BiggerCharacter2x ||
                 collectableItem.collectableType == CollectableItem.CollectableType.BiggerCharacter3x ||
                 collectableItem.collectableType == CollectableItem.CollectableType.SmallerCharacter2x ||
                 collectableItem.collectableType == CollectableItem.CollectableType.SmallerCharacter3x)
        {
            scaleFactor = collectableItem.itemFactor;

            if(fox)
                fox.ScaleFox(scaleFactor);
        }
        else if (collectableItem.collectableType == CollectableItem.CollectableType.Invincibility5s ||
                 collectableItem.collectableType == CollectableItem.CollectableType.Invincibility8s)
        {
            if (fox)
                fox.InvincibleFox(true);
        }
        
        if (uiHandler)
            uiHandler.WriteBonus(collectableItem);
    }

    private CollectableItem GetRandomBonus()
    {
        LevelProps levelProps = obstacleSpawner.GetLevelProps();

        int bonusCount = levelProps.levelParachutes.Length;

        CollectableItem randomCollectable = levelProps.levelParachutes[UnityEngine.Random.Range(0, bonusCount)]
                                                      .GetComponent<CollectableItem>();
        
        while (randomCollectable.collectableType==CollectableItem.CollectableType.RandomCollectable)
        {
            randomCollectable = levelProps.levelParachutes[UnityEngine.Random.Range(0, bonusCount)]
                                                      .GetComponent<CollectableItem>();
        }
        
        return randomCollectable;
    }

    public void IsBonusActive()
    {
        float currentTime;

        if (isBonusActive)
        {
            currentTime = Time.time;

            if((currentTime - bonusBeginTime)>=bonusDuration)
            {
                SetDefaultFactors();
            }
            else
            {
                float bonusSliderValue = 1 - (currentTime - bonusBeginTime) / bonusDuration;
                if (uiHandler)
                    uiHandler.SetBonusSliderValue(bonusSliderValue);
            }
        }
    }

    public void SetDefaultFactors()
    {
        jumpFactor = 1;
        scoreFactor = 1;
        speedFactor = 1.5f;
        scaleFactor = 1;
        obstacleGenerationFactor = 1;
        bonusDuration = 0;
        isBonusActive = false;

        if (uiHandler)
            uiHandler.BonusRestart();

        if(fox)
        { 
            fox.ScaleFox(scaleFactor);
            fox.InvincibleFox(false);
            fox.RunNormal();
        }
    }
}