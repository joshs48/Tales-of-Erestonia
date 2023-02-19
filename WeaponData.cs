using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Weapon", order = 1)]

public class WeaponData : ScriptableObject
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
        Hammer
    };

    public enum Owner
    {
        Player,
        Enemy,
        NPC
    }

    public string weaponName;
    public GameObject icon;//pic for ui
    public GameObject prefab;//what gets instantiated if something does
    public string weaponDescription;//description


    public Rarity weaponRarity;//Common/Rare weapons use numbers built in magic have extra effects

    public int weaponCost;// gp

    public int shieldVal;
    public int damageVal;
    public string damageEffects;
    public int effectDuration;
    public int damagePerSec;
    public ParticleSystem damageEffect;

    public Type weaponType;// Sword, Bow...

    public string location;// hands, right hand, head, legs...

    //public Owner weaponOwner;
}
