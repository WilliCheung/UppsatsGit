using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera: MonoBehaviour {
    //Author: Patrik Ahlgren

    public float CameraXSensitivity { get; set; }
    public float CameraYSensitivity { get; set; }
    [SerializeField] private Transform cameraTarget, player;
    [SerializeField] private Vector2 cameraClamp = new Vector2(-85, 85);
    [SerializeField] private Transform weaponCamera;


    private float cameraX;
    private float cameraY;
    private Vector3 targetRotation;
    private Vector3 velocity = Vector3.zero;

    private void Start() {
        CameraXSensitivity = 60;
        CameraYSensitivity = 60;
        player = GameObject.Find("Player").transform;
        cameraTarget = GameObject.Find("CameraTarget").transform;
        weaponCamera = transform.GetChild(0).GetChild(0);
    }

    private void Update() {
        if (!GameController.Instance.GameIsPaused) {
            if (!GameController.Instance.Player.GetComponent<PlayerInput>().InputIsFrozen) {
                CameraControl();
            }          
        }
    }

    private void CameraControl() {
        if (GameController.Instance.GameIsPaused) {

        } else {
            cameraX += Input.GetAxis("Camera X") * CameraXSensitivity * Time.unscaledDeltaTime;
            cameraY -= Input.GetAxis("Camera Y") * CameraYSensitivity * Time.unscaledDeltaTime;
        }
        cameraY = Mathf.Clamp(cameraY, cameraClamp.x, cameraClamp.y);

        targetRotation = new Vector3(cameraY, cameraX);
        transform.eulerAngles = targetRotation;
        player.rotation = Quaternion.Euler(0, cameraX, 0);
        transform.position = cameraTarget.position;
    }
}
