using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
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
    

    public void SetUpStatsAndInv(StatsManager.Classes playerClass, StatsManager.Races playerRace)
    {

        InventoryManager inv = GetComponent<InventoryManager>();
        StatsManager stats = GetComponent<StatsManager>();
        stats.Level = 1;
        stats.XP = 0;
        ClothingData[] clothes = null;
        ClothingData[] extraClothes = null;
        WeaponData[] weapons = null;
        GearData[] gear = null;
        SpellData[] spells = null;
        AbilityData[] abilities = null;
        GameObject[] liteClothes = null;
        switch (playerClass)
        {
            case StatsManager.Classes.Bard:
                stats.maxHealth += 9;
                stats.health += 9;
                stats.STR += -1;
                stats.DEX += 2;
                stats.CON += 1;
                stats.INT += 0;
                stats.WIS += 1;
                stats.CHA += 2;
                clothes = BardBaseClothes;
                liteClothes = BardBaseliteClothes;
                stats.usableWeapons.Add(StatsManager.Weapons.Sword);
                gear = BardBaseGear;
                weapons = BardBaseWeapons;
                spells = BardBaseSpells;
                extraClothes = BardExtraClothes;
                break;
            case StatsManager.Classes.Druid:
                stats.maxHealth += 8;
                stats.health += 8;
                stats.STR += 1;
                stats.DEX += 1;
                stats.CON += 0;
                stats.INT += 2;
                stats.WIS += 2;
                stats.CHA += -1;
                clothes = DruidBaseClothes;
                liteClothes = DruidBaseliteClothes;
                stats.usableWeapons.Add(StatsManager.Weapons.Staff);
                gear = DruidBaseGear;
                weapons = DruidBaseWeapons;
                spells = DruidBaseSpells;
                extraClothes = DruidExtraClothes;

                break;
            case StatsManager.Classes.Warlock:
                stats.maxHealth += 7;
                stats.health += 70;
                stats.STR += 0;
                stats.DEX += 1;
                stats.CON += -1;
                stats.INT += 1;
                stats.WIS += 2;
                stats.CHA += 2;
                clothes = WarlockBaseClothes;
                liteClothes = WarlockBaseliteClothes;
                gear = WarlockBaseGear;
                weapons = WarlockBaseWeapons;
                spells = WarlockBaseSpells;
                extraClothes = WarlockExtraClothes;

                break;
            case StatsManager.Classes.Wizard:
                stats.maxHealth += 6;
                stats.health += 6;
                stats.STR += -1;
                stats.DEX += 1;
                stats.CON += 0;
                stats.INT += 2;
                stats.WIS += 2;
                stats.CHA += 0;
                clothes = WizardBaseClothes;
                liteClothes = WizardBaseliteClothes;
                stats.usableWeapons.Add(StatsManager.Weapons.Staff);
                gear = WizardBaseGear;
                weapons = WizardBaseWeapons;
                spells = WizardBaseSpells;
                extraClothes = WizardExtraClothes;

                break;
            case StatsManager.Classes.Fighter:
                stats.maxHealth += 12;
                stats.health += 12;
                stats.STR += 2;
                stats.DEX += 1;
                stats.CON += 2;
                stats.INT += -1;
                stats.WIS += 0;
                stats.CHA += 1;
                clothes = FighterBaseClothes;
                liteClothes = FighterBaseliteClothes;
                stats.usableWeapons.Add(StatsManager.Weapons.Sword);
                stats.usableWeapons.Add(StatsManager.Weapons.DoubleSword);
                stats.usableWeapons.Add(StatsManager.Weapons.Axe);
                break;
            case StatsManager.Classes.Monk:
                stats.maxHealth += 8;
                stats.health += 8;
                stats.STR += 2;
                stats.DEX += 2;
                stats.CON += 0;
                stats.INT += 0;
                stats.WIS += 2;
                stats.CHA += 0;
                clothes = MonkBaseClothes;
                liteClothes = MonkBaseliteClothes;
                break;
            case StatsManager.Classes.Ranger:
                stats.maxHealth += 10;
                stats.health += 10;
                stats.STR += 2;
                stats.DEX += 2;
                stats.CON += 0;
                stats.INT += 1;
                stats.WIS += 1;
                stats.CHA += 0;
                clothes = RangerBaseClothes;
                liteClothes = RangerBaseliteClothes;
                stats.usableWeapons.Add(StatsManager.Weapons.Bow);
                stats.usableWeapons.Add(StatsManager.Weapons.Sword);
                break;
            case StatsManager.Classes.Rogue:
                stats.maxHealth += 7;
                stats.health += 7;
                stats.STR += 1;
                stats.DEX += 2;
                stats.CON += -1;
                stats.INT += 2;
                stats.WIS += 1;
                stats.CHA += -1;
                clothes = RogueBaseClothes;
                liteClothes = RogueBaseliteClothes;
                stats.usableWeapons.Add(StatsManager.Weapons.Sword);
                stats.usableWeapons.Add(StatsManager.Weapons.DoubleSword);
                stats.usableWeapons.Add(StatsManager.Weapons.Crossbow);
                break;
            case StatsManager.Classes.Barbarian:
                stats.maxHealth += 14;
                stats.health += 14;
                stats.STR += 2;
                stats.DEX += 1;
                stats.CON += 2;
                stats.INT += -1;
                stats.WIS += 0;
                stats.CHA += 1;
                clothes = BarbarianBaseClothes;
                liteClothes = BarbarianBaseliteClothes;
                break;
            case StatsManager.Classes.Cleric:
                stats.maxHealth += 9;
                stats.health += 9;
                stats.STR += 1;
                stats.DEX += -1;
                stats.CON += 1;
                stats.INT += 0;
                stats.WIS += 2;
                stats.CHA += 2;
                clothes = ClericBaseClothes;
                liteClothes = ClericBaseliteClothes;
                stats.usableWeapons.Add(StatsManager.Weapons.Sword);
                break;
            case StatsManager.Classes.Paladin:
                stats.maxHealth += 11;
                stats.health += 11;
                stats.STR += 2;
                stats.DEX += -1;
                stats.CON += 1;
                stats.INT += -1;
                stats.WIS += 2;
                stats.CHA += 2;
                clothes = PaladinBaseClothes;
                liteClothes = PaladinBaseliteClothes;
                stats.usableWeapons.Add(StatsManager.Weapons.Sword);
                stats.usableWeapons.Add(StatsManager.Weapons.Shield);
                stats.usableWeapons.Add(StatsManager.Weapons.Axe);
                break;
            case StatsManager.Classes.Tinkerer:
                stats.maxHealth += 8;
                stats.health += 8;
                stats.STR += -1;
                stats.DEX += 2;
                stats.CON += 0;
                stats.INT += 3;
                stats.WIS += 1;
                stats.CHA += -1;
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
        if (stats.bodyType == StatsManager.BodyTypes.Male && liteClothes[2] != null)
        {
            inv.SetClothingActiveLite(liteClothes[2].name, ClothingData.ClothingSlot.Facial_Hair);
        }

        switch (playerRace)
        {
            case StatsManager.Races.Tiefling:
                transform.Find("Modular Characters").Find("Head Attachments").Find("Horns").gameObject.SetActive(true);
                transform.Find("Modular Characters").Find("Head Attachments").Find("Long Bent Ear").gameObject.SetActive(true);

                playerMat.SetColor("_Color_Hair", tieflingColors[0]);
                playerMat.SetColor("_Color_Skin", tieflingColors[1]);
                playerMat.SetColor("_Color_Stubble", tieflingColors[2]);
                playerMat.SetColor("_Color_Scar", tieflingColors[3]);
                playerMat.SetColor("_Color_Eyes", tieflingColors[4]);
                playerMat.SetColor("_Color_BodyArt", tieflingColors[5]);

                stats.maxHealth += -1;
                stats.health += -1;
                stats.STR += 0;
                stats.DEX += -1;
                stats.CON += 0;
                stats.INT += 1;
                stats.WIS += 1;
                stats.CHA += 1;
                abilities = TieflingBaseAbilities;
                break;
            case StatsManager.Races.Elf:
                transform.Find("Modular Characters").Find("Head Attachments").Find("Long Straight Ear").gameObject.SetActive(true);

                playerMat.SetColor("_Color_Hair", elfColors[0]);
                playerMat.SetColor("_Color_Skin", elfColors[1]);
                playerMat.SetColor("_Color_Stubble", elfColors[2]);
                playerMat.SetColor("_Color_Scar", elfColors[3]);
                playerMat.SetColor("_Color_Eyes", elfColors[4]);
                playerMat.SetColor("_Color_BodyArt", elfColors[5]);

                stats.maxHealth += 0;
                stats.health += 0;
                stats.STR += 0;
                stats.DEX += 1;
                stats.CON += 0;
                stats.INT += 1;
                stats.WIS += 0;
                stats.CHA += -1;
                stats.usableWeapons.Add(StatsManager.Weapons.Bow);
                stats.ChangeSpeed(1);
                abilities = ElfBaseAbilities;
                break;
            case StatsManager.Races.Human:
                humanColors[1] = humanSkins[Random.Range(0, 4)];
                playerMat.SetColor("_Color_Hair", humanColors[0]);
                playerMat.SetColor("_Color_Skin", humanColors[1]);
                playerMat.SetColor("_Color_Stubble", humanColors[2]);
                playerMat.SetColor("_Color_Scar", humanColors[3]);
                playerMat.SetColor("_Color_Eyes", humanColors[4]);
                playerMat.SetColor("_Color_BodyArt", humanColors[5]);

                stats.maxHealth += 0;
                stats.health += 0;
                stats.STR += 1;
                stats.DEX += 1;
                stats.CON += -1;
                stats.INT += 1;
                stats.WIS += 0;
                stats.CHA += -1;
                stats.usableWeapons.Add(StatsManager.Weapons.Axe);
                abilities = HumanBaseAbilities;
                break;
            case StatsManager.Races.Bremri:
                transform.Find("Modular Characters").Find("Head Attachments").Find("Long Bent Ear").gameObject.SetActive(true);

                playerMat.SetColor("_Color_Hair", bremriColors[0]);
                playerMat.SetColor("_Color_Skin", bremriColors[1]);
                playerMat.SetColor("_Color_Stubble", bremriColors[2]);
                playerMat.SetColor("_Color_Scar", bremriColors[3]);
                playerMat.SetColor("_Color_Eyes", bremriColors[4]);
                playerMat.SetColor("_Color_BodyArt", bremriColors[5]);

                stats.maxHealth += 1;
                stats.health += 1;
                stats.STR += 1;
                stats.DEX += 0;
                stats.CON += 1;
                stats.INT += 0;
                stats.WIS += -1;
                stats.CHA += 0;
                stats.usableWeapons.Add(StatsManager.Weapons.Hammer);
                stats.ChangeSpeed(-1);
                abilities = BremriBaseAbilities;
                break;
        }

        foreach (AbilityData ability in abilities)
        {
            inv.addAbility(ability);
        }

        switch (playerClass)
        {
            case StatsManager.Classes.Bard:
                SetPlayerColors(BardColors[0], BardColors[1]);
                break;
            case StatsManager.Classes.Druid:
                SetPlayerColors(DruidColors[0], DruidColors[1]);
                break;
            case StatsManager.Classes.Warlock:
                SetPlayerColors(WarlockColors[0], WarlockColors[1]);
                break;
            case StatsManager.Classes.Wizard:
                SetPlayerColors(WizardColors[0], WizardColors[1]);
                break;
            case StatsManager.Classes.Fighter:
                SetPlayerColors(FighterColors[0], FighterColors[1]);
                break;
            case StatsManager.Classes.Monk:
                SetPlayerColors(MonkColors[0], MonkColors[1]);
                break;
            case StatsManager.Classes.Ranger:
                SetPlayerColors(RangerColors[0], RangerColors[1]);
                break;
            case StatsManager.Classes.Rogue:
                SetPlayerColors(RogueColors[0], RogueColors[1]);
                break;
            case StatsManager.Classes.Barbarian:
                SetPlayerColors(BarbarianColors[0], BarbarianColors[1]);
                break;
            case StatsManager.Classes.Cleric:
                SetPlayerColors(ClericColors[0], ClericColors[1]);
                break;
            case StatsManager.Classes.Paladin:
                SetPlayerColors(PaladinColors[0], PaladinColors[1]);
                break;
            case StatsManager.Classes.Tinkerer:
                SetPlayerColors(TinkererColors[0], TinkererColors[1]);
                break;
        }

    }

    public void SetPlayerColors(Color primary, Color secondary)
    {
        playerMat.SetColor("_Color_Primary", primary);
        playerMat.SetColor("_Color_Secondary", secondary);
    }
}
