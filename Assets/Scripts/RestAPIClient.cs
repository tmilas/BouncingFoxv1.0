using System.Collections;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class RestAPIClient : MonoBehaviour
{
    //private string geturi = "http://92.205.60.105:8181/api/leaderboard/players?gameid=gfox";
    //private string posturi = "http://92.205.60.105:8181/api/leaderboard/players";
    private string geturi = "http://todigames.com:8181/api/leaderboard/players?gameid=gfox";
    private string posturi = "http://todigames.com:8181/api/leaderboard/players";
    //private string geturi = "localhost:8181/api/leaderboard/players?gameid=gfox";
    //private string posturi = "localhost:8181/api/leaderboard/players";

    public delegate void OnLBGetSuccess(string test);
    public static OnLBGetSuccess OnSuccessLBGet;

    public delegate void OnLBPostSuccess(string data);
    public static OnLBPostSuccess OnSuccessLBPost;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.transform.gameObject);

    }

    public void GetLeaderBoard()
    {
        StartCoroutine(GetRequest(geturi));
    }

    IEnumerator GetRequest(string uri)
    {
        bool requestFinished = false;
        bool requestErrorOccurred = false;

        using UnityWebRequest request = UnityWebRequest.Get(uri) ;
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        requestFinished = true;
        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Something went wrong, and returned error: " + request.error);
            requestErrorOccurred = true;
            request.Dispose();
        }
        else
        {
            // Show results as text
            Debug.Log(request.downloadHandler.text);

            if (request.responseCode == 200)
            {
                Debug.Log("Request finished successfully!");
            }
            else if (request.responseCode == 401) // an occasional unauthorized error
            {
                Debug.Log("Error 401: Unauthorized. Resubmitted request!");
                //StartCoroutine(GetRequest(GenerateRequestURL(lastRequestURL, lastRequestParameters)));
                requestErrorOccurred = true;
                request.Dispose();
            }
            else
            {
                Debug.Log("Request failed (status:" + request.responseCode + ")");
                requestErrorOccurred = true;
                request.Dispose();
            }

            if (!requestErrorOccurred)
            {
                OnSuccessLBGet?.Invoke(request.downloadHandler.text);
                yield return null;
                // process results
            }
        }

    }

    public void SendPlayerScore(LeaderBoardEntry leaderBoardEntry)
    {
        StartCoroutine(PostRequest(posturi,leaderBoardEntry));
    }

    protected static byte[] GetBytes(string str)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        return bytes;
    }

    IEnumerator PostRequest(string uri,LeaderBoardEntry leaderBoardEntry)
    {
        bool requestFinished = false;
        bool requestErrorOccurred = false;

        Debug.Log("post json deserialize:" + JsonConvert.SerializeObject(leaderBoardEntry));
        byte[] bytes = GetBytes(JsonConvert.SerializeObject(leaderBoardEntry));
        using UnityWebRequest request = UnityWebRequest.Put(posturi, bytes);
        request.method = "POST";
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        requestFinished = true;
        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Something went wrong, and returned error: " + request.error);
            requestErrorOccurred = true;
            request.Dispose();

        }
        else
        {
            // Show results as text
            Debug.Log("result:" + request.downloadHandler.text);

            if (request.responseCode == 201)
            {
                Debug.Log("Request finished successfully!");
            }
            else if (request.responseCode == 401) // an occasional unauthorized error
            {
                Debug.Log("Error 401: Unauthorized. Resubmitted request!");
                //StartCoroutine(GetRequest(GenerateRequestURL(lastRequestURL, lastRequestParameters)));
                requestErrorOccurred = true;
                request.Dispose();

            }
            else
            {
                Debug.Log("Request failed (status:" + request.responseCode + ")");
                requestErrorOccurred = true;
                request.Dispose();

            }

            if (!requestErrorOccurred)
            {
                OnSuccessLBPost?.Invoke(leaderBoardEntry.playerscore.ToString());
                yield return null;
                // process results
            }
        }

    }


}
