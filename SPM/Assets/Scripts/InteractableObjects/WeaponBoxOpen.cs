using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBoxOpen : MonoBehaviour
{
    [SerializeField] private Animator anim;
    public GameObject p1;
    public GameObject p2;
    public GameObject p3;
    public GameObject p4;
    public GameObject aLight;

    private void Start()
    {
        //p1.Stop();
        //p2.Stop();
        //p3.Stop();
        
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetBool("isOpening", true);
            AudioController.Instance.Play_InWorldspace("Box", gameObject);
            Collider col = GetComponent<Collider>();
            col.enabled = false;
            aLight.SetActive(true);

            p1.SetActive(true);
            p2.SetActive(true);
            p3.SetActive(true);
            p4.SetActive(true);


            //p1.Play();
            //p2.Play();
            //p3.Play();
            //p4.Play();


        }
    }

}
