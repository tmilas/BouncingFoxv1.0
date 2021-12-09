using UnityEngine;
using UnityEngine.UI;

public class StartGameCanvas : MonoBehaviour
{
    public Text startGameText;

    void Start()
    {
        LanguageSupport langSupport = FindObjectOfType<LanguageSupport>();
        if (langSupport)
            startGameText.text = langSupport.GetText("startnewgame");
    }
}
