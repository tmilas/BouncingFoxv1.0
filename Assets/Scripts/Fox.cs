using UnityEngine;

public class Fox : MonoBehaviour
{
    //parameters
    private bool isAlive = true;
    private bool isPowerActive = false;

    //Jump parameters
    public float jumpSpeed = 5f;
    private float jumpConstantSpeed = 5f;

    private float jumpBeginTime = 0;
    private float jumpPrevTime = 0;
    private bool isTouched = false;
    private bool isPreviousTouched = false;

    //Key press parameters
    private bool isKeyPressed = false;
    private float keyPressedBeginTime = 0f;
    private float keyPressedEndTime = 0f;
    private float keyPressedMaxValue = 1f;
    private float keyPressedMinValue = 0.3f;

    //SFX
    [SerializeField] AudioClip jumpSound;
    [SerializeField] [Range(0, 1)] float jumpSoundVolume = 0.8f;

    [SerializeField] AudioClip stunSound;
    [SerializeField] [Range(0, 1)] float stunSoundVolume = 0.8f;

    //Cache parameters
    Rigidbody2D myRigidBody;
    BoxCollider2D myBoxCollider;
    Animator myAnimator;

    //Global game cache parameters
    GameEngine gameEngine;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();

        gameEngine = FindObjectOfType<GameEngine>();

        GetGameParameters();
    }

    void Update()
    {
        if (isAlive)
            IsObstacleTouched();

        if(isAlive)
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
            jumpConstantSpeed = gameEngine.jumpConstantSpeed;
            keyPressedMaxValue = gameEngine.keyPressedMaxValue;
            keyPressedMinValue = gameEngine.keyPressedMinValue;
        }
    }

    private void KeyDetect()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            keyPressedBeginTime = Time.time;
            isPowerActive = true;
        }

        if(isPowerActive)
        { 
            float keyPressedTime = Time.time - keyPressedBeginTime;
            if (keyPressedTime > keyPressedMaxValue)
                keyPressedTime = keyPressedMaxValue;
            //else if (keyPressedTime < keyPressedMinValue)
            //    keyPressedTime = keyPressedMinValue;

            gameEngine.SetPower(keyPressedTime, false);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            keyPressedEndTime = Time.time;
            isKeyPressed = true;
            isPowerActive = false;
        }
    }

    private void JumpAuto()
    {
        isPreviousTouched = isTouched;

        if (!myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Path")))
        {
            isTouched = false;
            myAnimator.ResetTrigger("isTouched");
            return;
        }

        isTouched = true;

        jumpPrevTime = jumpBeginTime;
        jumpBeginTime = Time.time;

        if (isTouched != isPreviousTouched)
        {
            myAnimator.SetTrigger("isTouched");
            
            if (isKeyPressed)
            {

                float keyPressedTime = keyPressedEndTime - keyPressedBeginTime;

                isKeyPressed = false;

                if (keyPressedEndTime >= jumpPrevTime && keyPressedEndTime <= jumpBeginTime)
                {
                    if (keyPressedTime > keyPressedMaxValue)
                        keyPressedTime = keyPressedMaxValue;
                    else if (keyPressedTime < keyPressedMinValue)
                        keyPressedTime = keyPressedMinValue;

                    jumpSpeed = jumpConstantSpeed + keyPressedTime * 10;

                    gameEngine.SetPower(keyPressedTime,true);
                }

                isKeyPressed = false;
            }
            else
            {
                jumpSpeed = jumpConstantSpeed;
            }

            //Set jump bonus factor
            jumpSpeed = jumpSpeed * gameEngine.GetJumpBonusFactor();

            Vector2 jumpVelocityToAdd = new Vector2(myRigidBody.velocity.x, jumpSpeed);
            myRigidBody.velocity = jumpVelocityToAdd;

            AudioSource.PlayClipAtPoint(jumpSound, Camera.main.transform.position, jumpSoundVolume);
        }
        else
        {
            myAnimator.ResetTrigger("isTouched");
        }
    }

    private void IsObstacleTouched()
    {
        if (myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Obstacle")))
        {
            isAlive = false;
            gameEngine.setGameOver(true);
            myAnimator.SetTrigger("isAlive");
            AudioSource.PlayClipAtPoint(stunSound, Camera.main.transform.position, stunSoundVolume);
        }
    }
}