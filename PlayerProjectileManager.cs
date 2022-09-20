using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileManager : MonoBehaviour
{
    public int damageVal;
    public string damageEffects;
    public int EffectDuration;
    public int DamagePerSec;
    public ParticleSystem damageEffect;

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
        

        if (other.GetComponent<ParticleSystem>() == null && !other.layer.Equals(3))
        {
            
            if (other.layer.Equals(6))
            {
                

                StatsManager statsManager = other.GetComponent<StatsManager>();
                statsManager.DealDamage(damageVal);


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

    



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ParticleSystem>() == null && !other.gameObject.layer.Equals(3))
        {

            if (other.gameObject.layer.Equals(6))
            {


                StatsManager statsManager = other.gameObject.GetComponent<StatsManager>();
                statsManager.DealDamage(damageVal);


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
                Destroy(gameObject);
            }
        }
    }

    IEnumerator delayStop(float time, Collider other)
    {
        yield return new WaitForSeconds(time);
        GetComponent<Rigidbody>().isKinematic = true;
        transform.parent = other.gameObject.transform;
    }

    IEnumerator destroySequence()
    {
        
        yield return new WaitForSeconds(20);
        
        Destroy(this.gameObject);
    }

    
}
