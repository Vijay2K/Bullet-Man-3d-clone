using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 30f;
    [SerializeField] private int bulletDamage = 10;

    private ParticleSystem hitImpactParticles;
    private ParticleSystem bloodParticles;
    private Vector3 direction;
    private Rigidbody rb;

    private const int maxCollisionHit = 5;
    private int collisionHitCount;

    private void Start() 
    {
        bloodParticles = Resources.Load<ParticleSystem>("Prefabs/BloodParticle");
        hitImpactParticles = Resources.Load<ParticleSystem>("Prefabs/HitImpactParticle");
        rb = GetComponent<Rigidbody>();
    }

    private void Update() 
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void FixedUpdate() 
    {
        rb.velocity = direction * bulletSpeed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.CompareTag("Bullet")) return;

        collisionHitCount++;
        
        if(other.gameObject.TryGetComponent<Enemy>(out Enemy enemy)) 
        {
            PoolManager.instance.ReleaseFromThePool(bloodParticles.gameObject, transform.position, Quaternion.identity);

            if(enemy.TryGetComponent<Health>(out Health health))
            {
                health.DealDamage(bulletDamage);
                PoolManager.instance.ReturnToPool(this.gameObject);
            }
            else
            {
                enemy.TakeDamage();
            }
        }

        if(other.gameObject.TryGetComponent<IInteractables>(out IInteractables interactables))
        {
            interactables.Interact();
        }

        if(collisionHitCount > maxCollisionHit)
        {
            collisionHitCount = 0;
            PoolManager.instance.ReturnToPool(this.gameObject);
        }

        if(other.gameObject.CompareTag("Wall"))
        {
            AudioManager.Instance.Play(SoundType.BulletHit);
            Vector3 hitNormal = other.contacts[0].normal;
            PoolManager.instance.ReleaseFromThePool(hitImpactParticles.gameObject, transform.position, Quaternion.identity);
            Vector3 newDir = Vector3.Reflect(direction, hitNormal).normalized;
            newDir.y = 0f;
            direction = newDir;
        }
    }

    public void SetUp(Vector3 direction)
    {
        this.direction = direction;
    }

}
