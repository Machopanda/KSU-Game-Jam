using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{ 
    private bool playerOverlap;
    private bool doorOpen;
    public String nextScene;
    public UnityEvent OnOpen;
    public UnityEvent OnClose;
    public UnityEvent OnUse;
    public Animator anim;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerOverlap = true;
            print("Player On Door");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerOverlap = false;
            print("Player Off Door");
        }
    }

    private void Update()
    {
        if (playerOverlap && doorOpen && Input.GetKeyDown(KeyCode.E))
        {
            OnUse.Invoke();
            print("LEAVE");
        }
    }

    public void setDoor(bool open)
    {
        doorOpen = open;
        if (open)
        {
            OnOpen.Invoke();
            anim.SetFloat("direction", 1);
            anim.Play("DOOR", -1, 0);

        }
        else if (!open)
        {
            OnClose.Invoke();
            anim.SetFloat("direction", -1);
            anim.Play("bubbleAnim",-1,float.NegativeInfinity);
        }
    }
}
