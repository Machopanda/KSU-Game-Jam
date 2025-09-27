using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    private Rigidbody2D rb;

    private List<PlayerInput> inputs;
    private int currentFrame = 0;

    public void Initialize(List<PlayerInput> recordedInputs, float playerSpeed, float playerJumpForce)
    {
        inputs = new List<PlayerInput>(recordedInputs); // copy the list
        speed = playerSpeed;
        jumpForce = playerJumpForce;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // If there are no inputs or we've reached the end, destroy this ghost
        if (inputs == null || currentFrame >= inputs.Count)
        {
            Destroy(gameObject);
            return;
        }

        PlayerInput input = inputs[currentFrame];

        // Apply horizontal movement
        rb.velocity = new Vector2(input.horizontal * speed, rb.velocity.y);

        // Apply jump if grounded
        if (input.jumpPressed && Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        currentFrame++;
    }
}