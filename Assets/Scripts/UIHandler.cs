using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    //cached parameters
    public Text scoreText;

    [Header("Bonus Properties")]
    public Text bonusText;
    public Slider powerSlider;
    public Slider bonusSlider;
    public Image bonusImage;
    public string bonusResourcesPath = "Images/Bonus/"; 

    [Header("Power Bar Properties")]
    public float waitSecondsForBar = 0.6f;
    public float incPowerBar = 0.01f;
    public float decPowerBar = 0.1f;

    [Header("Game Properties")]
    public Button continueButton;
    public Text continueText;

    private LanguageSupport langSupport;

    void Start()
    {
        langSupport = FindObjectOfType<LanguageSupport>();

        continueText.text = langSupport.GetText("continue");

        continueButton.gameObject.SetActive(false);
    }

    public void BonusRestart()
    {
        bonusText.text = "";
        bonusImage.enabled = false;
        bonusSlider.gameObject.SetActive(false);
    }

    public void WriteBonus(CollectableItem bonus)
    {
        bonusText.text = langSupport.GetText(bonus.collectableType.ToString());
        bonusImage.enabled = true;
        bonusImage.sprite = Resources.Load<Sprite>(bonusResourcesPath + bonus.collectableType.ToString());

        bonusSlider.value = 1;
        bonusSlider.gameObject.SetActive(true);
    }

    public void SetBonusSliderValue(float value)
    {
        bonusSlider.value = value;
    }

    public void writeScore(int score)
    {
        scoreText.text = score.ToString();
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

    public void ContinueGame()
    {
        GameEngine gameEngine = FindObjectOfType<GameEngine>();

        if(gameEngine)
            gameEngine.ContinueGame();
    }

    public void ShowContinueGame(bool isShow)
    {
        continueButton.gameObject.SetActive(isShow);
    }
}