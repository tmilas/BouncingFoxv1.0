using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class OptionsUIHandler : MonoBehaviour
{
    private LanguageSupport langSupport;

    [Header("Help Properties")]
    public GameObject helpCanvas;
    public GameObject bgObject;
    public Button helpNextButton;
    public Button helpPrevButton;
    public Image helpImage;
    public VideoPlayer helpVideo;
    public string helpResourcesPath = "Images/Game/";
    private int helpScreen = 1;

    private UIHandler gameCanvas;

    void Start()
    {
        langSupport = FindObjectOfType<LanguageSupport>();

        helpCanvas.gameObject.SetActive(false);

        gameCanvas = FindObjectOfType<UIHandler>();
    }

    #region help

    public void ShowHelp()
    {
        ShowHelpVideo();

        helpCanvas.gameObject.SetActive(true);

        helpPrevButton.gameObject.SetActive(false);
    }

    public void NextHelpScreen()
    {
        if (helpScreen == 1)
        {
            helpScreen = 2;

            CloseHelpVideo();

            helpPrevButton.gameObject.SetActive(true);
            
            helpImage.sprite = Resources.Load<Sprite>(helpResourcesPath + "help_jump");
        }
        else if (helpScreen == 2)
        {
            helpScreen = 3;
            helpPrevButton.gameObject.SetActive(true);
            helpImage.sprite = Resources.Load<Sprite>(helpResourcesPath + "help_slide");
        }
        else if (helpScreen == 3)
        {
            helpScreen = 4;
 
            helpImage.sprite = Resources.Load<Sprite>(helpResourcesPath + "help_potions");
        }
        else if (helpScreen == 4)
        {
            helpCanvas.gameObject.SetActive(false);

            GameEngine gameEngine = FindObjectOfType<GameEngine>();

            if (gameEngine)
                gameEngine.ContinueGame();
        }
    }

    public void PrevHelpScreen()
    {
        if (helpScreen == 2)
        {
            helpScreen = 1;

            ShowHelpVideo();

            helpPrevButton.gameObject.SetActive(false);
        }
        else if (helpScreen == 3)
        { 
            helpScreen = 2;
            helpImage.sprite = Resources.Load<Sprite>(helpResourcesPath + "help_jump");
        }
        else if (helpScreen == 4)
        {
            helpScreen = 3;
            helpImage.sprite = Resources.Load<Sprite>(helpResourcesPath + "help_slide");
        }
    }

    private void ShowHelpVideo()
    {
        if (gameCanvas)
        {
            gameCanvas.gameObject.SetActive(false);
        }

        bgObject.gameObject.SetActive(false);
        helpVideo.gameObject.SetActive(true);
        helpImage.gameObject.SetActive(false);
    }

    private void CloseHelpVideo()
    {
        if (gameCanvas)
        {
            gameCanvas.gameObject.SetActive(true);
        }

        helpVideo.gameObject.SetActive(false);
        bgObject.gameObject.SetActive(true);
        helpImage.gameObject.SetActive(true);
    }

    #endregion
}
