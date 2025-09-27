using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 70f;
    
    private Rigidbody2D rb;
    private float horizontal;
    private bool jumpPressed;
    public GameObject ghostPrefab;
    private PathRecorder2D recorder;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        recorder = GetComponent<PathRecorder2D>();
    }

    void Update()
    {
        // Player input
        horizontal = Input.GetAxisRaw("Horizontal");
        jumpPressed = Input.GetButtonDown("Jump");
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameObject ghost = Instantiate(ghostPrefab, transform.position, Quaternion.identity);
            ghost.GetComponent<GhostFollower2D>().Init(new List<PlayerState2D>(recorder.path));
        }
    }

    void FixedUpdate()
    {
        // Movement
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        // Jump
        if (jumpPressed && Mathf.Abs(rb.velocity.y) < 0.01f) // simple grounded check
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

}
