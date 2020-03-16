using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowmotion:MonoBehaviour{
    //Author: Patrik Ahlgren

    [SerializeField] private float slowdownAmount;
    private bool canSlowmo = true;
    private Animator anim;

    private void Start()
    {
        anim = GameObject.Find("SliderSlowmotion").GetComponent<Animator>();
    }

    public void SlowTime() {
        if (!GameController.Instance.GameIsPaused) {
            if (!GameController.Instance.GameIsSlowmotion && canSlowmo) {               
                Time.timeScale = slowdownAmount;
                AudioController.Instance.SFXSetPitch(0.5f);
                GameController.Instance.GameIsSlowmotion = true;
            } else if (GameController.Instance.GameIsSlowmotion) {
                Time.timeScale = 1f;
                AudioController.Instance.SFXSetPitch(1f);
                GameController.Instance.GameIsSlowmotion = false;
            }
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
    }

    private void Update() {
        if (GameController.Instance.SlowmotionSlider.value == 100) {
            canSlowmo = true;
            anim.SetBool("IsFull", true);

        } else {
            canSlowmo = false;
            anim.SetBool("IsFull",false);
        }
        if (GameController.Instance.GameIsSlowmotion && GameController.Instance.SlowmotionSlider.value == 0) {
            GameController.Instance.GameIsSlowmotion = false;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            AudioController.Instance.SFXSetPitch(1f);
        }
        SlowmotionSlider();
    }

    private void SlowmotionSlider() {
        if (!GameController.Instance.GameIsPaused) {
            if (!GameController.Instance.GameIsSlowmotion && GameController.Instance.SlowmotionSlider.value < 100) {
                GameController.Instance.SlowmotionSlider.value += 10 * Time.unscaledDeltaTime; //10sek
            } else if (GameController.Instance.GameIsSlowmotion) {
                GameController.Instance.SlowmotionSlider.value -= 20 * Time.unscaledDeltaTime; //5sek
            }
        }
    }

}
