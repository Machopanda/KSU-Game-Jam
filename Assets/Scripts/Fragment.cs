using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragment : MonoBehaviour
{
    public PPManager effect;
    public float bobbingSpeed = 2f;
    public float bobbingHeight = 0.5f;
    [Range(0f,1f)]
    public float newIntensity;
    private Vector3 initialPosition;

    private void OnTriggerEnter2D(Collider2D other)
    {
        effect.SetVignetteIntensity(newIntensity);
        Destroy(gameObject);
    }
    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        float newY = initialPosition.y + Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight;
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
    }
}
