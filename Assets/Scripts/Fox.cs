using UnityEngine;

public class Fox : MonoBehaviour
{
    //parameters
    private bool isAlive = true;
    private bool isPowerActive = false;
    private bool isInvincible = false;

    //Jump parameters
    public float jumpSpeed = 5f;

    private float jumpBeginTime = 0;
    private float jumpPrevTime = 0;
    public bool isTouched = true;
    public bool isJumped = false;

    //Key press parameters
    public bool isKeyPressed = false;
    private float keyPressedBeginTime = 0f;
    private float keyPressedEndTime = 0f;
    private float keyPressedMaxValue = 0.8f;
    private float keyPressedMinValue = 0.3f;

    //SFX
    [SerializeField] AudioClip jumpSound;
    [SerializeField] [Range(0, 1)] float jumpSoundVolume = 0.8f;

    [SerializeField] AudioClip stunSound;
    [SerializeField] [Range(0, 1)] float stunSoundVolume = 0.8f;

    //Cache parameters
    Rigidbody2D myRigidBody;
    BoxCollider2D myBoxCollider;
    BoxCollider2D myFootCollider;
    Animator myAnimator;
    SpriteRenderer mySpriteRenderer;

    public GameObject footObject;

    //Global game cache parameters
    GameEngine gameEngine;


    private Color defaultColor;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myFootCollider = footObject.GetComponent<BoxCollider2D>();


        if (mySpriteRenderer)
            defaultColor = mySpriteRenderer.color;

        gameEngine = FindObjectOfType<GameEngine>();

        GetGameParameters();
    }

    void Update()
    {
        if (isAlive)
            IsObstacleTouched();

        if (isAlive)
        {
            KeyDetect();
            JumpAuto();
        }
    }

    private void GetGameParameters()
    {
        //Get general game parameters
        if (gameEngine)
        {
            keyPressedMaxValue = gameEngine.keyPressedMaxValue;
            keyPressedMinValue = gameEngine.keyPressedMinValue;
        }
    }

    private void KeyDetect()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                keyPressedBeginTime = Time.time;
                isPowerActive = true;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended)
            {
                keyPressedEndTime = Time.time;
                isKeyPressed = true;
                isPowerActive = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            keyPressedBeginTime = Time.time;
            isPowerActive = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            keyPressedEndTime = Time.time;
            isKeyPressed = true;
            isPowerActive = false;
        }

        if (isPowerActive)
        {
            float keyPressedTime = Time.time - keyPressedBeginTime;
            if (keyPressedTime > keyPressedMaxValue)
                keyPressedTime = keyPressedMaxValue;

            gameEngine.SetPower(keyPressedTime, false);
        }
    }

    private void JumpAuto()
    {
        if (!myFootCollider.IsTouchingLayers(LayerMask.GetMask("Path")))
        {
            if (isTouched)
                isJumped = true;

            isTouched = false;
            return;
        }
        else
        {
            //Debug.Log("istouched, isjumped: " + isJumped);
            isTouched = true;

            if (isJumped)
            {
                myAnimator.SetTrigger("isTouched");
                isJumped = false;
            }
        }

        jumpPrevTime = jumpBeginTime;
        jumpBeginTime = Time.time;

        //if (isJumped && myRigidBody.velocity.y==0)
        /*if (isJumped && !isTouched)
        {
            myAnimator.SetTrigger("isTouched");
            isJumped = false;
        }*/

        if (isKeyPressed && keyPressedEndTime >= jumpPrevTime && keyPressedEndTime <= jumpBeginTime)
        {
            float keyPressedTime = keyPressedEndTime - keyPressedBeginTime;

            isKeyPressed = false;
            //isJumped = true;

            if (keyPressedTime > keyPressedMaxValue)
                keyPressedTime = keyPressedMaxValue;
            else if (keyPressedTime < keyPressedMinValue)
                keyPressedTime = keyPressedMinValue;

            jumpSpeed = keyPressedTime * gameEngine.jumpSpeedFactor;

            gameEngine.SetPower(keyPressedTime, true);

            //Set jump bonus factor
            jumpSpeed = jumpSpeed * gameEngine.GetJumpBonusFactor();

            if (jumpSpeed > gameEngine.maxJumpLimit)
                jumpSpeed = gameEngine.maxJumpLimit;

            myAnimator.SetTrigger("isJumped");

            Vector2 jumpVelocityToAdd = new Vector2(myRigidBody.velocity.x, jumpSpeed);
            myRigidBody.velocity = jumpVelocityToAdd;

            AudioSource.PlayClipAtPoint(jumpSound, Camera.main.transform.position, jumpSoundVolume);
        }
    }

    private void IsObstacleTouched()
    {
        if (!isInvincible && myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Obstacle")))
        {
            isAlive = false;
            gameEngine.setGameOver(true);
            myAnimator.SetTrigger("isAlive");
            AudioSource.PlayClipAtPoint(stunSound, Camera.main.transform.position, stunSoundVolume);
        }
    }

    public void ScaleFox(float scaleFactor)
    {
        transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
    }

    public void InvincibleFox(bool value)
    {
        if (mySpriteRenderer)
        {
            isInvincible = value;

            if (isInvincible)
            {
                Color temp = defaultColor;
                temp.a = 0.5f;
                mySpriteRenderer.color = temp;

                Debug.Log("Invincible active");
                Physics2D.IgnoreLayerCollision(3, 7, true);
            }
            else
            {
                Debug.Log("Invincible inactive");
                mySpriteRenderer.color = defaultColor;
                Physics2D.IgnoreLayerCollision(3, 7, false);
            }

        }
    }
}