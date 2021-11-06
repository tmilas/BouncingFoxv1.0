using UnityEngine;

public class Fox : MonoBehaviour
{
    //parameters
    private bool isAlive = true;
    private bool isPowerActive = false;
    private bool isInvincible = false;
    private bool isRunningFast = false;
    private bool isSliding = false;

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

    [SerializeField] AudioClip slideSound;
    [SerializeField] [Range(0, 1)] float slideSoundVolume = 0.8f;

    [SerializeField] AudioClip fastRunSound;
    [SerializeField] [Range(0, 1)] float fastRunSoundVolume = 0.8f;

    //Cache parameters
    Rigidbody2D myRigidBody;
    BoxCollider2D myBoxCollider;
    BoxCollider2D myFootCollider;
    Animator myAnimator;
    SpriteRenderer mySpriteRenderer;
    AudioSource myAudioSource;

    public GameObject footObject;

    //Global game cache parameters
    GameEngine gameEngine;

    private Color defaultColor;

    //box collider on run
    //  ofset: x:0.4, y:0.3
    //  size : x:2.5, y:4.7
    private Vector2 colliderDefaultOfset = new Vector2(0.4f, 0.3f);
    private Vector2 colliderDefaultSize  = new Vector2(2.5f, 4.7f);

    //box collider on slide
    //  offset: x: 0.7 y:-1
    //  size  : x: 4.8 y:1.8
    private Vector2 colliderSlidingOfset = new Vector2(0.7f, -1f);
    private Vector2 colliderSlidingSize = new Vector2(4.8f, 1.8f);

    //Touch tap or slide parameters
    private float minSwipeLength = 200f;
    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private Vector2 currentSwipe;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myFootCollider = footObject.GetComponent<BoxCollider2D>();
        myAudioSource = GetComponent<AudioSource>();

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
            Slide();
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

    public void SlidingEnd()
    {
        myBoxCollider.size = colliderDefaultSize;
        myBoxCollider.offset = colliderDefaultOfset;

        myAudioSource.Stop();
    }

    public void ContinueFox()
    {
        myAnimator.SetTrigger("isContinued");

        isAlive = true;
    }

    private void KeyDetect()
    {
        //For Phone Play - Touch Screen 
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);

            if (Input.touches[0].phase == TouchPhase.Began)
            {
                firstPressPos = new Vector2(t.position.x, t.position.y);

                keyPressedBeginTime = Time.time;
                isPowerActive = true;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended)
            {
                secondPressPos = new Vector2(t.position.x, t.position.y);
                currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                // Make sure it was a legit swipe, not a tap
                if (currentSwipe.magnitude < minSwipeLength)
                {
                    keyPressedEndTime = Time.time;
                    isKeyPressed = true;
                    isPowerActive = false;
                }

                currentSwipe.Normalize();

                if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    isSliding = true;
                }
            }
        }

        //For Laptop Play - Keyboard
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Jump
            keyPressedBeginTime = Time.time;
            isPowerActive = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            keyPressedEndTime = Time.time;
            isKeyPressed = true;
            isPowerActive = false;
        }
        else if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            //Slide
            isSliding = true;
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

                if(isRunningFast)
                {
                    myAnimator.SetTrigger("isRunFast"); 
                }
            }
        }

        jumpPrevTime = jumpBeginTime;
        jumpBeginTime = Time.time;

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

            //AudioSource.PlayClipAtPoint(jumpSound, Camera.main.transform.position, jumpSoundVolume);
            myAudioSource.PlayOneShot(jumpSound, jumpSoundVolume);
        }
    }

    public void SetJump()
    {
        isJumped = true;
    }

    public void Slide()
    {
        if (!myFootCollider.IsTouchingLayers(LayerMask.GetMask("Path")))
        {
            return;
        }

        if(isSliding)
        { 
            isSliding = false;

            myAnimator.SetTrigger("isSlide");
            myAnimator.SetBool("fastOrNormal", !isRunningFast);
            //arrange collider size and offset
            myBoxCollider.size = colliderSlidingSize;
            myBoxCollider.offset = colliderSlidingOfset;

            //AudioSource.PlayClipAtPoint(slideSound, Camera.main.transform.position, slideSoundVolume);
            myAudioSource.PlayOneShot(slideSound, slideSoundVolume);
        }
    }

    private void IsObstacleTouched()
    {
        if (!isInvincible && myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Obstacle")))
        {
            isAlive = false;
            gameEngine.SetGameOver(true);
            myAnimator.SetTrigger("isAlive");
            //AudioSource.PlayClipAtPoint(stunSound, Camera.main.transform.position, stunSoundVolume);
            myAudioSource.PlayOneShot(stunSound, stunSoundVolume);
        }
    }

    public void ScaleFox(float scaleFactor)
    {
        Vector3 foxPosition = transform.position;
        float changeFactor = (scaleFactor - transform.localScale.y);

        if(changeFactor>0)
            transform.position = new Vector3(foxPosition.x, foxPosition.y + 1, foxPosition.z);

        transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);

        //Debug.Log("after pos y: " + transform.position.y);
    }

    public bool isFoxInvincible()
    {
        return isInvincible;
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

    public void RunFast()
    {
        isRunningFast = true;
        myAnimator.SetTrigger("isRunFast");
        myAnimator.SetBool("fastOrNormal", false);

        //AudioSource.PlayClipAtPoint(fastRunSound, Camera.main.transform.position, fastRunSoundVolume);
        myAudioSource.clip = fastRunSound;
        myAudioSource.Play(); 
        myAudioSource.loop = true;
    }

    public void RunNormal()
    {
        isRunningFast = false;
        if(myAnimator)
        { 
            myAnimator.SetTrigger("isRunNormal");
            myAnimator.SetBool("fastOrNormal", true);
        }

        if(myAudioSource)
            myAudioSource.loop = false;
    }
}