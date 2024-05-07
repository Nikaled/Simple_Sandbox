using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController.Examples;
using KinematicCharacterController;
using Cinemachine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [SerializeField] public GameObject PlayerParent;
    [SerializeField] public KinematicCharacterMotor motor;
    [SerializeField] public ExampleCharacterController characterController;
    [SerializeField] private PlayerShooting playerShooting;
    [SerializeField] private AnimationPlayer animationPlayer;
    [SerializeField] GrenadeLauncher grenadeLauncher;
    public PlayerState currentState = PlayerState.Idle;
    public WeaponType CurrentWeapon;
    public event Action PistolFire;
    [SerializeField] public Animator animator;

    [SerializeField] public ExamplePlayer examplePlayer;
    [SerializeField] ExampleCharacterCamera normalCamera;

    public EnterController currentNearTransport;
    [SerializeField] GameObject GunModel;
    [SerializeField] GameObject PistolModel;
    [SerializeField] GameObject KnifeModel;
    [SerializeField] GameObject GrenadeModel;
    [SerializeField] GameObject CharacterModel;

    public KeyCode DeletingModeButton = KeyCode.N;
    public KeyCode BuildingModeButton = KeyCode.B;
    public static Player instance;
    public bool IsFirstView;
    public enum PlayerState
    {
        InTransport,
        Aiming,
        Shooting,
        Idle,
        Sitting,
        Building,
        DeletingBuilding,
        AimingGrenade,
    }
    float startTime;
    public enum WeaponType
    {
        Pistol,
        Gun,
        Knife,
        Hand,
        Grenade
    }

    private void Awake()
    {
        instance = this;
    }
    public void PlayerSetActive(bool Is)
    {
        PlayerParent.SetActive(Is);
    }
    public void SwitchPlayerState(PlayerState newPlayerState)
    {
        switch (newPlayerState)
        {
            case PlayerState.Aiming:
                if (currentState != PlayerState.Aiming)
                {
                    animator.SetBool("PistolAiming", true);
                    GoToAimCamera();
                }
                break;
            case PlayerState.Idle:
                animator.SetBool("PistolAiming", false);
                break;
        }
        if(currentState == PlayerState.Aiming && newPlayerState != PlayerState.Aiming)
        {
            GoToNormalCamera();
        }
        if(currentState == PlayerState.DeletingBuilding)
        {

            BuildingManager.instance.ActivateDeletingMode(false);
            examplePlayer.LockCursor(true);
        }
        StartCoroutine(DelaySwitchState(newPlayerState));
    }
    private IEnumerator DelaySwitchState(Player.PlayerState newPlayerState)
    {
        yield return new WaitForEndOfFrame();
        currentState = newPlayerState;
        Debug.Log("Player State:" + currentState);
    }
    private void SwitchWeapon(int PressedNumber)
    {
        GunModel.SetActive(false);
        PistolModel.SetActive(false);
        KnifeModel.SetActive(false);
        GrenadeModel.SetActive(false);

        if(CurrentWeapon == WeaponType.Grenade)
        {
        animator.SetBool("AimingGrenade", false);
            grenadeLauncher.ClearTrajectory();
        }


        switch (PressedNumber)
        {
            case 1:
                CurrentWeapon = WeaponType.Gun;
                GunModel.SetActive(true);
                break;
            case 2:
                CurrentWeapon = WeaponType.Pistol;
                PistolModel.SetActive(true);
                break;
            case 3:
                CurrentWeapon = WeaponType.Knife;
                KnifeModel.SetActive(true);
                break;
            case 4:
                CurrentWeapon = WeaponType.Hand;
                break;
            case 5:
                CurrentWeapon = WeaponType.Grenade;
                GrenadeModel.SetActive(true);
                break;

        }
    }
    private void SwitchCrossHair()
    {
        IsFirstView = !IsFirstView;
            CanvasManager.instance.IsCrossForThirdView(!IsFirstView);
    }
    private void Update()
    {
        if( currentState == PlayerState.Sitting)
        {
            return;
        }

        if(currentState == PlayerState.Idle)
        {
            if (Input.GetKeyDown(DeletingModeButton))
            {
                SwitchPlayerState(Player.PlayerState.DeletingBuilding);
            }
            if (Input.GetKeyDown(BuildingModeButton))
            {
                BuildingManager.instance.ActivateBuildingButton(true);
                SwitchPlayerState(Player.PlayerState.Building);
            }

        }
        if(currentState !=PlayerState.Building && currentState != PlayerState.DeletingBuilding)
        {
            FireInput();
            ChangeWeaponInput();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchCrossHair();
        }

        if (currentState == PlayerState.Building)
        {
            BuildingManager.instance.BuildingInput();
            if (Input.GetKeyDown(BuildingModeButton))
            {
                BuildingManager.instance.ActivateBuildingButton(false);
                SwitchPlayerState(Player.PlayerState.Idle);
            }
            return;
        }
        if (currentState == PlayerState.DeletingBuilding)
        {
            BuildingManager.instance.DeletingBuildingInput();
            if (Input.GetKeyDown(DeletingModeButton))
            {
                BuildingManager.instance.TurnDeletingObjectNormalAndClearFields();
                SwitchPlayerState(Player.PlayerState.Idle);
            }
        }
    }
    private void ChangeWeaponInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchWeapon(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchWeapon(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchWeapon(5);
        }
    }
    private void FireInput()
    {
      
        if (CurrentWeapon == WeaponType.Pistol)
        {
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger("PistolFire");
                Fire();
            }
        }
        if(CurrentWeapon == WeaponType.Knife)
        {
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger("Stab");
               
                playerShooting.HandAttack();
            }
        }
        if (CurrentWeapon == WeaponType.Hand)
        {
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetBool("IsRun", false);
                animator.SetTrigger("Punch");
                playerShooting.HandAttack();
            }
        }
        if (CurrentWeapon == WeaponType.Gun)
        {
            if (Input.GetMouseButton(0))
            {
                animator.SetTrigger("PistolFire");
                playerShooting.Fire(CurrentWeapon);
            }
        }
        if (CurrentWeapon == WeaponType.Grenade)
        {
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetBool("AimingGrenade", true);
                currentState = PlayerState.AimingGrenade;
            }
            if (Input.GetMouseButton(0))
            {
                grenadeLauncher.GrenadeInput();

            }
            if (Input.GetMouseButtonUp(0))
            {
                animator.SetBool("AimingGrenade", false);
                grenadeLauncher.LaunchGrenade();
                currentState = PlayerState.Idle;
            }
        }
        if (currentState != PlayerState.Aiming)
        {
            if (Input.GetMouseButton(1))
            {
                SwitchPlayerState(PlayerState.Aiming);
            }
        }
        if (currentState == PlayerState.Aiming)
        {
            if (Input.GetMouseButtonUp(1))
            {
                Debug.Log("Stop Aiming");
                SwitchPlayerState(PlayerState.Idle);
                return;
            }
        }

    }
    public void RotatePlayerOnShoot(Vector3 aimDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(aimDirection);
        Quaternion OnlyY = new Quaternion(0, targetRotation.y, 0, targetRotation.w);
        motor.RotateCharacter(OnlyY);
    }
    private void GoToAimCamera()
    {
        if(IsFirstView == false)
        {
        normalCamera.FollowPointFraming = new Vector2(1.5f, 1);
        }
        if (IsFirstView)
        {
        normalCamera.Camera.fieldOfView = 20;
        }
        else
        {
        normalCamera.Camera.fieldOfView = 40;
        }

    }
    private void GoToNormalCamera()
    {
        if(IsFirstView == false)
        {
        normalCamera.FollowPointFraming = new Vector2(0, 1.25f);
        }
        normalCamera.Camera.fieldOfView = 55;

    }
    private void Fire()
    {
        playerShooting.Fire(CurrentWeapon);
    }
}
