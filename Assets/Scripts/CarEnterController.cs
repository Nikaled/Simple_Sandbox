using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;

public class CarEnterController : MonoBehaviour
{
    private bool IsInterfaceActive;
    private bool IsPlayerInCar;
    [SerializeField] private Transform PlayerSpawnTransform;
    [SerializeField] private Transform FirstViewCameraTransform;
    [SerializeField] private GameObject CarUI;
    [SerializeField] private Camera CarCamera;
    private Player player;
    [SerializeField] VehicleControl vehicleControl;

    [SerializeField] private KinematicCharacterMotor motor;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player = other.GetComponent<Player>();
            CarUI.SetActive(true);
            IsInterfaceActive = true;
            Debug.Log("Активация интерфейса машины");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DeactivateCarEnterUI();
            player = null;
        }
    }
    private void Update()
    {
        if(IsInterfaceActive == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SitIntoCar();
                DeactivateCarEnterUI();
                return;
            }
        }
        if (IsPlayerInCar)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GetOutCar();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                CarCamera.transform.position = FirstViewCameraTransform.position;
            }
        }
    }
    private void DeactivateCarEnterUI()
    {
        CarUI.SetActive(false);
        IsInterfaceActive = false;
        Debug.Log("Деактивация интерфейса машины");
    }
    private void SitIntoCar()
    {
        player.PlayerSetActive(false);
        CarCamera.gameObject.SetActive(true);
        vehicleControl.enabled = true;
        IsPlayerInCar = true;
    }
    private void GetOutCar()
    {
        Debug.Log("выход из машины");
        player.PlayerParent.transform.position = PlayerSpawnTransform.position;
        motor.SetPositionAndRotation(PlayerSpawnTransform.position, PlayerSpawnTransform.rotation, true);
        player.PlayerSetActive(true);
        vehicleControl.enabled = false;
        CarCamera.gameObject.SetActive(false);
    }
}
