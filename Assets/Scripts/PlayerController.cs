using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerInput
{
    public float horizontal;
    public bool jumpPressed;
    public bool jumpHeld;

    public PlayerInput(float h, bool pressed, bool held)
    {
        horizontal = h;
        jumpPressed = pressed;
        jumpHeld = held;
    }
}

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 70f;
    private bool holding = false;
    public float pickupRange = 5f;
    public Transform holdPoint;
    public Transform dropPoint;
    private GameObject heldObject;
    private Rigidbody2D rb;
    private float horizontal;
    private bool jumpPressed;
    private bool jumpHeld;
    public Vector3 startPosition;
    public GameObject ghostPrefab;
    public LayerMask pickupMask;
    [HideInInspector] public List<PlayerInput> inputs = new List<PlayerInput>();

    // --- Jump Enhancements ---
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f;
    public float variableJumpMultiplier = 0.5f;

    private float coyoteTimer = 0f;
    private float jumpBufferTimer = 0f;

    private bool isGrounded => Mathf.Abs(rb.velocity.y) < 0.01f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Player input
        horizontal = Input.GetAxisRaw("Horizontal");
        jumpPressed = Input.GetButtonDown("Jump");
        jumpHeld = Input.GetButton("Jump");

        // Record input for ghost
        inputs.Add(new PlayerInput(horizontal, jumpPressed, jumpHeld));

        // Jump buffer
        if (jumpPressed)
            jumpBufferTimer = jumpBufferTime;
        else
            jumpBufferTimer -= Time.deltaTime;

        // Coyote timer
        if (isGrounded)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;

        // Jump logic
        if (jumpBufferTimer > 0f && coyoteTimer > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
        }

        // Variable jump height
        if (!jumpHeld && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpMultiplier);
        }

        // Ghost spawn
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameObject ghost = Instantiate(ghostPrefab, startPosition, Quaternion.identity);
            ghost.GetComponent<GhostController>().Initialize(inputs, speed, jumpForce);
            startPosition = transform.position;
            inputs.Clear();
        }

        // Pickup
        if (Input.GetKeyDown(KeyCode.E))
        {
            AttemptPickup();
        }

        // Horizontal movement
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        // Sprite facing
        if (horizontal != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(horizontal);
            transform.localScale = scale;
        }
    }

    public void AttemptPickup()
    {
        if (!holding)
        {
            Vector2 origin = transform.position;
            Vector2 dir = new Vector2(Mathf.Sign(transform.localScale.x), 0) * transform.right;
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, pickupRange, pickupMask);

            if (hit.collider != null && hit.collider.CompareTag("Box"))
            {
                heldObject = hit.collider.gameObject;
                Rigidbody2D rbHeld = heldObject.GetComponent<Rigidbody2D>();
                if (rbHeld != null)
                {
                    rbHeld.velocity = Vector2.zero;
                    rbHeld.angularVelocity = 0f;
                    rbHeld.bodyType = RigidbodyType2D.Kinematic;
                }
                heldObject.GetComponent<BoxCollider2D>().enabled = false;
                heldObject.transform.position = holdPoint.position;
                heldObject.transform.rotation = holdPoint.rotation;
                heldObject.transform.SetParent(holdPoint, true);
                holding = true;
            }
        }
        else
        {
            if (heldObject != null)
            {
                heldObject.transform.SetParent(null, true);
                heldObject.transform.position = dropPoint.position;
                heldObject.transform.rotation = dropPoint.rotation;

                Rigidbody2D rbHeld = heldObject.GetComponent<Rigidbody2D>();
                if (rbHeld != null)
                    rbHeld.bodyType = RigidbodyType2D.Dynamic;

                heldObject.GetComponent<BoxCollider2D>().enabled = true;
                heldObject = null;
                holding = false;
            }
        }
    }
}
