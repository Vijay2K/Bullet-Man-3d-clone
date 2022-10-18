using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableParticle : MonoBehaviour
{
    private void Update() 
    {
        if(!GetComponent<ParticleSystem>().IsAlive())
        {
            if(transform.parent != null)
            {
                PoolManager.instance.ReturnToPool(transform.parent.gameObject);
            }
            PoolManager.instance.ReturnToPool(this.gameObject);
        }    
    }
}
