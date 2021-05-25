using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingAnimal : MonoBehaviour
{

    public float walkingSpeed=0.09f;
    private Vector2 animalPosition= new Vector2();

    // Start is called before the first frame update
    void Start()
    {
        animalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        animalPosition.x -= walkingSpeed;
        transform.position = animalPosition;
    }
}
