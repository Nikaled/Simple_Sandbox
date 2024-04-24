using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]public GameObject PlayerParent;
    [SerializeField] public KinematicCharacterController.KinematicCharacterMotor motor;
    [SerializeField] public KinematicCharacterController.Examples.ExampleCharacterController characterController;

    public void PlayerSetActive(bool Is)
    {
        PlayerParent.SetActive(Is);
    }
}
