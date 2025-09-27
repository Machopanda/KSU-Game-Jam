using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerInput
{
    public float horizontal;
    public bool jumpPressed;

    
    public PlayerInput(float h, bool j)
    {
        horizontal = h;
        jumpPressed = j;
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
    public Vector3 startPosition;
    public GameObject ghostPrefab;
    public LayerMask pickupMask;
    [HideInInspector] public List<PlayerInput> inputs = new List<PlayerInput>();



    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Player input
        horizontal = Input.GetAxisRaw("Horizontal");
        jumpPressed = Input.GetButtonDown("Jump");
        inputs.Add(new PlayerInput(horizontal, jumpPressed));
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameObject ghost = Instantiate(ghostPrefab, startPosition, Quaternion.identity);
            ghost.GetComponent<GhostController>().Initialize(inputs, speed, jumpForce);
            startPosition = transform.position;
            inputs.Clear();
        }

        if (jumpPressed && Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            AttemptPickup();
        }

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        if (horizontal != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(horizontal); // 1 for right, -1 for left
            transform.localScale = scale;
        }
    }

    public void AttemptPickup()
    {
        if (!holding)
        {
            Vector2 origin = transform.position;
            Vector2 dir = new Vector2(Mathf.Sign(transform.localScale.x), 0) * transform.right; // 1 = right, -1 = left
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, pickupRange, pickupMask);

            if (hit.collider != null && hit.collider.CompareTag("Box"))
            {
                heldObject = hit.collider.gameObject;

                Rigidbody2D rb = heldObject.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    // stop motion and make kinematic while held
                    rb.velocity = Vector2.zero;
                    rb.angularVelocity = 0f;
                    rb.bodyType = RigidbodyType2D.Kinematic;
                }
                heldObject.GetComponent<BoxCollider2D>().enabled = false;
                // parent to hold point
                heldObject.transform.position = holdPoint.position;
                heldObject.transform.rotation = holdPoint.rotation;
                heldObject.transform.SetParent(holdPoint, worldPositionStays: true);

                holding = true;
            }
        }
        else
        {
            if (heldObject != null)
            {
                // unparent and move to drop point
                heldObject.transform.SetParent(null, worldPositionStays: true);
                heldObject.transform.position = dropPoint.position;
                heldObject.transform.rotation = dropPoint.rotation;

                Rigidbody2D rb = heldObject.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.bodyType = RigidbodyType2D.Dynamic; // re-enable physics
                }
                heldObject.GetComponent<BoxCollider2D>().enabled = true;
                heldObject = null;
                holding = false;
            }
        }
    }
}