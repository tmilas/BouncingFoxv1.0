using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    //cached parameters
    public Text scoreText;
    public Slider powerSlider;

    public void writeScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void writePowerSlider(float value)
    {
        StartCoroutine(SetPowerOverTime(value));
    }

    private IEnumerator SetPowerOverTime(float sliderValue)
    {
        for (float i = 0; i <= sliderValue; i = i + 0.1f)
        {
            powerSlider.value = i;
        }

        yield return new WaitForSeconds(0.2f);

        for (float i = sliderValue; i >= 0; i = i - 0.1f)
        {
            powerSlider.value = i;
        }

        powerSlider.value = 0;
    }
}