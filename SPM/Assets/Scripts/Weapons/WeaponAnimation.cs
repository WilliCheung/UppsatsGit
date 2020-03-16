using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimation : MonoBehaviour{
    //Author: Patrik Ahlgren

    [Header("Weapon Switch")]
    [SerializeField] private float switchWeaponTime = 0.2f;

    [Header("Weapon Sway")]
    [SerializeField] private float swayAmount = 0.01f;
    [SerializeField] private float maxSwayAmount = 0.05f;
    [SerializeField] private float swaySmoothAmount = 10f;

    [Header("Weapon Bobbing")]  
    [SerializeField] private float bobbingSpeed = 0.2f;
    [SerializeField] private float bobbingAmount = 0.1f;

    private float xOffset, yOffset;
    private float xInit, yInit;
    private float timer = 0.0f;

    
    public GameObject Rifle { get; set; }
    public GameObject Shotgun { get; set; }
    public GameObject RocketLauncher{ get; set; }
    [Header("Weapons")]
    [SerializeField] private Transform rifleOutOfScreen, shotgunOutOfScreen, rocketLauncherOutOfScreen;
    [SerializeField] private Transform rifleInScreen, shotgunInScreen, rocketLauncherInScreen;
    [SerializeField] private ParticleSystem rifleFlash, shotgunFlash, rocketFlash;
    private Transform weaponCamera;
    private GameObject selectedWeapon;
    private PlayerMovementController playerMovementController;

    private float recoilValue;
    private float recoilDuration;
    private float recoilPercentage;
    private float startRecoilValue;
    private float startRecoilDuration;

    private bool isRecoiling = false;

    private void Awake() {
        weaponCamera = Camera.main.transform.GetChild(0);

        Rifle = weaponCamera.GetChild(0).gameObject;
        Shotgun = weaponCamera.GetChild(1).gameObject;
        RocketLauncher = weaponCamera.GetChild(2).gameObject;

        rifleOutOfScreen = weaponCamera.GetChild(3);
        shotgunOutOfScreen = weaponCamera.GetChild(4);
        rocketLauncherOutOfScreen = weaponCamera.GetChild(5);

        rifleInScreen = weaponCamera.GetChild(6);
        shotgunInScreen = weaponCamera.GetChild(7);
        rocketLauncherInScreen = weaponCamera.GetChild(8);

        rifleFlash = Rifle.transform.GetChild(0).GetComponent<ParticleSystem>();
        shotgunFlash = Shotgun.transform.GetChild(0).GetComponent<ParticleSystem>();
        rocketFlash = RocketLauncher.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();

        Rifle.transform.localPosition = rifleOutOfScreen.localPosition;
        Shotgun.transform.localPosition = shotgunOutOfScreen.localPosition;
        RocketLauncher.transform.localPosition = rocketLauncherOutOfScreen.localPosition;

        Rifle.transform.localRotation = rifleOutOfScreen.localRotation;
        Shotgun.transform.localRotation = shotgunOutOfScreen.localRotation;
        RocketLauncher.transform.localRotation = rocketLauncherOutOfScreen.localRotation;
    
    }

    private void Start() {
        foreach(Transform weapon in weaponCamera) {
            if(weapon.name == GameController.Instance.SelectedWeapon.GetName()) {
                selectedWeapon = weapon.gameObject;
            }
        }
        RaiseWeaponAnimation(selectedWeapon.name);
        playerMovementController = GameController.Instance.Player.GetComponent<PlayerMovementController>();

        xInit = InitialPositionOfWeapon().x;
        yInit = InitialPositionOfWeapon().y;

        xOffset = xInit;
        yOffset = yInit;
    }

    private void Update() {
        if (!GameController.Instance.GameIsPaused) {
            WeaponSway();
            WeaponBobbing();
            if (playerMovementController.Jumped && playerMovementController.IsGrounded()) {
                selectedWeapon.transform.localPosition = Vector3.Lerp(selectedWeapon.transform.localPosition, new Vector3(0.02f, -0.1f, -0.01f) + InitialPositionOfWeapon(), Time.deltaTime);
            }
            if (playerMovementController.Jumped) {
                selectedWeapon.transform.localPosition = Vector3.Lerp(selectedWeapon.transform.localPosition, new Vector3(-0.01f, 0.05f, 0) + InitialPositionOfWeapon(), Time.deltaTime * swaySmoothAmount);
            }
            
        }      
    }

    private Vector3 InitialPositionOfWeapon() {
        switch (selectedWeapon.name) {
            case "Rifle":
                return rifleInScreen.transform.localPosition;
            case "Shotgun":
                return shotgunInScreen.transform.localPosition;
            case "Rocket Launcher":
                return rocketLauncherInScreen.transform.localPosition;
            default:
                Debug.LogWarning("InitialPositionOfWeapon, weaponName not found");
                break;
        }
        return Vector3.zero;
    }

    #region WeaponSway and Bobbing Methods
    private void WeaponSway() {
        float swayX = Input.GetAxis("Camera X") * swayAmount;
        float swayY = Input.GetAxis("Camera Y") * swayAmount;

        swayX = Mathf.Clamp(swayX, -maxSwayAmount, maxSwayAmount);
        swayY = Mathf.Clamp(swayY, -maxSwayAmount, maxSwayAmount);

        Vector3 finalPosition = new Vector3(swayX, swayY, 0);
        selectedWeapon.transform.localPosition = Vector3.Lerp(selectedWeapon.transform.localPosition, finalPosition + InitialPositionOfWeapon(), Time.deltaTime * swaySmoothAmount);
    }

    private void WeaponBobbing() {
        if (!playerMovementController.IsGrounded()) {
            return;
        }
        float xPlayerMovement = 0f;
        float yPlayerMovement = 0f;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 calculatePosition = InitialPositionOfWeapon();

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0) {
            timer = 0f;
        } else {
            xPlayerMovement = Mathf.Sin(timer) / 2;
            yPlayerMovement = -Mathf.Sin(timer);

            if (!playerMovementController.IsIdle) {
                timer += bobbingSpeed * 1.2f;
                xPlayerMovement *= 1.5f;
                yPlayerMovement *= 1.5f;
            } else {
                timer += bobbingSpeed;
            }

            if (timer > Mathf.PI * 2) {
                timer = timer - (Mathf.PI * 2);
            }
        }

        if (xPlayerMovement != 0) {
            float translateChange = xPlayerMovement * bobbingAmount;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0f, 1f);
            translateChange = totalAxes * translateChange;

            calculatePosition.x = xOffset + translateChange;
        } else {
            calculatePosition.x = xOffset;
        }
        if (yPlayerMovement != 0) {
            float translateChange = yPlayerMovement * bobbingAmount;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0f, 1f);
            translateChange = totalAxes * translateChange;

            calculatePosition.y = yOffset + translateChange;
        } else {
            calculatePosition.y = yOffset;
        }
        selectedWeapon.transform.localPosition = Vector3.Lerp(selectedWeapon.transform.localPosition, calculatePosition + InitialPositionOfWeapon(), Time.deltaTime);
    }
    #endregion

    #region Raise/Lower Weapon Methods
    public void RaiseWeaponAnimation(string weapon) {
        string weaponName = weapon;
        switch (weaponName){
            case "Rifle":
                selectedWeapon = Rifle;  
                MoveWeaponPosition(Rifle, switchWeaponTime, Rifle.transform, rifleInScreen);         
                break;
            case "Shotgun":
                selectedWeapon = Shotgun;
                MoveWeaponPosition(Shotgun, switchWeaponTime, Shotgun.transform, shotgunInScreen);             
                break;
            case "Rocket Launcher":
                selectedWeapon = RocketLauncher;
                MoveWeaponPosition(RocketLauncher, switchWeaponTime, RocketLauncher.transform, rocketLauncherInScreen);               
                break;
            default:
                Debug.LogWarning("RaiseWeaponAnimation, weaponName not found");
                break;
        }

        //Debug.Log("Raising "+ weaponName + "!");
    }

    public void LowerWeaponAnimation(string weapon) {
        string weaponName = weapon;
        switch (weaponName) {
            case "Rifle":
                MoveWeaponPosition(Rifle, switchWeaponTime, Rifle.transform, rifleOutOfScreen);
                break;
            case "Shotgun":
                MoveWeaponPosition(Shotgun, switchWeaponTime, Shotgun.transform, shotgunOutOfScreen);
                break;
            case "Rocket Launcher":
                MoveWeaponPosition(RocketLauncher, switchWeaponTime, RocketLauncher.transform, rocketLauncherOutOfScreen);
                break;
            default:
                Debug.LogWarning("LowerWeaponAnimation, weaponName not found");
                break;
        }
        //Debug.Log("Lowering " + weaponName + "!");
    }
    #endregion

    #region Shoot/Reload Weapon Methods
    public void ShootWeaponAnimation(string weapon) {
        string weaponName = weapon;
        switch (weaponName) {
            case "Rifle":
                if (GameController.Instance.GameIsSlowmotion) {
                    RecoilShake(0.4f, 0.1f, Rifle);
                } else {
                    RecoilShake(1f, 0.15f, Rifle);
                }
                rifleFlash.Play();
                break;
            case "Shotgun":
                if (GameController.Instance.GameIsSlowmotion) {
                    RecoilShake(2.2f, 0.15f, Shotgun);
                } else {
                    RecoilShake(5f, 0.3f, Shotgun);
                }
                shotgunFlash.Play();
                break;
            case "Rocket Launcher":
                if (GameController.Instance.GameIsSlowmotion) {
                    RecoilShake(3.4f, 0.3f, RocketLauncher);
                } else {
                    RecoilShake(7f, 0.7f, RocketLauncher);
                }
                rocketFlash.Play();
                break;
            default:
                Debug.LogWarning("ShootWeaponAnimation, weaponName not found");
                break;
        }

        //Debug.Log("Shooting " + weaponName + "!");
    }

    public void ReloadWeaponAnimation(string weapon) {
        string weaponName = weapon;
        switch (weaponName) {
            case "Rifle":
                if (!GameController.Instance.GameIsSlowmotion) {
                    StartCoroutine(ReloadAnimationSequence(Rifle, GameController.Instance.ReloadSlider.maxValue, Rifle.transform, rifleOutOfScreen));
                }
                break;
            case "Shotgun":
                if (!GameController.Instance.GameIsSlowmotion) {
                    StartCoroutine(ReloadAnimationSequence(Shotgun, GameController.Instance.ReloadSlider.maxValue, Shotgun.transform, shotgunOutOfScreen));
                }
                break;
            case "Rocket Launcher":
                if (!GameController.Instance.GameIsSlowmotion) {
                    StartCoroutine(ReloadAnimationSequence(RocketLauncher, GameController.Instance.ReloadSlider.maxValue, RocketLauncher.transform, rocketLauncherOutOfScreen));
                }
                break;
            default:
                Debug.LogWarning("ReloadWeaponAnimation, weaponName not found");
                break;
        }

        //Debug.Log("Reloading " + weaponName + "!");
    }
    #endregion

    #region MoveWeapon Methods
    private void MoveWeaponPosition(GameObject weapon, float moveDuration, Transform startPos, Transform endPos) {
        StartCoroutine(MoveWeaponSequence(weapon, moveDuration, startPos, endPos));
    }

    private IEnumerator MoveWeaponSequence(GameObject weapon, float moveDuration, Transform startPos, Transform endPos) {
        Quaternion startRot = startPos.transform.localRotation;
        Quaternion endRot = endPos.transform.localRotation;

        for (float time = 0f; time < moveDuration; time += Time.unscaledDeltaTime) {
            float normalizedTime = time / moveDuration;
            weapon.transform.localRotation = Quaternion.Lerp(startRot, endRot, normalizedTime);
            weapon.transform.localPosition = Vector3.Lerp(startPos.localPosition, endPos.localPosition, normalizedTime);
            yield return null;
        }

        weapon.transform.localRotation = Quaternion.identity;
        weapon.transform.localPosition = endPos.localPosition;
    }

    private IEnumerator ReloadAnimationSequence(GameObject weapon, float moveDuration, Transform startPos, Transform endPos) {
        Quaternion startRot = startPos.transform.localRotation;
        Quaternion endRot = endPos.transform.localRotation;

        moveDuration -= 0.5f;

        for (float time = 0f; time < moveDuration; time += Time.deltaTime) {
            if (!GameController.Instance.Player.GetComponent<PlayerInput>().IsReloading) {
                time += 10;
            }
            float normalizedTime = time / moveDuration;
            if(GameController.Instance.SelectedWeapon.GetName() == "Rifle") {
                weapon.transform.localRotation = Quaternion.Lerp(startRot, endRot * new Quaternion(0, 0, 0.5f, -1), normalizedTime);
                weapon.transform.localPosition = Vector3.Lerp(startPos.localPosition, endPos.localPosition / 2.6f + new Vector3(0, 0.05f, 0.4f), normalizedTime);
            } else {
                weapon.transform.localRotation = Quaternion.Lerp(startRot, endRot * new Quaternion(-0.4f, -0.3f, 0.5f, 4f), normalizedTime);
                weapon.transform.localPosition = Vector3.Lerp(startPos.localPosition, endPos.localPosition / 2.4f + new Vector3(0f, 0, 0.3f), normalizedTime);
            }
            
            yield return null;
        }

        weapon.transform.localRotation = Quaternion.identity;
        weapon.transform.localPosition = endPos.localPosition;
    }

    #endregion

    #region Recoil Methods
    private void RecoilShake(float value, float duration, GameObject weapon) {
        recoilValue += value;
        startRecoilValue = recoilValue;
        recoilDuration = duration;
        startRecoilDuration = recoilDuration;

        if (!isRecoiling) {
            StartCoroutine(RecoilAnimationSequence(weapon));
        }
    }

    private IEnumerator RecoilAnimationSequence(GameObject weapon) {
        isRecoiling = true;
        Vector3 rotationAmount;

        while (recoilDuration > 0.01f) {
            if (weapon == RocketLauncher) {
                rotationAmount = new Vector3(-1, 0, 0) * recoilValue;
            } else {
                rotationAmount = new Vector3(-1, 0, -1) * recoilValue;
            }

            recoilPercentage = recoilDuration / startRecoilDuration;
            recoilValue = startRecoilValue * recoilPercentage;
            recoilDuration -= 1 * Time.deltaTime;

            weapon.transform.localRotation = Quaternion.Euler(rotationAmount);

            yield return null;
        }
        weapon.transform.localRotation = Quaternion.identity;

        isRecoiling = false;
    }
    #endregion


}
