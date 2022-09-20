using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public string abilityName;//Fireball, Erupting Earth...
    public string abilityDescription;//description


    public int shieldVal;
    public int damageVal;
    public string damageEffects;
    public int effectDuration;
    public int damagePerSec;
    public ParticleSystem damageEffect;
    public int healVal;
    public int speedVal;


    public string abilityClass;//Attack, Healing, Protection...
    public string abilityType;// aim, targeted
    public int abilityLevel;

    public GameObject icon;

    public void ActivateAbility()
    {
        Debug.Log(abilityName + " should activate here");
    }
}
