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
        Bremri
    };

    public enum BodyTypes {
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

    [SerializeField] ClothingData[] BardBaseClothes;
    [SerializeField] GameObject[] BardBaseliteClothes;
    [SerializeField] Color[] BardColors;//primary, secondary
    [SerializeField] WeaponData[] BardBaseWeapons;
    [SerializeField] GearData[] BardBaseGear;
    [SerializeField] SpellData[] BardBaseSpells;
    [SerializeField] ClothingData[] BardExtraClothes;


    [SerializeField] ClothingData[] DruidBaseClothes;
    [SerializeField] GameObject[] DruidBaseliteClothes;
    [SerializeField] Color[] DruidColors;//primary, secondary
    [SerializeField] WeaponData[] DruidBaseWeapons;
    [SerializeField] GearData[] DruidBaseGear;
    [SerializeField] SpellData[] DruidBaseSpells;
    [SerializeField] ClothingData[] DruidExtraClothes;


    [SerializeField] ClothingData[] WarlockBaseClothes;
    [SerializeField] GameObject[] WarlockBaseliteClothes;
    [SerializeField] Color[] WarlockColors;//primary, secondary
    [SerializeField] WeaponData[] WarlockBaseWeapons;
    [SerializeField] GearData[] WarlockBaseGear;
    [SerializeField] SpellData[] WarlockBaseSpells;
    [SerializeField] ClothingData[] WarlockExtraClothes;


    [SerializeField] ClothingData[] WizardBaseClothes;
    [SerializeField] GameObject[] WizardBaseliteClothes;
    [SerializeField] Color[] WizardColors;//primary, secondary
    [SerializeField] WeaponData[] WizardBaseWeapons;
    [SerializeField] GearData[] WizardBaseGear;
    [SerializeField] SpellData[] WizardBaseSpells;
    [SerializeField] ClothingData[] WizardExtraClothes;


    [SerializeField] ClothingData[] FighterBaseClothes;
    [SerializeField] GameObject[] FighterBaseliteClothes;
    [SerializeField] Color[] FighterColors;//primary, secondary

    [SerializeField] ClothingData[] MonkBaseClothes;
    [SerializeField] GameObject[] MonkBaseliteClothes;
    [SerializeField] Color[] MonkColors;//primary, secondary

    [SerializeField] ClothingData[] RangerBaseClothes;
    [SerializeField] GameObject[] RangerBaseliteClothes;
    [SerializeField] Color[] RangerColors;//primary, secondary

    [SerializeField] ClothingData[] RogueBaseClothes;
    [SerializeField] GameObject[] RogueBaseliteClothes;
    [SerializeField] Color[] RogueColors;//primary, secondary

    [SerializeField] ClothingData[] BarbarianBaseClothes;
    [SerializeField] GameObject[] BarbarianBaseliteClothes;
    [SerializeField] Color[] BarbarianColors;//primary, secondary

    [SerializeField] ClothingData[] ClericBaseClothes;
    [SerializeField] GameObject[] ClericBaseliteClothes;
    [SerializeField] Color[] ClericColors;//primary, secondary

    [SerializeField] ClothingData[] PaladinBaseClothes;
    [SerializeField] GameObject[] PaladinBaseliteClothes;
    [SerializeField] Color[] PaladinColors;//primary, secondary

    [SerializeField] ClothingData[] TinkererBaseClothes;
    [SerializeField] GameObject[] TinkererBaseliteClothes;
    [SerializeField] Color[] TinkererColors;//primary, secondary


    [SerializeField] Material playerMat;
    [SerializeField] Color[] tieflingColors;//hair, skin, stubble, scar, eyes, bodyart
    [SerializeField] AbilityData[] TieflingBaseAbilities;
    [SerializeField] Color[] elfColors;
    [SerializeField] AbilityData[] ElfBaseAbilities;
    [SerializeField] Color[] bremriColors;
    [SerializeField] AbilityData[] BremriBaseAbilities;
    [SerializeField] Color[] humanSkins;
    [SerializeField] Color[] humanColors;
    [SerializeField] AbilityData[] HumanBaseAbilities;


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
        CheckIsAlive();
        totalShield = baseShield + tempShield;

    }

    public void SetUpStatsAndInv()
    {
        inv = GetComponent<InventoryManager>();
        Level = 1;
        XP = 0;
        ClothingData[] clothes = null;
        ClothingData[] extraClothes = null;
        WeaponData[] weapons = null;
        GearData[] gear = null;
        SpellData[] spells = null;
        AbilityData[] abilities = null;
        GameObject[] liteClothes = null;
        switch (Class)
        {
            case Classes.Bard:
                maxHealth += 9;
                health += 9;
                STR += -1;
                DEX += 2;
                CON += 1;
                INT += 0;
                WIS += 1;
                CHA += 2;
                clothes = BardBaseClothes;
                liteClothes = BardBaseliteClothes;
                usableWeapons.Add(Weapons.Sword);
                gear = BardBaseGear;
                weapons = BardBaseWeapons;
                spells = BardBaseSpells;
                extraClothes = BardExtraClothes;
                break;
            case Classes.Druid:
                maxHealth += 8;
                health += 8;
                STR += 1;
                DEX += 1;
                CON += 0;
                INT += 2;
                WIS += 2;
                CHA += -1;
                clothes = DruidBaseClothes;
                liteClothes = DruidBaseliteClothes;
                usableWeapons.Add(Weapons.Staff);
                gear = DruidBaseGear;
                weapons = DruidBaseWeapons;
                spells = DruidBaseSpells;
                extraClothes = DruidExtraClothes;

                break;
            case Classes.Warlock:
                maxHealth += 7;
                health += 70;
                STR += 0;
                DEX += 1;
                CON += -1;
                INT += 1;
                WIS += 2;
                CHA += 2;
                clothes = WarlockBaseClothes;
                liteClothes = WarlockBaseliteClothes;
                gear = WarlockBaseGear;
                weapons = WarlockBaseWeapons;
                spells = WarlockBaseSpells;
                extraClothes = WarlockExtraClothes;

                break;
            case Classes.Wizard:
                maxHealth += 6;
                health += 6;
                STR += -1;
                DEX += 1;
                CON += 0;
                INT += 2;
                WIS += 2;
                CHA += 0;
                clothes = WizardBaseClothes;
                liteClothes = WizardBaseliteClothes;
                usableWeapons.Add(Weapons.Staff);
                gear = WizardBaseGear;
                weapons = WizardBaseWeapons;
                spells = WizardBaseSpells;
                extraClothes = WizardExtraClothes;

                break;
            case Classes.Fighter:
                maxHealth += 12;
                health += 12;
                STR += 2;
                DEX += 1;
                CON += 2;
                INT += -1;
                WIS += 0;
                CHA += 1;
                clothes = FighterBaseClothes;
                liteClothes = FighterBaseliteClothes;
                usableWeapons.Add(Weapons.Sword);
                usableWeapons.Add(Weapons.DoubleSword);
                usableWeapons.Add(Weapons.Axe);
                break;
            case Classes.Monk:
                maxHealth += 8;
                health += 8;
                STR += 2;
                DEX += 2;
                CON += 0;
                INT += 0;
                WIS += 2;
                CHA += 0;
                clothes = MonkBaseClothes;
                liteClothes = MonkBaseliteClothes;
                break;
            case Classes.Ranger:
                maxHealth += 10;
                health += 10;
                STR += 2;
                DEX += 2;
                CON += 0;
                INT += 1;
                WIS += 1;
                CHA += 0;
                clothes = RangerBaseClothes;
                liteClothes = RangerBaseliteClothes;
                usableWeapons.Add(Weapons.Bow);
                usableWeapons.Add(Weapons.Sword);
                break;
            case Classes.Rogue:
                maxHealth += 7;
                health += 7;
                STR += 1;
                DEX += 2;
                CON += -1;
                INT += 2;
                WIS += 1;
                CHA += -1;
                clothes = RogueBaseClothes;
                liteClothes = RogueBaseliteClothes;
                usableWeapons.Add(Weapons.Sword);
                usableWeapons.Add(Weapons.DoubleSword);
                usableWeapons.Add(Weapons.Crossbow);
                break;
            case Classes.Barbarian:
                maxHealth += 14;
                health += 14;
                STR += 2;
                DEX += 1;
                CON += 2;
                INT += -1;
                WIS += 0;
                CHA += 1;
                clothes = BarbarianBaseClothes;
                liteClothes = BarbarianBaseliteClothes;
                break;
            case Classes.Cleric:
                maxHealth += 9;
                health += 9;
                STR += 1;
                DEX += -1;
                CON += 1;
                INT += 0;
                WIS += 2;
                CHA += 2;
                clothes = ClericBaseClothes;
                liteClothes = ClericBaseliteClothes;
                usableWeapons.Add(Weapons.Sword);
                break;
            case Classes.Paladin:
                maxHealth += 11;
                health += 11;
                STR += 2;
                DEX += -1;
                CON += 1;
                INT += -1;
                WIS += 2;
                CHA += 2;
                clothes = PaladinBaseClothes;
                liteClothes = PaladinBaseliteClothes;
                usableWeapons.Add(Weapons.Sword);
                usableWeapons.Add(Weapons.Shield);
                usableWeapons.Add(Weapons.Axe);
                break;
            case Classes.Tinkerer:
                maxHealth += 8;
                health += 8;
                STR += -1;
                DEX += 2;
                CON += 0;
                INT += 3;
                WIS += 1;
                CHA += -1;
                clothes = TinkererBaseClothes;
                liteClothes = TinkererBaseliteClothes;
                break;
        }

        foreach (ClothingData clothing in clothes)
        {
            inv.addClothing(clothing);
            inv.SetClothingActive(clothing);
        }
        foreach (ClothingData clothing in extraClothes)
        {
            inv.addClothing(clothing);
        }
        foreach (WeaponData weapon in weapons)
        {
            inv.addWeapon(weapon);
        }
        foreach (GearData item in gear)
        {
            inv.addGear(item);
        }
        foreach (SpellData spell in spells)
        {
            inv.addSpell(spell);
        }


        inv.SetClothingActiveLite(liteClothes[0].name, ClothingData.ClothingSlot.Face);
        if (liteClothes[1] != null)
        {
            inv.SetClothingActiveLite(liteClothes[1].name, ClothingData.ClothingSlot.Hair);
        }
        if (bodyType == BodyTypes.Male && liteClothes[2] != null)
        {
            inv.SetClothingActiveLite(liteClothes[2].name, ClothingData.ClothingSlot.Facial_Hair);
        }

        switch (Race)
        {
            case Races.Tiefling:
                transform.Find("Modular Characters").Find("Head Attachments").Find("Horns").gameObject.SetActive(true);
                transform.Find("Modular Characters").Find("Head Attachments").Find("Long Bent Ear").gameObject.SetActive(true);

                playerMat.SetColor("_Color_Hair", tieflingColors[0]);
                playerMat.SetColor("_Color_Skin", tieflingColors[1]);
                playerMat.SetColor("_Color_Stubble", tieflingColors[2]);
                playerMat.SetColor("_Color_Scar", tieflingColors[3]);
                playerMat.SetColor("_Color_Eyes", tieflingColors[4]);
                playerMat.SetColor("_Color_BodyArt", tieflingColors[5]);

                maxHealth += -1;
                health += -1;
                STR += 0;
                DEX += -1;
                CON += 0;
                INT += 1;
                WIS += 1;
                CHA += 1;
                abilities = TieflingBaseAbilities;
                break;
            case Races.Elf:
                transform.Find("Modular Characters").Find("Head Attachments").Find("Long Straight Ear").gameObject.SetActive(true);

                playerMat.SetColor("_Color_Hair", elfColors[0]);
                playerMat.SetColor("_Color_Skin", elfColors[1]);
                playerMat.SetColor("_Color_Stubble", elfColors[2]);
                playerMat.SetColor("_Color_Scar", elfColors[3]);
                playerMat.SetColor("_Color_Eyes", elfColors[4]);
                playerMat.SetColor("_Color_BodyArt", elfColors[5]);

                maxHealth += 0;
                health += 0;
                STR += 0;
                DEX += 1;
                CON += 0;
                INT += 1;
                WIS += 0;
                CHA += -1;
                usableWeapons.Add(Weapons.Bow);
                ChangeSpeed(1);
                abilities = ElfBaseAbilities;
                break;
            case Races.Human:
                humanColors[1] = humanSkins[Random.Range(0, 4)];
                playerMat.SetColor("_Color_Hair", humanColors[0]);
                playerMat.SetColor("_Color_Skin", humanColors[1]);
                playerMat.SetColor("_Color_Stubble", humanColors[2]);
                playerMat.SetColor("_Color_Scar", humanColors[3]);
                playerMat.SetColor("_Color_Eyes", humanColors[4]);
                playerMat.SetColor("_Color_BodyArt", humanColors[5]);

                maxHealth += 0;
                health += 0;
                STR += 1;
                DEX += 1;
                CON += -1;
                INT += 1;
                WIS += 0;
                CHA += -1;
                usableWeapons.Add(Weapons.Axe);
                abilities = HumanBaseAbilities;
                break;
            case Races.Bremri:
                transform.Find("Modular Characters").Find("Head Attachments").Find("Long Bent Ear").gameObject.SetActive(true);

                playerMat.SetColor("_Color_Hair", bremriColors[0]);
                playerMat.SetColor("_Color_Skin", bremriColors[1]);
                playerMat.SetColor("_Color_Stubble", bremriColors[2]);
                playerMat.SetColor("_Color_Scar", bremriColors[3]);
                playerMat.SetColor("_Color_Eyes", bremriColors[4]);
                playerMat.SetColor("_Color_BodyArt", bremriColors[5]);

                maxHealth += 1;
                health += 1;
                STR += 1;
                DEX += 0;
                CON += 1;
                INT += 0;
                WIS += -1;
                CHA += 0;
                usableWeapons.Add(Weapons.Hammer);
                ChangeSpeed(-1);
                abilities = BremriBaseAbilities;
                break;
        }

        foreach (AbilityData ability in abilities)
        {
            inv.addAbility(ability);
        }

        switch (Class)
        {
            case Classes.Bard:
                SetPlayerColors(BardColors[0], BardColors[1]);
                break;
            case Classes.Druid:
                SetPlayerColors(DruidColors[0], DruidColors[1]);
                break;
            case Classes.Warlock:
                SetPlayerColors(WarlockColors[0], WarlockColors[1]);
                break;
            case Classes.Wizard:
                SetPlayerColors(WizardColors[0], WizardColors[1]);
                break;
            case Classes.Fighter:
                SetPlayerColors(FighterColors[0], FighterColors[1]);
                break;
            case Classes.Monk:
                SetPlayerColors(MonkColors[0], MonkColors[1]);
                break;
            case Classes.Ranger:
                SetPlayerColors(RangerColors[0], RangerColors[1]);
                break;
            case Classes.Rogue:
                SetPlayerColors(RogueColors[0], RogueColors[1]);
                break;
            case Classes.Barbarian:
                SetPlayerColors(BarbarianColors[0], BarbarianColors[1]);
                break;
            case Classes.Cleric:
                SetPlayerColors(ClericColors[0], ClericColors[1]);
                break;
            case Classes.Paladin:
                SetPlayerColors(PaladinColors[0], PaladinColors[1]);
                break;
            case Classes.Tinkerer:
                SetPlayerColors(TinkererColors[0], TinkererColors[1]);
                break;
        }

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
        } else
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
    public void DealDamage(int damage)
    {
        if (damage - totalShield > 0)
        {
            health -= damage - totalShield;

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
        }
    }

    public void GiveHealth(int heal)
    {
        Instantiate(healFX, transform);

        if (heal + health > maxHealth)
        {
            StartCoroutine(SmoothHeal(maxHealth - health, 0.1f));
        } else
        {
            StartCoroutine(SmoothHeal(heal, 0.1f));
        }
    }

    private void CheckIsAlive()
    {
        if (health <= 0)
        {
            if (gameObject.layer == 6)
            {
                Destroy(gameObject);
            }
        }
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
        for (int i = 0; i < val; i ++)
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


    public void GetXP(int xpAmount)
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
            Instantiate(levelUpFX, transform);
        }
        else if (XP < 125)
        {
            Level = 2;
            nextLevelXP = 125;
            Instantiate(levelUpFX, transform);
        }//TODO add other levels
        UIUpdater.UpdateCharacterStatsBar();

    }

    public void SetPlayerColors(Color primary, Color secondary)
    {
        playerMat.SetColor("_Color_Primary", primary);
        playerMat.SetColor("_Color_Secondary", secondary);
    }

}
