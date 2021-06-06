using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingAnimal : MonoBehaviour
{

    public float walkingSpeed=0.12f;
    private Vector2 animalPosition= new Vector2();

    private GameEngine gameEngine;

    // Start is called before the first frame update
    void Start()
    {
        animalPosition = transform.position;
        gameEngine = FindObjectOfType<GameEngine>();
    }

    // Update is called once per frame
    void Update()
    {
        animalPosition.x -= walkingSpeed * gameEngine.speedFactor;
        transform.position = animalPosition;
    }
}
