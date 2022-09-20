using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    };

    public enum Type
    {
        Axe,
        Bow,
        Crossbow,
        Shield,
        Staff,
        Sword,
        Potion
    };


    public string itemName;//Sword Basic, Potion Healing
    public GameObject icon;//pic for ui
    public string itemDescription;//description


    public Rarity itemRarity;//Common/Rare items use numbers built in magic have extra effects

    public int itemCost;// gp

    public int shieldVal;
    public int damageVal;
    public string damageEffects;
    public int effectDuration;
    public int damagePerSec;
    public ParticleSystem damageEffect;
    public int healVal;

    public string itemClass;//Weapon, Gear
    public Type itemType;// Sword, Bow, Potion...

    public string location;// hands, right hand, head, legs...

    public string itemOwner;
    public int quantity = 1;

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
        if (itemClass == "Weapon" && itemType.ToString() != "Bow" && itemType.ToString() != "Crossbow" && itemType.ToString() != "Shield")
        {
            if (anim.GetCurrentAnimatorStateInfo(0).tagHash != GroundedTagHash && anim.GetCurrentAnimatorStateInfo(0).tagHash != AirborneTagHash)// Checks to see if it's not in grounded or jumping
            {
                if ((other.gameObject.layer.Equals(6) && itemOwner.Equals("Player")) || (other.gameObject.layer.Equals(3) && itemOwner.Equals("Enemy")))//This is the enemy and player layer, it checks if they're an enemy
                {
                    if (anim.GetCurrentAnimatorStateInfo(0).tagHash != PrevState)
                    {
                        StatsManager statsManager = other.gameObject.GetComponent<StatsManager>();
                        statsManager.DealDamage(damageVal);
                        if (damageEffects.Length > 0)
                        {
                            statsManager.EffectDuration = effectDuration;
                            statsManager.DamagePerSec = damagePerSec;
                            statsManager.effect = damageEffect;
                            
                            statsManager.NewEffects.Add(damageEffects);
                            
                        }
                        PrevState = anim.GetCurrentAnimatorStateInfo(0).tagHash;
                    }
                }
            }

            
        }
 
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    private void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).tagHash == GroundedTagHash)
        {
            PrevState = -1;
        }
        if (anim.GetBool("Shield") && !PrevShieldVal)
        {
            Parent.GetComponent<StatsManager>().tempShield += shieldVal;
            Parent.GetComponent<BaseCharacter>().canMove = false;
        }
        if (!anim.GetBool("Shield") && PrevShieldVal)
        {
            Parent.GetComponent<StatsManager>().tempShield -= shieldVal;
            Parent.GetComponent<BaseCharacter>().canMove = true; 

        }
        PrevShieldVal = anim.GetBool("Shield");
    }







}
