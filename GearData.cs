using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Gear", order = 2)]


public class GearData : ScriptableObject
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
        Potion
    };


    public enum Owner
    {
        Player,
        Enemy,
        NPC
    }

    public string gearName;
    public GameObject icon;//pic for ui
    public GameObject prefab;//what gets instantiated if something does
    public string gearDescription;//description


    public Rarity gearRarity;//Common/Rare gears use numbers built in magic have extra effects

    public int gearCost;// gp

    public int shieldVal;
    public int damageVal;
    public string damageEffects;
    public int effectDuration;
    public int damagePerSec;
    public ParticleSystem damageEffect;
    public int healVal;

    public Type gearType;// Potion, Gem


    public Owner gearOwner;

    public int quantity;
}
