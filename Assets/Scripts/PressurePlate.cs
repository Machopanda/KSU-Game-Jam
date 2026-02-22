using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    [Header("References")]
    public Transform button;      
    public Vector3 pressedOffset; 
    public float moveSpeed = 5f;  

    [Header("Events")]
    public UnityEvent onPressed;     // Fires when plate goes down
    public UnityEvent onReleased;    // Fires when plate comes back up

    private Vector3 initialPos;
    private int objectsOnPlate = 0;
    private bool isPressed = false;

    void Start()
    {
        initialPos = button.localPosition;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody != null)
        {
            objectsOnPlate++;
            CheckState();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.attachedRigidbody != null)
        {
            objectsOnPlate--;
            CheckState();
        }
    }

    void CheckState()
    {
        if (objectsOnPlate > 0 && !isPressed)
        {
            isPressed = true;
            onPressed.Invoke();
        }
        else if (objectsOnPlate <= 0 && isPressed)
        {
            isPressed = false;
            onReleased.Invoke();
        }
    }

    void Update()
    {
        Vector3 targetPos = isPressed
            ? initialPos + pressedOffset
            : initialPos;

        button.localPosition = Vector3.Lerp(
            button.localPosition,
            targetPos,
            Time.deltaTime * moveSpeed
        );
    }

    public void Push()
    {
        print("poo poo");
    }
    public void UnPush()
    {
        print("pee pee");
    }
}