using System.Collections;
using UnityEngine;

public class GameEngine : MonoBehaviour
{
    public int totalPoints = 0;
    public float jumpConstantSpeed = 5f;
    public float keyPressedMaxValue = 1f;
    public float keyPressedMinValue = 0.3f;

    private UIHandler uiHandler;

    private bool isGameOver = false;

    void Start()
    {
        uiHandler = FindObjectOfType<UIHandler>();
    }

    void Update()
    {
        CalculateScore();
    }

    private void CalculateScore()
    {
        totalPoints = Mathf.FloorToInt(Time.timeSinceLevelLoad);

        if (uiHandler)
            uiHandler.writeScore(totalPoints);
    }

    public void setGameOver(bool status)
    {
        isGameOver = status;
    }

    public bool IsGameOver()
    {
        return isGameOver;
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
}