using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCloseDoor : MonoBehaviour

    //Author: William
{
    public Animator anim;
    public Animator door2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag ("Player"))
        {
            anim.SetBool("isOpen", false);

            if (door2)
            {
                door2.SetBool("isOpen", true);
            }
    

        }
    }
}
