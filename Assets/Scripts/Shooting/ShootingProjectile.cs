using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingProjectile : MonoBehaviour
{
    [SerializeField] Rigidbody bulletRigidbody;
    float speed = 100f;
    int bulletDamage = 1;
    public GameObject ProjectileSource;
    // Start is called before the first frame update
    void Start()
    {
        bulletRigidbody.velocity = transform.forward * speed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other == ProjectileSource)
        {
            return;
        }
        Debug.Log(other.name);
        if (other.GetComponent<HpSystemCollision>() != null)
        {
            other.GetComponent<HpSystemCollision>().TakeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == ProjectileSource)
        {
            return;
        }
        Debug.Log(collision.gameObject);
        if (collision.gameObject.GetComponent<HpSystemCollision>() != null)
        {
            collision.gameObject.GetComponent<HpSystemCollision>().TakeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }
}