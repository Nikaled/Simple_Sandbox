using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterController : MonoBehaviour
{
    private bool IsInterfaceActive;
    protected bool IsPlayerIn;
    [SerializeField] private Transform PlayerSpawnTransform;
    [SerializeField] protected Camera TransportCamera;
    [SerializeField] private GameObject HpView;
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
           
            if(player != null)
            {
                if (player.currentNearTransport != null)
                {
                    if (player.currentNearTransport == this)
                    {
                        player.currentNearTransport = null;
                    }
                }
            }
        }
        if(IsPlayerIn == false)
        {
        player = null;
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
            if (player != null)
            {
                if (player.currentNearTransport != null)
                {
                    if (player.currentNearTransport == this)
                    {
                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            SitIntoTransport();
                            HideEnterInstruction();
                            return;
                        }
                    }

                }
               
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
        HpView.SetActive(false);
        player.PlayerSetActive(false);
        TransportCamera.gameObject.SetActive(true);
        ActivateTransport();
        IsPlayerIn = true;
        IsInterfaceActive = false;
        player.SwitchPlayerState(Player.PlayerState.InTransport, 0);
    }
    private void GetOutTransport()
    {
        HpView.SetActive(true);
        player.PlayerParent.transform.position = PlayerSpawnTransform.position;
        player.motor.SetPositionAndRotation(PlayerSpawnTransform.position, PlayerSpawnTransform.rotation, true);
        player.PlayerSetActive(true);
        DeactivateTransport();
        IsPlayerIn = false;
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
