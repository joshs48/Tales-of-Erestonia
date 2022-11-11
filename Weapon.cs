using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public WeaponData data;

    private GameObject Parent;
    private Animator anim;

    private int GroundedTagHash;
    private int AirborneTagHash;

    private int PrevState = 0;
    private bool PrevShieldVal;

    private void Start()
    {

        Parent = transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.gameObject;
        if (Parent.name.Equals("Characters"))
        {
            Parent = Parent.transform.parent.gameObject;
        }
        anim = Parent.GetComponent<Animator>();
        GroundedTagHash = Animator.StringToHash("Grounded");
        AirborneTagHash = Animator.StringToHash("Airborne");


    }


    private void OnTriggerEnter(Collider other)
    {

        if (anim.GetCurrentAnimatorStateInfo(0).tagHash != GroundedTagHash && anim.GetCurrentAnimatorStateInfo(0).tagHash != AirborneTagHash)// Checks to see if it's not in grounded or jumping
        {
            if (other.gameObject.layer.Equals(6) && Parent.layer.Equals(3) || (other.gameObject.layer.Equals(3) && Parent.layer.Equals(6)))//This is the enemy and player layer, it checks if they're an enemy
            {
                if (anim.GetCurrentAnimatorStateInfo(0).tagHash != PrevState)
                {
                    StatsManager statsManager = other.gameObject.GetComponent<StatsManager>();
                    statsManager.DealDamage(data.damageVal);
                    if (data.damageEffects.Length > 0)
                    {
                        statsManager.EffectDuration = data.effectDuration;
                        statsManager.DamagePerSec = data.damagePerSec;
                        statsManager.effect = data.damageEffect;

                        statsManager.NewEffects.Add(data.damageEffects);

                    }
                    PrevState = anim.GetCurrentAnimatorStateInfo(0).tagHash;
                }
            }
        }




    }



    private void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).tagHash == GroundedTagHash)
        {
            PrevState = -1;
        }
        if (anim.GetBool("Shield") && !PrevShieldVal)
        {
            Parent.GetComponent<StatsManager>().tempShield += data.shieldVal;
            Parent.GetComponent<BaseCharacter>().canMove = false;
        }
        if (!anim.GetBool("Shield") && PrevShieldVal)
        {
            Parent.GetComponent<StatsManager>().tempShield -= data.shieldVal;
            Parent.GetComponent<BaseCharacter>().canMove = true;

        }
        PrevShieldVal = anim.GetBool("Shield");
    }







}
