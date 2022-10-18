using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    [SerializeField] private float sensitivity = 2f;

    public event Action OnShoot;

    private bool canAim;
    private int currentBullet;
    private Shooting shoot;
    private bool isHitByEnemy;

    private void Awake() 
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    private void Start() 
    {
        shoot = GetComponent<Shooting>();
    }

    private void Update()
    {
        if(shoot.GetCurrentBullet() <= 0) return;
        if(isHitByEnemy) return;

        if(GameManager.Instance.GetEnemyCount == 0) return;

        if(Input.GetMouseButtonDown(0))
        {
            AimSetUp(true);
        }
        if(Input.GetMouseButtonUp(0))
        {            
            AimSetUp(false);
            OnShoot?.Invoke();
        }

        if(canAim)
        {
            Rotate();
            StartAim();
        }
    }

    private void AimSetUp(bool state)
    {
        canAim = state;
        shoot.GetCurrentGun.lineRenderer.enabled = state;
    }

    private void StartAim()
    {
        if(Physics.Raycast(shoot.GetCurrentGun.leaserAimPoint.position, shoot.GetCurrentGun.leaserAimPoint.forward, out RaycastHit hit))
        {
            if(hit.collider)
            {
                DrawLeaser(shoot.GetCurrentGun.leaserAimPoint.position, hit.point);
            }
        }
    }

    private void DrawLeaser(Vector3 startPoint, Vector3 targetPoint)
    {
        shoot.GetCurrentGun.lineRenderer.SetPosition(0, startPoint);
        shoot.GetCurrentGun.lineRenderer.SetPosition(1, targetPoint);
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * sensitivity);
    }

    private Ray GetRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }


    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            isHitByEnemy = true;
        }    
    }

    public bool IsHitByEnemy => isHitByEnemy;

    public Shooting Gun => shoot;
}
