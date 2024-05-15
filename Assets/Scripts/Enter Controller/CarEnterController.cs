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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TransportCamera.transform.position = FirstViewCameraTransform.position;
        }
    }
    protected override void ActivateTransport()
    {
        CanvasManager.instance.ShowControlCarInstruction(true);
        vehicleControl.GetComponent<VehicleControl>().activeControl = true;
        vehicleControl.enabled = true;
    }
    protected override void DeactivateTransport()
    {
        CanvasManager.instance.ShowControlCarInstruction(false);
        vehicleControl.carSetting.brakePower = float.MaxValue;
        vehicleControl.GetComponent<VehicleControl>().activeControl = false;
        vehicleControl.GetComponent<VehicleControl>().OnCarQuit();  
        //vehicleControl.enabled = false;
    }
}
