using System.IO;
using UnityEngine;

public class StorageEngine : MonoBehaviour
{
    public static StorageEngine instance;

    [Header("File properties")]
    public string scoreFileName = "GFoxScore.txt";
    public string nickFileName = "GFoxNick.txt";
    public string lifeFileName = "GFoxLife.txt";
    public string postedScoreFileName = "GFoxPostedScore.txt";
    public string helpShowedFileName = "GFoxHelpShowed.txt";
    public string nickChangeCntFileName = "GFoxNickChangeCnt.txt";

    private string filePathScore = "";
    private string filePathNick = "";
    private string filePathLife = "";
    private string filePathPostedScore = "";
    private string filePathHelpShowed;
    private string filePathNickChangeCnt;

    public static string userIdSeperator = "%ZZZ%";

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        SafeCreateDirectory(Application.persistentDataPath);

        filePathScore = Application.persistentDataPath + "/" + scoreFileName;
        filePathNick = Application.persistentDataPath + "/" + nickFileName;
        filePathLife = Application.persistentDataPath + "/" + lifeFileName;
        filePathPostedScore = Application.persistentDataPath + "/" + postedScoreFileName;
        filePathHelpShowed = Application.persistentDataPath + "/" + helpShowedFileName;
        filePathNickChangeCnt = Application.persistentDataPath + "/" + nickChangeCntFileName;

        Debug.Log(Application.persistentDataPath);


        //Debug.Log("ffff");
        //Debug.Log(filePathNick);

    }

    private void SafeCreateDirectory(string path)
    {
        //Generate if you don't check if the directory exists
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public void SaveHelpShowed(string data)
    {
        //Data storage
        var Writer = new StreamWriter(filePathHelpShowed);
        Writer.Write(data);
        Writer.Flush();
        Writer.Close();
        Debug.Log("Help Showed Saved2");
    }

    public void SaveNickChangeCnt(string data)
    {
        //Data storage
        var Writer = new StreamWriter(filePathNickChangeCnt);
        Writer.Write(data);
        Writer.Flush();
        Writer.Close();
        Debug.Log("Nick Change Cnt Saved2");
    }

    public void SaveDataScore(string data)
    {
        //Data storage
        var Writer = new StreamWriter(filePathScore);
        Writer.Write(data);
        Writer.Flush();
        Writer.Close();
    }

    public void SaveDataPostedScore(string data)
    {
        //Data storage
        var Writer = new StreamWriter(filePathPostedScore);
        Writer.Write(data);
        Writer.Flush();
        Writer.Close();
    }

    public string LoadHelpShowed()
    {
        Debug.Log("Help Showed Load1");

        //Data acquisition
        if (File.Exists(filePathHelpShowed))
        {
            Debug.Log("Help Showed Load2");
            var reader = new StreamReader(filePathHelpShowed);
            string data = reader.ReadToEnd();
            reader.Close();
            return data;
        }
        Debug.Log("Help Showed Load3");

        return "";
    }

    public string LoadNickChangeCnt()
    {
        Debug.Log("Nick Change Cnt Load1");

        //Data acquisition
        if (File.Exists(filePathNickChangeCnt))
        {
            Debug.Log("Nick Change Cnt Load2");
            var reader = new StreamReader(filePathNickChangeCnt);
            string data = reader.ReadToEnd();
            reader.Close();
            return data;
        }
        Debug.Log("Nick Change Cnt Load3");

        return "";
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

    public string LoadDataPostedScore()
    {
        //Data acquisition
        if (File.Exists(filePathPostedScore))
        {
            var reader = new StreamReader(filePathPostedScore);
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
        Debug.Log("SaveDataNick:" + data + userIdSeperator + SystemInfo.deviceUniqueIdentifier);
        Writer.Write(data + userIdSeperator + SystemInfo.deviceUniqueIdentifier);
        Writer.Flush();
        Writer.Close();
    }

    public string LoadDataNick(bool getWithUniqueId)
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
            if (!getWithUniqueId)
                if (data.IndexOf(userIdSeperator)>0)
                    data = data.Substring(0, data.IndexOf(userIdSeperator));
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