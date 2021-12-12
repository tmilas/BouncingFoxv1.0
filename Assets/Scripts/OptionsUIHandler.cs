using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUIHandler : MonoBehaviour
{
    private LanguageSupport langSupport;

    [Header("Help Properties")]
    public GameObject helpCanvas;
    public Button helpNextButton;
    public Button helpPrevButton;
    public Image helpImage;
    public string helpResourcesPath = "Images/Game/";
    private int helpScreen = 1;

    void Start()
    {
        langSupport = FindObjectOfType<LanguageSupport>();

        helpCanvas.gameObject.SetActive(false);
    }

    #region help

    public void ShowHelp(bool isShow)
    {
        helpCanvas.gameObject.SetActive(isShow);
        helpPrevButton.gameObject.SetActive(false);
        helpImage.sprite = Resources.Load<Sprite>(helpResourcesPath + "help_jump");
    }

    public void NextHelpScreen()
    {
        if (helpScreen == 1)
        {
            helpScreen = 2;
            helpPrevButton.gameObject.SetActive(true);
            helpImage.sprite = Resources.Load<Sprite>(helpResourcesPath + "help_slide");
        }
        else if (helpScreen == 2)
        {
            helpCanvas.gameObject.SetActive(false);

            GameEngine gameEngine = FindObjectOfType<GameEngine>();

            if (gameEngine)
                gameEngine.ContinueGame();
        }
    }

    public void PrevHelpScreen()
    {
        helpScreen = 1;
        helpPrevButton.gameObject.SetActive(false);
        helpImage.sprite = Resources.Load<Sprite>(helpResourcesPath + "help_jump");
    }

    #endregion
}
