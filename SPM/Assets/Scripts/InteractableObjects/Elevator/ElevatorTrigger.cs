using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    //Main author: Marcus Söderberg
    //Secondary author: Fredrik

    private bool triggered;
    public GameObject Player;
    private GameObject parent;
    
    public bool GetTriggered() { return triggered; }


    private void Start()
    {
        Player = GameObject.Find("Player");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggered = true;
            parent = new GameObject();
            parent.transform.parent = transform;
            Player.transform.parent = parent.transform;
        }
        Debug.Log("Trigger on");
        if (other.CompareTag("Panel"))
        {
            triggered = true;
            other.gameObject.transform.parent = transform;
        }
        Debug.Log("Trigger on");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggered = false;
            Player.transform.parent = null;
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            
        }
        Debug.Log("Trigger off");
    }
}
