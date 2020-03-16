using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {
    //Author: Patrik Ahlgren

    public float mouseSensitivity = 10;
    public Transform cameraTarget, player;
            
    public Vector2 cameraClamp = new Vector2(-40, 85);
    
    private float mouseX;
    private float mouseY;
    
    [Header("Collision Speed")]
    public float moveSpeed = 40;
    public float wallPush = 0.2f;

    [Header("Distances")]
    public float distanceFromPlayer = 2.4f;
    //public float closestDistanceToPlayer = 0.5f;

    [Header("Mask")]
    public LayerMask collisionMask;
    
    private void Start() {
        player = GameObject.Find("Player").transform;
        cameraTarget = GameObject.Find("CameraTarget").transform;
    }

    private void Update() {
        if (GameController.Instance.GameIsPaused){

        }
        else{
            CameraControl();
            ScrollWheelZoom();
        }

    }

    private void CameraControl() {
        if (GameController.Instance.GameIsPaused) {

        } else {
            mouseX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.unscaledDeltaTime;
            mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.unscaledDeltaTime;
        }      
        mouseY = Mathf.Clamp(mouseY, cameraClamp.x, cameraClamp.y);

        Vector3 targetRotation = new Vector3(mouseY, mouseX);
        transform.eulerAngles = targetRotation;
        player.rotation = Quaternion.Euler(0, mouseX, 0);

        CollisionCheck(cameraTarget.position - transform.forward * distanceFromPlayer);
    }

    private void ScrollWheelZoom() {
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f) {
            distanceFromPlayer -= 0.15f; //in
        } else if (scroll < 0f) {
            distanceFromPlayer += 0.15f; //out
        }
    }

    private void CollisionCheck(Vector3 returnPoint) {
        RaycastHit hit;

        if (Physics.Linecast(cameraTarget.position, returnPoint, out hit, collisionMask)) {
            Vector3 normal = hit.normal * wallPush;
            Vector3 point = hit.point + normal;

            transform.position = Vector3.Lerp(transform.position, point, moveSpeed * Time.deltaTime);
        } else {
            transform.position = cameraTarget.position - transform.forward * distanceFromPlayer;
        }
    }
}
