using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEventAudio : MonoBehaviour{

    public void PlayEnemySound(string name)
    {
        AudioController.Instance.Play_InWorldspace(name, gameObject);
    }
}
