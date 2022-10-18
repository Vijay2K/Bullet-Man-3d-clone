using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour, IInteractables
{
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionForce;
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private LayerMask enemyLayer;

    public void Interact()
    {
        AudioManager.Instance.Play(SoundType.Explosion);
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);
        PoolManager.instance.ReleaseFromThePool(explosionParticle.gameObject, transform.position + Vector3.up * 2f, Quaternion.identity);

        foreach(Collider collider in colliders)
        {
            if(collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                PoolManager.instance.ReleaseFromThePool(Resources.Load<ParticleSystem>("Prefabs/BloodParticle").gameObject, enemy.transform.position + Vector3.up * 2f, Quaternion.identity);
                enemy.TakeDamage();
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
