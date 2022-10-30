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
}
