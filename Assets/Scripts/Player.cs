using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController.Examples;
using KinematicCharacterController;
using Cinemachine;
using UnityEngine.UI;
using DG.Tweening;

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
    [SerializeField] Animator animatior;

    [SerializeField] public ExamplePlayer examplePlayer;
    [SerializeField] ExampleCharacterCamera normalCamera;

    [SerializeField] GameObject FirstViewCross;
    [SerializeField] GameObject ThirdViewCross;

    [SerializeField] GameObject GunModel;
    [SerializeField] GameObject PistolModel;
    [SerializeField] GameObject CharacterModel;
    public bool IsFirstView;
    public enum PlayerState
    {
        InTransport,
        Aiming,
        Shooting,
        Idle,
        Building,
        DeletingBuilding,
    }
    float startTime;
    public enum WeaponType
    {
        Pistol,
        Gun
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
                    animatior.SetBool("PistolAiming", true);
                    GoToAimCamera();
                }
                break;
            case PlayerState.Idle:
                animatior.SetBool("PistolAiming", false);
                break;
        }
        if(currentState == PlayerState.Aiming && newPlayerState != PlayerState.Aiming)
        {
            GoToNormalCamera();
        }
        currentState = newPlayerState;
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
        FirstViewCross.SetActive(IsFirstView);
        ThirdViewCross.SetActive(!IsFirstView);
        if(IsFirstView)
        playerShooting.Crosshair = FirstViewCross.GetComponent<Image>();
        if(!IsFirstView)
        playerShooting.Crosshair = ThirdViewCross.GetComponent<Image>();
    }
    private void Update()
    {
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
                animatior.SetTrigger("PistolFire");
                //PistolFire?.Invoke();
                Fire();
            }
        }
        if(CurrentWeapon == WeaponType.Gun)
        {
            if (Input.GetMouseButton(0))
            {
                animatior.SetTrigger("PistolFire");
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
        Debug.Log(aimDirection);
        Quaternion rot = Quaternion.Euler(aimDirection.x, aimDirection.y, aimDirection.z);
        motor.SetRotation(rot);
        Quaternion targetRotation = Quaternion.LookRotation(aimDirection);
        motor.RotateCharacter(targetRotation);
    }
    private void GoToAimCamera()
    {
        normalCamera.FollowPointFraming = new Vector2(1.5f, 1);
        normalCamera.Camera.fieldOfView = 40;

    }
    private void GoToNormalCamera()
    {
        normalCamera.FollowPointFraming = new Vector2(0, 1.25f);
        normalCamera.Camera.fieldOfView = 55;

    }
    private void Fire()
    {
        playerShooting.Fire(CurrentWeapon);
    }
}
