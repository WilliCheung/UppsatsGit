using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1ArenaDoors : MonoBehaviour
{
    
    
    private GameObject[] doors = { null, null, null };
    // Start is called before the first frame update
    void Start()
    {
        doors[0] = transform.GetChild(0).gameObject;
        doors[1] = transform.GetChild(1).gameObject;
        doors[2] = transform.GetChild(2).gameObject;

        doors[0].SetActive(true);
        doors[1].SetActive(true);
        doors[2].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ArenaChange()
    {


        doors[2].SetActive(true);
        
       
    }
}
