using UnityEngine;

public class TestObject : MonoBehaviour
{

    public bool isBigger2x = false;
    public bool isBigger3x = false;
    public bool isSmaller2x = false;
    public bool isSmaller3x = false;
    public bool isFast2x = false;
    public bool isFast3x = false;
    public bool isSlow2x = false;
    public bool isSlow3x = false;
    public bool isJump2x = false;
    public bool isJump3x = false;
    public bool isGravity2x = false;
    public bool isGravity3x = false;
    public bool isObstacle2x = false;
    public bool isObstacle3x = false;
    public bool isScore2x = false;
    public bool isScore3x = false;
    public bool isInvincible = false;

    private GameEngine gameEngine;

    // Start is called before the first frame update
    void Start()
    {
        gameEngine = FindObjectOfType<GameEngine>();
    }

    // Update is called once per frame
    void Update()
    {
        CollectableItem collectableItem;

        if(isBigger2x)
        {
            collectableItem = new CollectableItem();
            collectableItem.itemDuration = 3;
            collectableItem.itemFactor = 2;
            collectableItem.collectableType = CollectableItem.CollectableType.BiggerCharacter2x;

            isBigger2x = false;
            gameEngine.SetGameBonus(collectableItem);
        }
        else if(isBigger3x)
        {
            collectableItem = new CollectableItem();
            collectableItem.itemDuration = 3;
            collectableItem.itemFactor = 3;
            collectableItem.collectableType = CollectableItem.CollectableType.BiggerCharacter3x;

            isBigger3x = false;
            gameEngine.SetGameBonus(collectableItem);
        }
        else if (isSmaller2x)
        {
            collectableItem = new CollectableItem();
            collectableItem.itemDuration = 3;
            collectableItem.itemFactor = 0.5f;
            collectableItem.collectableType = CollectableItem.CollectableType.SmallerCharacter2x;

            isSmaller2x = false;
            gameEngine.SetGameBonus(collectableItem);
        }
        else if (isSmaller3x)
        {
            collectableItem = new CollectableItem();
            collectableItem.itemDuration = 3;
            collectableItem.itemFactor = 0.3f;
            collectableItem.collectableType = CollectableItem.CollectableType.SmallerCharacter3x;

            isSmaller3x = false;
            gameEngine.SetGameBonus(collectableItem);
        }
        else if (isJump2x)
        {
            collectableItem = new CollectableItem();
            collectableItem.itemDuration = 3;
            collectableItem.itemFactor = 2;
            collectableItem.collectableType = CollectableItem.CollectableType.JumpPower2x;

            isJump2x = false;
            gameEngine.SetGameBonus(collectableItem);
        }
        else if (isJump3x)
        {
            collectableItem = new CollectableItem();
            collectableItem.itemDuration = 3;
            collectableItem.itemFactor = 3;
            collectableItem.collectableType = CollectableItem.CollectableType.JumpPower3x;

            isJump3x = false;
            gameEngine.SetGameBonus(collectableItem);
        }
        else if (isGravity2x)
        {
            collectableItem = new CollectableItem();
            collectableItem.itemDuration = 3;
            collectableItem.itemFactor = 0.8f;
            collectableItem.collectableType = CollectableItem.CollectableType.GravityIncrease2x;

            isGravity2x = false;
            gameEngine.SetGameBonus(collectableItem);
        }
        else if (isGravity3x)
        {
            collectableItem = new CollectableItem();
            collectableItem.itemDuration = 3;
            collectableItem.itemFactor = 0.7f;
            collectableItem.collectableType = CollectableItem.CollectableType.GravityIncrease3x;

            isGravity3x = false;
            gameEngine.SetGameBonus(collectableItem);
        }
        else if (isFast2x)
        {
            collectableItem = new CollectableItem();
            collectableItem.itemDuration = 10;
            collectableItem.itemFactor = 1.6f;
            collectableItem.collectableType = CollectableItem.CollectableType.FastMotion2x;

            isFast2x = false;
            gameEngine.SetGameBonus(collectableItem);
        }
        else if (isFast3x)
        {
            collectableItem = new CollectableItem();
            collectableItem.itemDuration = 10;
            collectableItem.itemFactor = 2.2f;
            collectableItem.collectableType = CollectableItem.CollectableType.FastMotion3x;

            isFast3x = false;
            gameEngine.SetGameBonus(collectableItem);
        }
        else if (isSlow2x)
        {
            collectableItem = new CollectableItem();
            collectableItem.itemDuration = 10;
            collectableItem.itemFactor = 0.7f;
            collectableItem.collectableType = CollectableItem.CollectableType.SlowMotion2x;

            isSlow2x = false;
            gameEngine.SetGameBonus(collectableItem);
        }
        else if (isSlow3x)
        {
            collectableItem = new CollectableItem();
            collectableItem.itemDuration = 10;
            collectableItem.itemFactor = 0.4f;
            collectableItem.collectableType = CollectableItem.CollectableType.SlowMotion3x;

            isSlow3x = false;
            gameEngine.SetGameBonus(collectableItem);
        }
        else if (isObstacle2x)
        {
            collectableItem = new CollectableItem();
            collectableItem.itemDuration = 3;
            collectableItem.itemFactor = 2;
            collectableItem.collectableType = CollectableItem.CollectableType.ObstacleIncrease2x;

            isObstacle2x = false;
            gameEngine.SetGameBonus(collectableItem);
        }
        else if (isObstacle3x)
        {
            collectableItem = new CollectableItem();
            collectableItem.itemDuration = 3;
            collectableItem.itemFactor = 3;
            collectableItem.collectableType = CollectableItem.CollectableType.ObstacleIncrease3x;

            isObstacle3x = false;
            gameEngine.SetGameBonus(collectableItem);
        }
        else if (isScore2x)
        {
            collectableItem = new CollectableItem();
            collectableItem.itemDuration = 3;
            collectableItem.itemFactor = 2;
            collectableItem.collectableType = CollectableItem.CollectableType.ScoreIncrease2x;

            isScore2x = false;
            gameEngine.SetGameBonus(collectableItem);
        }
        else if (isScore3x)
        {
            collectableItem = new CollectableItem();
            collectableItem.itemDuration = 3;
            collectableItem.itemFactor = 3;
            collectableItem.collectableType = CollectableItem.CollectableType.ScoreIncrease3x;

            isScore3x = false;
            gameEngine.SetGameBonus(collectableItem);
        }
        else if (isInvincible)
        {
            collectableItem = new CollectableItem();
            collectableItem.itemDuration = 3;
            collectableItem.itemFactor = 3;
            collectableItem.collectableType = CollectableItem.CollectableType.Invincibility5s;

            isInvincible = false;
            gameEngine.SetGameBonus(collectableItem);
        }
    }
}
