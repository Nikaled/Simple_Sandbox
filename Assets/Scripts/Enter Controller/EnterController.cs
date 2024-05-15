using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterController : MonoBehaviour
{
    protected bool IsInterfaceActive;
    protected bool IsPlayerIn;
    [SerializeField] private Transform PlayerSpawnTransform;
    [SerializeField] protected Camera TransportCamera;
    [SerializeField] protected GameObject HpView;
    protected Player player;
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
    protected virtual void ShowEnterInstruction()
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
    protected virtual void HideEnterInstruction()
    {
        CanvasManager.instance.ShowTransportEnterInstruction(false);
    }
    protected  virtual void SitIntoTransport()
    {
        HpView.SetActive(false);
        player.PlayerSetActive(false);
        TransportCamera.gameObject.SetActive(true);
        ActivateTransport();
        IsPlayerIn = true;
        IsInterfaceActive = false;
        player.SwitchPlayerState(Player.PlayerState.InTransport, 0);
    }
    protected virtual void GetOutTransport()
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
