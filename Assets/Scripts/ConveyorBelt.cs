using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ConveyorBelt : MonoBehaviour
{
    public float speed = 5f;
    public LayerMask layerMask;
    public bool isRight;
    private BoxCollider2D box;

    void Awake()
    {
        box = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate() // physics update
    {
        Vector2 direction = isRight ? transform.right : -transform.right;
        Vector2 movement = direction * speed * Time.fixedDeltaTime;

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            box.bounds.center,
            box.bounds.size,
            transform.eulerAngles.z,
            layerMask
        );

        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject) continue;

            Rigidbody2D rb = hit.attachedRigidbody;
            if (rb != null)
            {
                // If kinematic, MovePosition is smooth & ignores forces
                if (rb.bodyType == RigidbodyType2D.Kinematic)
                {
                    rb.MovePosition(rb.position + movement);
                }
                else
                {
                    // Dynamic rigidbody -> apply force
                    rb.AddForce(direction * speed, ForceMode2D.Force);
                }
            }
            else
            {
                // fallback for non-rigidbody objects
                hit.transform.Translate(movement, Space.World);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (box == null) box = GetComponent<BoxCollider2D>();

        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(box.bounds.center, Quaternion.Euler(0, 0, transform.eulerAngles.z), Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, box.bounds.size);
    }
}