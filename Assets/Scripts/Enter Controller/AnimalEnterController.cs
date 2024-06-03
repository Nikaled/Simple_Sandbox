using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalEnterController : EnterController
{
    [SerializeField] AnimalRiding animalRiding;
    protected override void SitIntoTransport()
    {
        IsPlayerIn = true;
        HideEnterInstruction();
        HpView.SetActive(false);
        ActivateTransport();
        IsInterfaceActive = false;
        player.motor.SetPositionAndRotation(animalRiding.PlayerSittingTransform.position, animalRiding.PlayerSittingTransform.rotation, true);

        CanvasManager.instance.InteracteButton.onClick.RemoveAllListeners();
        CanvasManager.instance.InteracteButton.onClick.AddListener(delegate { GetOutTransport(); });
    }
    protected override void ShowEnterInstruction()
    {
        base.ShowEnterInstruction();
        CanvasManager.instance.ShowCurrentInteracteButton(2);
    }
    protected override void GetOutTransport()
    {
        animalRiding.DeactivateRiding();
        animalRiding.enabled = false;

        HpView.SetActive(true);
        player.PlayerParent.transform.position = PlayerSpawnTransform.position;
        player.motor.SetPositionAndRotation(PlayerSpawnTransform.position, PlayerSpawnTransform.rotation, true);
        player.PlayerSetActive(true);
        DeactivateTransport();
        IsPlayerIn = false;
        CanvasManager.instance.InteracteButton.gameObject.SetActive(false);
        CanvasManager.instance.InteracteButton.onClick.RemoveAllListeners();
        player.SwitchPlayerState(Player.PlayerState.Idle);
    }
    protected override void ActivateTransport()
    {
        //CanvasManager.instance.ShowHelicopterInstruction(true);
        //CanvasManager.instance.ShowHelicopterMobileInstruction(true);

        animalRiding.enabled = true;
        animalRiding.ActivateRiding();
        //if (Geekplay.Instance.mobile)
        //{
        //    HelicopterButtons.instance.GetOutButton.onClick.AddListener(delegate { GetOutTransport(); });
        //}
    }
    protected override void DeactivateTransport()
    {
        //CanvasManager.instance.ShowHelicopterInstruction(false);
        //CanvasManager.instance.ShowHelicopterMobileInstruction(false);
        //if (Geekplay.Instance.mobile)
        //{
        //    //animalRiding.MyClearButtons();
        //    HelicopterButtons.instance.GetOutButton.onClick.RemoveAllListeners();
        //}
        animalRiding.DeactivateRiding();
        animalRiding.enabled = false;
    }
}