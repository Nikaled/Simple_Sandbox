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
        CanvasManager.instance.ShowHelicopterMobileInstruction(true);
        _helicopterController.enabled = true;
        _helicopterController.MyInitializeButtons();
        HelicopterButtons.instance.GetOutButton.onClick.AddListener(delegate { GetOutTransport(); });
    }
    protected override void GetOutTransport()
    {
        base.GetOutTransport();
        HelicopterButtons.instance.GetOutButton.onClick.RemoveAllListeners();

    }
    protected override void DeactivateTransport()
    {
        CanvasManager.instance.ShowHelicopterInstruction(false);
        CanvasManager.instance.ShowHelicopterMobileInstruction(false);
        _helicopterController.MyClearButtons();
        _helicopterController.enabled = false;
    }
}
