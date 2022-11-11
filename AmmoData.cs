using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Ammo", order = 3)]

public class AmmoData : ScriptableObject
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
        Arrow,
        Bolt
    };




    public string ammoName;//Flaming Arrows, Frozen Bolts...
    public GameObject icon;//icon
    public GameObject prefab;//what gets instantiated
    public string ammoDescription;//description

    public Rarity ammoRarity;//Common/Rare items use numbers built in magic have extra effects

    public int ammoCost;// gp

    public float damageMult;
    public string damageEffects;
    public int effectDuration;
    public int damagePerSec;
    public ParticleSystem damageEffect;

    public int quantity = 1;

    public Type ammoType;// arrow, bolt
}
