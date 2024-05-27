using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour
{
    [SerializeField] Image ActiveWeaponImage;
    [SerializeField] TextMeshProUGUI NumberOfSlot;
    // Start is called before the first frame update
    void Start()
    {
        ActiveWeaponImage.enabled = false;
        if (Geekplay.Instance.mobile)
        {
            NumberOfSlot.gameObject.SetActive(false);
        }
    }
    public void WeaponIsActive()
    {
        ActiveWeaponImage.enabled = true;
    }
    public void WeaponIsInactive()
    {
        ActiveWeaponImage.enabled = false;
    }
}
