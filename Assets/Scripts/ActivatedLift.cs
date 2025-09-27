using UnityEngine;

public class ActivatedLift : MonoBehaviour
{
    public float liftDistance = 5f;
    public float speed = 2f;

    private float initialY;
    private bool movingUp = false;
    private bool movingDown = false;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.isKinematic = true;
        }

        initialY = transform.position.y;
    }

    void FixedUpdate()
    {
        float newY = transform.position.y;

        if (movingUp)
        {
            newY = Mathf.Min(transform.position.y + speed * Time.fixedDeltaTime, initialY + liftDistance);
            if (newY >= initialY + liftDistance) movingUp = false;
        }
        else if (movingDown)
        {
            newY = Mathf.Max(transform.position.y - speed * Time.fixedDeltaTime, initialY);
            if (newY <= initialY) movingDown = false;
        }

        rb.MovePosition(new Vector3(transform.position.x, newY, transform.position.z));
    }

    public void Lift()
    {
        movingUp = true;
        movingDown = false;
    }

    public void Lower()
    {
        movingDown = true;
        movingUp = false;
    }
}