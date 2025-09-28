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
        if (inputs == null || currentFrame >= inputs.Count)
        {
            Destroy(gameObject);
            return;
        }

        PlayerInput input = inputs[currentFrame];

        // Horizontal movement
        rb.velocity = new Vector2(input.horizontal * speed, rb.velocity.y);

        // Jump when recorded
        if (input.jumpPressed && Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Variable jump height: release jump early if player did
        if (!input.jumpHeld && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        currentFrame++;
    }
}