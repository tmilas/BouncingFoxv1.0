using System.Collections;
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

    private float bonusBeginTime = 0f;

    private UIHandler uiHandler;

    private bool isGameOver = false;

    void Start()
    {
        uiHandler = FindObjectOfType<UIHandler>();

        SetDefaultFactors();
    }

    void Update()
    {
        if(!isGameOver)
        {
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

    public void SetGameBonus(int type, float amount, int duration)
    {
        //get type of bonus, amount, duration
        bonusDuration = duration;
        bonusBeginTime = Time.time;
        isBonusActive = true;

        if(type==1)
        {
            //if score point bonus
            totalPoints += (int)amount;
        }
        else if(type==2)
        {
            //score factor bonus
            scoreFactor = (int)amount;
        }
        else if (type == 3)
        {
            //jump factor bonus
            jumpFactor = (int)amount;
        }
        else if (type == 4)
        {
            //speed factor bonus
            speedFactor = (int)amount;
        }
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
    }
}