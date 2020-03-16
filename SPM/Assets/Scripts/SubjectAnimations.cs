using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubjectAnimations : MonoBehaviour
{
    public Animator anim;

    public  bool isSuperHuman;
    public bool isAlien1;
    public bool isSleepDude;


    void Start()
    {
        if (isSuperHuman)
        {
            anim.SetBool("SuperHuman", true);
            AudioController.Instance.Play_InWorldspace_WithTag("LabBubbling", "SuperHuman");
        }else if (isAlien1)
        {
            anim.SetBool("Alien1", true);
            AudioController.Instance.Play_InWorldspace("AlienTalk", gameObject);
        }
        else if (isSleepDude)
        {
            anim.SetBool("SleepDude", true);
            AudioController.Instance.Play_InWorldspace("Snore", gameObject);
        }
    }

    // Update is called once per frame
    
}
