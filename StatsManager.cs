using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public enum Classes
    {
        Bard,
        Druid,
        Warlock,
        Wizard,
        Fighter,
        Ranger,
        Rogue,
        Monk,
        Barbarian,
        Cleric,
        Paladin,
        Tinkerer
    };
    public enum Races
    {
        Tiefling,
        Elf,
        Human,
        Bremri,
        Dwarf
    };

    public enum BodyTypes
    {
        Male,
        Female
    }

    public enum Weapons
    {
        Axe,
        Bow,
        Crossbow,
        Hammer,
        Shield,
        Staff,
        Sword,
        DoubleSword
    }

    public Classes Class;
    public Races Race;
    public BodyTypes bodyType;
    public List<Weapons> usableWeapons;

    public int maxHealth;
    public int health;
    public int maxStamina;
    public int stamina;
    public int staminaRechargeSpeed;
    public bool isRechargingStamina;
    
    public int XP;
    public int nextLevelXP;
    public int minLevelXP = 0;
    public int Level;

    [HideInInspector] public bool isMagician;

    public int baseShield = 0;// This is stuff like armor which is always active
    public int tempShield = 0;// This temporary things(shields)
    private int totalShield = 0;// the total shield val

    public int STR;
    public int DEX;
    public int CON;
    public int INT;
    public int WIS;
    public int CHA;

    public ParticleSystem effect;
    public List<string> NewEffects = new List<string>();
    public List<string> CurrentEffects = new List<string>();

    public List<ParticleSystem> CurrentParticleEffects = new List<ParticleSystem>();

    public int DamagePerSec = 0;
    public int EffectDuration = 0;

    public int TotalDPS = 0;

    public GameObject healFX;
    public GameObject levelUpFX;

    


    private InventoryManager inv;


    // Start is called before the first frame update
    void Start()
    {
        inv = GetComponent<InventoryManager>();
        if (gameObject.name == "Player")
        {
            GetComponent<CharacterAndWeaponController>().setCharacter(Class.ToString());
        }

        switch (Class.ToString())
        {
            case "Druid":
            case "Warlock":
            case "Wizard":
                isMagician = true;
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        AddEffects();
        totalShield = baseShield + tempShield;

    }

    

    private void AddEffects()
    {
        ParticleSystem newEffect;
        if (NewEffects.Contains("Burning"))
        {
            if (!CurrentEffects.Contains("Burning"))
            {

                newEffect = Instantiate(effect, transform.position, transform.rotation, transform);

                CurrentParticleEffects.Add(newEffect);
            }
            CurrentEffects.Add("Burning");
            NewEffects.Remove("Burning");
            StartCoroutine(EffectLife(EffectDuration, "Burning", DamagePerSec));
        }

        if (NewEffects.Contains("Poisoned"))
        {
            if (!CurrentEffects.Contains("Poisoned"))
            {
                newEffect = Instantiate(effect, transform.position, transform.rotation, transform);

                CurrentParticleEffects.Add(newEffect);
            }
            CurrentEffects.Add("Poisoned");
            NewEffects.Remove("Poisoned");
            StartCoroutine(EffectLife(EffectDuration, "Poisoned", DamagePerSec));
        }

        if (NewEffects.Contains("Beam"))
        {
            CurrentEffects.Add("Beam");
            NewEffects.Remove("Beam");
            StartCoroutine(EffectLife(EffectDuration, "Beam", DamagePerSec));
        }
    }



    IEnumerator DamageOverTime()
    {

        while (TotalDPS > 0)
        {
            yield return new WaitForSeconds(1);
            health -= TotalDPS;
        }
        DamagePerSec = 0;
        EffectDuration = 0;

    }

    IEnumerator EffectLife(int time, string effect, int dps)
    {
        if (TotalDPS > 0)
        {
            TotalDPS += dps;
        }
        else
        {
            TotalDPS = dps;
            StartCoroutine(DamageOverTime());
        }
        yield return new WaitForSeconds(time);
        TotalDPS -= dps;
        int index = CurrentEffects.IndexOf(effect);
        CurrentEffects.Remove(effect);

        if (!CurrentEffects.Contains(effect) && effect != "Beam")
        {
            Destroy(CurrentParticleEffects[index].gameObject);
            CurrentParticleEffects.RemoveAt(index);
        }

    }
    public void DealDamage(int damage, GameObject source)
    {
        if (damage - totalShield > 0)
        {
            health -= damage - totalShield;
            if (health <= 0)
            {
                if (gameObject.layer != 3)
                {
                    source.GetComponent<StatsManager>().GiveXP(XP);

                    Destroy(gameObject);

                }
            }


        }
        Debug.Log(gameObject.name + health);

        if (gameObject.name.Equals("Player"))
        {
            if (damage < 10)
            {
                GetComponent<PlayerControl>().CreateRumble(0.5f, 0.5f, 0.1f);
                CameraShake.Instance.ShakeCamera(1f, 0.2f, false);

            }
            else if (damage < 15)
            {
                GetComponent<PlayerControl>().CreateRumble(0.5f, 0.75f, 0.1f);
                CameraShake.Instance.ShakeCamera(3f, 0.2f, true);

            }
            else
            {
                GetComponent<PlayerControl>().CreateRumble(0.75f, 0.9f, 0.1f);
                CameraShake.Instance.ShakeCamera(5f, 0.2f, true);

            }
        } else
        {
            if (source != null)
            {
                GetComponent<AICharacter>().SetTarget(source.transform);
            }
        }
    }

    public void GiveHealth(int heal)
    {
        Instantiate(healFX, transform);

        if (heal + health > maxHealth)
        {
            StartCoroutine(SmoothHeal(maxHealth - health, 0.1f));
        }
        else
        {
            StartCoroutine(SmoothHeal(heal, 0.1f));
        }
    }

    private void CheckIsAlive()
    {
        
    }

    public void AddTemporaryShield(int shieldVal, int time)
    {
        StartCoroutine(AddTempShield(shieldVal, time));
    }

    public void ChangeSpeed(int changeVal)
    {
        GetComponent<BaseCharacter>().m_MoveSpeedMultiplier += changeVal * .05f;
        GetComponent<BaseCharacter>().m_MovingTurnSpeed += changeVal * 10;
        GetComponent<BaseCharacter>().m_StationaryTurnSpeed += changeVal * 10;

    }
    IEnumerator AddTempShield(int shieldVal, int time)
    {
        tempShield += shieldVal;
        yield return new WaitForSeconds(time);
        tempShield -= shieldVal;
    }

    IEnumerator SmoothHeal(int val, float time)
    {
        for (int i = 0; i < val; i++)
        {
            health += 1;
            yield return new WaitForSeconds(time);
        }

    }

    public void startRechargeStamina()
    {
        StartCoroutine(RechargeStamina());
    }

    IEnumerator RechargeStamina()
    {
        isRechargingStamina = true;

        yield return new WaitForSeconds(0.1f);
        while (stamina < maxStamina)
        {

            if (stamina + staminaRechargeSpeed < maxStamina)
            {
                stamina += staminaRechargeSpeed;
            }
            else
            {
                stamina += maxStamina - stamina;
            }
            yield return new WaitForSeconds(0.1f);
        }
        isRechargingStamina = false;
    }


    public void GiveXP(int xpAmount)
    {

        StartCoroutine(SmoothXP(xpAmount, 0.1f));

        CheckLevelUp();
    }

    IEnumerator SmoothXP(int val, float time)
    {
        for (int i = 0; i < val; i++)
        {
            XP += 1;
            yield return new WaitForSeconds(time);
        }

    }

    private void CheckLevelUp()
    {
        if (XP < 50)
        {
            Level = 1;
            nextLevelXP = 50;
            minLevelXP = 0;
        }
        else if (XP < 125)
        {
            Level = 2;
            nextLevelXP = 125;
            minLevelXP = 50;

            Instantiate(levelUpFX, transform);
        }//TODO add other levels
        UIUpdater.UpdateCharacterStatsBar();

    }


}
