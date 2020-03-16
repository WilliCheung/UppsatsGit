using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {
    //Author: Teo

    public bool isPickup;
    public bool isGlobe;
    // Update is called once per frame
    void Update () {
        if (isPickup)
        {
            transform.Rotate(new Vector3(0, 60, 0) * Time.deltaTime);
        }
        if (isGlobe)
        {
            transform.Rotate(new Vector3(0, 0, 30) * Time.deltaTime);
        }
	}
}
