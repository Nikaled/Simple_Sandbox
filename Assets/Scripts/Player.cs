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
    public PlayerState currentState = PlayerState.Idle;
    public WeaponType CurrentWeapon;
    public event Action PistolFire;
    [SerializeField] public Animator animator;

    [SerializeField] public ExamplePlayer examplePlayer;
    [SerializeField] ExampleCharacterCamera normalCamera;


    [SerializeField] GameObject GunModel;
    [SerializeField] GameObject PistolModel;
    [SerializeField] GameObject CharacterModel;
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
    }
    float startTime;
    public enum WeaponType
    {
        Pistol,
        Gun
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
        if(currentState == PlayerState.Building || currentState == PlayerState.DeletingBuilding)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(2);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchCrossHair();
        }
        if(CurrentWeapon == WeaponType.Pistol)
        {
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger("PistolFire");
                //PistolFire?.Invoke();
                Fire();
            }
        }
        if(CurrentWeapon == WeaponType.Gun)
        {
            if (Input.GetMouseButton(0))
            {
                animator.SetTrigger("PistolFire");
                //PistolFire?.Invoke();
                playerShooting.Fire(CurrentWeapon);
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
