using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public event System.Action<int> OnStart;
    public event System.Action<int> OnBulletReduce;

    [SerializeField] private WeaponType currentWeaponType;
    [SerializeField] private int totalBullets;
    [SerializeField] private GunData[] gunData;

    private GameObject bullet;
    private GunData currentGun;
    private int currentBullet;

    private void OnEnable() 
    {
        currentBullet = totalBullets;
    }

    private void Start()
    {
        bullet = Resources.Load<GameObject>("Prefabs/Bullet");
        Player.Instance.OnShoot += Shoot;
        GunSetup();
    }

    private void GunSetup()
    {
        foreach (GunData gun in gunData)
        {
            gun.gunPrefab.SetActive(false);
            if (gun.weaponType == currentWeaponType)
            {
                currentGun = gun;
                currentGun.gunPrefab.SetActive(true);
                currentGun.lineRenderer.enabled = false;
            }
        }
    }

    private void Shoot()
    {
        AudioManager.Instance.Play(currentGun.soundType);
        ReduceBullet();

        foreach(Transform bulletPoint in currentGun.bulletSpawnPoints)
        {
            if(bulletPoint == null) continue;

            if(Physics.Raycast(bulletPoint.position, bulletPoint.forward, out RaycastHit hit))
            {
                Vector3 direction = (hit.point - bulletPoint.position).normalized;
                direction.y = 0f;
                GameObject bulletInstance = PoolManager.instance.ReleaseFromThePool(this.bullet, bulletPoint.position, Quaternion.LookRotation(direction));

                if(bulletInstance.TryGetComponent<Bullet>(out Bullet bullet))
                {
                    bullet.SetUp(direction);
                }
            }            
        }

        OnBulletReduce.Invoke(currentBullet);
        Debug.Log("Bullet Percent : " + GetBulletPercent());
    }

    private void ReduceBullet()
    {
        currentBullet--;
    }            

    public GunData GetCurrentGun
    {
        get
        {
            return currentGun;
        }
    }
    
    public WeaponType GetCurrentWeaponType()
    {
        return currentWeaponType;
    }

    public int GetTotalBullet() => totalBullets;
    public int GetCurrentBullet() => currentBullet;
    public float GetBulletPercent()
    {
        return (float)currentBullet / totalBullets;
    }

}
