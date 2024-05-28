using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    [SerializeField] WeaponSlot[] WeaponSlots;
    private void Awake()
    {
        
    }
    private void Start()
    {
        OnWeaponSwitched(4);
    }
    private void OnEnable()
    {
        Player.instance.SwitchedWeapon += OnWeaponSwitched;
    }

    private void OnDisable()
    {
        Player.instance.SwitchedWeapon -= OnWeaponSwitched;
    }
    private void OnWeaponSwitched(int PressedNumber)
    {
        for (int i = 0; i < WeaponSlots.Length; i++)
        {
            WeaponSlots[i].WeaponIsInactive();
        }
        int ActiveWeaponIndex = PressedNumber - 1;
        WeaponSlots[ActiveWeaponIndex].WeaponIsActive();
    }
}
