using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using alelavoie;

public class HelicopterEnterController : EnterController
{
    [SerializeField] AHC _helicopterController;
    protected override void ActivateTransport()
    {
        CanvasManager.instance.ShowHelicopterInstruction(true);
        _helicopterController.enabled = true;
    }
    protected override void DeactivateTransport()
    {
        CanvasManager.instance.ShowHelicopterInstruction(false);
        _helicopterController.enabled = false;
    }
}
