using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZoneScript : MonoBehaviour
{
    //Author: Teo
    [SerializeField] private PlayerRespawner playerRespawner;
    [SerializeField] private Animator anim;
 
    // Start is called before the first frame update
    void Start()
    {
        anim = GameObject.Find("Fade").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")){
            
            StartCoroutine(FadeOutDie());  
        }
        if (other.gameObject.CompareTag("Enemy")){
            other.GetComponent<Enemy>().InvokeDeath();
        }
    }

    private IEnumerator FadeOutDie() {
        AudioController.Instance.PlaySFX("Scream", 0.95f, 1f);
        yield return new WaitForSeconds(0.4f);
        anim.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1.3f);
        GameController.Instance.GetComponent<Timer>().AddToTimer(15);
        playerRespawner.RespawnMethod();
        anim.SetTrigger("FadeIn");
        yield return null;
    }

}
