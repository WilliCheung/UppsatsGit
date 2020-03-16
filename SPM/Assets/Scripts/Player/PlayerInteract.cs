using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour{

    //Author: Patrik Ahlgren
    [SerializeField] private Text interactText;
    [SerializeField] private Image textBackground;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private Transform interaction;

    private float distanceToTarget = 3f;
	
    private void Awake(){
        interactText = GameObject.Find("InteractionText").GetComponent<Text>();
        textBackground = GameObject.Find("TextBackground").GetComponent<Image>();
    }

    private void Start() {
        interaction = GameObject.Find("Player").transform.GetChild(2);
        interaction.position -= new Vector3(0, 0, 0.1f);
    }

    private void LateUpdate() {
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, distanceToTarget, layerMask);
        if (interactText != null) {
            interactText.enabled = false;
        }
        if (textBackground != null) {
            textBackground.enabled = false;
        }
        try {
            if (hit.transform.gameObject.tag == "InteractableObject") {
                if (interactText != null) {
                    interactText.enabled = true;
                }
                if (textBackground != null) {
                    textBackground.enabled = true;
                }
                if (GameController.Instance.PlayerIsInteracting) {
                    hit.transform.GetComponent<InteractableObject>().Interact();
                    GameController.Instance.PlayerIsInteracting = false;
                }
            }
        } catch (System.NullReferenceException) {

        }

        Debug.DrawLine(interaction.position, hit.point, Color.green);
    }

}
