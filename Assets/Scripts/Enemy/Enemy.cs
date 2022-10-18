using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private enum EnemyState { Idle, ChaseState, PatrolState }

    [SerializeField] private EnemyState enemyState;
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private PatrolPath patrolPath;

    public static event System.Action<Enemy> OnEnemySpawned;
    public static event System.Action<Enemy> OnEnemyDead;

    private readonly int walkStateHash = Animator.StringToHash("Walking");
    private Player player;
    private Health health;
    private int currentWaypointIndex;
    private Vector3 nextPosition;

    private void Start() 
    {
        if(enemyState == EnemyState.ChaseState)
        {
            health = GetComponent<Health>();
            player = FindObjectOfType<Player>();
            animator.Play(walkStateHash);
            health.OnDead += TakeDamage;
        }

        if(enemyState == EnemyState.PatrolState)
        {
            animator.Play(walkStateHash);
        }

        OnEnemySpawned?.Invoke(this);
        SetRigidbodyState(true);
        SetColliderState(false);
    }

    private void Update() 
    {
        if(enemyState == EnemyState.ChaseState)
        {
            if(agent.enabled && !health.IsDead())
            {
                agent.SetDestination(player.transform.position);
            }
        }

        if(enemyState == EnemyState.PatrolState)
        {
            if(agent.enabled && patrolPath != null)
            {
                nextPosition = transform.position;
                if(AtWaypoint())
                {
                    CycleWaypoint();
                }

                nextPosition = GetCurrentWaypoint();

                agent.SetDestination(nextPosition);
            }
        }
    }

    private bool AtWaypoint()
    {
        return Vector3.Distance(transform.position, GetCurrentWaypoint()) < 0.1f;
    }

    private Vector3 GetCurrentWaypoint()
    {
        return patrolPath.GetWayPoint(currentWaypointIndex);
    }

    private void CycleWaypoint()
    {
        currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
    }

    public void TakeDamage()
    {
        StartCoroutine(Utils.HitImpact(GetComponentInChildren<SkinnedMeshRenderer>(),Color.red ,Color.white, 5f));
        OnEnemyDead?.Invoke(this);
        if(agent != null)
        {
            agent.enabled = false;
        }
        GetComponent<Animator>().enabled = false;
        SetRigidbodyState(false);
        SetColliderState(true);

        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        ApplyExplosionToRagdoll(transform, 300f, transform.position + randomDirection, 10f);
    }

    private void SetRigidbodyState(bool state)
    {
        Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody rb in rbs)
        {
            rb.isKinematic = state;
        }
    }

    private void SetColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach(Collider collider in colliders)
        {
            collider.enabled = state;
        }

        GetComponent<Collider>().enabled = !state;
    }   

    private void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach(Transform child in root)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidBody))
            {
                childRigidBody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }

            ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRange);
        }
    } 
}
