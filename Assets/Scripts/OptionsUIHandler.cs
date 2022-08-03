using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class OptionsUIHandler : MonoBehaviour
{
    private LanguageSupport langSupport;
    private GameEngine gameEngine;
    private AudioSource audioSource;
    private StorageEngine storageEngine;

    [Header("Options Properties")]
    public GameObject optionsMainCanvas;
    public Slider musicSoundSlider;

    [Header("Help Properties")]
    public GameObject helpCanvas;
    public GameObject bgObject;
    public Button helpNextButton;
    public Button helpPrevButton;
    public Image helpImage;
    public VideoPlayer helpVideo;
    public string helpResourcesPath = "Images/Game/";
    public GameObject nickNameField;
    private int helpScreen = 1;

    private UIHandler gameCanvas;
    private StartGameCanvas startGameCanvas;
    //private GameOverCanvas gameOverCanvas;

    void Start()
    {

        optionsMainCanvas.gameObject.SetActive(false);

        helpCanvas.gameObject.SetActive(false);

        helpVideo.gameObject.SetActive(false);

        bgObject.gameObject.SetActive(false);

        langSupport = FindObjectOfType<LanguageSupport>();

        helpCanvas.gameObject.SetActive(false);

        gameCanvas = FindObjectOfType<UIHandler>();

        startGameCanvas = FindObjectOfType<StartGameCanvas>();

        //gameOverCanvas = FindObjectOfType<GameOverCanvas>();

        gameEngine = FindObjectOfType<GameEngine>();

        audioSource = Camera.main.GetComponent<AudioSource>();

        storageEngine = FindObjectOfType<StorageEngine>();

    }

    #region options

    public void ShowOptions()
    {
        InputField nickInputField= nickNameField.GetComponent<InputField>();
        string nickName = storageEngine.LoadDataNick(false);
        nickInputField.text = nickName;

        optionsMainCanvas.gameObject.SetActive(true);

        helpCanvas.gameObject.SetActive(false);

        helpVideo.gameObject.SetActive(false);

        bgObject.gameObject.SetActive(true);

        musicSoundSlider.value = audioSource.volume;
    }

    public void CloseOptions()
    {
        optionsMainCanvas.gameObject.SetActive(false);
        bgObject.gameObject.SetActive(false);
        string currentNickName = storageEngine.LoadDataNick(false);
        string nickName = nickNameField.GetComponent<InputField>().text;
        if (!currentNickName.Equals(nickName))
        {
            storageEngine.SaveDataNick(nickName);
            storageEngine.SaveDataScore("0");
            storageEngine.SaveDataPostedScore("0");
            GameObject highScoreText = GameObject.Find("HighScore Text");
            highScoreText.GetComponent<Text>().text = "High Score: 0";
        }


        if (gameEngine)
        {
            gameEngine.ContinueGame();
        }
            
    }

    public void MusicSoundChange()
    {
        audioSource.volume = musicSoundSlider.value;
    }

    #endregion

    #region help

    public void ShowHelp()
    {
        helpScreen = 1;

        ShowHelpVideo();

        optionsMainCanvas.gameObject.SetActive(false);

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
            bgObject.gameObject.SetActive(false);

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

        if(startGameCanvas)
        {
            startGameCanvas.gameObject.SetActive(false);
        }
        /*
        if (gameOverCanvas)
        {
            gameOverCanvas.gameObject.SetActive(false);
        }
        */
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

        if (startGameCanvas)
        {
            startGameCanvas.gameObject.SetActive(true);
        }
        /*
        if (gameOverCanvas)
        {
            gameOverCanvas.gameObject.SetActive(true);
        }
        */
        helpVideo.gameObject.SetActive(false);
        bgObject.gameObject.SetActive(true);
        helpImage.gameObject.SetActive(true);
    }

    #endregion
}
