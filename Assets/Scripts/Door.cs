using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    private bool playerOverlap;
    private bool doorOpen;
    public UnityEvent OnOpen;
    public UnityEvent OnClose;
    public UnityEvent OnUse;
    public Animator anim;
    private int triggers;
    public int requiredTriggers = 1;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerOverlap = true;
            Debug.Log("Player On Door");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerOverlap = false;
            Debug.Log("Player Off Door");
        }
    }

    private void Update()
    {
        if (playerOverlap && doorOpen && Input.GetKeyDown(KeyCode.E))
        {
            OnUse.Invoke();
            Debug.Log("LEAVE");
        }
    }

    private void setDoor(bool open)
    {
        if (doorOpen == open) return; // already in desired state

        doorOpen = open;

        if (open)
        {
            OnOpen.Invoke();
            anim.SetFloat("direction", 1);
            anim.Play("DOOR", -1, 0);
        }
        else
        {
            OnClose.Invoke();
            anim.SetFloat("direction", -1);
            anim.Play("DOOR", -1, float.NegativeInfinity);
        }
    }

    public void incrementTriggers()
    {
        triggers++;
        if (triggers >= requiredTriggers)
        {
            setDoor(true);
        }
    }

    public void decrementTriggers()
    {
        triggers = Mathf.Max(0, triggers - 1);
        if (triggers < requiredTriggers)
        {
            setDoor(false);
        }
    }

}