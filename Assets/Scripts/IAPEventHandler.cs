using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPEventHandler : MonoBehaviour
{
    public void UnlockGameGranted()
    {
        Debug.Log("Unlock Game Granted!!!");
    }

    public void UnlockGameNotGranted()
    {
        Debug.Log("Unlock Game Not Granted!!!");
    }
}
