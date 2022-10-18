using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public event System.Action OnDead;
    [SerializeField] private int maxHealth;
    [SerializeField] private UnityEvent<float> OnDealDamage;

    private int currentHealth;
    private bool isDead;

    private void Start() 
    {
        currentHealth = maxHealth;
    }

    public void DealDamage(int damage)
    {
        StartCoroutine(Utils.HitImpact(GetComponentInChildren<SkinnedMeshRenderer>(), Color.red, Color.white, 5f));
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        OnDealDamage?.Invoke(GetHealthPercent);
        if(currentHealth == 0) 
        {
            OnDead?.Invoke();
            isDead = true;
            return;
        }
    }

    private float GetHealthPercent => (float)currentHealth / maxHealth;

    public bool IsDead()
    {
        return isDead;
    }

}
