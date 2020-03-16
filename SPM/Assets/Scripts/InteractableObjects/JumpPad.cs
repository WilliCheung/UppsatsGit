using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    //Author: Marcus Söderberg
    public float jumpForce;
    public float forwardForce;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "InteractionPlayer")
        {
            other.transform.parent.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce);
            other.transform.parent.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * forwardForce);
            AudioController.Instance.Play_InWorldspace("Jumppad", gameObject, 0.95f, 1.0f);

        }
    }


}
    