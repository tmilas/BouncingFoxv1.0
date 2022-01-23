using System.IO;
using UnityEngine;

public class StorageEngine : MonoBehaviour
{

    [Header("File properties")]
    public string scoreFileName = "GFoxScore.txt";
    public string nickFileName = "GFoxNick.txt";
    public string lifeFileName = "GFoxLife.txt";
    private string filePathScore = "";
    private string filePathNick = "";
    private string filePathLife = "";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SafeCreateDirectory(Application.persistentDataPath);

        filePathScore = Application.persistentDataPath + "/" + scoreFileName;
        filePathNick = Application.persistentDataPath + "/" + nickFileName;
        filePathLife = Application.persistentDataPath + "/" + lifeFileName;
        //Debug.Log("ffff");
        Debug.Log(filePathNick);

    }

    private void SafeCreateDirectory(string path)
    {
        //Generate if you don't check if the directory exists
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public void SaveDataScore(string data)
    {
        //Data storage
        var Writer = new StreamWriter(filePathScore);
        Writer.Write(data);
        Writer.Flush();
        Writer.Close();
    }

    public string LoadDataScore()
    {
        //Data acquisition
        if(File.Exists(filePathScore))
        { 
            var reader = new StreamReader(filePathScore);
            string data = reader.ReadToEnd();
            reader.Close();
            return data;
        }

        return "";
    }

    public void SaveDataNick(string data)
    {
        //Data storage
        var Writer = new StreamWriter(filePathNick);
        Writer.Write(data);
        Writer.Flush();
        Writer.Close();
    }

    public string LoadDataNick()
    {
        //Data acquisition
        Debug.Log("qqqqqqq");

        Debug.Log(filePathNick);
        if (File.Exists(filePathNick))
        {
            Debug.Log("yyyyy");

            var reader = new StreamReader(filePathNick);
            string data = reader.ReadToEnd();
            reader.Close();
            return data;
        }

        return "";
    }

    public void SaveLifeCount(string data)
    {
        //Data storage
        var Writer = new StreamWriter(filePathLife);
        Writer.Write(data);
        Writer.Flush();
        Writer.Close();
    }

    public string LoadLifeCount()
    {
        //Data acquisition
        if (File.Exists(filePathLife))
        {
            var reader = new StreamReader(filePathLife);
            string data = reader.ReadToEnd();
            reader.Close();
            return data;
        }

        return "";
    }
}