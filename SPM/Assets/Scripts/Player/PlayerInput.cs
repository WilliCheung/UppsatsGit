using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//TA BORT SEN

public class PlayerInput : MonoBehaviour {
    //Author: Patrik Ahlgren
    [SerializeField] private Slowmotion slowmotion;

    [SerializeField] private GameObject startTeleport;//TA BORT SEN
    [SerializeField] private GameObject secondTeleport;//TA BORT SEN
    [SerializeField] private GameObject thirdTeleport;//TA BORT SEN

    private PlayerShoot playerShoot;
    private PlayerMovementController playerMovementController;

    private WeaponAnimation weaponAnimation;
    private BaseWeapon selectedWeapon;
    private BaseWeapon lastSelectedWeapon;
    private BaseWeapon firstWeapon, secondWeapon, thirdWeapon;
    private float nextTimeToFire = 0f;
    public bool IsReloading = false;
    public bool InputIsFrozen;
    private bool skipShootDelayToSlowmotion;

    private void Awake() {
        GameController.Instance.Player = gameObject;
        IsReloading = false;
    }
    private void Start() {
        playerShoot = GetComponentInChildren<PlayerShoot>();

        playerMovementController = GetComponent<PlayerMovementController>();

        weaponAnimation = WeaponController.Instance.GetComponent<WeaponAnimation>();
        selectedWeapon = GameController.Instance.SelectedWeapon;
        lastSelectedWeapon = selectedWeapon;

        slowmotion = GameController.Instance.GetComponent<Slowmotion>();
        
          
    }


    private void Update() {
        if (!GameController.Instance.GameIsPaused) {
            if (!InputIsFrozen) {
                ReloadWeaponInput();
                ReloadSequence();
                ShootWeaponInput();
                SwitchWeaponInput();
                SlowmotionInput();
                InteractInput();
                DashInput();
            }           
        }      
        if (!InputIsFrozen) {
            InGameMenu();
        }       

        //Teleport();//TA BORT SEN
    }

    //private void Teleport() {//TA BORT SEN
    //    if (Input.GetKeyDown(KeyCode.T)) {
    //        SceneManager.LoadScene("Level2WhiteBox");
    //    }
    //    if (Input.GetKeyDown(KeyCode.I)) {
    //        try {
    //            transform.position = startTeleport.transform.position;
    //        } catch (System.Exception) {
    //            Debug.Log("FINNS INGEN DEFINERAD 'startTeleport'");
    //        }
    //    }
    //    if (Input.GetKeyDown(KeyCode.O)) {
    //        try {
    //            transform.position = secondTeleport.transform.position;
    //        } catch (System.Exception) {
    //            Debug.Log("FINNS INGEN DEFINERAD 'secondTeleport'");
    //        }
    //    }
    //    if (Input.GetKeyDown(KeyCode.P)) {
    //        try {
    //            transform.position = thirdTeleport.transform.position;
    //        } catch (System.Exception) {
    //            Debug.Log("FINNS INGEN DEFINERAD 'thirdTeleport'");
    //        }
    //    }
    //}//TA BORT SEN

    #region Reload Methods
    private void ReloadWeaponInput() {
        if (Input.GetButtonDown("Reload")) {
            ReloadWeapon();
        }
    }

    private void ReloadWeapon() {
        int ammoInClip = selectedWeapon.GetAmmoInClip();
        int maxAmmoInClip = selectedWeapon.GetMaxAmmoInClip();
        int totalAmmoLeft = selectedWeapon.GetTotalAmmoLeft();

        if (ammoInClip != maxAmmoInClip && totalAmmoLeft > 0 && !IsReloading) {
            PlayReloadSound();          
            int ammoSpent = maxAmmoInClip - ammoInClip;
            GameController.Instance.ReloadSlider.gameObject.SetActive(true);
            GameController.Instance.ReloadSlider.maxValue = selectedWeapon.GetReloadTime();
            IsReloading = true;
            weaponAnimation.ReloadWeaponAnimation(selectedWeapon.GetName());
        }
    }

    private void ReloadSequence() {
        if (IsReloading) {
            if (GameController.Instance.GameIsPaused) {

            } else {
                GameController.Instance.ReloadSlider.value += 1 * Time.unscaledDeltaTime;
                
            }          
        }
        if (GameController.Instance.ReloadSlider.value >= selectedWeapon.GetReloadTime()) {
            weaponAnimation.RaiseWeaponAnimation(selectedWeapon.GetName());
            int ammoInClip = selectedWeapon.GetAmmoInClip();
            int maxAmmoInClip = selectedWeapon.GetMaxAmmoInClip();
            int totalAmmoLeft = selectedWeapon.GetTotalAmmoLeft();
            int ammoSpent = maxAmmoInClip - ammoInClip;
            GameController.Instance.ReloadSlider.value = 0;
            FinishReload(ammoInClip, totalAmmoLeft, ammoSpent);
            GameController.Instance.UpdateSelectedWeapon_AmmoText();
            GameController.Instance.ReloadSlider.gameObject.SetActive(false);
            IsReloading = false;
        }
    }
    private void FinishReload(int ammoInClip, int totalAmmoLeft, int ammoSpent) {
        if (ammoSpent > totalAmmoLeft) {
            selectedWeapon.SetAmmoInClip(ammoInClip + totalAmmoLeft);
            selectedWeapon.SetTotalAmmoLeft(0);
            return;
        }
        selectedWeapon.SetAmmoInClip(selectedWeapon.GetMaxAmmoInClip());
        selectedWeapon.SetTotalAmmoLeft(totalAmmoLeft - ammoSpent);
        StopAllCoroutines();
    }

    public void AbortReload() {
        IsReloading = false;
        GameController.Instance.ReloadSlider.value = 0;
        GameController.Instance.ReloadSlider.gameObject.SetActive(false);
        StopReloadSound();
    }

    private void PlayReloadSound() {
        if (!GameController.Instance.GameIsSlowmotion) {
            string name = selectedWeapon.GetName();
            switch (name) {
                case "Rifle":
                    AudioController.Instance.PlaySFX("Rifle_Reload", 0.95f, 1f);
                    break;
                case "Shotgun":
                    AudioController.Instance.PlaySFX("Shotgun_Reload", 0.95f, 1f);
                    break;
                case "Rocket Launcher":
                    AudioController.Instance.PlaySFX("RocketLauncher_Reload", 0.95f, 1f);
                    break;
                default:
                    Debug.LogWarning("Couldn't find reload sound for weapon: " + name + "!");
                    break;
            }
        }
    }

    private void StopReloadSound() {
        AudioController.Instance.Stop("Rifle_Reload");
        AudioController.Instance.Stop("Shotgun_Reload");
        AudioController.Instance.Stop("RocketLauncher_Reload");
    }

    #endregion

    #region Shoot Method
    private void ShootWeaponInput() {
        if (!IsReloading) {
            if (Input.GetButton("Fire1") && GameController.Instance.SelectedWeapon.GetAmmoInClip() == 0) {
                ReloadWeapon();
                return;
            }
            if (!GameController.Instance.GameIsSlowmotion) {
                skipShootDelayToSlowmotion = true;
            }
            if (GameController.Instance.GameIsSlowmotion && Time.time <= nextTimeToFire && skipShootDelayToSlowmotion) {
                nextTimeToFire = Time.time + 1f / selectedWeapon.GetFireRate();
                skipShootDelayToSlowmotion = false;
            }
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire) {
                nextTimeToFire = Time.time + 1f / selectedWeapon.GetFireRate();
                playerShoot.StartShooting(selectedWeapon);
            }
        }
    }
    #endregion

    #region SwitchWeapon Methods
    private void SwitchWeaponInput() {
        selectedWeapon = GameController.Instance.SelectedWeapon;
        try {
            GetWeaponFromGameController(ref firstWeapon, 0);
            GetWeaponFromGameController(ref secondWeapon, 1);
            GetWeaponFromGameController(ref thirdWeapon, 2);
        } catch (System.ArgumentOutOfRangeException) {

        }
        if (Input.GetButtonDown("Weapon1") && firstWeapon != null || Input.GetAxis("Weapon1") > 0 && firstWeapon != null) {
            AbortReload();
            if (selectedWeapon != firstWeapon){
                SwitchWeaponAnimation(firstWeapon);       
                GameController.Instance.SelectedWeapon = firstWeapon;
            }
        }
        if (Input.GetButtonDown("Weapon2") && secondWeapon != null || Input.GetAxis("Weapon2") < 0 && secondWeapon != null) {
            AbortReload();
            if (selectedWeapon != secondWeapon) {
                SwitchWeaponAnimation(secondWeapon);
                GameController.Instance.SelectedWeapon = secondWeapon;
            }
        }
        if (Input.GetButtonDown("Weapon3") && thirdWeapon != null || Input.GetAxis("Weapon3") > 0 && thirdWeapon != null) {
            AbortReload();
            if (selectedWeapon != thirdWeapon) {
                SwitchWeaponAnimation(thirdWeapon);
                GameController.Instance.SelectedWeapon = thirdWeapon;
            }
        }
        GameController.Instance.UpdateSelectedWeapon();
    }

    private BaseWeapon GetWeaponFromGameController(ref BaseWeapon weapon, int i) {
        if (GameController.Instance.PlayerWeapons[i] != null) {
            return weapon = GameController.Instance.PlayerWeapons[i];
        } else {
            return null;
        }
    }

    public void SwitchWeaponAnimation(BaseWeapon switchedWeapon) {
        weaponAnimation.LowerWeaponAnimation(lastSelectedWeapon.GetName());
        weaponAnimation.RaiseWeaponAnimation(switchedWeapon.GetName());
        WeaponSwitchSound(switchedWeapon);
        lastSelectedWeapon = switchedWeapon;
    }

    public void WeaponSwitchSound(BaseWeapon switchedWeapon) {
        string weaponName = switchedWeapon.GetName();
        switch (weaponName){
            case "Rifle":
                AudioController.Instance.PlaySFX("RifleWeaponSwitch", 0.95f, 1f);
                break;
            case "Shotgun":
                AudioController.Instance.PlaySFX("ShotgunWeaponSwitch", 0.95f, 1f);
                break;
            case "Rocket Launcher":
                AudioController.Instance.PlaySFX("RocketLWeaponSwitch", 0.95f, 1f);
                break;
        }
    }

    #endregion

    #region Slowmotion Method
    private void SlowmotionInput() {
        if (Input.GetButtonDown("Slowmotion")) {
            slowmotion.SlowTime();
        }
    }
    #endregion

    #region Interaction Method
    private void InteractInput() {
        if (Input.GetButtonDown("Interact")) {
            GameController.Instance.PlayerIsInteracting = true;
            Debug.Log("Player tried to interact");
        } else { GameController.Instance.PlayerIsInteracting = false; }
    }
    #endregion

    #region Dash Method
    private void DashInput() {
        if (Input.GetButtonDown("Dash")) {
            playerMovementController.Dash();
            //playerShoot.Melee();
        }
    }
    #endregion

    #region Menu Method
    private void InGameMenu() {
        if (Input.GetButtonDown("Menu")) {
            try {
                GameObject menucontroller = GameObject.Find("MenuController");
                if (menucontroller.GetComponent<MenuController>().InGameMenuActive) {
                    menucontroller.GetComponent<MenuController>().DeactivateMenu();
                } else {
                    menucontroller.GetComponent<MenuController>().ActivateMenu();
                }
            } catch (System.Exception) {
                Debug.Log("FINNS INGEN DEFINERAD 'MenuController'");
            }
        }
    }
    #endregion

}
