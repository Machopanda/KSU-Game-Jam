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
    
    private Rigidbody2D rb;
    private float horizontal;
    private bool jumpPressed;
    public Transform startPosition;
    public GameObject ghostPrefab;
    [HideInInspector]
    public List<PlayerInput> inputs = new List<PlayerInput>();


    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        // Player input
        horizontal = Input.GetAxisRaw("Horizontal");
        jumpPressed = Input.GetButtonDown("Jump");
        inputs.Add(new  PlayerInput(horizontal, jumpPressed));
        
        if (Input.GetKeyDown(KeyCode.G))
        { 
            GameObject ghost = Instantiate(ghostPrefab, startPosition.position, Quaternion.identity);
            ghost.GetComponent<GhostController>().Initialize(inputs, speed, jumpForce);
            startPosition = transform;
        }
        if (jumpPressed && Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }
}
