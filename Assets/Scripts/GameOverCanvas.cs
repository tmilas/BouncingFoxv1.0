using UnityEngine;
using UnityEngine.UI;

public class GameOverCanvas : MonoBehaviour
{
    public Text gameOverText;
    public Text tryAgainText;

    void Start()
    {
        LanguageSupport langSupport = FindObjectOfType<LanguageSupport>();

        gameOverText.text = langSupport.GetText("gameover");

        tryAgainText.text = langSupport.GetText("tryagain");
    }
}