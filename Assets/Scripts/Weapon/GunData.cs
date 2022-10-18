using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    SingleShotGun, MultipleShootGun 
}

[System.Serializable]
public class GunData
{
    public WeaponType weaponType;
    public GameObject gunPrefab;
    public SoundType soundType;
    public LineRenderer lineRenderer;
    public Transform leaserAimPoint;
    public Transform[] bulletSpawnPoints;
}
