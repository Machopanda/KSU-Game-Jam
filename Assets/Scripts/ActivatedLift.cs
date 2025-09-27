using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedLift : MonoBehaviour
{
    public float liftDistance = 5f; // maximum distance to move up
    public float speed = 2f; // movement speed

    private float initialY;   // starting Y position
    private float targetY;    // current target position
    private bool movingUp = false;
    private bool movingDown = false;

    void Start()
    {
        initialY = transform.position.y;
        targetY = initialY;
    }

    void Update()
    {
        if (movingUp)
        {
            targetY = Mathf.Min(transform.position.y + speed * Time.deltaTime, initialY + liftDistance);
            transform.position = new Vector3(transform.position.x, targetY, transform.position.z);

            if (transform.position.y >= initialY + liftDistance)
                movingUp = false; // stop at max height
        }
        else if (movingDown)
        {
            targetY = Mathf.Max(transform.position.y - speed * Time.deltaTime, initialY);
            transform.position = new Vector3(transform.position.x, targetY, transform.position.z);

            if (transform.position.y <= initialY)
                movingDown = false; // stop at min height
        }
    }

    public void Lift()
    {
        movingUp = true;
        movingDown = false; // cancel downward movement
    }

    public void Lower()
    {
        movingDown = true;
        movingUp = false; // cancel upward movement
    }
}