using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodyScreenScript : MonoBehaviour{
    //Author: Patrik Ahlgren

    private Image bloodyScreen;
    private Image armorScreen;

    private void Awake() {
        bloodyScreen = GameObject.Find("BloodyScreen").GetComponent<Image>();
        bloodyScreen.color = new Color(bloodyScreen.color.r, bloodyScreen.color.g, bloodyScreen.color.b, 0);
        armorScreen = GameObject.Find("ArmorScreen").GetComponent<Image>();
        armorScreen.color = new Color(armorScreen.color.r, armorScreen.color.g, armorScreen.color.b, 0);
    }

    public void ShowHurtScreen(string screen) {
        StopAllCoroutines();
        if(screen == "Health") {
            StartCoroutine(FadeScreenDamage(0.5f, 1f, bloodyScreen));
        } else if (screen == "Armor") {
            StartCoroutine(FadeScreenDamage(0.5f, 1f, armorScreen));
        }
        
    }

    private IEnumerator FadeScreenDamage(float waitBeforeFade, float fadeTime, Image screen) {
        screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, 0.2f);
        yield return new WaitForSeconds(waitBeforeFade);
        while (screen.color.a > 0.0f) {
            screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, screen.color.a - (Time.unscaledDeltaTime / fadeTime));
            yield return null;
        }

    }

}
