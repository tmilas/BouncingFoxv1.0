using System.IO;
using UnityEngine;

public class StorageEngine : MonoBehaviour
{

    [Header("File properties")]
    public string fileName = "JumpingFoxProperties.txt";
    private string filePath = "";

    private void Awake()
    {

        /* int numGameSessions = FindObjectsOfType<StorageEngine>().Length;

         if (numGameSessions > 1)
         {
             Destroy(gameObject);
         }
         else
         {
             DontDestroyOnLoad(gameObject);
         }*/
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SafeCreateDirectory(Application.persistentDataPath);

        filePath = Application.persistentDataPath + "/" + fileName;

    }

    private void SafeCreateDirectory(string path)
    {
        //Generate if you don't check if the directory exists
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public void SaveData(string data)
    {
        //Data storage
        var Writer = new StreamWriter(filePath);
        Writer.Write(data);
        Writer.Flush();
        Writer.Close();
    }

    public string LoadData()
    {
        //Data acquisition
        if(File.Exists(filePath))
        { 
            var reader = new StreamReader(filePath);
            string data = reader.ReadToEnd();
            reader.Close();
            return data;
        }

        return "";
    }
}