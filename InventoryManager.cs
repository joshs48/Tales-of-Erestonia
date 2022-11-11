using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<WeaponData> weaponInv = new List<WeaponData>();
    public List<AmmoData> ammoInv = new List<AmmoData>();
    public List<GearData> gearInv = new List<GearData>();
    public List<SpellData> spellInv = new List<SpellData>();
    public List<ClothingData> clothingInv = new List<ClothingData>();
    public List<AbilityData> abilityInv = new List<AbilityData>();


    public List<int> gearQs = new List<int>();
    public List<int> ammoQs = new List<int>();

    public GameObject activeWeaponR;
    public GameObject activeWeaponL;
    public AmmoData activeAmmo;
    public int activeAmmoQs;

    public List<GearData> gearActive = new List<GearData>();// [0], [1], [2]...
    public List<int> gearActiveQs = new List<int>();

    public List<SpellData> spellsActive = new List<SpellData>();
    public List<int> spellQuantitiesBylvl = new List<int>();

    public List<AbilityData> abilitiesActive = new List<AbilityData>(3);

    public GameObject activeHead;
    public GameObject activeTorso;
    public GameObject activeHands;
    public GameObject activeLegs;
    public GameObject activeBoots;

    public GameObject activeFace;
    public GameObject activeHair;
    public GameObject activeFacialHair;


    public int maxWeapons = 10;
    public int maxGear = 10;//TODO change these later!!

    public GameObject startFace;
    public ClothingData startTorso;
    public ClothingData startHands;
    public ClothingData startLegs;
    public ClothingData startBoots;

    public List<GameObject> facesMale;
    public List<GameObject> facesFemale;
    public List<GameObject> hair;
    public List<GameObject> facialHair;
    public List<GameObject> clothingColors;

    public List<WeaponData> testWeapons;
    public List<AmmoData> testAmmos;
    public List<GearData> testGear;
    public List<SpellData> testSpells;
    public List<ClothingData> testClothing;
    public List<AbilityData> testAbilities;

    public bool isActualPlayer;

    private void Start()
    {
        SetStartClothes();

        foreach (WeaponData weapon in testWeapons)
        {
            addWeapon(weapon);
        }

        foreach (AmmoData ammo in testAmmos)
        {
            addAmmo(ammo);
        }
        foreach (GearData gear in testGear)
        {
            addGear(gear);
        }
        foreach (SpellData spell in testSpells)
        {
            addSpell(spell);
        }
        foreach (ClothingData clothing in testClothing)
        {
            addClothing(clothing);
        }
        foreach (AbilityData ability in testAbilities)
        {
            addAbility(ability);
        }
        
    }

    public void addWeapon(WeaponData weapon)
    {
        if (weaponInv.Count + 1 < maxWeapons)
            weaponInv.Add(weapon);
    }
    public void addGear(GearData gear)
    {
        if (gearInv.Count + 1 < maxGear)
        {
            if (gearInv.Contains(gear))
            {
                gearQs[gearInv.IndexOf(gear)] += gear.quantity;
            }
            else
            {
                gearInv.Add(gear);
                gearQs.Add(gear.quantity);
            }
        }
    }
    public void addAmmo(AmmoData ammo)
    {

        if (ammoInv.Contains(ammo))
        {
            ammoQs[ammoInv.IndexOf(ammo)] += ammo.quantity;
        }
        else
        {
            ammoInv.Add(ammo);
            ammoQs.Add(ammo.quantity);
        }
    }
    public void addSpell(SpellData spell)
    {
        spellInv.Add(spell);
    }
    public void addAbility(AbilityData ability)
    {
        abilityInv.Add(ability);
    }
    public void addClothing(ClothingData clothing)
    {
        clothingInv.Add(clothing);
    }

    

    public void SetWeaponActive(WeaponData weapon, string location)
    {

        switch (location)
        {
            case "right":
                if (weapon != null)
                {
                    activeWeaponR = weapon.prefab;
                }
                else
                {
                    activeWeaponR = null;
                }
                GetComponent<CharacterAndWeaponController>().setWeapons(weapon, null);
                break;
            case "left":
                if (weapon != null)
                {
                    activeWeaponL = weapon.prefab;
                }
                else
                {
                    activeWeaponL = null;
                }
                GetComponent<CharacterAndWeaponController>().setWeapons(null, weapon);
                break;
        }

    }

    public void SetAmmoActive(AmmoData ammo)
    {
        activeAmmo = ammo;
    }

    public void SetClothingActive(ClothingData clothing)
    {
        GameObject clothingObj = null;
        Transform modCharLevel = transform.Find("Modular Characters");
        clothingInv.Remove(clothing);
        switch (clothing.slot)
        {
            case ClothingData.ClothingSlot.Head_Covering_Base:
                if (activeHead != null)
                {
                    activeHead.SetActive(false);
                    clothingInv.Add(activeHead.GetComponent<Clothing>().data);
                }
                clothingObj = modCharLevel.transform.Find("Head Coverings").Find("Base Hair").Find(clothing.clothingName).gameObject;

                activeHead = clothingObj;
                activeFace.SetActive(true);
                break;
            case ClothingData.ClothingSlot.Head_Covering_No_Facial_Hair:
                if (activeHead != null)
                {
                    activeHead.gameObject.SetActive(false);
                    clothingInv.Add(activeHead.GetComponent<Clothing>().data);
                }
                clothingObj = modCharLevel.transform.Find("Head Coverings").Find("No Facial Hair").Find(clothing.clothingName).gameObject;

                activeHead = clothingObj;

                activeFace.SetActive(true);
                if (activeFacialHair != null && activeFacialHair.gameObject != null)
                {
                    activeFacialHair.SetActive(false);
                    activeFacialHair.transform.parent.gameObject.SetActive(false);

                }
                break;
            case ClothingData.ClothingSlot.Head_Covering_No_Hair:
                if (activeHead != null)
                {
                    activeHead.gameObject.SetActive(false);
                    clothingInv.Add(activeHead.GetComponent<Clothing>().data);
                }
                clothingObj = modCharLevel.transform.Find("Head Coverings").Find("No Hair").Find(clothing.clothingName).gameObject;

                activeHead = clothingObj;
                activeFace.SetActive(true);
                if (activeFacialHair != null && activeFacialHair.gameObject != null)
                {
                    activeFacialHair.SetActive(false);
                    activeFacialHair.transform.parent.gameObject.SetActive(false);
                }
                if (activeHair != null && activeHair.gameObject != null)
                {
                    activeHair.SetActive(false);
                    activeHair.transform.parent.gameObject.SetActive(false);

                }
                break;

            case ClothingData.ClothingSlot.Helmet:
                if (activeFace != null)
                {
                    activeFace.SetActive(false);
                    activeFace.transform.parent.gameObject.SetActive(false);
                }
                if (activeHead != null)
                {
                    activeHead.gameObject.SetActive(false);
                    activeHead.transform.GetChild(0).gameObject.SetActive(false);
                    activeHead.transform.GetChild(1).gameObject.SetActive(false);

                    clothingInv.Add(activeHead.GetComponent<Clothing>().data);
                }
                clothingObj = modCharLevel.transform.Find("Helmets").Find(clothing.clothingName).gameObject;
                clothingObj.SetActive(true);


                activeHead = clothingObj;
                clothingObj = clothingObj.transform.Find(GameObject.Find("Player").GetComponent<StatsManager>().bodyType.ToString()).gameObject;

                break;
            case ClothingData.ClothingSlot.Torso:
                if (activeTorso != null)
                {
                    activeTorso.gameObject.SetActive(false);
                    activeTorso.transform.GetChild(0).gameObject.SetActive(false);
                    activeTorso.transform.GetChild(1).gameObject.SetActive(false);

                    if (!clothingInv.Contains(activeTorso.GetComponent<Clothing>().data) && activeTorso != modCharLevel.transform.Find("Torsos").Find(clothing.clothingName).GetComponent<Clothing>().gameObject)
                    {
                        clothingInv.Add(activeTorso.GetComponent<Clothing>().data);
                    }
                }
                clothingObj = modCharLevel.transform.Find("Torsos").Find(clothing.clothingName).gameObject;
                clothingObj.SetActive(true);

                activeTorso = clothingObj;
                clothingObj = clothingObj.transform.Find(GameObject.Find("Player").GetComponent<StatsManager>().bodyType.ToString()).gameObject;

                break;
            case ClothingData.ClothingSlot.Hand:
                if (activeHands != null)
                {
                    activeHands.gameObject.SetActive(false);
                    activeHands.transform.GetChild(0).gameObject.SetActive(false);
                    activeHands.transform.GetChild(1).gameObject.SetActive(false);

                    if (!clothingInv.Contains(activeHands.GetComponent<Clothing>().data) && activeHands != modCharLevel.transform.Find("Hands").Find(clothing.clothingName).GetComponent<Clothing>().gameObject)
                    {
                        clothingInv.Add(activeHands.GetComponent<Clothing>().data);
                    }
                }
                clothingObj = modCharLevel.transform.Find("Hands").Find(clothing.clothingName).gameObject;
                clothingObj.SetActive(true);

                activeHands = clothingObj;
                clothingObj = clothingObj.transform.Find(GameObject.Find("Player").GetComponent<StatsManager>().bodyType.ToString()).gameObject;

                break;
            case ClothingData.ClothingSlot.Leg:
                if (activeLegs != null)
                {
                    activeLegs.gameObject.SetActive(false);
                    activeLegs.transform.GetChild(0).gameObject.SetActive(false);
                    activeLegs.transform.GetChild(1).gameObject.SetActive(false);
                    if (!clothingInv.Contains(activeLegs.GetComponent<Clothing>().data) && activeLegs != modCharLevel.transform.Find("Legs").Find(clothing.clothingName).GetComponent<Clothing>().gameObject)
                    {
                        clothingInv.Add(activeLegs.GetComponent<Clothing>().data);
                    }
                }
                clothingObj = modCharLevel.transform.Find("Legs").Find(clothing.clothingName).gameObject;
                clothingObj.SetActive(true);

                activeLegs = clothingObj;
                clothingObj = clothingObj.transform.Find(GameObject.Find("Player").GetComponent<StatsManager>().bodyType.ToString()).gameObject;

                break;
            case ClothingData.ClothingSlot.Boot:
                if (activeBoots != null)
                {
                    activeBoots.gameObject.SetActive(false);
                    activeBoots.transform.GetChild(0).gameObject.SetActive(false);
                    activeBoots.transform.GetChild(1).gameObject.SetActive(false);
                    if (!clothingInv.Contains(activeBoots.GetComponent<Clothing>().data) && activeBoots != modCharLevel.transform.Find("Boots").Find(clothing.clothingName).GetComponent<Clothing>().gameObject)
                    {
                        clothingInv.Add(activeBoots.GetComponent<Clothing>().data);
                    }
                }
                clothingObj = modCharLevel.transform.Find("Boots").Find(clothing.clothingName).gameObject;
                clothingObj.SetActive(true);

                activeBoots = clothingObj;
                clothingObj = clothingObj.transform.Find(GameObject.Find("Player").GetComponent<StatsManager>().bodyType.ToString()).gameObject;

                break;

        }
        clothingObj.SetActive(true);
    }

    public void SetClothingActiveLite(string clothingName, ClothingData.ClothingSlot slot)
    {
        if (clothingName == "")
        {
            return;
        }
        GameObject clothingObj = null;
        Transform modCharLevel = transform.Find("Modular Characters");
        switch (slot)
        {
            case ClothingData.ClothingSlot.Face:
                if (activeFace != null)
                {
                    activeFace.SetActive(false);
                    activeFace.transform.GetChild(0).gameObject.SetActive(false);
                    activeFace.transform.GetChild(1).gameObject.SetActive(false);

                }
                clothingObj = modCharLevel.transform.Find("Faces").Find(clothingName).gameObject;
                clothingObj.SetActive(true);

                clothingObj = clothingObj.transform.Find(GameObject.Find("Player").GetComponent<StatsManager>().bodyType.ToString()).gameObject;
                activeFace = clothingObj.transform.parent.gameObject;
                break;
            case ClothingData.ClothingSlot.Hair:
                if (activeHair != null)
                {
                    activeHair.SetActive(false);
                }
                clothingObj = modCharLevel.transform.Find("Hair").Find(clothingName).gameObject;
                clothingObj.SetActive(true);

                activeHair = clothingObj.gameObject;
                break;
            case ClothingData.ClothingSlot.Facial_Hair:
                if (activeFacialHair != null)
                {
                    activeFacialHair.SetActive(false);
                }
                clothingObj = modCharLevel.transform.Find("Facial Hair").Find(clothingName).gameObject;
                clothingObj.SetActive(true);

                activeFacialHair = clothingObj.gameObject;
                break;

        }
        clothingObj.SetActive(true);

    }

    public void SetStartClothes()
    {
        SetStartFace();
        SetStartTorso();
        SetStartHands();
        SetStartLegs();
        SetStartBoots();
    }
    public void SetStartFace()
    {
        SetClothingActiveLite(startFace.name, ClothingData.ClothingSlot.Face);

    }

    public void SetStartTorso()
    {
        SetClothingActive(startTorso);
        if (isActualPlayer)
        {
            GameObject torsoIcon = null;
            if (GameObject.Find("Player").GetComponent<StatsManager>().bodyType == StatsManager.BodyTypes.Male)
            {
                torsoIcon = startTorso.maleIcon;
            }
            else
            {
                torsoIcon = startTorso.femaleIcon;
            }

            GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Torso slot").GetChild(0).gameObject.SetActive(false);

            Instantiate(torsoIcon, GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Torso slot"));

        }
    }


    public void SetStartHands()
    {
        SetClothingActive(startHands);
        if (isActualPlayer)
        {
            GameObject handsIcon = null;
            if (GameObject.Find("Player").GetComponent<StatsManager>().bodyType == StatsManager.BodyTypes.Male)
            {
                handsIcon = startHands.maleIcon;
            }
            else
            {
                handsIcon = startHands.femaleIcon;
            }

            GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Hands slot").GetChild(0).gameObject.SetActive(false);

            Instantiate(handsIcon, GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Hands slot"));

        }
    }

    public void SetStartLegs()
    {
        SetClothingActive(startLegs);
        if (isActualPlayer)
        {
            GameObject legsIcon = null;
            if (GameObject.Find("Player").GetComponent<StatsManager>().bodyType == StatsManager.BodyTypes.Male)
            {
                legsIcon = startLegs.maleIcon;
            }
            else
            {
                legsIcon = startLegs.femaleIcon;
            }

            GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Legs slot").GetChild(0).gameObject.SetActive(false);
            Instantiate(legsIcon, GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Legs slot"));
        }
    }

    public void SetStartBoots()
    {
        SetClothingActive(startBoots);
        if (isActualPlayer)
        {
            GameObject bootsIcon = null;
            if (GameObject.Find("Player").GetComponent<StatsManager>().bodyType == StatsManager.BodyTypes.Male)
            {
                bootsIcon = startBoots.maleIcon;
            }
            else
            {
                bootsIcon = startBoots.femaleIcon;
            }
            GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Boots slot").GetChild(0).gameObject.SetActive(false);
            Instantiate(bootsIcon, GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Boots slot"));
        }
    }

    public void UpdateActiveUI()
    {
        GameObject torsoIcon = null;
        GameObject handsIcon = null;
        GameObject legsIcon = null;
        GameObject bootsIcon = null;
        if (GetComponent<StatsManager>().bodyType == StatsManager.BodyTypes.Male)
        {
            torsoIcon = activeTorso.GetComponent<Clothing>().data.maleIcon;
            handsIcon = activeHands.GetComponent<Clothing>().data.maleIcon;
            legsIcon = activeLegs.GetComponent<Clothing>().data.maleIcon;
            bootsIcon = activeBoots.GetComponent<Clothing>().data.maleIcon;
        }
        else
        {
            torsoIcon = activeTorso.GetComponent<Clothing>().data.femaleIcon;
            handsIcon = activeHands.GetComponent<Clothing>().data.femaleIcon;
            legsIcon = activeLegs.GetComponent<Clothing>().data.femaleIcon;
            bootsIcon = activeBoots.GetComponent<Clothing>().data.femaleIcon;

        }
        GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Torso slot").GetChild(0).gameObject.SetActive(false);
        Instantiate(torsoIcon, GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Torso slot"));
        GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Hands slot").GetChild(0).gameObject.SetActive(false);
        Instantiate(handsIcon, GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Hands slot"));
        GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Legs slot").GetChild(0).gameObject.SetActive(false);
        Instantiate(legsIcon, GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Legs slot"));
        GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Boots slot").GetChild(0).gameObject.SetActive(false);
        Instantiate(bootsIcon, GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Boots slot"));

        GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Player Model").GetComponent<InventoryManager>().SetClothingActive(activeTorso.GetComponent<Clothing>().data);
        GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Player Model").GetComponent<InventoryManager>().SetClothingActive(activeHands.GetComponent<Clothing>().data);
        GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Player Model").GetComponent<InventoryManager>().SetClothingActive(activeLegs.GetComponent<Clothing>().data);
        GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Player Model").GetComponent<InventoryManager>().SetClothingActive(activeBoots.GetComponent<Clothing>().data);
        GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Player Model").GetComponent<InventoryManager>().SetClothingActiveLite(activeFace.name, ClothingData.ClothingSlot.Face);
        if (activeFacialHair != null)
        {
            GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Player Model").GetComponent<InventoryManager>().SetClothingActiveLite(activeFacialHair.name, ClothingData.ClothingSlot.Facial_Hair);
        }
        if (activeHair != null)
        {
            GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Player Model").GetComponent<InventoryManager>().SetClothingActiveLite(activeHair.name, ClothingData.ClothingSlot.Hair);
        }

        switch (GetComponent<StatsManager>().Race)
        {
            case StatsManager.Races.Tiefling:
                GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Player Model").transform.Find("Modular Characters").Find("Head Attachments").Find("Long Bent Ear").gameObject.SetActive(true);
                GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Player Model").transform.Find("Modular Characters").Find("Head Attachments").Find("Horns").gameObject.SetActive(true);

                break;
            case StatsManager.Races.Elf:
                GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Player Model").transform.Find("Modular Characters").Find("Head Attachments").Find("Long Straight Ear").gameObject.SetActive(true);

                break;
            case StatsManager.Races.Bremri:
                GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Player Model").transform.Find("Modular Characters").Find("Head Attachments").Find("Long Bent Ear").gameObject.SetActive(true);
                break;
        } 
    }
}
