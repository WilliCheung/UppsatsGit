using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaEnter : MonoBehaviour
{
    //Author: Teo
    public GameObject objectRef;

    void Start(){
        objectRef.SetActive(false);
    }

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Player")){
            objectRef.SetActive(true);
            
        }
    }
}
