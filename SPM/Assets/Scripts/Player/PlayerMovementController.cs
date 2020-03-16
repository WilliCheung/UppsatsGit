using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour{
    //Author: Patrik Ahlgren

    public bool IsIdle;
    public bool Jumped;

    [Header("Movementspeeds")]
    [SerializeField] private float movementSpeed = 14;
    [Tooltip("Denna används för att öka movmentspeed med pickups, per 0,1 ökas 10%")]
    [SerializeField] private float speedMultiplier;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private int extraJumps = 1;
    [SerializeField] private float fakeExtraGravity = 15;
    [Tooltip("The chance to play a jumpgrunt sound 1-100")]
    [SerializeField] private float jumpSoundPercentChance = 33;

    [Header("Dash")]
    [SerializeField] private float dashForce = 20;
    [SerializeField] private float nextTimeToDash = 2;
    [SerializeField] private float dashDuration = 0.5f;

    private int jumpCount;
    private float timeToDash;   
    private float distanceToGround;
    private bool isDashing;

    private Rigidbody rigidBody;
    private CapsuleCollider capsuleCollider;
    private BoxCollider groundCheck;
    private Vector2 velocity;

    void Start(){
        rigidBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        groundCheck = GetComponent<BoxCollider>();
        distanceToGround = groundCheck.bounds.extents.y;       
    }

    private void Update() {
        if (!GameController.Instance.GameIsPaused) {
            Jump();
        }    
    }
    private void FixedUpdate(){
        if (!GameController.Instance.GameIsPaused) {
            if (!isDashing) {
                Move();
            }
            FakeExtraGravity();
            if (Time.time <= timeToDash) {
                rigidBody.velocity = new Vector3(0, 0, 0);
                transform.position += (Camera.main.transform.forward * (dashForce * (1 + speedMultiplier))) * Time.deltaTime;
            } else {
                isDashing = false;
            }
        }      
    }

    private void Move() {
        if (IsGrounded()) {
            Jumped = false;
        }
        Vector2 movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        IsIdle = movementInput.magnitude == 0;
        if (IsGrounded() && !IsIdle) {
            AudioController.Instance.PlaySFX_Finish("Walking", 0.9f, 1f, 0, 0.3f, 0.4f);
            //Debug.Log("Walking!");
        } else {
        }
        movementInput *= (movementSpeed * (1 + speedMultiplier)) * Time.deltaTime;

        velocity = movementInput;

        transform.Translate(new Vector3(velocity.x, 0f, velocity.y));
    }


    private void Jump() {
        if (Input.GetButtonDown("Jump")) {
            if (jumpCount>0 || IsGrounded()) {
                jumpCount--;
                JumpSound();
                if (rigidBody.velocity.y > 0) {
                    rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, 0);
                } else {
                    rigidBody.velocity = new Vector3(0, 0, 0);
                }               
                rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            IsGrounded();
            Jumped = true;
        }
    }

    private void JumpSound() {
        int soundChance = Random.Range(1, 100);
        if (soundChance <= jumpSoundPercentChance) {
            AudioController.Instance.PlayRandomSFX("Jump1", "Jump2", "Jump3");
        }
    }

    public void Dash() {
        isDashing = true;
        if(Time.time >= timeToDash+nextTimeToDash) {
            AudioController.Instance.PlaySFX("Dash", 0.93f, 1f);
            timeToDash = Time.time + dashDuration;
        }
    }

    public bool IsGrounded() {
        if (Physics.Raycast(transform.position, Vector3.down, distanceToGround + 0.1f)) {
            jumpCount = extraJumps;
            return true;
        } else return false;           
    }

    public void SpeedMultiplier(float speedDuration, float speedChange) {
        StartCoroutine(SpeedChange(speedDuration, speedChange));
    }

    private IEnumerator SpeedChange(float speedDuration, float speedChange) {
        speedMultiplier = speedChange;
        yield return new WaitForSeconds(speedDuration);
        speedMultiplier = 0;
    }

    private void FakeExtraGravity() {
        if (!IsGrounded()) {
            rigidBody.velocity += new Vector3(0, -fakeExtraGravity * Time.deltaTime, 0);
        }      
    }


}
