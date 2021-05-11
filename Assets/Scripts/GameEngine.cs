using UnityEngine;

public class GameEngine : MonoBehaviour
{
    public int totalPoints = 0;

    void Start()
    {

    }

    void Update()
    {
        totalPoints = Mathf.FloorToInt(Time.timeSinceLevelLoad);
    }
}
