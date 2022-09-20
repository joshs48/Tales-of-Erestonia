using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public string spellName;//Fireball, Erupting Earth...
    public string spellDescription;//description


    public int shieldVal;
    public int damageVal;
    public string damageEffects;
    public int effectDuration;
    public int damagePerSec;
    public ParticleSystem damageEffect;
    public int healVal;
    public int speedVal;


    public string spellClass;//Attack, Healing, Protection...
    public string spellType;// aim, targeted
    public int spellLevel;

    public GameObject icon;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
