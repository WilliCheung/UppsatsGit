using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAmbience : MonoBehaviour
{

    void Start()
    {
        AudioController.Instance.Play("Space Ambience");
        AudioController.Instance.Play_Delay("RandomAmbience", 15f, 30f);
        AudioController.Instance.Play_InWorldspace_WithTag("LabBubbling", "SuperHuman");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
