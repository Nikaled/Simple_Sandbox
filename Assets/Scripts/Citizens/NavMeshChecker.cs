using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshChecker : MonoBehaviour
{
    public CitizenMovement citizen;
    [SerializeField] LayerMask NotAvailableToGoMask;

    private void OnTriggerEnter(Collider other)
    {
        if ((NotAvailableToGoMask & (1 << other.gameObject.layer)) == 0)
        {
            Debug.Log("����� ����������� �� �������");
            if(citizen !=null)
            citizen.FindNewDestination();
        }
    }
}
