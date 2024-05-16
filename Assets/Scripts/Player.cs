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
    public KeyCode RotatingModeButton = KeyCode.M;
    public static Player instance;
    public bool IsFirstView;

    public SkinnedMeshRenderer CurrentCitizenMesh;
    public SkinnedMeshRenderer[] PlayerMeshes;
    private IEnumerator lockOnShoot;
    public enum PlayerState
    {
        InTransport,
        Aiming,
        Shooting,
        Idle,
        Sitting,
        Building,
        DeletingBuilding,
        RotatingBuilding,
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
    public void SwitchPlayerState(PlayerState newPlayerState, float Delay = 0.1f)
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
        if (currentState == PlayerState.Aiming && newPlayerState != PlayerState.Aiming)
        {
            GoToNormalCamera();
        }
        if (currentState == PlayerState.DeletingBuilding)
        {

            BuildingManager.instance.ActivateDeletingMode(false);
            examplePlayer.LockCursor(true);
        }
        if (newPlayerState == PlayerState.Idle)
        {
            CanvasManager.instance.ShowIdleInstruction(true);
        }
        else
        {
            CanvasManager.instance.ShowIdleInstruction(false);
        }
        if (Delay > 0)
        {
            StartCoroutine(DelaySwitchState(newPlayerState, Delay));
        }
        else
        {
            if (currentState == PlayerState.DeletingBuilding)
            {
                BuildingManager.instance.TurnDeletingObjectNormalAndClearFields();
            }
            if (currentState == PlayerState.RotatingBuilding)
            {
                BuildingManager.instance.TurnRotatingObjectNormalAndClearFields();
            }
            currentState = newPlayerState;
        }
    }
    private IEnumerator DelaySwitchState(Player.PlayerState newPlayerState, float Delay)
    {
        yield return new WaitForSeconds(Delay);
        if (currentState == PlayerState.DeletingBuilding)
        {
            BuildingManager.instance.TurnDeletingObjectNormalAndClearFields();
        }
        if (currentState == PlayerState.RotatingBuilding)
        {
            BuildingManager.instance.TurnRotatingObjectNormalAndClearFields();
        }
        currentState = newPlayerState;
        Debug.Log("Player State:" + currentState);
    }
    private void SwitchWeapon(int PressedNumber)
    {
        GunModel.SetActive(false);
        PistolModel.SetActive(false);
        KnifeModel.SetActive(false);
        GrenadeModel.SetActive(false);

        if (CurrentWeapon == WeaponType.Grenade)
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
        SwitchPlayerState(PlayerState.Idle);
    }
    private void SwitchView()
    {
        IsFirstView = !IsFirstView;
        GoToNormalCamera();
    }
    private void Update()
    {
        if (currentState == PlayerState.Sitting)
        {
            return;
        }

        if (currentState == PlayerState.Idle)
        {
            if (Input.GetKeyDown(DeletingModeButton))
            {
                BuildingManager.instance.ActivateDeletingMode(true);
                SwitchPlayerState(Player.PlayerState.DeletingBuilding);
            }
            if (Input.GetKeyDown(BuildingModeButton))
            {
                BuildingManager.instance.ActivateBuildingButton(true);
                SwitchPlayerState(Player.PlayerState.Building);
            }
            if (Input.GetKeyDown(RotatingModeButton))
            {
                BuildingManager.instance.ActivateRotatingMode(true);
                SwitchPlayerState(Player.PlayerState.RotatingBuilding);
            }

        }
        if (currentState != PlayerState.Building && currentState != PlayerState.DeletingBuilding && currentState != PlayerState.RotatingBuilding)
        {
            FireInput();
            ChangeWeaponInput();
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SwitchView();
            }
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
                SwitchPlayerState(Player.PlayerState.Idle);
                BuildingManager.instance.TurnDeletingObjectNormalAndClearFields();
            }
        }
        if (currentState == PlayerState.RotatingBuilding)
        {
            BuildingManager.instance.RotatingInput();
            if (BuildingManager.instance.RotateChosenObjectMode == false)
            {
                if (Input.GetKeyDown(RotatingModeButton))
                {
                    SwitchPlayerState(Player.PlayerState.Idle);
                    BuildingManager.instance.ActivateRotatingMode(false);
                }
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

    public IEnumerator LockPositionOnShoot()
    {
        examplePlayer.MyLockOnShoot = true;
        yield return new WaitForSeconds(1f);
        //for (int i = 0; i < 30; i++)
        //{
        //    yield return new WaitForEndOfFrame();
        //}
        examplePlayer.MyLockOnShoot = false;
    }
    private void FireInput()
    {
        if (CurrentWeapon != WeaponType.Grenade)
        {
        }
        if (CurrentWeapon == WeaponType.Pistol)
        {
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger("PistolFire");
                motor.SetPosition(transform.position);
                Fire();
            }
        }
        if (CurrentWeapon == WeaponType.Knife)
        {
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger("Stab");

                playerShooting.HandAttack(WeaponType.Knife);
            }
        }
        if (CurrentWeapon == WeaponType.Hand)
        {
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetBool("IsRun", false);
                animator.SetTrigger("Punch");
                playerShooting.HandAttack(WeaponType.Hand);
            }
        }
        if (CurrentWeapon == WeaponType.Gun)
        {
            if (Input.GetMouseButton(0))
            {
                animator.SetTrigger("GunFire");
                playerShooting.Fire(CurrentWeapon);
                examplePlayer.MyLockOnShoot = true;

                //motor.SetPositionAndRotation(transform.position, transform.rotation);
                //motor.SetRotation(transform.rotation);
                motor.SetPosition(transform.position);
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
            if (CurrentWeapon == WeaponType.Pistol || CurrentWeapon == WeaponType.Gun)
            {
                if (Input.GetMouseButton(1))
                {

                    SwitchPlayerState(PlayerState.Aiming, 0);
                }
            }
        }
        if (currentState == PlayerState.Aiming)
        {
            if (Input.GetMouseButtonUp(1))
            {
                Debug.Log("Stop Aiming");
                SwitchPlayerState(PlayerState.Idle, 0);
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
            if (IsFirstView)
            {
                normalCamera.Camera.fieldOfView = 20;
                normalCamera.FollowPointFraming = new Vector2(0f, 0f);
            }
            else
            {
                normalCamera.Camera.fieldOfView = 30;
            }
            playerShooting.lineRenderer.enabled = true;

    }
    private void GoToNormalCamera()
    {
        if (IsFirstView == false)
        {
            normalCamera.FollowPointFraming = new Vector2(1.8f, 1.8f);
            normalCamera.Camera.fieldOfView = 55;
        }
        else
        {
            normalCamera.FollowPointFraming = new Vector2(0, 0);
            normalCamera.Camera.fieldOfView = 40;
        }
        playerShooting.lineRenderer.enabled = false;

    }
    private void Fire()
    {
        playerShooting.Fire(CurrentWeapon);
    }
}
