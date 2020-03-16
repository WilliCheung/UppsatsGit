using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Author: Teo

public class AudioLevel1 : MonoBehaviour{
    [SerializeField] bool barSign;
    [SerializeField] bool gasLeak;
    [SerializeField] bool generator;
    void Start(){
        //AudioController.Instance.Play("RandomAmbience");
       // AudioController.Instance.Play_InWorldspace_WithTag("LabBubbling", "SuperHuman");

        if (barSign)
        {
            AudioController.Instance.Play_InWorldspace("BarSign", gameObject);
            AudioController.Instance.Play_InWorldspace("BarSignFlick", gameObject);
        }
        if (gasLeak)
        {
            AudioController.Instance.Play_InWorldspace("GasLeak", gameObject);
        }
        if (generator)
        {
            AudioController.Instance.Play_InWorldspace("GeneratorSound", gameObject);
        }

    }
    private void Update()
    {
        
    }




}
