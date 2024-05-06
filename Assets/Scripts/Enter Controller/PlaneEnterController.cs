using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeneGames.Airplane;

public class PlaneEnterController : EnterController
{
    [SerializeField] private Transform _rotorsTransform;
    [SerializeField] SimpleAirPlaneController _planeController;
    float startTime;
    float timeToUp = 1.5f;
    protected override void ActivateTransport()
    {
        CanvasManager.instance.ShowPlaneInstruction(true);
        _planeController.enabled = true;
        _planeController.airplaneState = SimpleAirPlaneController.AirplaneState.Takeoff;
        _planeController.GetComponent<Rigidbody>().isKinematic = true;
        _planeController.GetComponent<Rigidbody>().useGravity = false;
    }

    protected override void DeactivateTransport()
    {
        CanvasManager.instance.ShowPlaneInstruction(false);
        _planeController.airplaneState = SimpleAirPlaneController.AirplaneState.Landing;
        _planeController.GetComponent<Rigidbody>().isKinematic = false;
        _planeController.GetComponent<Rigidbody>().useGravity = true;
        _planeController.enabled = false;
    }
    protected override void Update()
    {
        base.Update();
        if (IsPlayerIn)
        {
            if (_planeController.airplaneState == SimpleAirPlaneController.AirplaneState.Landing)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _planeController.airplaneState = SimpleAirPlaneController.AirplaneState.Takeoff;
                }
            }
            if (_planeController.airplaneState != SimpleAirPlaneController.AirplaneState.Flying)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    startTime = Time.time;
                }
                if (Input.GetKey(KeyCode.Space) && Time.time - startTime > timeToUp)
                {
                    _planeController.airplaneState = SimpleAirPlaneController.AirplaneState.Flying;
                    _planeController.UpThePlaneFromGround();
                    Debug.Log((Time.time - startTime).ToString("00:00.00"));
                }

            }
            if (Input.GetKey(KeyCode.Space) && _planeController.airplaneState == SimpleAirPlaneController.AirplaneState.Flying)
            {
                _planeController.UpThePlane();
            }
            if (_planeController.airplaneState != SimpleAirPlaneController.AirplaneState.Landing)
            {
                if(_rotorsTransform !=null)
                _rotorsTransform.Rotate(Vector3.forward * _planeController.currentSpeed);
            }
        }
    }
}
