using System;
using UnityEngine;
using UnityEngine.UI;

public class StartGameCanvas : MonoBehaviour
{
    #region life
    public Text lifeText;
    public Text timesLeftTexT;
    public Button startButton;

    private LifeEngine lifeEngine;

    void Start()
    {
        timesLeftTexT.gameObject.SetActive(false);

        lifeEngine = FindObjectOfType<LifeEngine>();

        lifeEngine.SetActiveScene("start");
    }

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
        if(!timesLeftTexT)
        {
            return false;
        }

        //Debug.Log(timesLeftTexT.IsActive());

        return timesLeftTexT.IsActive();
    }

    public void SetLifeTimesLeftActive(bool active)
    {
        timesLeftTexT.gameObject.SetActive(active);
    }

    public void StartGame()
    {
        if(lifeEngine)
        {
            lifeEngine.DecideStart();
        }
    }

    public void EnableStartButton(bool status)
    {
        startButton.gameObject.SetActive(status);
    }
    #endregion
}
