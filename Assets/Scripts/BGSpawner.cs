using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSpawner : MonoBehaviour
{

    public GameObject bgoNearFG;
    public GameObject bgoFG;
    public GameObject bgoFGPath;
    public GameObject bgoMG;
    public GameObject bgoBG;


    public GameObject initiateObject(string bgoName, Vector2 objPosition)
    {
        GameObject createdObject=null;
        Debug.Log("xxx:" + bgoName);

        switch (bgoName)
        {
            case "near_fg":
                Debug.Log("test nearfg");
                if (bgoNearFG)
                {
                    Debug.Log("test nearfg222");
                    createdObject = Instantiate(bgoNearFG, objPosition, Quaternion.identity);
                    return createdObject;
                }
                break;
            case "fg":
                Debug.Log("test fg");
                if (bgoFG)
                {
                    Debug.Log("test fg222");
                    createdObject = Instantiate(bgoFG, objPosition, Quaternion.identity);
                    return createdObject;

                }
                break;
            case "fg_path":
                Debug.Log("test fgpath");
                if (bgoFGPath)
                {
                    Debug.Log("test fgpath222");
                    createdObject = Instantiate(bgoFGPath, objPosition, Quaternion.identity);
                    return createdObject;

                }
                break;
            case "mg":
                Debug.Log("test mg");
                if (bgoMG)
                {
                    Debug.Log("test mg222");
                    createdObject = Instantiate(bgoMG, objPosition, Quaternion.identity);
                    return createdObject;

                }
                break;
            case "bg":
                Debug.Log("test bg");
                if (bgoBG)
                {
                    Debug.Log("test bg222");
                    createdObject = Instantiate(bgoBG, objPosition, Quaternion.identity);
                    return createdObject;

                }
                break;
            default:
                return null;
        }
        return null;

    }
}
