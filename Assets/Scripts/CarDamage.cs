using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDamage : MonoBehaviour
{
    public Rigidbody carRb;
    float CarRbVelocityToKill = 1;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ÑarDamageCollider â êîëëèçèè ñ:" + other.gameObject.name);
        if (other.gameObject.CompareTag("Citizen"))
        {
            Debug.Log("Âðåçàëñÿ â æèòåëÿ èëè çâåðÿ");
            if (carRb.velocity.sqrMagnitude > CarRbVelocityToKill)
            {
                other.gameObject.GetComponent<HpSystemCollision>().TakeDamage(50);
            }
        }
    }
}
