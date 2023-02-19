using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconManager : MonoBehaviour
{
    public enum Type
    {
        Axe,
        Bow,
        Crossbow,
        Shield,
        Staff,
        Sword,
        Potion,
        Spell,
        Ability,
        Clothing,
        Hammer
    };

    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    };

    public enum Class
    {
        Weapon,
        Ammo,
        Gear,
        Spell,
        Clothing,
        Ability
    };

    public string Name;//Sword Basic, Potion Healing
    public string Description;//description

    public Rarity rarity;//Common/Rare items use numbers built in magic have extra effects

    public int Cost;// gp

    public Class objectClass;//Weapon, Gear
    public Type objectType;// Sword, Bow, Potion...

    public string Location;// hands, right hand, head, legs...

    public WeaponData Weapon;
    public GearData Gear;
    public AmmoData Ammo;
    public SpellData Spell;
    public ClothingData Clothing;
    public AbilityData Ability;
}
