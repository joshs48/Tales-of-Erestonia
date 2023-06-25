using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileManager : MonoBehaviour
{
    public int damageVal;
    public string damageEffects;
    public int EffectDuration;
    public int DamagePerSec;
    public ParticleSystem damageEffect;

    private GameObject prevParticles;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(destroySequence());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    
    private void OnParticleCollision(GameObject other)
    {
        if (other.GetComponent<ParticleSystem>() == null && !other.layer.Equals(6) && !gameObject.Equals(prevParticles))
        {
            if (other.gameObject.layer.Equals(3) || other.gameObject.layer.Equals(9))
            {
                
                prevParticles = gameObject;
                StatsManager statsManager = other.GetComponent<StatsManager>();
                statsManager.DealDamage(damageVal, null);
                

                if (damageEffects.Length > 0)
                {
                    statsManager.EffectDuration = EffectDuration;
                    statsManager.DamagePerSec = DamagePerSec;
                    if (damageEffect != null)
                    {
                        statsManager.effect = damageEffect;
                    }
                    
                    statsManager.NewEffects.Add(damageEffects);
                    

                }
            }
        }
    }



    IEnumerator delayStop(float time, Collider other)
    {
        yield return new WaitForSeconds(time);
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.transform.parent = other.gameObject.transform;
    }

    IEnumerator destroySequence()
    {
        
        yield return new WaitForSeconds(20);
        
        Destroy(this.gameObject);
    }
}
