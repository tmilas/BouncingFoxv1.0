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
    private float scrollDividend = 15f;
    private float destroyObjectFactor = 25f;
    private GameEngine gameEngine;
    private float currentSpeedFactor;


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
        gameEngine = FindObjectOfType<GameEngine>();
        currentSpeedFactor = gameEngine.speedFactor;

    }

    void Update () {

        if (!gameEngine.IsGameOver())
        {
            //test2
            //scroll the object according to the defined speed, loop when running out of width
            float tempNewPostion = Time.time * scrollSpeed;
            float newPosition = Mathf.Repeat(tempNewPostion, tileSizeX);
            Vector2 transformTempPos = new Vector2();

            if (gameEngine.speedFactor > 1)
            {
                if (currentSpeedFactor < gameEngine.speedFactor)
                    currentSpeedFactor +=  0.01f;
                 else
                    currentSpeedFactor = gameEngine.speedFactor;

            }
            else if(gameEngine.speedFactor<1)
            {
                if (currentSpeedFactor > gameEngine.speedFactor)
                    currentSpeedFactor -= 0.01f;
                else
                    currentSpeedFactor = gameEngine.speedFactor;
            }
            else
                currentSpeedFactor = gameEngine.speedFactor;

            if (gameObject.name.Equals("fg_path"))
            {
                //Debug.Log("fg_path speed:" + currentSpeedFactor);
                //Debug.Log("scroll speed:" + scrollSpeed);
                //Debug.Log("transformx=" + transform.position.x.ToString());
                //Debug.Log("startposx=" + startPosition.x.ToString());
                //Debug.Log("boundsx=" + bounds.size.x.ToString());
            }

            
            /*if (newPosition >= 0f && newPosition <= 1f && objCreationControl == false && isObjCreated == false)
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
                GameObject.Destroy(gameObject, scrollDividend / scrollSpeed * destroyObjectFactor);
            }
            else
            {
                if (!(newPosition >= 0f && newPosition <= 1f))
                    objCreationControl = false;
            }*/


            /*if (gameObject.name.StartsWith("fg_path"))
                Debug.Log("speedfactor=" + currentSpeedFactor.ToString());

            if (gameObject.name.StartsWith("fg_path"))
                Debug.Log("gameengspeedfactor=" + gameEngine.speedFactor.ToString());*/

            //transformTempPos = startPosition + Vector2.left * tempNewPostion * currentSpeedFactor;

            transformTempPos.x = transform.position.x;
            transformTempPos.y = transform.position.y;
            transformTempPos.x = transformTempPos.x + -1 * scrollSpeed * currentSpeedFactor;
            transform.position = transformTempPos;


            if (Mathf.Abs((transform.position.x - startPosition.x)) > (bounds.size.x - screenWidth) && isObjCreated == false)
            {
                isObjCreated = true;
                Vector2 newObjPosition = new Vector2(transform.position.x + bounds.size.x, startPosition.y);
                GameObject newBG = bgSpawner.initiateObject(backGroundObject, newObjPosition);
                if (gameObject.name.StartsWith("fg_path"))
                {
                    Debug.Log("main path set");
                    obstacleSpawner.SetMainPath(newBG);
                }
                GameObject.Destroy(gameObject, 60f);

            }

            /*if (transform.position.x<transformTempPos.x)
            {
                if (gameObject.name.StartsWith("fg_path"))
                    Debug.Log("transformx=" + transform.position.x.ToString());
                if (gameObject.name.StartsWith("fg_path"))
                    Debug.Log("transformTempPos=" + transformTempPos.x.ToString());
                startPosition.x = startPosition.x - Mathf.Abs((Mathf.Abs(transformTempPos.x) - Mathf.Abs(transform.position.x)));

                transformTempPos = startPosition + Vector2.left * tempNewPostion * currentSpeedFactor;

                //transformTempPos.x = transform.position.x - Mathf.Abs((Mathf.Abs(transformTempPos.x) - Mathf.Abs(transform.position.x)));
                if (gameObject.name.StartsWith("fg_path"))
                    Debug.Log("newtransformTempPos=" + transformTempPos.x.ToString());

                transform.position = transformTempPos;
            }
            else
                transform.position = transformTempPos;*/

        }

    }
}
