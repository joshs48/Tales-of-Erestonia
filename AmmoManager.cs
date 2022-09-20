using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    public string ammoName;//Flaming Arrows, Frozen Bolts...
    public GameObject icon;//icon
    public string ammoDescription;//description

    public string ammoRarity;//Common/Rare items use numbers built in magic have extra effects

    public int ammoCost;// gp

    public float damageMult;
    public string damageEffects;
    public int effectDuration;
    public int damagePerSec;
    public ParticleSystem damageEffect;

    public int quantity = 1;

    public string ammoType;// arrow, bolt

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
