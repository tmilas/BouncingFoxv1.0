using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameOverCanvas : MonoBehaviour
{
    public GameObject gameOverMainCanvas;
    public Text totalPointsText;
    public Text totalGoldsText;
    public GameObject coinObject;
    private Animator coinAnimator;

    [Header("Sound Parameters")]
    [SerializeField] AudioClip coinSound;
    [SerializeField][Range(0, 1)] float coinSoundVolume = 0.8f;

    public int coinBasePoint = 25;
    private int currentPoints = 0;
    private int currentGolds = 0;

    private float waitTime = 0.1f;
    private float timer = 0.0f;

    void Start()
    {
        gameOverMainCanvas.gameObject.SetActive(false);

        if(coinObject)
        {
            coinAnimator = coinObject.GetComponent<Animator>();
            coinAnimator.enabled = false;
        }

        if(GameOverModel.isGameOver)
        {
            currentGolds = GameOverModel.GetTotalGolds();
            currentPoints = GameOverModel.GetTotalPoints();

            ShowGameOverCanvas();

            GameOverModel.EndGameOver();
        }

        //written for test purpose
        //ShowGameOverCanvas(25, 50);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > waitTime)
        {
            timer = timer - waitTime;

            CountDownCoins();
        }
    }

    private void CountDownCoins()
    {
        if(currentGolds > 0)
        {
            currentGolds -= 1;
            currentPoints += coinBasePoint;

            UpdateTextFields();
            if (coinSound)
                AudioSource.PlayClipAtPoint(coinSound, Camera.main.transform.position, coinSoundVolume);

            if (currentGolds <= 0)
            {
                coinAnimator.enabled = false;
                coinObject.SetActive(false);
            }
        }
    }

    public void ShowGameOverCanvas()
    {
        UpdateTextFields();

        gameOverMainCanvas.gameObject.SetActive(true);
        coinAnimator.enabled = true;
    }

    private void UpdateTextFields()
    {
        totalGoldsText.text = currentGolds.ToString();
        totalPointsText.text = currentPoints.ToString();
    }

    public void CloseGameOverCanvas()
    {
        coinAnimator.enabled = false;
        gameOverMainCanvas.gameObject.SetActive(false);
    }
}
