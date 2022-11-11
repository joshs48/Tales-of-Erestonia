using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Ability", order = 5)]

public class AbilityData : ScriptableObject
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
        Instant,
        Active,
        Continuous
    }

    public string abilityName;//Fireball, Erupting Earth...
    public GameObject icon;
    public GameObject prefab;
    public string abilityDescription;//description


    public int shieldVal;
    public int damageVal;
    public string damageEffects;
    public int effectDuration;
    public int damagePerSec;
    public ParticleSystem damageEffect;
    public int healVal;
    public int speedVal;


    public Class abilityClass;//Attack, Healing, Protection...
    public Type abilityType;// instant, active, continuous
    public int abilityLevel;


}
