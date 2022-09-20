using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothingManager : MonoBehaviour
{
    
    public enum Classes
    {
        Barbarian,
        Fighter,
        Rogue,
        Cleric,
        Paladin,
        Druid,
        Ranger,
        Bard,
        Warlock,
        Wizard,
        Tinkerer
    };

    public enum GenderType
    {
        Has_Gender,
        All_Gender
    };

    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    };

    public enum ClothingSlot
    {
        Head_Covering_Base,
        Head_Covering_No_Facial_Hair,
        Head_Covering_No_Hair,
        Hair,
        Hips_Attachment,
        Ears,
        Face,
        Helmet,
        Eyebrows,
        Facial_Hair,
        Torso,
        Hand,
        Leg,
        Boot
    };

    public enum ClothingTags
    {
        Acolyte,
        Charltan,
        Criminal,
        Entartainer,
        Artisan,
        Hermit,
        Noble,
        Outlander,
        Sage,
        Soldier,
        Urchin,
        None

    };

    public Classes[] boostedClasses;
    public List<ClothingTags> tags;//something so that if you have a lot of desert or forest or knight clothing you get boost 
    public GenderType genderType;
    public Rarity rarity;
    public ClothingSlot slot;

    public string clothingName;
    public GameObject maleIcon;//pic for ui
    public GameObject femaleIcon;
    public string description;//description


    public int cost;
    public int armorVal;
    public int speedVal;//for boots this is postiive, for heavy armor it's negative, the total sum is your speed
    
    


}
