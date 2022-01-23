using System;
using UnityEngine;
using UnityEngine.UI;

public class GameOverCanvas : MonoBehaviour
{
    public Text gameOverText;
    public Text tryAgainText;

    public Text lifeText;
    public Text timesLeftTexT;

    private LifeEngine lifeEngine;

    void Start()
    {
        Debug.Log("game over scene");
        LanguageSupport langSupport = FindObjectOfType<LanguageSupport>();

        gameOverText.text = langSupport.GetText("gameover");

        tryAgainText.text = langSupport.GetText("tryagain");

        timesLeftTexT.gameObject.SetActive(false);

        lifeEngine = FindObjectOfType<LifeEngine>();

        lifeEngine.SetActiveScene("gameover");
    }

    #region life
    public void ShowLife(string livesLeft)
    {
        lifeText.text = livesLeft;
    }

    public void ShowLifeCounter(TimeSpan timesLeft)
    {
        timesLeftTexT.text = timesLeft.Minutes.ToString("00") + ":" + timesLeft.Seconds.ToString("00");
    }

    public bool IsLifeTimesLeftActive()
    {
        if (!timesLeftTexT)
        {
            return false;
        }

        return timesLeftTexT.IsActive();
    }

    public void SetLifeTimesLeftActive(bool active)
    {
        timesLeftTexT.gameObject.SetActive(active);
    }

    public void StartGame()
    {
        if (lifeEngine)
        {
            lifeEngine.DecideStart();
        }
    }
    #endregion
}