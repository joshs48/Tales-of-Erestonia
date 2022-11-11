using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Spell", order = 4)]

public class SpellData : ScriptableObject
{
    public enum Class
    {
        Melee_Attack,
        Ranged_Attack,
        Area_of_Effect,
        Healing,
        Protection
    }
    public enum Type
    {
        Aim,
        Targeted
    }

    public string spellName;//Fireball, Erupting Earth...
    public GameObject icon;
    public GameObject prefab;
    public string spellDescription;//description


    public int shieldVal;
    public int damageVal;
    public string damageEffects;
    public int effectDuration;
    public int damagePerSec;
    public ParticleSystem damageEffect;
    public int healVal;
    public int speedVal;


    public Class spellClass;
    public Type spellType;// aim, targeted
    public int spellLevel;

}
