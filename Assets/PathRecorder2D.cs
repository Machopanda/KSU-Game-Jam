using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerState2D
{
    public Vector2 position;
}

public class PathRecorder2D : MonoBehaviour
{
    public List<PlayerState2D> path = new List<PlayerState2D>();
    public float recordInterval = 0.05f; // smaller = smoother
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= recordInterval)
        {
            path.Add(new PlayerState2D { position = transform.position });
            timer = 0f;
        }
    }
}