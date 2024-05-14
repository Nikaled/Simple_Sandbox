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
    [SerializeField] MeleeAttackHitbox handHitbox;
    Vector3 AimDirection;
  [HideInInspector] public  Vector3 CrosshairWorldPosition;
    float GunTimer;
    float GunShootInterval=0.05f;
    public static PlayerShooting instance;
    private void Start()
    {
        instance = this;
    }
    public bool Reloading { get; private set; }

    void Update()
    {
         CrosshairWorldPosition = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Crosshair.transform.position);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999, aimColliderLayerMask))
        {
            CrosshairWorldPosition = raycastHit.point;
        }
        else
        {
            CrosshairWorldPosition = ray.GetPoint(998);
        }

    }
    public void Fire(Player.WeaponType currentWeapon)
    {
        Vector3 aimDirection = (CrosshairWorldPosition - ProjectileSpawnPoint.position).normalized;
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
        Vector3 aimDirection = (CrosshairWorldPosition - ProjectileSpawnPoint.position).normalized;
        ShootingProjectile proj = Instantiate(projectile, ProjectileSpawnPoint.position, Quaternion.LookRotation(aimDirection, Vector3.up));
        GunTimer = Time.time;
        }

    }
    public void HandAttack(Player.WeaponType meleeWeapon)
    {
        Vector3 aimDirection = (CrosshairWorldPosition - ProjectileSpawnPoint.position).normalized;
        player.RotatePlayerOnShoot(aimDirection);
        List<HpSystemCollision> targets = new();
        if(handHitbox.GetEnemies() != null)
        {
        targets.AddRange(handHitbox.GetEnemies());
        }
        int meleeDamage = 1;
        if(meleeWeapon == Player.WeaponType.Knife)
        {
            meleeDamage = 2;
        }
        if(targets.Count > 0)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                targets[i].TakeDamage(meleeDamage);
            }
        }
        handHitbox.EndAttack();
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Ray ray = Camera.main.ScreenPointToRay(Crosshair.transform.position);
    //    Gizmos.DrawRay(RaycastOrigin.position, ray.direction * 999);
    //}
}
