using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSpawner : MonoBehaviour
{
    public GameObject initiateObject(GameObject objToBeCreate, Vector2 objPosition)
    {
        GameObject createdObject = Instantiate(objToBeCreate, objPosition, Quaternion.identity);
        return createdObject;
    }
}
