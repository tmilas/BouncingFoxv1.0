using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    //cached parameters
    public Text scoreText;

    //Text Spawner parameters
    private TextSpawner textSpawner;
    private int cloneTextIndex;

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
    public GameObject continueCanvas;


    public Text coinText;

    private LanguageSupport langSupport;

    void Start()
    {
        langSupport = FindObjectOfType<LanguageSupport>();

        textSpawner = FindObjectOfType<TextSpawner>();
        DontDestroyOnLoad(textSpawner);

        continueText.text = langSupport.GetText("continue");

        continueCanvas.gameObject.SetActive(false);
    }

    public void BonusRestart()
    {
        bonusText.text = "";
        bonusImage.enabled = false;
        bonusSlider.gameObject.SetActive(false);
    }

    public void WriteBonus(CollectableItem bonus)
    {
        string collectableText = langSupport.GetText(bonus.collectableType.ToString());

        if (bonus.collectableType!=CollectableItem.CollectableType.CoinScore)
        {
            bonusText.text = collectableText;
            bonusImage.enabled = true;
            bonusImage.sprite = Resources.Load<Sprite>(bonusResourcesPath + bonus.collectableType.ToString());

            bonusSlider.value = 1;
            bonusSlider.gameObject.SetActive(true);
        }

        cloneTextIndex = collectableText.IndexOf("(Clone)");
        if (cloneTextIndex > 0)
            collectableText = collectableText.Substring(0, cloneTextIndex);

        textSpawner.spawnText(collectableText);
    }

    public void WriteCoin(int value)
    {
        if (coinText)
            coinText.text = value.ToString();
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

    public void EndGame()
    {
        GameEngine gameEngine = FindObjectOfType<GameEngine>();

        if (gameEngine)
            gameEngine.SetGameOver(true, false);

        Debug.Log("hhh");
    }

    public void ShowContinueGame(bool isShow)
    {
        continueCanvas.gameObject.SetActive(isShow);
    }
}