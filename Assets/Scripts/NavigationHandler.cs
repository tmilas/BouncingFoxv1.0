using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class NavigationHandler : MonoBehaviour
{
    public Text highScoreText;
    private StorageEngine storageEngine;

    // Start is called before the first frame update
    void Start()
    {
        if(highScoreText)
        {
            storageEngine = FindObjectOfType<StorageEngine>();
            string scoreText = storageEngine.LoadDataScore();
            if (scoreText == "")
                highScoreText.text = "High Score: 0";
            else
                highScoreText.text = "High Score: " + scoreText;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main Scene");
    }

    public void LoadLeaderBoard()
    {
        SceneManager.LoadScene("Leaderboard");
    }

    public void LoadStartScreen()
    {
        SceneManager.LoadScene("Start Screen");
    }

}
