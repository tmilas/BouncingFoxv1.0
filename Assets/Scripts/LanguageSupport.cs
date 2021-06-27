using System.Collections.Generic;
using UnityEngine;

public class LanguageSupport : MonoBehaviour
{
    public int language;
    private string langResourcesPath = "Language/";
    private string langTrText = "langTr";
    private string langEnText = "langEn";

    private Dictionary<string,string> langTr;
    private Dictionary<string, string> langEn;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        Debug.Log("Language awake");

        SetLanguage();

        FillDictionaries();
    }

    void Start()
    {
        Debug.Log("Language start");
    }

    public string GetText(string key)
    {
        string value = "";

        if(language == 0)
        {
            value = langTr[key];
        }
        else if(language ==1)
        {
            value = langEn[key];
        }

        return value;
    }

    private void FillDictionaries()
    {
        TextAsset myTrtxtData = (TextAsset)Resources.Load(langResourcesPath + langTrText);
        TextAsset myEntxtData = (TextAsset)Resources.Load(langResourcesPath + langEnText);

        langTr = new Dictionary<string, string>();
        langEn = new Dictionary<string, string>();

        string[] rows = myTrtxtData.text.Split('\n');

        foreach (string row in rows)
        {
            string[] items = row.Trim().Split('=');
            langTr.Add(items[0], items[1]);
        }

        rows = myEntxtData.text.Split('\n');

        foreach (string row in rows)
        {
            string[] items = row.Trim().Split('=');
            langEn.Add(items[0], items[1]);
        }
    }

    private void SetLanguage()
    {
        if (PlayerPrefs.HasKey("Language"))
        {
            language = PlayerPrefs.GetInt("Language");
        }
        else
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.English:
                    PlayerPrefs.SetInt("Language", 1);
                    language = 1;
                    break;
                case SystemLanguage.Turkish:
                    PlayerPrefs.SetInt("Language", 0);
                    language = 0;
                    break;
                default:
                    PlayerPrefs.SetInt("Language", 1);
                    language = 1;
                    break;
            }
        }
    }
}
