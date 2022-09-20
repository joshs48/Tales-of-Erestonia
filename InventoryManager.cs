using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<ItemManager> weaponInv = new List<ItemManager>();
    public List<AmmoManager> ammoInv = new List<AmmoManager>();
    public List<ItemManager> gearInv = new List<ItemManager>();
    public List<SpellManager> spellInv = new List<SpellManager>();
    public List<ClothingManager> clothingInv = new List<ClothingManager>();
    public List<AbilityManager> abilityInv = new List<AbilityManager>();


    public List<int> gearQs = new List<int>();
    public List<int> ammoQs = new List<int>();

    public ItemManager activeWeaponR;
    public ItemManager activeWeaponL;
    public AmmoManager activeAmmo;
    public int activeAmmoQs;

    public List<ItemManager> gearActive = new List<ItemManager>();// [0], [1], [2]...
    public List<int> gearActiveQs = new List<int>();

    public List<SpellManager> spellsActive = new List<SpellManager>();
    public List<int> spellQuantitiesBylvl = new List<int>();

    public List<AbilityManager> abilitiesActive = new List<AbilityManager>(3);

    public ClothingManager activeHead;
    public ClothingManager activeTorso;
    public ClothingManager activeHands;
    public ClothingManager activeLegs;
    public ClothingManager activeBoots;

    public GameObject activeFace;
    public GameObject activeHair;
    public GameObject activeFacialHair;


    public int maxWeapons = 10;
    public int maxGear = 10;//TODO change these later!!

    public GameObject startFace;
    public ClothingManager startTorso;
    public ClothingManager startHands;
    public ClothingManager startLegs;
    public ClothingManager startBoots;

    public List<GameObject> facesMale;
    public List<GameObject> facesFemale;
    public List<GameObject> hair;
    public List<GameObject> facialHair;
    public List<GameObject> clothingColors;

    public List<GameObject> testWeapons;
    public List<GameObject> testAmmos;
    public List<GameObject> testGear;
    public List<GameObject> testSpells;
    public List<GameObject> testClothing;
    public List<GameObject> testAbilities;

    public bool isActualPlayer;

    private void Start()
    {
        SetStartClothes();

        foreach (GameObject gameObject in testWeapons)
        {
            addItem(gameObject.GetComponent<ItemManager>(), null, null, null, null);
        }
        foreach (GameObject gameObject in testAmmos)
        {
            addItem(null, gameObject.GetComponent<AmmoManager>(), null, null, null);
        }
        foreach (GameObject gameObject in testGear)
        {
            addItem(gameObject.GetComponent<ItemManager>(), null, null, null, null);
        }
        foreach (GameObject gameObject in testSpells)
        {
            addItem(null, null, gameObject.GetComponent<SpellManager>(), null, null);
        }
        foreach (GameObject gameObject in testClothing)
        {
            addItem(null, null, null, gameObject.GetComponent<ClothingManager>(), null);
        }
        foreach (GameObject gameObject in testAbilities)
        {
            addItem(null, null, null, null, gameObject.GetComponent<AbilityManager>());
        }
    }

    public void addItem(ItemManager item, AmmoManager ammo, SpellManager spell, ClothingManager clothing, AbilityManager ability)
    {
        if (item != null)
        {
            if (item.itemClass.Equals("Gear") && gearInv.Count + 1 < maxGear)
            {
                if (item.quantity == 0)
                {
                    item.quantity = 1;
                }
                if (gearInv.Contains(item))
                {
                    gearQs[gearInv.IndexOf(item)] += item.quantity;
                }
                else
                {
                    gearInv.Add(item);
                    gearQs.Add(item.quantity);
                }

            }
            else if (item.itemClass.Equals("Weapon") && weaponInv.Count + 1 < maxWeapons)
            {

                weaponInv.Add(item);
            }
        }
        else if (ammo != null)
        {
            if (ammo.quantity == 0)
            {
                ammo.quantity = 1;
            }
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
        else if (spell != null)
        {

            spellInv.Add(spell);
        }
        else if (clothing != null)
        {
            clothingInv.Add(clothing);
        } else if (ability != null)
        {
            abilityInv.Add(ability);
        }
    }

    public void SetWeaponActive(GameObject weapon, string location)
    {

        switch (location)
        {
            case "right":
                if (weapon != null)
                {
                    activeWeaponR = weapon.GetComponent<ItemManager>();
                } else
                {
                    activeWeaponR = null;
                }
                GetComponent<CharacterAndWeaponController>().setWeapons(weapon, null);
                break;
            case "left":
                if (weapon != null)
                {
                    activeWeaponL = weapon.GetComponent<ItemManager>();
                }
                else
                {
                    activeWeaponL = null;
                }
                GetComponent<CharacterAndWeaponController>().setWeapons(null, weapon);
                break;
            case "ammo":
                if (weapon != null)
                {
                    activeAmmo = weapon.GetComponent<AmmoManager>();
                }
                break;
        }

    }

    public void SetClothingActive(ClothingManager clothing)
    {
        GameObject clothingObj = null;
        Transform modCharLevel = transform.Find("Modular Characters");
        clothingInv.Remove(clothing);
        switch (clothing.slot)
        {
            case ClothingManager.ClothingSlot.Head_Covering_Base:
                if (activeHead != null)
                {
                    activeHead.gameObject.SetActive(false);
                    clothingInv.Add(activeHead);
                }
                clothingObj = modCharLevel.transform.Find("Head Coverings").Find("Base Hair").Find(clothing.clothingName).gameObject;

                activeHead = clothingObj.GetComponent<ClothingManager>();
                activeFace.SetActive(true);
                break;
            case ClothingManager.ClothingSlot.Head_Covering_No_Facial_Hair:
                if (activeHead != null)
                {
                    activeHead.gameObject.SetActive(false);
                    clothingInv.Add(activeHead);
                }
                clothingObj = modCharLevel.transform.Find("Head Coverings").Find("No Facial Hair").Find(clothing.clothingName).gameObject;

                activeHead = clothingObj.GetComponent<ClothingManager>();
                
                activeFace.SetActive(true);
                if (activeFacialHair != null && activeFacialHair.gameObject != null)
                {
                    activeFacialHair.SetActive(false);
                    activeFacialHair.transform.parent.gameObject.SetActive(false);

                }
                break;
            case ClothingManager.ClothingSlot.Head_Covering_No_Hair:
                if (activeHead != null)
                {
                    activeHead.gameObject.SetActive(false);
                    clothingInv.Add(activeHead);
                }
                clothingObj = modCharLevel.transform.Find("Head Coverings").Find("No Hair").Find(clothing.clothingName).gameObject;

                activeHead = clothingObj.GetComponent<ClothingManager>();
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
            
            case ClothingManager.ClothingSlot.Helmet:
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

                    clothingInv.Add(activeHead);
                }
                clothingObj = modCharLevel.transform.Find("Helmets").Find(clothing.clothingName).gameObject;
                clothingObj.SetActive(true);


                activeHead = clothingObj.GetComponent<ClothingManager>();
                clothingObj = clothingObj.transform.Find(GameObject.Find("Player").GetComponent<StatsManager>().bodyType.ToString()).gameObject;

                break;
            case ClothingManager.ClothingSlot.Torso:
                if (activeTorso != null)
                {
                    activeTorso.gameObject.SetActive(false);
                    activeTorso.transform.GetChild(0).gameObject.SetActive(false);
                    activeTorso.transform.GetChild(1).gameObject.SetActive(false);

                    if (!clothingInv.Contains(activeTorso) && activeTorso != modCharLevel.transform.Find("Torsos").Find(clothing.clothingName).GetComponent<ClothingManager>())
                    {
                        clothingInv.Add(activeTorso);
                    }
                }
                clothingObj = modCharLevel.transform.Find("Torsos").Find(clothing.clothingName).gameObject;
                clothingObj.SetActive(true);

                activeTorso = clothingObj.GetComponent<ClothingManager>();
                clothingObj = clothingObj.transform.Find(GameObject.Find("Player").GetComponent<StatsManager>().bodyType.ToString()).gameObject;

                break;
            case ClothingManager.ClothingSlot.Hand:
                if (activeHands != null)
                {
                    activeHands.gameObject.SetActive(false);
                    activeHands.transform.GetChild(0).gameObject.SetActive(false);
                    activeHands.transform.GetChild(1).gameObject.SetActive(false);

                    if (!clothingInv.Contains(activeHands) && activeHands != modCharLevel.transform.Find("Hands").Find(clothing.clothingName).GetComponent<ClothingManager>())
                    {
                        clothingInv.Add(activeHands);
                    }
                }
                clothingObj = modCharLevel.transform.Find("Hands").Find(clothing.clothingName).gameObject;
                clothingObj.SetActive(true);

                activeHands = clothingObj.GetComponent<ClothingManager>();
                clothingObj = clothingObj.transform.Find(GameObject.Find("Player").GetComponent<StatsManager>().bodyType.ToString()).gameObject;

                break;
            case ClothingManager.ClothingSlot.Leg:
                if (activeLegs != null)
                {
                    activeLegs.gameObject.SetActive(false);
                    activeLegs.transform.GetChild(0).gameObject.SetActive(false);
                    activeLegs.transform.GetChild(1).gameObject.SetActive(false);
                    if (!clothingInv.Contains(activeLegs) && activeLegs != modCharLevel.transform.Find("Legs").Find(clothing.clothingName).GetComponent<ClothingManager>())
                    {
                        clothingInv.Add(activeLegs);
                    }
                }
                clothingObj = modCharLevel.transform.Find("Legs").Find(clothing.clothingName).gameObject;
                clothingObj.SetActive(true);
                
                activeLegs = clothingObj.GetComponent<ClothingManager>();
                clothingObj = clothingObj.transform.Find(GameObject.Find("Player").GetComponent<StatsManager>().bodyType.ToString()).gameObject;

                break;
            case ClothingManager.ClothingSlot.Boot:
                if (activeBoots != null)
                {
                    activeBoots.gameObject.SetActive(false);
                    activeBoots.transform.GetChild(0).gameObject.SetActive(false);
                    activeBoots.transform.GetChild(1).gameObject.SetActive(false);
                    if (!clothingInv.Contains(activeBoots) && activeBoots != modCharLevel.transform.Find("Boots").Find(clothing.clothingName).GetComponent<ClothingManager>())
                    {
                        clothingInv.Add(activeBoots);
                    }
                }
                clothingObj = modCharLevel.transform.Find("Boots").Find(clothing.clothingName).gameObject;
                clothingObj.SetActive(true);
                
                activeBoots = clothingObj.GetComponent<ClothingManager>();
                clothingObj = clothingObj.transform.Find(GameObject.Find("Player").GetComponent<StatsManager>().bodyType.ToString()).gameObject;
                
                break;

        }
        clothingObj.SetActive(true);
    }

    public void SetClothingActiveLite(string clothingName, ClothingManager.ClothingSlot slot)
    {
        if (clothingName == "") {
            return;
        }
        GameObject clothingObj = null;
        Transform modCharLevel = transform.Find("Modular Characters");
        switch (slot)
        {
            case ClothingManager.ClothingSlot.Face:
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
            case ClothingManager.ClothingSlot.Hair:
                if (activeHair != null)
                {
                    activeHair.SetActive(false);
                }
                clothingObj = modCharLevel.transform.Find("Hair").Find(clothingName).gameObject;
                clothingObj.SetActive(true);
                
                activeHair = clothingObj.gameObject;
                break;
            case ClothingManager.ClothingSlot.Facial_Hair:
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
        SetClothingActiveLite(startFace.name, ClothingManager.ClothingSlot.Face); 
        
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
            torsoIcon = activeTorso.maleIcon;
            handsIcon = activeHands.maleIcon;
            legsIcon = activeLegs.maleIcon;
            bootsIcon = activeBoots.maleIcon;
        }
        else
        {
            torsoIcon = activeTorso.femaleIcon;
            handsIcon = activeHands.femaleIcon;
            legsIcon = activeLegs.femaleIcon;
            bootsIcon = activeBoots.femaleIcon;

        }
        GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Torso slot").GetChild(0).gameObject.SetActive(false);
        Instantiate(torsoIcon, GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Torso slot"));
        GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Hands slot").GetChild(0).gameObject.SetActive(false);
        Instantiate(handsIcon, GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Hands slot"));
        GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Legs slot").GetChild(0).gameObject.SetActive(false);
        Instantiate(legsIcon, GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Legs slot"));
        GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Boots slot").GetChild(0).gameObject.SetActive(false);
        Instantiate(bootsIcon, GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Boots slot"));

        GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Player Model").GetComponent<InventoryManager>().SetClothingActive(activeTorso);
        GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Player Model").GetComponent<InventoryManager>().SetClothingActive(activeHands);
        GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Player Model").GetComponent<InventoryManager>().SetClothingActive(activeLegs);
        GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Player Model").GetComponent<InventoryManager>().SetClothingActive(activeBoots);
        GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Player Model").GetComponent<InventoryManager>().SetClothingActiveLite(activeFace.name, ClothingManager.ClothingSlot.Face);
        if (activeFacialHair != null)
        {
            GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Player Model").GetComponent<InventoryManager>().SetClothingActiveLite(activeFacialHair.name, ClothingManager.ClothingSlot.Facial_Hair);
        }
        if (activeHair!= null)
        {
            GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("Player Model").GetComponent<InventoryManager>().SetClothingActiveLite(activeHair.name, ClothingManager.ClothingSlot.Hair);
        }

        switch(GetComponent<StatsManager>().Race)
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
