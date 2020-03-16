using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAudioMethods : MonoBehaviour
{
    public void SlidingDoorsSound()
    {
        AudioController.Instance.Play_InWorldspace("SlidingDoors", gameObject);
    }
}
