using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour {

    public float scrollSpeed;
    public float screenWidth;
    public GameObject backGroundObject;

    private float tileSizeX;
    private Vector2 startPosition;
    private bool objCreationControl;
    private bool isObjCreated;
    private BGSpawner bgSpawner;
    private ObstacleSpawner obstacleSpawner;
    private Bounds bounds;


    void Start () {
        //get the width of the scrollable object
        SpriteRenderer[] srs = this.gameObject.GetComponentsInChildren<SpriteRenderer>();
        bounds = new Bounds(this.transform.position, Vector3.zero);

        foreach (SpriteRenderer renderer in srs)
        {
            bounds.Encapsulate(renderer.bounds);
        }

        
        tileSizeX = bounds.size.x - screenWidth;
        //get the startposition
        Transform startTransform = GetComponent<Transform>();
        startPosition.x = startTransform.position.x;
        startPosition.y = startTransform.position.y;
        objCreationControl = true;
        isObjCreated = false;
        bgSpawner=FindObjectOfType<BGSpawner>();
        obstacleSpawner = FindObjectOfType<ObstacleSpawner>();


    }

    void Update () {
        //test2
        //scroll the object according to the defined speed, loop when running out of width
        float tempNewPostion = Time.time * scrollSpeed;
        float newPosition = Mathf.Repeat(tempNewPostion, tileSizeX);
      
        if (newPosition >=0f && newPosition<=1f && objCreationControl==false && isObjCreated==false)
        {
            //Debug.Log(newPosition.ToString());
            //Debug.Log(gameObject.name);
            objCreationControl = true;
            isObjCreated = true;
            Vector2 newObjPosition = new Vector2(startPosition.x + bounds.size.x, startPosition.y);
            GameObject newBG = bgSpawner.initiateObject(backGroundObject, newObjPosition);
            if (gameObject.name.StartsWith("fg_path"))
            {
                Debug.Log("main path set");
                obstacleSpawner.SetMainPath(newBG);
            }
        }
        else
        {
            if (!(newPosition >= 0f && newPosition <= 1f))
                objCreationControl = false;
        }

        transform.position = startPosition + Vector2.left * tempNewPostion;
    }
}
