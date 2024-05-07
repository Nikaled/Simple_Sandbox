using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterController : MonoBehaviour
{
    private bool IsInterfaceActive;
    protected bool IsPlayerIn;
    [SerializeField] private Transform PlayerSpawnTransform;
    [SerializeField] protected Camera TransportCamera;
    private Player player;
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collider entered");
            player = other.GetComponent<Player>();
            ShowEnterInstruction();
            IsInterfaceActive = true;
            player.currentNearTransport = this;
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HideEnterInstruction();
            player = null;
            if (player.currentNearTransport == this)
            {
                player.currentNearTransport = null;
            }
        }
    }
    private void ShowEnterInstruction()
    {
        CanvasManager.instance.ShowTransportEnterInstruction(true);
    }

    protected virtual void Update()
    {
        if (IsInterfaceActive == true)
        {
            if(player.currentNearTransport != this)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                SitIntoTransport();
                HideEnterInstruction();
                return;
            }
        }
        if (IsPlayerIn)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("F pressed");
                GetOutTransport();
            }
        }
    }
    private void HideEnterInstruction()
    {
        CanvasManager.instance.ShowTransportEnterInstruction(false);
    }
    private void SitIntoTransport()
    {
        player.PlayerSetActive(false);
        TransportCamera.gameObject.SetActive(true);
        ActivateTransport();
        IsPlayerIn = true;
        IsInterfaceActive = false;
        player.SwitchPlayerState(Player.PlayerState.InTransport);
    }
    private void GetOutTransport()
    {
        player.PlayerParent.transform.position = PlayerSpawnTransform.position;
        player.motor.SetPositionAndRotation(PlayerSpawnTransform.position, PlayerSpawnTransform.rotation, true);
        player.PlayerSetActive(true);
        DeactivateTransport();
        TransportCamera.gameObject.SetActive(false);
        player.SwitchPlayerState(Player.PlayerState.Idle);

    }
    protected virtual void ActivateTransport()
    {
    }
    protected virtual void DeactivateTransport()
    {
    }
}
