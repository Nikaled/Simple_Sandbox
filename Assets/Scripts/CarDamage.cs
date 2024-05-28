using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDamage : MonoBehaviour
{
    public Rigidbody carRb;
    float CarRbVelocityToKill = 1;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("�arDamageCollider � �������� �:" + other.gameObject.name);
        if (other.gameObject.CompareTag("Citizen"))
        {
            Debug.Log("�������� � ������ ��� �����");
            if (carRb.velocity.sqrMagnitude > CarRbVelocityToKill)
            {
                other.gameObject.GetComponent<HpSystemCollision>().TakeDamage(50);
            }
        }
    }
}
