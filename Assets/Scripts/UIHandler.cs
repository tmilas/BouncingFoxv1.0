using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    //cached parameters
    public Text scoreText;
    public Text bonusText;
    public Slider powerSlider;

    [Header("Power Bar Properties")]
    public float waitSecondsForBar = 1f;
    public float incPowerBar = 0.01f;
    public float decPowerBar = 0.1f;

    public void writeScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void writeBonus(string bonus)
    {
        bonusText.text = bonus;
    }

    public void updatePowerSlider(float value)
    {
        for (float i = powerSlider.value; i <= value; i = i + incPowerBar)
        {
            powerSlider.value = i;
        }
    }

    public void writePowerSlider(float value)
    {
        StartCoroutine(SetPowerOverTime(value));
    }

    private IEnumerator SetPowerOverTime(float sliderValue)
    {
        for (float i = powerSlider.value; i <= sliderValue; i = i + incPowerBar)
        {
            powerSlider.value = i;
        }

        yield return new WaitForSeconds(waitSecondsForBar);

        for (float i = sliderValue; i >= 0; i = i - decPowerBar)
        {
            powerSlider.value = i;
        }

        powerSlider.value = 0;
    }
}