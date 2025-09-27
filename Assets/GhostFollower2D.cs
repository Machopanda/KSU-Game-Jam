using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GhostFollower2D : MonoBehaviour
{
    private List<PlayerState2D> path;
    private int index = 0;
    public float moveSpeed = 5f; // horizontal speed

    private Rigidbody2D rb;

    public void Init(List<PlayerState2D> pathData)
    {
        path = pathData;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1f; // ensure gravity affects the ghost
    }

    void FixedUpdate()
    {
        if (path == null || index >= path.Count)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 targetPos = path[index].position;

        // Only move horizontally toward the target
        float step = moveSpeed * Time.fixedDeltaTime;
        Vector2 newPos = new Vector2(
            Mathf.MoveTowards(rb.position.x, targetPos.x, step),
            rb.position.y // keep vertical physics
        );

        rb.MovePosition(newPos);

        // Advance to next point if horizontally close enough
        if (Mathf.Abs(rb.position.x - targetPos.x) < 0.05f)
        {
            index++;
        }
    }
}