using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;

public class CarEnterController : EnterController
{
    [SerializeField] private Transform FirstViewCameraTransform;
    [SerializeField] VehicleControl vehicleControl;
    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Q) && IsPlayerIn)
        {
            TransportCamera.transform.position = FirstViewCameraTransform.position;
        }
    }
    protected override void ActivateTransport()
    {
        CanvasManager.instance.ShowControlCarInstruction(true);
        vehicleControl.GetComponent<VehicleControl>().activeControl = true;
        vehicleControl.enabled = true;
        vehicleControl.OnCarEnter();
        if (Geekplay.Instance.mobile)
        {
            CanvasManager.instance.ShowCarMobileInstruction(true);
            vehicleControl.MyInitializeButtons();
            CarButtons.instance.GetOutButton.onClick.AddListener(delegate { GetOutTransport(); });
        }
    }
    protected override void DeactivateTransport()
    {
        CanvasManager.instance.ShowControlCarInstruction(false);
        vehicleControl.carSetting.brakePower = float.MaxValue;
        vehicleControl.GetComponent<VehicleControl>().activeControl = false;
        vehicleControl.GetComponent<VehicleControl>().OnCarQuit();
        if (Geekplay.Instance.mobile)
        {
            vehicleControl.MyClearButtons();
            CarButtons.instance.GetOutButton.onClick.RemoveAllListeners();
            CanvasManager.instance.ShowCarMobileInstruction(false);
        }
        //vehicleControl.enabled = false;
    }
}
