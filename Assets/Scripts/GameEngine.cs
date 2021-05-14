using System;
using UnityEngine;

public class GameEngine : MonoBehaviour
{
    [Header("Game Parameters")]
    public int totalPoints = 0;
    public int totalTimePoints = 0;
    public float jumpConstantSpeed = 5f;
    public float keyPressedMaxValue = 1f;
    public float keyPressedMinValue = 0.3f;

    [Header("Bonus Effects")]
    public float jumpFactor = 1;
    public int scoreFactor = 1;
    public float speedFactor = 1;
    public float bonusDuration = 0;
    public bool isBonusActive = false;

    //--------------sil---------------
    public bool setTestBonus = false;
    //--------------sil---------------

    public int highScore = 0;
    private float bonusBeginTime = 0f;

    private UIHandler uiHandler;
    private StorageEngine storageEngine;

    private bool isGameOver = false;

    void Start()
    {
        uiHandler = FindObjectOfType<UIHandler>();
        storageEngine = FindObjectOfType<StorageEngine>();

        SetDefaultFactors();

        GetHighScore();
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

    void Update()
    {
        if(!isGameOver)
        {
            //--------------sil---------------
            if(setTestBonus)
            {
                setTestBonus = false;
                //SetGameBonus(3,4,5);
            }
            //--------------sil---------------

            IsBonusActive();
            CalculateScore();
        }
    }

    private void CalculateScore()
    {
        int newTimePoints = Mathf.FloorToInt(Time.timeSinceLevelLoad) - totalTimePoints;
        totalTimePoints += newTimePoints;
        totalPoints += newTimePoints * scoreFactor;

        if (uiHandler)
            uiHandler.writeScore(totalPoints);
    }

    public void setGameOver(bool status)
    {
        isGameOver = status;
        speedFactor = 0;

        SetHighScore();
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
        //get type of bonus, amount, duration
        bonusDuration = collectableItem.itemDuration;
        bonusBeginTime = Time.time;
        isBonusActive = true;

        if(collectableItem.collectableType==CollectableItem.CollectableType.FastMotion2x)
        {
            //if score point bonus
            totalPoints += (int)collectableItem.itemFactor;
        }
        else if(collectableItem.collectableType == CollectableItem.CollectableType.FastMotion2x)
        {
            //score factor bonus
            scoreFactor = (int)collectableItem.itemFactor;
        }
        else if (collectableItem.collectableType == CollectableItem.CollectableType.FastMotion2x)
        {
            //jump factor bonus
            jumpFactor = (int)collectableItem.itemFactor;
        }
        else if (collectableItem.collectableType == CollectableItem.CollectableType.FastMotion2x)
        {
            //speed factor bonus
            speedFactor = (int)collectableItem.itemFactor;
        }
            
        if (uiHandler)
            uiHandler.writeBonus("Bonus Active");
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
        }
    }

    public void SetDefaultFactors()
    {
        jumpFactor = 1;
        scoreFactor = 1;
        speedFactor = 1;
        bonusDuration = 0;
        isBonusActive = false;

        if (uiHandler)
            uiHandler.writeBonus("");
    }
}