using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shoot parametres")]
    [SerializeField] private float normalSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask;
    [SerializeField] private Transform RaycastOrigin;
    [SerializeField] private Transform ProjectileSpawnPoint;
    [SerializeField] ShootingProjectile projectile;
    [SerializeField] public Image Crosshair;
    [SerializeField] Player player;
    Vector3 AimDirection;
    Vector3 mouseWorldPosition;
    float GunTimer;
    float GunShootInterval=0.05f;

    // TODO:анимации автомата, взрывы
    public bool Reloading { get; private set; }

    void Update()
    {
         mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(Crosshair.transform.position);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            mouseWorldPosition = raycastHit.point;
        }
        else
        {
            mouseWorldPosition = Vector3.forward;
        }

    }
    public void Fire(Player.WeaponType currentWeapon)
    {
        Vector3 aimDirection = (mouseWorldPosition - ProjectileSpawnPoint.position).normalized;
         //Vector3 RotatingDirection = new Vector3(aimDirection.x, 0, aimDirection.z);
        player.RotatePlayerOnShoot(aimDirection);
        if (currentWeapon == Player.WeaponType.Gun)
        {
            FireGun();
            return;
        }
        if(currentWeapon == Player.WeaponType.Pistol)
        {
            ShootingProjectile proj = Instantiate(projectile, ProjectileSpawnPoint.position, Quaternion.LookRotation(aimDirection, Vector3.up));
        }
    }
    public void FireGun()
    {
        if (Time.time - GunTimer > GunShootInterval)
        {
            Reloading = false;
        }
        if (Reloading == false)
        {
            Reloading = true;
        Vector3 aimDirection = (mouseWorldPosition - ProjectileSpawnPoint.position).normalized;
        ShootingProjectile proj = Instantiate(projectile, ProjectileSpawnPoint.position, Quaternion.LookRotation(aimDirection, Vector3.up));
        GunTimer = Time.time;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Gizmos.DrawRay(RaycastOrigin.position, ray.direction * 999);
    }
}
