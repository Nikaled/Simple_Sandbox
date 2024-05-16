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
    [SerializeField] private Transform PistolProjectileSpawnPoint;
    [SerializeField] private Transform GunProjectileSpawnPoint;
    [SerializeField] ShootingProjectile projectile;
    [SerializeField] public Image Crosshair;
    [SerializeField] Player player;
    [SerializeField] MeleeAttackHitbox handHitbox;
    Vector3 AimDirection;
    [HideInInspector] public Vector3 CrosshairWorldPosition;
    float GunTimer;
    float GunShootInterval = 0.05f;
    public static PlayerShooting instance;
    [SerializeField] public LineRenderer lineRenderer;

    private void Start()
    {
        instance = this;
    }
    public bool Reloading { get; private set; }

    void Update()
    {
        CrosshairWorldPosition = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 2000, aimColliderLayerMask))
        {
            CrosshairWorldPosition = raycastHit.point;
            if (player.CurrentWeapon == Player.WeaponType.Pistol)
            {
                lineRenderer.SetPosition(0, PistolProjectileSpawnPoint.position);
            }
            if (player.CurrentWeapon == Player.WeaponType.Gun)
            {
                lineRenderer.SetPosition(0, GunProjectileSpawnPoint.position);
            }
            lineRenderer.SetPosition(1, raycastHit.point);
        }
        else
        {
            CrosshairWorldPosition = ray.GetPoint(1998);
        }
        

    }
    public void Fire(Player.WeaponType currentWeapon)
    {
        if (currentWeapon == Player.WeaponType.Pistol)
        {
            Vector3 aimDirection = (CrosshairWorldPosition - PistolProjectileSpawnPoint.position).normalized;
            player.RotatePlayerOnShoot(aimDirection);
            ShootingProjectile proj = Instantiate(projectile, PistolProjectileSpawnPoint.position, Quaternion.LookRotation(aimDirection, Vector3.up));

        }

        if (currentWeapon == Player.WeaponType.Gun)
        {
            FireGun();
        }
        LockPlayerMovement();
    }
    private void LockPlayerMovement()
    {
        var cor = player.LockPositionOnShoot();
        if (cor != null)
        {
            StopCoroutine(cor);
        }
        StartCoroutine(cor);
    }
    public void FireGun()
    {
        Vector3 aimDirection = (CrosshairWorldPosition - GunProjectileSpawnPoint.position).normalized;
        player.RotatePlayerOnShoot(aimDirection);
        if (Time.time - GunTimer > GunShootInterval)
        {
            Reloading = false;
        }
        if (Reloading == false)
        {
            Reloading = true;
           
            ShootingProjectile proj = Instantiate(projectile, GunProjectileSpawnPoint.position, Quaternion.LookRotation(aimDirection, Vector3.up));
            GunTimer = Time.time;
        }

    }
    public void HandAttack(Player.WeaponType meleeWeapon)
    {
        Vector3 aimDirection = (CrosshairWorldPosition - PistolProjectileSpawnPoint.position).normalized;
        player.RotatePlayerOnShoot(aimDirection);
        //LockPlayerMovement();
        List<HpSystemCollision> targets = new();
        if (handHitbox.GetEnemies() != null)
        {
            targets.AddRange(handHitbox.GetEnemies());
        }
        int meleeDamage = 1;
        if (meleeWeapon == Player.WeaponType.Knife)
        {
            meleeDamage = 2;
        }
        if (targets.Count > 0)
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
