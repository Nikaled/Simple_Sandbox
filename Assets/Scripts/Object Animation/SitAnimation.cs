using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitAnimation : ObjectInteraction
{
    [SerializeField] Transform SitTransform;
    protected override void ActivateObject()
    {
        Player.instance.motor.SetPositionAndRotation(SitTransform.position, SitTransform.rotation);
        Player.instance.motor.enabled = false;
        Player.instance.SwitchPlayerState(Player.PlayerState.Sitting);
        Player.instance.animator.SetBool("Sitting", true);
    }
    protected override void DeactivateObject()
    {
        Player.instance.animator.SetBool("Sitting", false);
        Player.instance.motor.enabled = true;
        Player.instance.SwitchPlayerState(Player.PlayerState.Idle);

    }
}
