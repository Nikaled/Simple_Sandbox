using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    private Image Crosshair;
    [SerializeField] private LayerMask aimColliderLayerMask;
    [HideInInspector] public Vector3 CrosshairWorldPosition;
    public Vector3 AimDirection;
    [SerializeField] private Transform TankProjectileSpawnPoint;
    [SerializeField] ShootingProjectile TankProjectile;
    [SerializeField] GameObject TankHpCollider;


    private void Start()
    {
         Crosshair = CanvasManager.instance.Crosshair;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }
    public void ActivateTankShooting(bool Is)
    {
        TankHpCollider.SetActive(!Is);
    }
    public void Fire()
    {
        ShootingProjectile proj = Instantiate(TankProjectile, TankProjectileSpawnPoint.position, TankProjectileSpawnPoint.transform.rotation);
    }
}
