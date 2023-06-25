using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public bool isGameActive = true;
    public GameObject characterBox;
    public GameObject inventoryBox;
    public GameObject customizeBox;
    public GameObject selectionContainer;
    public GameObject cSBox;
    public GameObject charStatsBox;
    public GameObject QSAnchor;
    public GameObject QSContainer;
    public GameObject mainUIBackground;
    public GameObject player;
    public GameObject selector;
    public GameObject selected;
    public GameObject quantityText;

    public GameObject baseQSBarObj;
    public Material baseQSBarMat;
    public Material selectedQSBarMat;

    public GameObject baseQSSpellObj;
    public Material baseQSSpellMat;
    public Material selectedSpellBarMat;


    public Shader outlineTargetShader;
    public Color targetColor;
    public float targetPower;

    public GameObject barbarianQSBox;
    public GameObject fighterQSBox;
    public GameObject rogueQSBox;
    public GameObject clericQSBox;
    public GameObject paladinQSBox;
    public GameObject druidQSBox;
    public GameObject rangerQSBox;
    public GameObject bardQSBox;
    public GameObject warlockQSBox;
    public GameObject wizardQSBox;

    public Material commonMat;
    public Material uncommonMat;
    public Material rareMat;
    public Material epicMat;
    public Material legendaryMat;


    public enum Tabs
    {
        Home,
        Clothing,
        Weapons,
        Ammo,
        Gear,
        Spells,
        Abilities,
        Customize

    };

    public Tabs currTab;

    [HideInInspector] public PlayerControl pc;
    [HideInInspector] public GameObject invSelector;
    [HideInInspector] public GameObject csSelector;
    [HideInInspector] public bool prevMenuVal;
    [HideInInspector] public bool prevIsGameActive;

    [HideInInspector] public bool prevQSGCycle = false;
    [HideInInspector] public bool prevQSGUse = false;
    [HideInInspector] public int CurQSG = 0;
    [HideInInspector] public int totalQSG;

    [HideInInspector] public bool prevQSSCycle = false;
    [HideInInspector] public bool prevQSSUse = false;
    [HideInInspector] public int CurQSS = 0;
    [HideInInspector] public int totalQSS;

    [HideInInspector] public bool prevAbility1Use = false;
    [HideInInspector] public bool prevAbility2Use = false;
    [HideInInspector] public bool prevAbility3Use = false;

    [HideInInspector] public bool isInvCreated = false;
    [HideInInspector] public bool isEquipping = false;
    [HideInInspector] public bool isUnEquipping = false;
    [HideInInspector] public bool isSelectInvRunning = false;

    [HideInInspector] public StatsManager playerStats;



    [HideInInspector] public GameObject healthBar;
    [HideInInspector] public GameObject XPBar;
    [HideInInspector] public static GameManager gameManager;


    [HideInInspector] public GameObject selectedFace = null;
    [HideInInspector] public GameObject selectedHair = null;
    [HideInInspector] public GameObject selectedFacialHair = null;
    [HideInInspector] public GameObject selectedClothingColors = null;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = this;
        player = GameObject.Find("Player");
        pc = player.GetComponent<PlayerControl>();
        healthBar = charStatsBox.transform.Find("Health bar").gameObject;
        XPBar = charStatsBox.transform.Find("XP bar").gameObject;
        playerStats = player.GetComponent<StatsManager>();
        UIUpdater.CreateBaseUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (pc.menu && !prevMenuVal)
        {
            isGameActive = !isGameActive;
        }
        if (!isGameActive)
        {
            pc.ChangeLightBar(Color.green, Color.blue, 0.3f);
            Time.timeScale = 0;
            if (!isInvCreated && prevIsGameActive != isGameActive)
            {
                inventoryBox.SetActive(true);
                characterBox.SetActive(true);
                mainUIBackground.SetActive(true);
                selectionContainer.SetActive(true);
                //GameObject.Find("SaveAndLoadRunner").GetComponent<SaveAndLoadRunner>().save_game();

                InventoryUIRunner.CreateInventory(currTab, true);//this also starts the selection and unequipping coroutine and can swap tabs of the inventory
            }
            player.GetComponent<BaseCharacter>().canMove = false;
        }
        if (isGameActive)
        {
            if (prevIsGameActive != isGameActive)
            {
                Time.timeScale = 1;
                pc.ChangeLightBar(Color.cyan, Color.cyan, 1);

                cSBox.SetActive(false);
                characterBox.SetActive(false);
                inventoryBox.SetActive(false);
                mainUIBackground.SetActive(false);
                selectionContainer.SetActive(false);
                customizeBox.SetActive(false);

                isInvCreated = false;
                InventoryUIRunner.DestroyInventory();
                ChestUIRunner.DestroyCS();
                //SaveDataManager.Instance.invManager = player.GetComponent<InventoryManager>();
                //SaveDataManager.Instance.Save();
            }
            UIUpdater.UpdateHealthBar();
            UIUpdater.UpdateStaminaBar();
            UIUpdater.UpdateQSBar();
            UIUpdater.UpdateMagicBar();
            UIUpdater.UpdateAbilities();
            player.GetComponent<BaseCharacter>().canMove = true;
        }
        prevMenuVal = pc.menu;
        prevIsGameActive = isGameActive;
    }
}


public class UIUpdater : MonoBehaviour
{
    static readonly GameManager game = GameManager.gameManager;

    //sets up the player stats and quick select menus based on player info
    public static void CreateBaseUI()
    {

        Transform QSAnchorTrans = game.QSAnchor.transform;

        int itemBoxCnt = 0;
        int spellBoxCnt = 0;
        int abilityBoxCnt = 0;
        GameObject currAbilityBox = null;
        UpdateCharacterStatsBar();

        switch (game.playerStats.Class)
        {
            case StatsManager.Classes.Barbarian:
                itemBoxCnt = 3;
                spellBoxCnt = 1;
                abilityBoxCnt = 2;
                currAbilityBox = game.barbarianQSBox;
                break;
            case StatsManager.Classes.Fighter:
                itemBoxCnt = 5;
                spellBoxCnt = 0;
                abilityBoxCnt = 1;
                currAbilityBox = game.fighterQSBox;
                break;
            case StatsManager.Classes.Rogue:
                itemBoxCnt = 3;
                spellBoxCnt = 0;
                abilityBoxCnt = 3;
                currAbilityBox = game.rogueQSBox;
                break;
            case StatsManager.Classes.Cleric:
                itemBoxCnt = 3;
                spellBoxCnt = 0;
                abilityBoxCnt = 3;
                currAbilityBox = game.clericQSBox;
                break;
            case StatsManager.Classes.Paladin:
                itemBoxCnt = 2;
                spellBoxCnt = 2;
                abilityBoxCnt = 2;
                currAbilityBox = game.paladinQSBox;
                break;
            case StatsManager.Classes.Druid:
                itemBoxCnt = 0;
                spellBoxCnt = 4;
                abilityBoxCnt = 2;
                currAbilityBox = game.druidQSBox;
                break;
            case StatsManager.Classes.Ranger:
                itemBoxCnt = 4;
                spellBoxCnt = 0;
                abilityBoxCnt = 2;
                currAbilityBox = game.rangerQSBox;
                break;
            case StatsManager.Classes.Bard:
                itemBoxCnt = 0;
                spellBoxCnt = 3;
                abilityBoxCnt = 3;
                currAbilityBox = game.bardQSBox;
                break;
            case StatsManager.Classes.Warlock:
                itemBoxCnt = 0;
                spellBoxCnt = 4;
                abilityBoxCnt = 2;
                currAbilityBox = game.warlockQSBox;
                break;
            case StatsManager.Classes.Wizard:
                itemBoxCnt = 0;
                spellBoxCnt = 5;
                abilityBoxCnt = 1;
                currAbilityBox = game.wizardQSBox;
                break;
        }
        game.totalQSG = itemBoxCnt - 1;
        game.totalQSS = spellBoxCnt - 1;
        Transform objTransform = QSAnchorTrans;

        for (int i = 0; i < abilityBoxCnt; i++)
        {
            GameObject newBox = Instantiate(currAbilityBox, game.QSContainer.transform);
            newBox.transform.SetPositionAndRotation(objTransform.position, QSAnchorTrans.transform.rotation);
            newBox.name = "QS_Ability " + i;
            objTransform.localPosition = new Vector3(objTransform.localPosition.x + 110, objTransform.localPosition.y, objTransform.localPosition.z);
        }



        for (int i = 0; i < itemBoxCnt; i++)
        {
            GameObject newBox = Instantiate(game.baseQSBarObj, game.QSContainer.transform);
            newBox.transform.SetPositionAndRotation(objTransform.position, QSAnchorTrans.transform.rotation);
            newBox.name = "QS_Item " + i;
            if (i == 0)
            {
                Material[] materials = { newBox.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[0], game.selectedQSBarMat };
                newBox.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials = materials;
            }
            else
            {
                Material[] materials = { newBox.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[0], game.baseQSBarMat };
                newBox.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials = materials;
            }
            objTransform.localPosition = new Vector3(objTransform.localPosition.x + 110, objTransform.localPosition.y, objTransform.localPosition.z);

        }

        for (int i = 0; i < spellBoxCnt; i++)
        {
            GameObject newBox = Instantiate(game.baseQSSpellObj, game.QSContainer.transform);
            newBox.transform.SetPositionAndRotation(objTransform.position, QSAnchorTrans.transform.rotation);
            newBox.name = "QS_Spell " + i;
            if (i == 0)
            {
                Material[] materials = { newBox.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[0], game.selectedSpellBarMat };
                newBox.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials = materials;
            }
            else
            {
                Material[] materials = { newBox.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[0], game.baseQSSpellMat };
                newBox.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials = materials;
            }
            objTransform.localPosition = new Vector3(objTransform.localPosition.x + 110, objTransform.localPosition.y, objTransform.localPosition.z);

        }

    }

    //all the functions below connect the ui to all the in game stuff(health, inventory, changing weapons...)
    public static void UpdateHealthBar()
    {
        game.healthBar.GetComponent<Slider>().maxValue = game.playerStats.maxHealth;
        game.healthBar.GetComponent<Slider>().value = game.playerStats.health;
    }

    public static void UpdateStaminaBar()
    {
        game.XPBar.GetComponent<Slider>().maxValue = game.playerStats.nextLevelXP;
        game.XPBar.GetComponent<Slider>().value = game.playerStats.XP;
        game.XPBar.GetComponent<Slider>().minValue = game.playerStats.minLevelXP;
    }

    public static void UpdateCharacterStatsBar()
    {
        string playerClass = game.playerStats.Class.ToString();
        game.charStatsBox.transform.Find("Class").GetComponent<TextMeshProUGUI>().text = playerClass;
        game.charStatsBox.transform.Find("Level").GetComponent<TextMeshProUGUI>().text = "Level " + game.playerStats.Level.ToString();
    }

    //sets the quickselect bar
    static GameObject curQSGitem = null;
    public static void UpdateQSBar()
    {
        if (game.totalQSG <= 0)
        {
            return;
        }
        InventoryManager im = game.player.GetComponent<InventoryManager>();
        if (game.prevQSGCycle != game.pc.QSGCycle && game.pc.QSGCycle == true)
        {
            Material[] newMaterials = { GameObject.Find("Canvas").transform.Find("QS Container").transform.Find("QS_Item " + game.CurQSG.ToString()).transform.Find("Background").gameObject.GetComponent<MeshRenderer>().materials[0], game.baseQSBarMat };
            GameObject.Find("Canvas").transform.Find("QS Container").transform.Find("QS_Item " + game.CurQSG.ToString()).transform.Find("Background").gameObject.GetComponent<MeshRenderer>().materials = newMaterials;

            if (game.CurQSG < game.totalQSG)
            {
                game.CurQSG += 1;
            }
            else
            {
                game.CurQSG = 0;
            }
            GameObject curQSGbackground = GameObject.Find("Canvas").transform.Find("QS Container").transform.Find("QS_Item " + game.CurQSG.ToString()).transform.Find("Background").gameObject;

            if (GameObject.Find("Canvas").transform.Find("QS Container").transform.Find("QS_Item " + game.CurQSG.ToString()).transform.childCount > 1)
            {
                curQSGitem = GameObject.Find("Canvas").transform.Find("QS Container").transform.Find("QS_Item " + game.CurQSG.ToString()).transform.GetChild(1).gameObject;


            }
            newMaterials = new Material[] { curQSGbackground.GetComponent<MeshRenderer>().material, game.selectedQSBarMat };
            curQSGbackground.GetComponent<MeshRenderer>().materials = newMaterials;
        }

        game.prevQSGCycle = game.pc.QSGCycle;

        if (game.prevQSGUse != game.pc.QSGUse && game.pc.QSGUse == true && GameObject.Find("Canvas").transform.Find("QS Container").transform.Find("QS_Item " + game.CurQSG.ToString()).transform.childCount > 1)
        {
            curQSGitem = GameObject.Find("Canvas").transform.Find("QS Container").transform.Find("QS_Item " + game.CurQSG.ToString()).transform.GetChild(1).gameObject;
            if (curQSGitem != null)
            {
                if (curQSGitem.GetComponent<IconManager>() != null)
                {
                    switch (curQSGitem.GetComponent<IconManager>().objectClass)
                    {
                        case IconManager.Class.Weapon:
                            GameObject.Find("Player").GetComponent<InventoryManager>().gearActive.Remove(curQSGitem.GetComponent<IconManager>().Gear);
                            GameObject eqSlot;

                            if (curQSGitem.GetComponent<IconManager>().Location.Equals("Weapon"))
                            {
                                eqSlot = GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("L_weapon slot").gameObject;
                            }
                            else
                            {
                                eqSlot = GameObject.Find("Canvas").transform.Find("Character Box").transform.Find(curQSGitem.GetComponent<IconManager>().Location + " slot").gameObject;
                            }
                            InventoryUIRunner.EquipItem(curQSGitem, eqSlot);
                            break;
                        case IconManager.Class.Gear:
                            im.gearActiveQs[im.gearActive.IndexOf(curQSGitem.GetComponent<IconManager>().Gear)] -= 1;
                            curQSGitem.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = im.gearActiveQs[im.gearActive.IndexOf(curQSGitem.GetComponent<IconManager>().Gear)].ToString();

                            switch (curQSGitem.GetComponent<IconManager>().objectType)
                            {
                                case IconManager.Type.Potion:
                                    if (curQSGitem.GetComponent<IconManager>().Gear.shieldVal > 0)
                                    {
                                        //idk
                                    }
                                    else if (curQSGitem.GetComponent<IconManager>().Gear.healVal > 0)
                                    {
                                        game.playerStats.GiveHealth(curQSGitem.GetComponent<IconManager>().Gear.healVal);
                                    }

                                    break;
                            }
                            if (im.gearActiveQs[im.gearActive.IndexOf(curQSGitem.GetComponent<IconManager>().Gear)] <= 0)
                            {

                                im.gearActiveQs.RemoveAt(im.gearActive.IndexOf(curQSGitem.GetComponent<IconManager>().Gear));
                                im.gearActive.Remove(curQSGitem.GetComponent<IconManager>().Gear);
                                Destroy(curQSGitem);
                            }
                            break;
                    }
                }

            }
        }
        game.prevQSGUse = game.pc.QSGUse;

    }

    //Updates Magic bar
    static GameObject curQSSitem = null;
    public static void UpdateMagicBar()
    {
        if (game.totalQSS < 0)
        {
            return;
        }
        if (game.prevQSSCycle != game.pc.QSSCycle && game.pc.QSSCycle == true)
        {
            Material[] newMaterials = { GameObject.Find("Canvas").transform.Find("QS Container").transform.Find("QS_Spell " + game.CurQSS.ToString()).transform.Find("Background").gameObject.GetComponent<MeshRenderer>().materials[0], game.baseQSSpellMat };
            GameObject.Find("Canvas").transform.Find("QS Container").transform.Find("QS_Spell " + game.CurQSS.ToString()).transform.Find("Background").gameObject.GetComponent<MeshRenderer>().materials = newMaterials;

            if (game.CurQSS < game.totalQSS)
            {
                game.CurQSS += 1;
            }
            else
            {
                game.CurQSS = 0;
            }
            GameObject curQSSbackground = GameObject.Find("Canvas").transform.Find("QS Container").transform.Find("QS_Spell " + game.CurQSS.ToString()).transform.Find("Background").gameObject;


            if (GameObject.Find("Canvas").transform.Find("QS Container").transform.Find("QS_Spell " + game.CurQSS.ToString()).transform.childCount > 1)
            {
                curQSSitem = GameObject.Find("Canvas").transform.Find("QS Container").transform.Find("QS_Spell " + game.CurQSS.ToString()).transform.GetChild(1).gameObject;
                if (curQSSitem.GetComponent<IconManager>().Spell.spellType == SpellData.Type.Targeted)
                {
                    game.StartCoroutine(OutlineNearestEnemy(game.CurQSS));
                }
            }
            newMaterials = new Material[] { curQSSbackground.GetComponent<MeshRenderer>().material, game.selectedSpellBarMat };
            curQSSbackground.GetComponent<MeshRenderer>().materials = newMaterials;
        }

        game.prevQSSCycle = game.pc.QSSCycle;

        if (game.prevQSSUse != game.pc.QSSUse && game.pc.QSSUse == true && GameObject.Find("Canvas").transform.Find("QS Container").transform.Find("QS_Spell " + game.CurQSS.ToString()).transform.childCount > 1)
        {
            curQSSitem = GameObject.Find("Canvas").transform.Find("QS Container").transform.Find("QS_Spell " + game.CurQSS.ToString()).transform.GetChild(1).gameObject;
            if (curQSSitem != null)
            {
                if (curQSSitem.GetComponent<IconManager>().Spell != null)
                {
                    switch (curQSSitem.GetComponent<IconManager>().Spell.spellClass)
                    {
                        case SpellData.Class.Melee_Attack:
                        case SpellData.Class.Ranged_Attack:

                            game.player.GetComponent<Animator>().SetTrigger("Magic Attack");
                            int spellVal = 0;
                            switch (curQSSitem.GetComponent<IconManager>().Spell.spellName)
                            {
                                case "Fireball":
                                    spellVal = 1;
                                    break;
                                case "Ice Explosion":
                                    spellVal = 2;
                                    break;
                                case "Ice Path":
                                    spellVal = 3;
                                    break;
                                case "Blue Fireball":
                                    spellVal = 4;
                                    break;
                                case "Power Beam":
                                    spellVal = 5;
                                    break;
                                case "Lightning Strike":
                                    spellVal = 6;
                                    break;
                                case "Crystal Explosion":
                                    spellVal = 7;
                                    break;
                                case "Crystal Path":
                                    spellVal = 8;
                                    break;
                                case "Rock Explosion":
                                    spellVal = 9;
                                    break;
                                case "Rock Path":
                                    spellVal = 10;
                                    break;
                                case "Vine Explosion":
                                    spellVal = 11;
                                    break;
                                case "Vine Path":
                                    spellVal = 12;
                                    break;
                            }
                            game.player.GetComponent<Animator>().SetInteger("Spell", spellVal);
                            break;
                    }
                }
            }
        }

        game.prevQSSUse = game.pc.QSSUse;

    }
    //updates abilities

    public static void UpdateAbilities()
    {
        InventoryManager inv = game.player.GetComponent<InventoryManager>();


        if (inv.abilitiesActive[0] != null && game.prevAbility1Use != game.pc.ability1Use && game.pc.ability1Use)
        {
            ActivateAbility(inv.abilitiesActive[0]);
        }

        if (inv.abilitiesActive[1] != null && game.prevAbility2Use != game.pc.ability2Use && game.pc.ability2Use)
        {
            ActivateAbility(inv.abilitiesActive[1]);

        }

        if (inv.abilitiesActive[2] != null && game.prevAbility3Use != game.pc.ability3Use && game.pc.ability3Use)
        {
            ActivateAbility(inv.abilitiesActive[2]);

        }
        game.prevAbility1Use = game.pc.ability1Use;
        game.prevAbility2Use = game.pc.ability2Use;
        game.prevAbility3Use = game.pc.ability3Use;

    }

    private static void ActivateAbility(AbilityData ability)
    {
        switch (ability.abilityType)
        {
            case AbilityData.Type.Instant:

                game.player.GetComponent<Animator>().SetTrigger("Use Ability");
                int abilityVal = 0;
                switch (ability.abilityName)
                {
                    case "Fire Blast":
                        abilityVal = 1;
                        break;

                }
                game.player.GetComponent<Animator>().SetInteger("Ability", abilityVal);
                break;

            case AbilityData.Type.Active:
                Debug.Log("Active abilities not set up yet");
                break;

            case AbilityData.Type.Continuous:
                Debug.Log("Continuous abilities not set up yet");
                break;
        }
    }

    //Creates any notifications
    public static void CreateNotificationBar(string text)
    {
        GameObject notification = GameObject.Find("Canvas").transform.Find("Notification Text").gameObject;
        notification.SetActive(true);
        notification.GetComponent<TextMeshProUGUI>().text = text;

        Vector3 startPos = new(0, -480, -44);
        int distance = 85;
        notification.transform.localPosition = startPos;
        GameObject background = notification.transform.GetChild(0).gameObject;

        background.transform.localScale = new Vector3(text.Length * 100, background.transform.localScale.y, background.transform.localScale.z);
        game.StartCoroutine(NotificationDelay(1.5f, notification, distance));
    }

    public static void CreateLevelSelect()
    {

    }

    private static IEnumerator OutlineNearestEnemy(int prevQSS)
    {
        GameObject currEnemyTarget = null;
        Shader prevShader = null;
        Material newMaterial;
        Texture prevTexture;
        while (game.CurQSS == prevQSS)
        {
            GameObject enemyTarget = game.player.GetComponent<PlayerControl>().FindNearestEnemy();
            if (enemyTarget != currEnemyTarget)
            {
                if (currEnemyTarget != null)
                {
                    currEnemyTarget.GetComponentInChildren<SkinnedMeshRenderer>(false).material.shader = prevShader;
                }
                prevShader = enemyTarget.GetComponentInChildren<SkinnedMeshRenderer>(false).material.shader;
                newMaterial = enemyTarget.GetComponentInChildren<SkinnedMeshRenderer>(false).material;
                prevTexture = newMaterial.mainTexture;
                newMaterial.shader = game.outlineTargetShader;
                newMaterial.SetColor("Color_1dc44c8b59a94ba2b1774b9575cdbe9b", game.targetColor);
                newMaterial.SetFloat("Vector1_75f17a0aa44e4590b2907dfe0d781665", game.targetPower);
                newMaterial.SetTexture("Texture2D_b4da09b4ebcb49edaa884d93d113e777", prevTexture);

            }
            currEnemyTarget = enemyTarget;
            yield return new WaitForSeconds(0.1f);

        }
        if (currEnemyTarget != null)
        {
            currEnemyTarget.GetComponentInChildren<SkinnedMeshRenderer>(false).material.shader = prevShader;
        }

    }

    private static IEnumerator NotificationDelay(float time, GameObject notification, float distance)
    {
        for (int i = 0; i < 25; i++)
        {
            notification.transform.localPosition = new Vector3(notification.transform.localPosition.x, notification.transform.localPosition.y + distance / 25, notification.transform.localPosition.z);
            yield return new WaitForSecondsRealtime(0.02f);
        }
        yield return new WaitForSecondsRealtime(time);
        notification.SetActive(false);
    }
}

public class InventoryUIRunner : MonoBehaviour
{
    static readonly GameManager game = GameManager.gameManager;

    public static void SetInvSelectorPos(float x, float y)
    {

        game.invSelector.transform.parent = game.inventoryBox.transform.Find("Inventory Slot (" + ((y * 4) + x) + ")");
        game.invSelector.transform.SetAsLastSibling();
        game.invSelector.transform.localPosition = new Vector3(0, 0, game.invSelector.transform.localPosition.z);
    }

    public static void SetCustomizeSelectorPos(float x, float y)
    {
        switch (y)
        {
            case 0:
                game.invSelector.transform.parent = game.customizeBox.transform.Find("Face Slot (" + x + ")");
                game.invSelector.transform.SetAsLastSibling();
                game.invSelector.transform.localPosition = new Vector3(0, 0, game.customizeBox.transform.localPosition.z);
                break;
            case 1:
                game.invSelector.transform.parent = game.customizeBox.transform.Find("Hair Slot (" + x + ")");
                game.invSelector.transform.SetAsLastSibling();
                game.invSelector.transform.localPosition = new Vector3(0, 0, game.customizeBox.transform.localPosition.z);
                break;
            case 2:
                game.invSelector.transform.parent = game.customizeBox.transform.Find("Facial Hair Slot (" + x + ")");
                game.invSelector.transform.SetAsLastSibling();
                game.invSelector.transform.localPosition = new Vector3(0, 0, game.customizeBox.transform.localPosition.z);
                break;
            case 3:
                game.invSelector.transform.parent = game.customizeBox.transform.Find("Clothing Colors Slot (" + x + ")");
                game.invSelector.transform.SetAsLastSibling();
                game.invSelector.transform.localPosition = new Vector3(0, 0, game.customizeBox.transform.localPosition.z);
                break;
        }

    }


    public static void CreateInventory(GameManager.Tabs tab, bool beginSelection)
    {
        InventoryManager im = game.player.GetComponent<InventoryManager>();
        game.invSelector = null;

        if (tab == GameManager.Tabs.Clothing)
        {
            game.inventoryBox.SetActive(true);
            game.customizeBox.SetActive(false);
            int index = 0;
            foreach (ClothingData clothing in im.clothingInv)
            {
                GameObject icon = null;
                switch (GameObject.Find("Player").GetComponent<StatsManager>().bodyType)
                {
                    case StatsManager.BodyTypes.Male:
                        icon = clothing.maleIcon;
                        break;
                    case StatsManager.BodyTypes.Female:
                        icon = clothing.femaleIcon;
                        break;
                }
                icon.GetComponent<IconManager>().Clothing = clothing;
                GameObject curBox = game.inventoryBox.transform.Find("Inventory Slot (" + index + ")").gameObject;

                Instantiate(icon, curBox.transform, false);

                if (index == 0 && beginSelection)
                {
                    game.invSelector = Instantiate(game.selector, curBox.transform, false);
                    game.invSelector.transform.SetAsLastSibling();
                }
                index += 1;
            }
            game.inventoryBox.transform.Find("Secondary Text").gameObject.GetComponent<TextMeshProUGUI>().text = "Clothing";

            if (im.clothingInv.Count == 0 && beginSelection)
            {
                game.invSelector = Instantiate(game.selector, game.inventoryBox.transform.Find("Inventory Slot (0)"), false);
                game.invSelector.transform.SetAsLastSibling();
            }
            game.isInvCreated = true;

        }
        else if (tab == GameManager.Tabs.Weapons)
        {
            game.inventoryBox.SetActive(true);
            game.customizeBox.SetActive(false);
            int index = 0;
            foreach (WeaponData weapon in im.weaponInv)
            {
                GameObject icon = weapon.icon;
                icon.GetComponent<IconManager>().Weapon = weapon;
                GameObject curBox = game.inventoryBox.transform.Find("Inventory Slot (" + index + ")").gameObject;

                Instantiate(icon, curBox.transform, false);

                if (index == 0 && beginSelection)
                {
                    game.invSelector = Instantiate(game.selector, curBox.transform, false);
                    game.invSelector.transform.SetAsLastSibling();
                }
                index += 1;
            }
            game.inventoryBox.transform.Find("Secondary Text").gameObject.GetComponent<TextMeshProUGUI>().text = "Weapons";

            if (im.weaponInv.Count == 0 && beginSelection)
            {
                game.invSelector = Instantiate(game.selector, game.inventoryBox.transform.Find("Inventory Slot (0)"), false);
                game.invSelector.transform.SetAsLastSibling();
            }
            game.isInvCreated = true;

        }
        else if (tab == GameManager.Tabs.Ammo)
        {
            game.inventoryBox.SetActive(true);
            game.customizeBox.SetActive(false);
            int index = 0;
            foreach (AmmoData ammo in im.ammoInv)
            {
                GameObject icon = ammo.icon;
                icon.GetComponent<IconManager>().Ammo = ammo;
                GameObject curBox = game.inventoryBox.transform.Find("Inventory Slot (" + index + ")").gameObject;
                GameObject newIcon = Instantiate(icon, curBox.transform, false);

                if (index == 0 && beginSelection)
                {
                    game.invSelector = Instantiate(game.selector, curBox.transform, false);
                    game.invSelector.transform.SetAsLastSibling();
                }
                GameObject quantity = Instantiate(game.quantityText, curBox.transform);
                quantity.transform.SetParent(newIcon.transform);
                quantity.transform.SetAsFirstSibling();
                quantity.GetComponent<TextMeshProUGUI>().text = im.ammoQs[im.ammoInv.IndexOf(ammo)].ToString();
                index += 1;
            }
            game.inventoryBox.transform.Find("Secondary Text").gameObject.GetComponent<TextMeshProUGUI>().text = "Ammo";
            if (im.ammoInv.Count == 0 && beginSelection)
            {
                game.invSelector = Instantiate(game.selector, game.inventoryBox.transform.Find("Inventory Slot (0)"), false);
                game.invSelector.transform.SetAsLastSibling();
            }
            game.isInvCreated = true;

        }
        else if (tab == GameManager.Tabs.Gear)
        {
            game.inventoryBox.SetActive(true);
            game.customizeBox.SetActive(false);
            int index = 0;
            foreach (GearData gear in im.gearInv)
            {
                GameObject icon = gear.icon;
                icon.GetComponent<IconManager>().Gear = gear;
                GameObject curBox = game.inventoryBox.transform.Find("Inventory Slot (" + index + ")").gameObject;
                GameObject newIcon = Instantiate(icon, curBox.transform, false);

                if (index == 0 && beginSelection)
                {
                    game.invSelector = Instantiate(game.selector, curBox.transform, false);
                    game.invSelector.transform.SetAsLastSibling();
                }
                GameObject quantity = Instantiate(game.quantityText, curBox.transform);
                quantity.transform.SetParent(newIcon.transform);
                quantity.transform.SetAsFirstSibling();
                quantity.GetComponent<TextMeshProUGUI>().text = im.gearQs[im.gearInv.IndexOf(gear)].ToString();
                index += 1;
            }
            game.inventoryBox.transform.Find("Secondary Text").gameObject.GetComponent<TextMeshProUGUI>().text = "Gear";
            if (im.gearInv.Count == 0 && beginSelection)
            {
                game.invSelector = Instantiate(game.selector, game.inventoryBox.transform.Find("Inventory Slot (0)"), false);
                game.invSelector.transform.SetAsLastSibling();
            }
            game.isInvCreated = true;

        }
        else if (tab == GameManager.Tabs.Spells)
        {
            game.inventoryBox.SetActive(true);
            game.customizeBox.SetActive(false);
            int index = 0;
            foreach (SpellData spell in im.spellInv)
            {
                GameObject icon = spell.icon;
                icon.GetComponent<IconManager>().Spell = spell;
                GameObject curBox = game.inventoryBox.transform.Find("Inventory Slot (" + index + ")").gameObject;

                GameObject newIcon = Instantiate(icon, curBox.transform, false);

                if (index == 0 && beginSelection)
                {
                    game.invSelector = Instantiate(game.selector, curBox.transform, false);
                    game.invSelector.transform.SetAsLastSibling();
                }
                index += 1;
            }
            game.inventoryBox.transform.Find("Secondary Text").gameObject.GetComponent<TextMeshProUGUI>().text = "Spells";
            if (im.spellInv.Count == 0 && beginSelection)
            {
                game.invSelector = Instantiate(game.selector, game.inventoryBox.transform.Find("Inventory Slot (0)"), false);
                game.invSelector.transform.SetAsLastSibling();
            }
            game.isInvCreated = true;

        }
        else if (tab == GameManager.Tabs.Abilities)
        {
            game.inventoryBox.SetActive(true);
            game.customizeBox.SetActive(false);
            int index = 0;
            foreach (AbilityData ability in im.abilityInv)
            {
                GameObject icon = ability.icon;
                icon.GetComponent<IconManager>().Ability = ability;
                GameObject curBox = game.inventoryBox.transform.Find("Inventory Slot (" + index + ")").gameObject;

                GameObject newIcon = Instantiate(icon, curBox.transform, false);

                if (index == 0 && beginSelection)
                {
                    game.invSelector = Instantiate(game.selector, curBox.transform, false);
                    game.invSelector.transform.SetAsLastSibling();
                }
                index += 1;
            }
            game.inventoryBox.transform.Find("Secondary Text").gameObject.GetComponent<TextMeshProUGUI>().text = "Abilities";
            if (im.abilityInv.Count == 0 && beginSelection)
            {
                game.invSelector = Instantiate(game.selector, game.inventoryBox.transform.Find("Inventory Slot (0)"), false);
                game.invSelector.transform.SetAsLastSibling();
            }
            game.isInvCreated = true;

        }
        else if (tab == GameManager.Tabs.Customize)
        {
            game.inventoryBox.SetActive(false);
            game.customizeBox.SetActive(true);
            StatsManager stats = im.GetComponent<StatsManager>();
            List<GameObject> faceSet;

            if (stats.bodyType == StatsManager.BodyTypes.Male)
            {
                faceSet = im.facesMale;
            }
            else
            {
                faceSet = im.facesFemale;
            }

            for (int i = 0; i < 7; i++)
            {
                Transform curBox = game.customizeBox.transform.Find("Face Slot (" + i + ")");
                Instantiate(faceSet[i], curBox, false);

                if (i == 0)
                {
                    game.invSelector = Instantiate(game.selector, game.customizeBox.transform.Find("Face Slot (0)"), false);
                    game.invSelector.transform.SetAsLastSibling();
                }
            }

            for (int i = 0; i < 7; i++)
            {
                Transform curBox = game.customizeBox.transform.Find("Hair Slot (" + i + ")");
                Instantiate(im.hair[i], curBox, false);
            }

            for (int i = 0; i < 7; i++)
            {
                Transform curBox = game.customizeBox.transform.Find("Facial Hair Slot (" + i + ")");
                Instantiate(im.facialHair[i], curBox, false);
            }

            for (int i = 0; i < 7; i++)
            {
                Transform curBox = game.customizeBox.transform.Find("Clothing Colors Slot (" + i + ")");
                Instantiate(im.clothingColors[i], curBox, false);
            }
            game.isInvCreated = false;

        }
        game.currTab = tab;

        if (!game.isSelectInvRunning && beginSelection)
        {
            if (tab == GameManager.Tabs.Customize)
            {
                game.StartCoroutine(SelectCustomize());

            }
            else
            {
                game.StartCoroutine(SelectInv(4, 7));//this can branch into equipping or unequipping
            }
        }
    }

    public static void DestroyInventory()
    {
        for (int i = 0; i < 27; i++)
        {
            GameObject curBox = game.inventoryBox.transform.Find("Inventory Slot (" + i + ")").gameObject;
            foreach (Transform child in curBox.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public static void DestroyCustomize()
    {
        for (int i = 0; i < 7; i++)
        {
            GameObject curBox = game.customizeBox.transform.Find("Face Slot (" + i + ")").gameObject;
            foreach (Transform child in curBox.transform)
            {
                Destroy(child.gameObject);
            }
            curBox = game.customizeBox.transform.Find("Hair Slot (" + i + ")").gameObject;
            foreach (Transform child in curBox.transform)
            {
                Destroy(child.gameObject);
            }
            curBox = game.customizeBox.transform.Find("Facial Hair Slot (" + i + ")").gameObject;
            foreach (Transform child in curBox.transform)
            {
                Destroy(child.gameObject);
            }
            curBox = game.customizeBox.transform.Find("Clothing Colors Slot (" + i + ")").gameObject;
            foreach (Transform child in curBox.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public static void EquipItem(GameObject icon, GameObject currBox)
    {
        InventoryManager im = game.player.GetComponent<InventoryManager>();
        InventoryManager pmim = game.characterBox.transform.Find("Player Model").GetComponent<InventoryManager>();
        string eqSlot = currBox.name.Substring(0, currBox.name.IndexOf(" "));

        if (!currBox.name.Substring(0, currBox.name.IndexOf(" ")).Equals("QS_Item") && !currBox.name.Substring(0, currBox.name.IndexOf(" ")).Equals("QS_Spell") && !currBox.name.Substring(0, currBox.name.IndexOf(" ")).Equals("QS_Ability"))
            currBox.transform.GetChild(0).gameObject.SetActive(false);

        Vector3 prevLocalPos = icon.transform.localPosition;
        Vector3 prevLocalScale = icon.transform.localScale;
        icon.transform.parent = currBox.transform;
        icon.transform.localPosition = prevLocalPos;
        icon.transform.localScale = prevLocalScale;

        game.isEquipping = false;

        if (currBox.transform.childCount > 3 || (currBox.transform.childCount > 2 && (currBox.name.Substring(0, currBox.name.IndexOf(" ")) == "Spell") || currBox.name.Substring(0, currBox.name.IndexOf(" ")) == "Ability"))
        {
            GameObject prevIcon = currBox.transform.GetChild(1).gameObject;
            GameObject newParent;
            prevLocalPos = prevIcon.transform.localPosition;

            prevIcon.transform.localPosition = prevLocalPos;

            if (prevIcon.GetComponent<IconManager>().objectClass == IconManager.Class.Weapon)
            {
                im.weaponInv.Add(prevIcon.GetComponent<IconManager>().Weapon);
            }
            else if (prevIcon.GetComponent<IconManager>().objectClass == IconManager.Class.Ammo)
            {
                im.ammoInv.Add(prevIcon.GetComponent<IconManager>().Ammo);
            }
            else if (prevIcon.GetComponent<IconManager>().objectClass == IconManager.Class.Gear)
            {
                im.gearInv.Add(prevIcon.GetComponent<IconManager>().Gear);
            }
            else if (prevIcon.GetComponent<IconManager>().objectClass == IconManager.Class.Spell)
            {
                im.spellInv.Add(prevIcon.GetComponent<IconManager>().Spell);
            }
            else if (prevIcon.GetComponent<IconManager>().objectClass == IconManager.Class.Ability)
            {
                im.abilityInv.Add(prevIcon.GetComponent<IconManager>().Ability);
            }
            //clothing handled by the inventoryManager
            if (eqSlot == "R_weapon")
            {

                Destroy(im.gameObject.GetComponent<CharacterAndWeaponController>().weaponRObject);
                Destroy(pmim.gameObject.GetComponent<CharacterAndWeaponController>().weaponRObject);
            }
            else if (eqSlot == "L_weapon")
            {

                Destroy(im.gameObject.GetComponent<CharacterAndWeaponController>().weaponLObject);
                Destroy(pmim.gameObject.GetComponent<CharacterAndWeaponController>().weaponLObject);

            }
            else if (eqSlot == "Item")
            {
                im.gearActive.Remove(icon.GetComponent<IconManager>().Gear);
            }
            else if (eqSlot == "Spell")
            {
                im.spellsActive.Remove(icon.GetComponent<IconManager>().Spell);
            }
            else if (eqSlot == "Ability")
            {
                im.abilitiesActive.Remove(icon.GetComponent<IconManager>().Ability);
            }
            //clothing handled by inventoryManager

            if ((game.currTab == GameManager.Tabs.Weapons && prevIcon.GetComponent<IconManager>().objectClass == IconManager.Class.Weapon) || (game.currTab == GameManager.Tabs.Ammo && prevIcon.GetComponent<IconManager>().objectClass == IconManager.Class.Ammo) || (game.currTab == GameManager.Tabs.Gear && prevIcon.GetComponent<IconManager>().objectClass == IconManager.Class.Gear) || (game.currTab == GameManager.Tabs.Spells && prevIcon.GetComponent<IconManager>().objectClass == IconManager.Class.Spell) || (game.currTab == GameManager.Tabs.Clothing && prevIcon.GetComponent<IconManager>().objectClass == IconManager.Class.Clothing) || (game.currTab == GameManager.Tabs.Abilities && prevIcon.GetComponent<IconManager>().objectClass == IconManager.Class.Ability))
            {
                int emptySpot = 0;
                for (int i = 19; i >= 0; i--)
                {
                    if (game.inventoryBox.transform.Find("Inventory Slot (" + i + ")").transform.childCount == 0)
                    {
                        emptySpot = i;
                    }
                }
                newParent = game.inventoryBox.transform.Find("Inventory Slot (" + emptySpot + ")").gameObject;
                Vector3 prevLocalPos2 = icon.transform.localPosition;
                prevIcon.transform.parent = newParent.transform;
                prevIcon.transform.localPosition = prevLocalPos2;
            }
            else
            {
                Destroy(prevIcon);
            }
        }
        //if you equip a bow, any right weapon goes away
        if (icon.GetComponent<IconManager>().objectType == IconManager.Type.Bow && eqSlot.Equals("L_weapon") && im.activeWeaponR != null && icon.GetComponent<IconManager>().objectType != IconManager.Type.Shield)
        {
            GameObject prevIcon = GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("R_weapon slot").transform.GetChild(1).gameObject;

            im.weaponInv.Add(prevIcon.GetComponent<IconManager>().Weapon);
            GameObject newParent;
            if ((game.currTab == GameManager.Tabs.Weapons && prevIcon.GetComponent<IconManager>().objectClass == IconManager.Class.Weapon) || (game.currTab == GameManager.Tabs.Ammo && prevIcon.GetComponent<IconManager>().objectClass == IconManager.Class.Ammo) || (game.currTab == GameManager.Tabs.Gear && prevIcon.GetComponent<IconManager>().objectClass == IconManager.Class.Gear) || (game.currTab == GameManager.Tabs.Spells && prevIcon.GetComponent<IconManager>().objectClass == IconManager.Class.Spell) || (game.currTab == GameManager.Tabs.Abilities && prevIcon.GetComponent<IconManager>().objectClass == IconManager.Class.Ability))
            {
                int emptySpot = 0;
                for (int i = 19; i >= 0; i--)
                {
                    if (game.inventoryBox.transform.Find("Inventory Slot (" + i + ")").transform.childCount == 0)
                    {
                        emptySpot = i;
                    }
                }

                newParent = game.inventoryBox.transform.Find("Inventory Slot (" + emptySpot + ")").gameObject;
                Vector3 prevLocalPos2 = icon.transform.localPosition;
                prevIcon.transform.parent = newParent.transform;
                prevIcon.transform.localPosition = prevLocalPos2;
            }
            else
            {
                Destroy(prevIcon);
            }

            GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("R_weapon slot").transform.GetChild(0).gameObject.SetActive(true);
            im.activeWeaponR = null;
            Destroy(im.gameObject.GetComponent<CharacterAndWeaponController>().weaponRObject);
            im.SetWeaponActive(null, "right");
            pmim.activeWeaponR = null;
            Destroy(pmim.gameObject.GetComponent<CharacterAndWeaponController>().weaponRObject);
            pmim.SetWeaponActive(null, "right");

        }

        //if you have an axe or a staff it removes the left weapon if there is one
        if ((icon.GetComponent<IconManager>().objectType == IconManager.Type.Staff || icon.GetComponent<IconManager>().objectType == IconManager.Type.Axe) && im.activeWeaponL != null)
        {
            GameObject prevIcon = GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("L_weapon slot").transform.GetChild(1).gameObject;

            im.weaponInv.Add(prevIcon.GetComponent<IconManager>().Weapon);
            GameObject newParent;
            if ((game.currTab == GameManager.Tabs.Weapons && prevIcon.GetComponent<IconManager>().objectClass == IconManager.Class.Weapon) || (game.currTab == GameManager.Tabs.Ammo && prevIcon.GetComponent<IconManager>().objectClass == IconManager.Class.Ammo) || (game.currTab == GameManager.Tabs.Gear && prevIcon.GetComponent<IconManager>().objectClass == IconManager.Class.Gear) || (game.currTab == GameManager.Tabs.Spells && prevIcon.GetComponent<IconManager>().objectClass == IconManager.Class.Spell) || (game.currTab == GameManager.Tabs.Abilities && prevIcon.GetComponent<IconManager>().objectClass == IconManager.Class.Ability))
            {
                int emptySpot = 0;
                for (int i = 19; i >= 0; i--)
                {
                    if (game.inventoryBox.transform.Find("Inventory Slot (" + i + ")").transform.childCount == 0)
                    {
                        emptySpot = i;
                    }
                }
                newParent = game.inventoryBox.transform.Find("Inventory Slot (" + emptySpot + ")").gameObject;
                Vector3 prevLocalPos2 = icon.transform.localPosition;
                prevIcon.transform.parent = newParent.transform;
                prevIcon.transform.localPosition = prevLocalPos2;
            }
            else
            {
                Destroy(prevIcon);
            }

            GameObject.Find("Canvas").transform.Find("Character Box").transform.Find("L_weapon slot").transform.GetChild(0).gameObject.SetActive(true);
            im.activeWeaponL = null;
            Destroy(im.gameObject.GetComponent<CharacterAndWeaponController>().weaponLObject);
            im.SetWeaponActive(null, "left");
            pmim.activeWeaponL = null;
            Destroy(pmim.gameObject.GetComponent<CharacterAndWeaponController>().weaponLObject);
            pmim.SetWeaponActive(null, "left");
        }

        if (icon.GetComponent<IconManager>().objectClass == IconManager.Class.Weapon)
        {

            im.weaponInv.Remove(icon.GetComponent<IconManager>().Weapon);

            if (eqSlot == "R_weapon")
            {
                im.activeWeaponR = icon.GetComponent<IconManager>().Weapon.prefab;
                im.SetWeaponActive(icon.GetComponent<IconManager>().Weapon, "right");

                pmim.activeWeaponR = icon.GetComponent<IconManager>().Weapon.prefab;
                pmim.SetWeaponActive(icon.GetComponent<IconManager>().Weapon, "right");
                pmim.gameObject.GetComponent<CharacterAndWeaponController>().weaponRObject.layer = 5;
            }
            else if (eqSlot == "L_weapon")
            {
                im.activeWeaponL = icon.GetComponent<IconManager>().Weapon.prefab;
                im.SetWeaponActive(icon.GetComponent<IconManager>().Weapon, "left");
                pmim.activeWeaponL = icon.GetComponent<IconManager>().Weapon.prefab;
                pmim.SetWeaponActive(icon.GetComponent<IconManager>().Weapon, "left");
                pmim.gameObject.GetComponent<CharacterAndWeaponController>().weaponLObject.layer = 5;
            }
            else
            {
                im.gearActive.Add(icon.GetComponent<IconManager>().Gear);
            }
        }
        else if (icon.GetComponent<IconManager>().objectClass == IconManager.Class.Ammo)
        {
            im.activeAmmoQs = im.ammoQs[im.ammoInv.IndexOf(icon.GetComponent<IconManager>().Ammo)];
            im.ammoQs.Remove(im.ammoInv.IndexOf(icon.GetComponent<IconManager>().Ammo));
            im.ammoInv.Remove(icon.GetComponent<IconManager>().Ammo);
            im.activeAmmo = icon.GetComponent<IconManager>().Ammo;
            im.SetAmmoActive(im.activeAmmo);
        }
        else if (icon.GetComponent<IconManager>().objectClass == IconManager.Class.Gear)
        {
            im.gearActiveQs.Add(im.gearQs[im.gearInv.IndexOf(icon.GetComponent<IconManager>().Gear)]);

            im.gearQs.RemoveAt(im.gearInv.IndexOf(icon.GetComponent<IconManager>().Gear));
            im.gearInv.Remove(icon.GetComponent<IconManager>().Gear);

            im.gearActive.Add(icon.GetComponent<IconManager>().Gear);
        }
        else if (icon.GetComponent<IconManager>().objectClass == IconManager.Class.Spell)
        {
            im.spellInv.Remove(icon.GetComponent<IconManager>().Spell);

            im.spellsActive.Add(icon.GetComponent<IconManager>().Spell);
        }
        else if (icon.GetComponent<IconManager>().objectClass == IconManager.Class.Clothing)
        {


            im.SetClothingActive(icon.GetComponent<IconManager>().Clothing);
            pmim.SetClothingActive(icon.GetComponent<IconManager>().Clothing);
        }
        else if (icon.GetComponent<IconManager>().objectClass == IconManager.Class.Ability)
        {
            im.abilityInv.Remove(icon.GetComponent<IconManager>().Ability);
            im.abilitiesActive[int.Parse(currBox.name.Substring(currBox.name.IndexOf(" ")))] = icon.GetComponent<IconManager>().Ability;
        }


        if (icon.GetComponent<IconManager>().objectType == IconManager.Type.Bow || icon.GetComponent<IconManager>().objectType == IconManager.Type.Crossbow)
        {
            UIUpdater.CreateNotificationBar("Equip ammo in the 2nd tab!");
        }
    }

    public static IEnumerator SelectInv(int maxW, int maxH)
    {
        Vector2 moveVal = game.player.GetComponent<PlayerControl>().moveStick;
        Vector2 currentPos = new(0, 0);
        PlayerControl pc = game.player.GetComponent<PlayerControl>();
        GameObject currBox;

        game.invSelector.SetActive(true);
        SetInvSelectorPos(currentPos.x, currentPos.y);

        int prevAltMove = pc.altMove;
        GameManager.Tabs prevTab = game.currTab;
        double prevMoveMag = 1;

        while (game.isInvCreated && !game.isEquipping && !game.isUnEquipping)
        {
            game.isSelectInvRunning = true;

            moveVal = pc.moveStick;
            bool select = pc.jump;
            bool unEquip = pc.shield;

            GameObject tabSelector = null;
            if (game.selectionContainer.transform.Find("Home Tab").transform.Find("Tab selector") != null)
            {
                tabSelector = game.selectionContainer.transform.Find("Home Tab").transform.Find("Tab selector").gameObject;
            }
            else if (game.selectionContainer.transform.Find("Clothing Tab").transform.Find("Tab selector") != null)
            {
                tabSelector = game.selectionContainer.transform.Find("Clothing Tab").transform.Find("Tab selector").gameObject;
            }
            else if (game.selectionContainer.transform.Find("Weapon Tab").transform.Find("Tab selector") != null)
            {
                tabSelector = game.selectionContainer.transform.Find("Weapon Tab").transform.Find("Tab selector").gameObject;
            }
            else if (game.selectionContainer.transform.Find("Ammo Tab").transform.Find("Tab selector") != null)
            {
                tabSelector = game.selectionContainer.transform.Find("Ammo Tab").transform.Find("Tab selector").gameObject;
            }
            else if (game.selectionContainer.transform.Find("Gear Tab").transform.Find("Tab selector") != null)
            {
                tabSelector = game.selectionContainer.transform.Find("Gear Tab").transform.Find("Tab selector").gameObject;
            }
            if (game.selectionContainer.transform.Find("Spells Tab").transform.Find("Tab selector") != null)
            {
                tabSelector = game.selectionContainer.transform.Find("Spells Tab").transform.Find("Tab selector").gameObject;
            }
            if (game.selectionContainer.transform.Find("Abilities Tab").transform.Find("Tab selector") != null)
            {
                tabSelector = game.selectionContainer.transform.Find("Abilities Tab").transform.Find("Tab selector").gameObject;
            }

            if (game.currTab == GameManager.Tabs.Weapons && tabSelector.transform.parent.name != "Weapon Tab")
            {
                tabSelector.transform.parent = game.selectionContainer.transform.Find("Weapon Tab");
                tabSelector.transform.localPosition = new Vector3(0, 0, tabSelector.transform.localPosition.z);
            }
            //allows for tab swapping

            if (prevAltMove != pc.altMove && ((pc.altMove > 0 && ((int)game.currTab) < 7) || (pc.altMove < 0 && ((int)game.currTab) > 0)))
            {

                game.currTab += pc.altMove;

                DestroyInventory();
                switch (game.currTab)
                {
                    case GameManager.Tabs.Clothing:
                        //clothing tab

                        tabSelector.transform.parent = game.selectionContainer.transform.Find("Clothing Tab");
                        tabSelector.transform.localPosition = new Vector3(0, 0, tabSelector.transform.localPosition.z);
                        game.isInvCreated = false;
                        CreateInventory(GameManager.Tabs.Clothing, true);
                        yield return new WaitUntil(() => game.isInvCreated);
                        currentPos = new Vector2(0, 0);
                        break;
                    case GameManager.Tabs.Weapons:
                        //weapon tab

                        tabSelector.transform.parent = game.selectionContainer.transform.Find("Weapon Tab");
                        tabSelector.transform.localPosition = new Vector3(0, 0, tabSelector.transform.localPosition.z);
                        game.isInvCreated = false;
                        CreateInventory(GameManager.Tabs.Weapons, true);
                        yield return new WaitUntil(() => game.isInvCreated);
                        currentPos = new Vector2(0, 0);
                        break;
                    case GameManager.Tabs.Ammo:
                        //ammo tab

                        tabSelector.transform.parent = game.selectionContainer.transform.Find("Ammo Tab");
                        tabSelector.transform.localPosition = new Vector3(0, 0, tabSelector.transform.localPosition.z);
                        game.isInvCreated = false;
                        CreateInventory(GameManager.Tabs.Ammo, true);
                        yield return new WaitUntil(() => game.isInvCreated);
                        currentPos = new Vector2(0, 0);
                        break;
                    case GameManager.Tabs.Gear:
                        // gear tab
                        tabSelector.transform.parent = game.selectionContainer.transform.Find("Gear Tab");
                        tabSelector.transform.localPosition = new Vector3(0, 0, tabSelector.transform.localPosition.z);
                        game.isInvCreated = false;
                        CreateInventory(GameManager.Tabs.Gear, true);
                        yield return new WaitUntil(() => game.isInvCreated);
                        currentPos = new Vector2(0, 0);
                        break;
                    case GameManager.Tabs.Spells:
                        tabSelector.transform.parent = game.selectionContainer.transform.Find("Spells Tab");
                        tabSelector.transform.localPosition = new Vector3(0, 0, tabSelector.transform.localPosition.z);
                        game.isInvCreated = false;
                        CreateInventory(GameManager.Tabs.Spells, true);
                        yield return new WaitUntil(() => game.isInvCreated);
                        currentPos = new Vector2(0, 0);
                        break;
                    case GameManager.Tabs.Abilities:
                        tabSelector.transform.parent = game.selectionContainer.transform.Find("Abilities Tab");
                        tabSelector.transform.localPosition = new Vector3(0, 0, tabSelector.transform.localPosition.z);
                        game.isInvCreated = false;
                        CreateInventory(GameManager.Tabs.Abilities, true);
                        yield return new WaitUntil(() => game.isInvCreated);
                        currentPos = new Vector2(0, 0);
                        break;
                    case GameManager.Tabs.Customize:
                        tabSelector.transform.parent = game.selectionContainer.transform.Find("Customize Tab");
                        tabSelector.transform.localPosition = new Vector3(0, 0, tabSelector.transform.localPosition.z);
                        game.isInvCreated = false;
                        game.isSelectInvRunning = false;
                        CreateInventory(GameManager.Tabs.Customize, true);
                        break;
                }
            }
            currBox = game.inventoryBox.transform.Find("Inventory Slot (" + ((currentPos.y * 4) + currentPos.x) + ")").gameObject;

            if (moveVal.x > 0.5 && currentPos.x < maxW - 1)
            {
                SetInvSelectorPos(currentPos.x + 1, currentPos.y);
                currentPos.x++;
            }
            else if (moveVal.x < -0.5 && currentPos.x > 0)
            {
                SetInvSelectorPos(currentPos.x - 1, currentPos.y);
                currentPos.x--;
            }

            if (moveVal.y > 0.5 && currentPos.y > 0)
            {
                SetInvSelectorPos(currentPos.x, currentPos.y - 1);
                currentPos.y--;
            }
            else if (moveVal.y < -0.5 && currentPos.y < maxH - 1)
            {
                SetInvSelectorPos(currentPos.x, currentPos.y + 1);
                currentPos.y++;
            }

            currBox = game.inventoryBox.transform.Find("Inventory Slot (" + ((currentPos.y * 4) + currentPos.x) + ")").gameObject;
            GameObject icon = null;
            if (currBox.transform.childCount > 1)
            {
                game.invSelector.transform.SetAsLastSibling();
                icon = currBox.transform.GetComponentInChildren<IconManager>().gameObject;
            }
            if (((moveVal.magnitude < 0.05 && moveVal.magnitude != prevMoveMag) || prevTab != game.currTab) && icon != null && icon.GetComponent<IconManager>() != null && currBox.transform.childCount > 1)
            {
                GameObject ds = game.inventoryBox.transform.Find("Description Section").gameObject;

                if (game.currTab == GameManager.Tabs.Clothing)
                {
                    ClothingData currClothingData = icon.GetComponent<IconManager>().Clothing;



                    ds.transform.Find("Name text").gameObject.GetComponent<TextMeshProUGUI>().text = currClothingData.clothingName;
                    ds.transform.Find("Type text").gameObject.GetComponent<TextMeshProUGUI>().text = currClothingData.tags.ToString();
                    ds.transform.Find("Rarity text").gameObject.GetComponent<TextMeshProUGUI>().text = currClothingData.rarity.ToString();

                    ds.transform.Find("Description text").gameObject.GetComponent<TextMeshProUGUI>().text = currClothingData.description;

                    TextMeshProUGUI mainValtmp = ds.transform.Find("Main Value").transform.Find("Main Value text").gameObject.GetComponent<TextMeshProUGUI>();

                    if (currClothingData.armorVal > 0)
                    {
                        ds.transform.Find("Main Value").transform.Find("Shield icon").gameObject.SetActive(true);
                        ds.transform.Find("Main Value").transform.Find("Damage icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Heal icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Mult icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Speed icon").gameObject.SetActive(false);

                        mainValtmp.text = currClothingData.armorVal.ToString();
                    }
                    else if (currClothingData.speedVal > 0)
                    {
                        ds.transform.Find("Main Value").transform.Find("Heal icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Damage icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Shield icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Mult icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Speed icon").gameObject.SetActive(true);

                        mainValtmp.text = currClothingData.speedVal.ToString();
                    }
                    else
                    {
                        ds.transform.Find("Main Value").transform.Find("Heal icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Damage icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Shield icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Mult icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Speed icon").gameObject.SetActive(false);

                        mainValtmp.text = "";
                    }

                    ds.transform.Find("Price").transform.Find("Price text").gameObject.GetComponent<TextMeshProUGUI>().text = currClothingData.cost.ToString();


                    if (ds.transform.Find("Icon Slot").childCount > 1)
                    {
                        Destroy(ds.transform.Find("Icon Slot").GetChild(1).gameObject);
                    }

                    GameObject bigIcon = Instantiate(icon, ds.transform.Find("Icon Slot"));
                    bigIcon.transform.localScale = bigIcon.transform.localScale * 500;
                    bigIcon.transform.localPosition = new Vector3(bigIcon.transform.localPosition.x * 500, bigIcon.transform.localPosition.y * 500, bigIcon.transform.localPosition.z);
                    bigIcon.transform.SetAsLastSibling();


                }
                else if (game.currTab == GameManager.Tabs.Weapons)
                {
                    WeaponData currWeaponData = icon.GetComponent<IconManager>().Weapon;



                    ds.transform.Find("Name text").gameObject.GetComponent<TextMeshProUGUI>().text = currWeaponData.weaponName;
                    ds.transform.Find("Type text").gameObject.GetComponent<TextMeshProUGUI>().text = currWeaponData.weaponType.ToString();
                    ds.transform.Find("Rarity text").gameObject.GetComponent<TextMeshProUGUI>().text = currWeaponData.weaponRarity.ToString();

                    ds.transform.Find("Description text").gameObject.GetComponent<TextMeshProUGUI>().text = currWeaponData.weaponDescription;

                    TextMeshProUGUI mainValtmp = ds.transform.Find("Main Value").transform.Find("Main Value text").gameObject.GetComponent<TextMeshProUGUI>();
                    if (currWeaponData.damageVal > 0)
                    {
                        ds.transform.Find("Main Value").transform.Find("Damage icon").gameObject.SetActive(true);
                        ds.transform.Find("Main Value").transform.Find("Shield icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Heal icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Mult icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Speed icon").gameObject.SetActive(false);

                        mainValtmp.text = currWeaponData.damageVal.ToString();
                    }
                    else if (currWeaponData.shieldVal > 0)
                    {
                        ds.transform.Find("Main Value").transform.Find("Shield icon").gameObject.SetActive(true);
                        ds.transform.Find("Main Value").transform.Find("Damage icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Heal icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Mult icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Speed icon").gameObject.SetActive(false);

                        mainValtmp.text = currWeaponData.shieldVal.ToString();
                    }
                    else
                    {
                        ds.transform.Find("Main Value").transform.Find("Heal icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Damage icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Shield icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Mult icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Speed icon").gameObject.SetActive(false);

                        mainValtmp.text = "";

                    }

                    ds.transform.Find("Price").transform.Find("Price text").gameObject.GetComponent<TextMeshProUGUI>().text = currWeaponData.weaponCost.ToString();

                    switch (currWeaponData.damageEffects)
                    {
                        case "Burning":
                            ds.transform.Find("Effect").transform.Find("Burning Icon").gameObject.SetActive(true);
                            ds.transform.Find("Effect").transform.Find("Poisoned Icon").gameObject.SetActive(false);
                            break;
                        case "Poisoned":
                            ds.transform.Find("Effect").transform.Find("Poisoned Icon").gameObject.SetActive(true);
                            ds.transform.Find("Effect").transform.Find("Burning Icon").gameObject.SetActive(false);

                            break;
                        default:
                            ds.transform.Find("Effect").transform.Find("Burning Icon").gameObject.SetActive(false);
                            ds.transform.Find("Effect").transform.Find("Poisoned Icon").gameObject.SetActive(false);

                            break;
                    }
                    if (ds.transform.Find("Icon Slot").childCount > 1)
                    {
                        Destroy(ds.transform.Find("Icon Slot").GetChild(1).gameObject);
                    }

                    GameObject bigIcon = Instantiate(icon, ds.transform.Find("Icon Slot"));
                    bigIcon.transform.localScale = bigIcon.transform.localScale * 500;
                    bigIcon.transform.localPosition = new Vector3(bigIcon.transform.localPosition.x * 500, bigIcon.transform.localPosition.y * 500, bigIcon.transform.localPosition.z);
                    bigIcon.transform.SetAsLastSibling();

                }
                else if (game.currTab == GameManager.Tabs.Gear)
                {
                    GearData currGearData = icon.GetComponent<IconManager>().Gear;


                    ds.transform.Find("Name text").gameObject.GetComponent<TextMeshProUGUI>().text = currGearData.gearName;
                    ds.transform.Find("Type text").gameObject.GetComponent<TextMeshProUGUI>().text = currGearData.gearType.ToString();
                    ds.transform.Find("Rarity text").gameObject.GetComponent<TextMeshProUGUI>().text = currGearData.gearRarity.ToString();

                    ds.transform.Find("Description text").gameObject.GetComponent<TextMeshProUGUI>().text = currGearData.gearDescription;

                    TextMeshProUGUI mainValtmp = ds.transform.Find("Main Value").transform.Find("Main Value text").gameObject.GetComponent<TextMeshProUGUI>();
                    if (currGearData.damageVal > 0)
                    {
                        ds.transform.Find("Main Value").transform.Find("Damage icon").gameObject.SetActive(true);
                        ds.transform.Find("Main Value").transform.Find("Shield icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Heal icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Mult icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Speed icon").gameObject.SetActive(false);

                        mainValtmp.text = currGearData.damageVal.ToString();
                    }
                    else if (currGearData.shieldVal > 0)
                    {
                        ds.transform.Find("Main Value").transform.Find("Shield icon").gameObject.SetActive(true);
                        ds.transform.Find("Main Value").transform.Find("Damage icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Heal icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Mult icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Speed icon").gameObject.SetActive(false);

                        mainValtmp.text = currGearData.shieldVal.ToString();
                    }
                    else if (currGearData.healVal > 0)
                    {
                        ds.transform.Find("Main Value").transform.Find("Heal icon").gameObject.SetActive(true);
                        ds.transform.Find("Main Value").transform.Find("Damage icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Shield icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Mult icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Speed icon").gameObject.SetActive(false);

                        mainValtmp.text = currGearData.healVal.ToString();
                    }
                    else
                    {
                        ds.transform.Find("Main Value").transform.Find("Heal icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Damage icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Shield icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Mult icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Speed icon").gameObject.SetActive(false);

                        mainValtmp.text = "";

                    }

                    ds.transform.Find("Price").transform.Find("Price text").gameObject.GetComponent<TextMeshProUGUI>().text = currGearData.gearCost.ToString();

                    switch (currGearData.damageEffects)
                    {
                        case "Burning":
                            ds.transform.Find("Effect").transform.Find("Burning Icon").gameObject.SetActive(true);
                            ds.transform.Find("Effect").transform.Find("Poisoned Icon").gameObject.SetActive(false);
                            break;
                        case "Poisoned":
                            ds.transform.Find("Effect").transform.Find("Poisoned Icon").gameObject.SetActive(true);
                            ds.transform.Find("Effect").transform.Find("Burning Icon").gameObject.SetActive(false);

                            break;
                        default:
                            ds.transform.Find("Effect").transform.Find("Burning Icon").gameObject.SetActive(false);
                            ds.transform.Find("Effect").transform.Find("Poisoned Icon").gameObject.SetActive(false);

                            break;
                    }
                    if (ds.transform.Find("Icon Slot").childCount > 1)
                    {
                        Destroy(ds.transform.Find("Icon Slot").GetChild(1).gameObject);
                    }

                    GameObject bigIcon = Instantiate(icon, ds.transform.Find("Icon Slot"));
                    bigIcon.transform.localScale = bigIcon.transform.localScale * 500;
                    bigIcon.transform.localPosition = new Vector3(bigIcon.transform.localPosition.x * 500, bigIcon.transform.localPosition.y * 500, bigIcon.transform.localPosition.z);
                    bigIcon.transform.SetAsLastSibling();
                }
                else if (game.currTab == GameManager.Tabs.Ammo)
                {
                    AmmoData currAmmoData = icon.GetComponent<IconManager>().Ammo;



                    ds.transform.Find("Name text").gameObject.GetComponent<TextMeshProUGUI>().text = currAmmoData.ammoName;
                    ds.transform.Find("Type text").gameObject.GetComponent<TextMeshProUGUI>().text = currAmmoData.ammoType.ToString();
                    ds.transform.Find("Rarity text").gameObject.GetComponent<TextMeshProUGUI>().text = currAmmoData.ammoRarity.ToString();

                    ds.transform.Find("Description text").gameObject.GetComponent<TextMeshProUGUI>().text = currAmmoData.ammoDescription;

                    TextMeshProUGUI mainValtmp = ds.transform.Find("Main Value").transform.Find("Main Value text").gameObject.GetComponent<TextMeshProUGUI>();

                    ds.transform.Find("Main Value").transform.Find("Mult icon").gameObject.SetActive(true);
                    ds.transform.Find("Main Value").transform.Find("Shield icon").gameObject.SetActive(false);
                    ds.transform.Find("Main Value").transform.Find("Damage icon").gameObject.SetActive(false);
                    ds.transform.Find("Main Value").transform.Find("Heal icon").gameObject.SetActive(false);
                    ds.transform.Find("Main Value").transform.Find("Speed icon").gameObject.SetActive(false);

                    mainValtmp.text = currAmmoData.damageMult.ToString();


                    ds.transform.Find("Price").transform.Find("Price text").gameObject.GetComponent<TextMeshProUGUI>().text = currAmmoData.ammoCost.ToString();

                    switch (currAmmoData.damageEffects)
                    {
                        case "Burning":
                            ds.transform.Find("Effect").transform.Find("Burning Icon").gameObject.SetActive(true);
                            ds.transform.Find("Effect").transform.Find("Poisoned Icon").gameObject.SetActive(false);
                            break;
                        case "Poisoned":
                            ds.transform.Find("Effect").transform.Find("Poisoned Icon").gameObject.SetActive(true);
                            ds.transform.Find("Effect").transform.Find("Burning Icon").gameObject.SetActive(false);

                            break;
                        default:
                            ds.transform.Find("Effect").transform.Find("Burning Icon").gameObject.SetActive(false);
                            ds.transform.Find("Effect").transform.Find("Poisoned Icon").gameObject.SetActive(false);
                            break;
                    }
                    if (ds.transform.Find("Icon Slot").childCount > 1)
                    {
                        Destroy(ds.transform.Find("Icon Slot").GetChild(1).gameObject);
                    }

                    GameObject bigIcon = Instantiate(icon, ds.transform.Find("Icon Slot"));
                    bigIcon.transform.localScale = bigIcon.transform.localScale * 500;
                    bigIcon.transform.localPosition = new Vector3(bigIcon.transform.localPosition.x * 500, bigIcon.transform.localPosition.y * 500, bigIcon.transform.localPosition.z);
                    bigIcon.transform.SetAsLastSibling();



                }
                else if (game.currTab == GameManager.Tabs.Spells)
                {
                    SpellData currSpellData = icon.GetComponent<IconManager>().Spell;

                    ds.transform.Find("Name text").gameObject.GetComponent<TextMeshProUGUI>().text = currSpellData.spellName;
                    ds.transform.Find("Type text").gameObject.GetComponent<TextMeshProUGUI>().text = currSpellData.spellType.ToString();
                    ds.transform.Find("Rarity text").gameObject.GetComponent<TextMeshProUGUI>().text = "Level " + currSpellData.spellLevel.ToString();

                    ds.transform.Find("Description text").gameObject.GetComponent<TextMeshProUGUI>().text = currSpellData.spellDescription;

                    TextMeshProUGUI mainValtmp = ds.transform.Find("Main Value").transform.Find("Main Value text").gameObject.GetComponent<TextMeshProUGUI>();

                    if (currSpellData.damageVal > 0)
                    {
                        ds.transform.Find("Main Value").transform.Find("Damage icon").gameObject.SetActive(true);
                        ds.transform.Find("Main Value").transform.Find("Mult icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Shield icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Heal icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Speed icon").gameObject.SetActive(false);

                        mainValtmp.text = currSpellData.damageVal.ToString();
                    }

                    ds.transform.Find("Price").transform.Find("Price text").gameObject.GetComponent<TextMeshProUGUI>().text = "0";

                    switch (currSpellData.damageEffects)
                    {
                        case "Burning":
                            ds.transform.Find("Effect").transform.Find("Burning Icon").gameObject.SetActive(true);
                            ds.transform.Find("Effect").transform.Find("Poisoned Icon").gameObject.SetActive(false);
                            break;
                        case "Poisoned":
                            ds.transform.Find("Effect").transform.Find("Poisoned Icon").gameObject.SetActive(true);
                            ds.transform.Find("Effect").transform.Find("Burning Icon").gameObject.SetActive(false);

                            break;
                        default:
                            ds.transform.Find("Effect").transform.Find("Burning Icon").gameObject.SetActive(false);
                            ds.transform.Find("Effect").transform.Find("Poisoned Icon").gameObject.SetActive(false);
                            break;
                    }
                    if (ds.transform.Find("Icon Slot").childCount > 1)
                    {
                        Destroy(ds.transform.Find("Icon Slot").GetChild(1).gameObject);
                    }

                    GameObject bigIcon = Instantiate(icon, ds.transform.Find("Icon Slot"));
                    bigIcon.transform.localScale = bigIcon.transform.localScale * 500;
                    bigIcon.transform.localPosition = new Vector3(bigIcon.transform.localPosition.x * 500, bigIcon.transform.localPosition.y * 500, bigIcon.transform.localPosition.z);
                    bigIcon.transform.SetAsLastSibling();

                }
                else if (game.currTab == GameManager.Tabs.Abilities)
                {
                    AbilityData currAbilityData = icon.GetComponent<IconManager>().Ability;

                    ds.transform.Find("Name text").gameObject.GetComponent<TextMeshProUGUI>().text = currAbilityData.abilityName;
                    ds.transform.Find("Type text").gameObject.GetComponent<TextMeshProUGUI>().text = currAbilityData.abilityType.ToString();
                    ds.transform.Find("Rarity text").gameObject.GetComponent<TextMeshProUGUI>().text = "Level " + currAbilityData.abilityLevel.ToString();

                    ds.transform.Find("Description text").gameObject.GetComponent<TextMeshProUGUI>().text = currAbilityData.abilityDescription;

                    TextMeshProUGUI mainValtmp = ds.transform.Find("Main Value").transform.Find("Main Value text").gameObject.GetComponent<TextMeshProUGUI>();

                    if (currAbilityData.damageVal > 0)
                    {
                        ds.transform.Find("Main Value").transform.Find("Damage icon").gameObject.SetActive(true);
                        ds.transform.Find("Main Value").transform.Find("Mult icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Shield icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Heal icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Speed icon").gameObject.SetActive(false);

                        mainValtmp.text = currAbilityData.damageVal.ToString();
                    }

                    ds.transform.Find("Price").transform.Find("Price text").gameObject.GetComponent<TextMeshProUGUI>().text = "0";

                    switch (currAbilityData.damageEffects)
                    {
                        case "Burning":
                            ds.transform.Find("Effect").transform.Find("Burning Icon").gameObject.SetActive(true);
                            ds.transform.Find("Effect").transform.Find("Poisoned Icon").gameObject.SetActive(false);
                            break;
                        case "Poisoned":
                            ds.transform.Find("Effect").transform.Find("Poisoned Icon").gameObject.SetActive(true);
                            ds.transform.Find("Effect").transform.Find("Burning Icon").gameObject.SetActive(false);

                            break;
                        default:
                            ds.transform.Find("Effect").transform.Find("Burning Icon").gameObject.SetActive(false);
                            ds.transform.Find("Effect").transform.Find("Poisoned Icon").gameObject.SetActive(false);
                            break;
                    }
                    if (ds.transform.Find("Icon Slot").childCount > 1)
                    {
                        Destroy(ds.transform.Find("Icon Slot").GetChild(1).gameObject);
                    }

                    GameObject bigIcon = Instantiate(icon, ds.transform.Find("Icon Slot"));
                    bigIcon.transform.localScale = bigIcon.transform.localScale * 500;
                    bigIcon.transform.localPosition = new Vector3(bigIcon.transform.localPosition.x * 500, bigIcon.transform.localPosition.y * 500, bigIcon.transform.localPosition.z);
                    bigIcon.transform.SetAsLastSibling();
                }

                switch (ds.transform.Find("Rarity text").gameObject.GetComponent<TextMeshProUGUI>().text)
                {
                    case "Common":
                    case "Level 0":
                    case "Level 1":
                        ds.transform.Find("Rarity Box").GetComponent<MeshRenderer>().material = game.commonMat;
                        ds.transform.Find("Rarity text").GetComponent<TextMeshProUGUI>().color = new Color(game.commonMat.GetColor("Color_1dc44c8b59a94ba2b1774b9575cdbe9b").r, game.commonMat.GetColor("Color_1dc44c8b59a94ba2b1774b9575cdbe9b").g, game.commonMat.GetColor("Color_1dc44c8b59a94ba2b1774b9575cdbe9b").b, 1);
                        break;
                    case "Uncommon":
                    case "Level 2":
                        ds.transform.Find("Rarity Box").GetComponent<MeshRenderer>().material = game.uncommonMat;
                        ds.transform.Find("Rarity text").GetComponent<TextMeshProUGUI>().color = new Color(game.uncommonMat.GetColor("Color_1dc44c8b59a94ba2b1774b9575cdbe9b").r, game.uncommonMat.GetColor("Color_1dc44c8b59a94ba2b1774b9575cdbe9b").g, game.uncommonMat.GetColor("Color_1dc44c8b59a94ba2b1774b9575cdbe9b").b, 1);
                        break;
                    case "Rare":
                    case "Level 3":
                        ds.transform.Find("Rarity Box").GetComponent<MeshRenderer>().material = game.rareMat;
                        ds.transform.Find("Rarity text").GetComponent<TextMeshProUGUI>().color = new Color(game.rareMat.GetColor("Color_1dc44c8b59a94ba2b1774b9575cdbe9b").r, game.rareMat.GetColor("Color_1dc44c8b59a94ba2b1774b9575cdbe9b").g, game.rareMat.GetColor("Color_1dc44c8b59a94ba2b1774b9575cdbe9b").b, 1);
                        break;
                    case "Epic":
                    case "Level 4":
                        ds.transform.Find("Rarity Box").GetComponent<MeshRenderer>().material = game.epicMat;
                        ds.transform.Find("Rarity text").GetComponent<TextMeshProUGUI>().color = new Color(game.epicMat.GetColor("Color_1dc44c8b59a94ba2b1774b9575cdbe9b").r, game.epicMat.GetColor("Color_1dc44c8b59a94ba2b1774b9575cdbe9b").g, game.epicMat.GetColor("Color_1dc44c8b59a94ba2b1774b9575cdbe9b").b, 1);
                        break;
                    case "Legendary":
                    case "Level 5":
                        ds.transform.Find("Rarity Box").GetComponent<MeshRenderer>().material = game.legendaryMat;
                        ds.transform.Find("Rarity text").GetComponent<TextMeshProUGUI>().color = new Color(game.legendaryMat.GetColor("Color_1dc44c8b59a94ba2b1774b9575cdbe9b").r, game.legendaryMat.GetColor("Color_1dc44c8b59a94ba2b1774b9575cdbe9b").g, game.legendaryMat.GetColor("Color_1dc44c8b59a94ba2b1774b9575cdbe9b").b, 1);
                        break;
                }

            }
            //allows for equipping
            if (select && currBox.transform.childCount > 1)
            {
                game.StartCoroutine(EquippingItems(icon));
                game.isEquipping = true;
            }
            // allows for unequipping
            if (unEquip)
            {
                game.StartCoroutine(UnequippingItems());
                game.isUnEquipping = true;

            }

            prevAltMove = pc.altMove;
            prevTab = game.currTab;
            prevMoveMag = moveVal.magnitude;

            yield return new WaitForSecondsRealtime(0.1f);
        }

        yield return new WaitUntil(() => !game.isInvCreated || game.isEquipping || game.isUnEquipping);

        game.isSelectInvRunning = false;
    }

    public static IEnumerator SelectCustomize()
    {
        InventoryManager inv = game.player.GetComponent<InventoryManager>();
        InventoryManager pmim = game.characterBox.transform.Find("Player Model").GetComponent<InventoryManager>();
        Vector2 moveVal = game.player.GetComponent<PlayerControl>().moveStick;
        Vector2 currentPos = new(0, 0);
        PlayerControl pc = game.player.GetComponent<PlayerControl>();



        game.invSelector.SetActive(true);

        int prevAltMove = pc.altMove;
        GameManager.Tabs prevTab = game.currTab;
        while ((game.currTab == GameManager.Tabs.Customize && !game.isGameActive))
        {
            game.isSelectInvRunning = true;

            moveVal = pc.moveStick;
            bool select = pc.jump;

            GameObject tabSelector = game.selectionContainer.transform.Find("Customize Tab").transform.Find("Tab selector").gameObject;

            //allows for tab swapping

            if (prevAltMove != pc.altMove && ((pc.altMove > 0 && ((int)game.currTab) < 7) || (pc.altMove < 0 && ((int)game.currTab) > 0)))
            {
                game.currTab += pc.altMove;
                DestroyCustomize();

                game.customizeBox.SetActive(false);

                switch (game.currTab)
                {

                    case GameManager.Tabs.Abilities:
                        tabSelector.transform.parent = game.selectionContainer.transform.Find("Abilities Tab");
                        tabSelector.transform.localPosition = new Vector3(0, 0, tabSelector.transform.localPosition.z);
                        game.isInvCreated = false;
                        game.isSelectInvRunning = false;
                        CreateInventory(GameManager.Tabs.Abilities, true);

                        break;
                }
            }
            else if (moveVal.x > 0.5 && currentPos.x == 6)
            {
                GameObject temp = null;
                switch (currentPos.y)
                {
                    case 0:
                        if (game.playerStats.bodyType == StatsManager.BodyTypes.Male)
                        {
                            temp = inv.facesMale[0];
                            inv.facesMale.RemoveAt(0);
                            inv.facesMale.Add(temp);

                            for (int i = 0; i < 7; i++)
                            {
                                GameObject icon = Instantiate(inv.facesMale[i], game.customizeBox.transform.Find("Face Slot (" + i + ")"), false);
                                Destroy(game.customizeBox.transform.Find("Face Slot (" + i + ")").GetChild(0).gameObject);
                                icon.transform.SetAsFirstSibling();
                            }
                        }
                        else
                        {
                            temp = inv.facesFemale[0];
                            inv.facesFemale.RemoveAt(0);
                            inv.facesFemale.Add(temp);

                            for (int i = 0; i < 7; i++)
                            {
                                GameObject icon = Instantiate(inv.facesFemale[i], game.customizeBox.transform.Find("Face Slot (" + i + ")"), false);
                                Destroy(game.customizeBox.transform.Find("Face Slot (" + i + ")").GetChild(0).gameObject);
                                icon.transform.SetAsFirstSibling();

                            }
                        }
                        break;
                    case 1:
                        temp = inv.hair[0];
                        inv.hair.RemoveAt(0);
                        inv.hair.Add(temp);

                        for (int i = 0; i < 7; i++)
                        {
                            GameObject icon = Instantiate(inv.hair[i], game.customizeBox.transform.Find("Hair Slot (" + i + ")"), false);
                            Destroy(game.customizeBox.transform.Find("Hair Slot (" + i + ")").GetChild(0).gameObject);
                            icon.transform.SetAsFirstSibling();
                        }
                        break;
                    case 2:
                        temp = inv.facialHair[0];
                        inv.facialHair.RemoveAt(0);
                        inv.facialHair.Add(temp);

                        for (int i = 0; i < 7; i++)
                        {
                            GameObject icon = Instantiate(inv.facialHair[i], game.customizeBox.transform.Find("Facial Hair Slot (" + i + ")"), false);
                            Destroy(game.customizeBox.transform.Find("Facial Hair Slot (" + i + ")").GetChild(0).gameObject);
                            icon.transform.SetAsFirstSibling();
                        }
                        break;
                    case 3:
                        temp = inv.clothingColors[0];
                        inv.clothingColors.RemoveAt(0);
                        inv.clothingColors.Add(temp);

                        for (int i = 0; i < 7; i++)
                        {
                            GameObject icon = Instantiate(inv.clothingColors[i], game.customizeBox.transform.Find("Clothing Colors Slot (" + i + ")"), false);
                            Destroy(game.customizeBox.transform.Find("Clothing Colors Slot (" + i + ")").GetChild(0).gameObject);
                            icon.transform.SetAsFirstSibling();
                        }
                        break;
                }
            }
            else if (moveVal.x < -0.5 && currentPos.x == 0)
            {
                GameObject temp = null;
                switch (currentPos.y)
                {
                    case 0:

                        if (game.playerStats.bodyType == StatsManager.BodyTypes.Male)
                        {
                            temp = inv.facesMale[22];
                            inv.facesMale.RemoveAt(22);
                            inv.facesMale.Insert(0, temp);

                            for (int i = 0; i < 7; i++)
                            {
                                GameObject icon = Instantiate(inv.facesMale[i], game.customizeBox.transform.Find("Face Slot (" + i + ")"), false);
                                Destroy(game.customizeBox.transform.Find("Face Slot (" + i + ")").GetChild(0).gameObject);
                                icon.transform.SetAsFirstSibling();
                            }
                        }
                        else
                        {
                            temp = inv.facesFemale[22];
                            inv.facesFemale.RemoveAt(22);
                            inv.facesFemale.Insert(0, temp);

                            for (int i = 0; i < 7; i++)
                            {
                                GameObject icon = Instantiate(inv.facesFemale[i], game.customizeBox.transform.Find("Face Slot (" + i + ")"), false);
                                Destroy(game.customizeBox.transform.Find("Face Slot (" + i + ")").GetChild(0).gameObject);
                                icon.transform.SetAsFirstSibling();
                            }
                        }
                        break;
                    case 1:
                        temp = inv.hair[37];
                        inv.hair.RemoveAt(37);
                        inv.hair.Insert(0, temp);

                        for (int i = 0; i < 7; i++)
                        {
                            GameObject icon = Instantiate(inv.hair[i], game.customizeBox.transform.Find("Hair Slot (" + i + ")"), false);
                            Destroy(game.customizeBox.transform.Find("Hair Slot (" + i + ")").GetChild(0).gameObject);
                            icon.transform.SetAsFirstSibling();
                        }
                        break;
                    case 2:
                        temp = inv.facialHair[17];
                        inv.facialHair.RemoveAt(17);
                        inv.facialHair.Insert(0, temp);

                        for (int i = 0; i < 7; i++)
                        {
                            GameObject icon = Instantiate(inv.facialHair[i], game.customizeBox.transform.Find("Facial Hair Slot (" + i + ")"), false);
                            Destroy(game.customizeBox.transform.Find("Facial Hair Slot (" + i + ")").GetChild(0).gameObject);
                            icon.transform.SetAsFirstSibling();
                        }
                        break;
                    case 3:
                        temp = inv.clothingColors[9];
                        inv.clothingColors.RemoveAt(9);
                        inv.clothingColors.Insert(0, temp);

                        for (int i = 0; i < 7; i++)
                        {
                            GameObject icon = Instantiate(inv.clothingColors[i], game.customizeBox.transform.Find("Clothing Colors Slot (" + i + ")"), false);
                            Destroy(game.customizeBox.transform.Find("Clothing Colors Slot (" + i + ")").GetChild(0).gameObject);
                            icon.transform.SetAsFirstSibling();
                        }
                        break;
                }
            }

            if (moveVal.x > 0.5 && currentPos.x < 6)
            {
                SetCustomizeSelectorPos(currentPos.x + 1, currentPos.y);
                currentPos.x++;
            }
            else if (moveVal.x < -0.5 && currentPos.x > 0)
            {
                SetCustomizeSelectorPos(currentPos.x - 1, currentPos.y);
                currentPos.x--;
            }

            if (moveVal.y > 0.5 && currentPos.y > 0)
            {
                SetCustomizeSelectorPos(currentPos.x, currentPos.y - 1);
                currentPos.y--;
            }
            else if (moveVal.y < -0.5 && currentPos.y < 3)
            {
                SetCustomizeSelectorPos(currentPos.x, currentPos.y + 1);
                currentPos.y++;
            }

            GameObject currBox = null;
            GameObject currIcon = null;
            string section = null;

            if (game.invSelector != null)
            {
                currBox = game.invSelector.transform.parent.gameObject;
                section = currBox.name.Substring(0, currBox.name.IndexOf(" Slot"));
            }

            if (select && currBox.transform.childCount > 1)
            {
                currIcon = currBox.transform.GetChild(0).gameObject;
                switch (section)
                {
                    case "Face":
                        /*if (game.selectedFace != null)
                        {
                            Destroy(game.selectedFace);
                        }
                        game.selectedFace = Instantiate(game.selected, currIcon.transform.parent, false);
                        game.selectedFace.transform.SetAsLastSibling();
                        game.selectedFace.transform.parent = currIcon.transform;*/

                        inv.SetClothingActiveLite(currIcon.name.Substring(0, currIcon.name.IndexOf("(")), ClothingData.ClothingSlot.Face);
                        pmim.SetClothingActiveLite(currIcon.name.Substring(0, currIcon.name.IndexOf("(")), ClothingData.ClothingSlot.Face);
                        break;
                    case "Hair":
                        /*if (game.selectedHair != null)
                        {
                            Destroy(game.selectedHair);
                        }
                        game.selectedHair = Instantiate(game.selected, currIcon.transform.parent, false);
                        game.selectedHair.transform.SetAsLastSibling();
                        game.selectedHair.transform.parent = currIcon.transform;*/

                        inv.SetClothingActiveLite(currIcon.name.Substring(0, currIcon.name.IndexOf("(")), ClothingData.ClothingSlot.Hair);
                        pmim.SetClothingActiveLite(currIcon.name.Substring(0, currIcon.name.IndexOf("(")), ClothingData.ClothingSlot.Hair);
                        break;
                    case "Facial Hair":
                        /*if (game.selectedFacialHair != null)
                        {
                            Destroy(game.selectedFacialHair);
                        }
                        game.selectedFacialHair = Instantiate(game.selected, currIcon.transform.parent, false);
                        game.selectedFacialHair.transform.SetAsLastSibling();
                        game.selectedFacialHair.transform.parent = currIcon.transform;*/

                        inv.SetClothingActiveLite(currIcon.name.Substring(0, currIcon.name.IndexOf("(")), ClothingData.ClothingSlot.Facial_Hair);
                        pmim.SetClothingActiveLite(currIcon.name.Substring(0, currIcon.name.IndexOf("(")), ClothingData.ClothingSlot.Facial_Hair);
                        break;
                    case "Clothing Colors":
                        /*(if (game.selectedClothingColors != null)
                        {
                            Destroy(game.selectedClothingColors);
                        }
                        game.selectedClothingColors = Instantiate(game.selected, currIcon.transform.parent, false);
                        game.selectedClothingColors.transform.SetAsLastSibling();
                        game.selectedClothingColors.transform.parent = currIcon.transform;*/
                        game.player.GetComponent<PlayerSetup>().SetPlayerColors(currIcon.transform.GetChild(0).GetComponent<MeshRenderer>().material.GetColor("_BaseColor"), currIcon.transform.GetChild(1).GetComponent<MeshRenderer>().material.GetColor("_BaseColor"));
                        break;
                }

            }
            yield return new WaitForSecondsRealtime(0.1f);
        }


    }

    public static IEnumerator EquippingItems(GameObject icon)
    {
        string location = icon.GetComponent<IconManager>().Location;
        List<GameObject> possibleSlots = new();

        for (int i = 0; i < game.characterBox.transform.childCount + 7; i++)// the six of the QS 
        {
            GameObject currChild = null;
            if (i < game.characterBox.transform.childCount)
            {
                currChild = game.characterBox.transform.GetChild(i).gameObject;
            }
            else
            {
                currChild = game.QSContainer.transform.GetChild(i - game.characterBox.transform.childCount).gameObject;

            }
            string currLocation = currChild.name.Substring(0, currChild.name.IndexOf(" "));

            if (location == "Weapon" && !currLocation.Equals("QS_Item"))
            {
                if (currLocation == "L_weapon" && game.characterBox.transform.Find("R_weapon slot").GetComponentInChildren<IconManager>() != null)
                {
                    possibleSlots.Add(currChild);

                }
                if (currLocation == "R_weapon")
                {
                    possibleSlots.Add(currChild);

                }

            }
            else if (currLocation == location || (currLocation.Equals("QS_Item") && (icon.GetComponent<IconManager>().Weapon != null || icon.GetComponent<IconManager>().Gear != null || icon.GetComponent<IconManager>().Ammo)) || (currLocation.Equals("QS_Spell") && icon.GetComponent<IconManager>().Spell != null) || (currLocation.Equals("QS_Ability") && icon.GetComponent<IconManager>().Ability != null))
            {
                possibleSlots.Add(currChild);
            }
        }


        Vector2 moveVal = game.player.GetComponent<PlayerControl>().moveStick;
        int currentPos = 0;//0 is head, 1 is left weapon...(hierarchy order)
        PlayerControl pc = game.player.GetComponent<PlayerControl>();
        GameObject currBox = possibleSlots[currentPos];

        moveVal = pc.moveStick;
        bool select = pc.jump;

        game.invSelector.transform.parent = currBox.transform;
        game.invSelector.transform.SetAsLastSibling();
        game.invSelector.transform.localPosition = new Vector3(0, 0, game.invSelector.transform.localPosition.z);

        while (select)
        {
            select = pc.jump;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        yield return new WaitUntil(() => !select);
        possibleSlots.TrimExcess();
        select = pc.jump;
        while (!select)
        {
            select = pc.jump;
            moveVal = pc.moveStick;
            currBox = possibleSlots[currentPos];

            if ((moveVal.x > 0.5 || moveVal.y < -0.5) && currentPos < possibleSlots.Count - 1)
            {
                currentPos += 1;
                currBox = possibleSlots[currentPos];
                game.invSelector.transform.parent = currBox.transform;
                game.invSelector.transform.SetAsLastSibling();
                game.invSelector.transform.localPosition = new Vector3(0, 0, game.invSelector.transform.localPosition.z);
            }
            if ((moveVal.x < -0.5 || moveVal.y > 0.5) && currentPos > 0)
            {
                currentPos -= 1;
                currBox = possibleSlots[currentPos];
                game.invSelector.transform.parent = currBox.transform;
                game.invSelector.transform.SetAsLastSibling();
                game.invSelector.transform.localPosition = new Vector3(0, 0, game.invSelector.transform.localPosition.z);
            }

            yield return new WaitForSecondsRealtime(0.1f);
        }
        yield return new WaitUntil(() => select);

        EquipItem(icon, currBox);
        game.StartCoroutine(SelectInv(4, 7));
    }

    public static IEnumerator UnequippingItems()
    {

        yield return new WaitForSecondsRealtime(0.1f);
        Vector2 moveVal;
        int currentPos = 0;//0 is head, 1 is left weapon...(hierarchy order)
        GameObject currBox = game.characterBox.transform.GetChild(currentPos).gameObject;

        InventoryManager pmim = game.characterBox.transform.Find("Player Model").GetComponent<InventoryManager>();

        bool select = game.pc.jump;
        bool unequip = game.pc.shield;

        game.invSelector.transform.parent = currBox.transform;
        game.invSelector.transform.SetAsLastSibling();
        game.invSelector.transform.localPosition = new Vector3(0, 0, game.invSelector.transform.localPosition.z);

        while (!game.isEquipping && game.isInvCreated && currBox.transform.childCount > 0 && (!select || currBox.transform.GetChild(1).gameObject.GetComponent<IconManager>() == null) && !unequip)
        {
            select = game.pc.jump;
            unequip = game.pc.shield;
            moveVal = game.pc.moveStick;
            currBox = game.characterBox.transform.GetChild(currentPos).gameObject;

            if ((moveVal.x > 0.5 || moveVal.y < -0.5) && currentPos < 7)
            {
                currentPos += 1;
                currBox = game.characterBox.transform.GetChild(currentPos).gameObject;
                game.invSelector.transform.parent = currBox.transform;
                game.invSelector.transform.SetAsLastSibling();
                game.invSelector.transform.localPosition = new Vector3(0, 0, game.invSelector.transform.localPosition.z);
            }
            if ((moveVal.x < -0.5 || moveVal.y > 0.5) && currentPos > 0)
            {
                currentPos -= 1;
                currBox = game.characterBox.transform.GetChild(currentPos).gameObject;
                game.invSelector.transform.parent = currBox.transform;
                game.invSelector.transform.SetAsLastSibling();
                game.invSelector.transform.localPosition = new Vector3(0, 0, game.invSelector.transform.localPosition.z);
            }


            yield return new WaitForSecondsRealtime(0.1f);
        }
        if (select && !game.isEquipping && game.isInvCreated)
        {
            currBox.transform.GetChild(0).gameObject.SetActive(true);
            GameObject icon = currBox.transform.GetChild(1).gameObject;
            InventoryManager im = game.player.GetComponent<InventoryManager>();



            string eqSlot = currBox.name.Substring(0, currBox.name.IndexOf(" "));

            if (icon.GetComponent<IconManager>().objectClass == IconManager.Class.Weapon)
            {
                im.weaponInv.Add(icon.GetComponent<IconManager>().Weapon);

                if (eqSlot == "R_weapon")
                {

                    im.activeWeaponR = null;
                    Destroy(im.gameObject.GetComponent<CharacterAndWeaponController>().weaponRObject);
                    Destroy(pmim.gameObject.GetComponent<CharacterAndWeaponController>().weaponRObject);
                    im.SetWeaponActive(null, "right");
                    pmim.SetWeaponActive(null, "right");

                }
                else if (eqSlot == "L_weapon")
                {
                    im.activeWeaponL = null;
                    Destroy(im.gameObject.GetComponent<CharacterAndWeaponController>().weaponLObject);
                    Destroy(pmim.gameObject.GetComponent<CharacterAndWeaponController>().weaponLObject);
                    im.SetWeaponActive(null, "left");
                    pmim.SetWeaponActive(null, "left");

                }
                else
                {
                    im.gearActive.Remove(icon.GetComponent<IconManager>().Gear);
                }

                if (game.currTab == GameManager.Tabs.Weapons)
                {
                    int emptySpot = 0;
                    for (int i = 19; i >= 0; i--)
                    {
                        if (game.inventoryBox.transform.Find("Inventory Slot (" + i + ")").transform.childCount == 0)
                        {
                            emptySpot = i;
                        }
                    }

                    GameObject newParent = game.inventoryBox.transform.Find("Inventory Slot (" + emptySpot + ")").gameObject;
                    Vector3 prevLocalPos = icon.transform.localPosition;
                    icon.transform.parent = newParent.transform;
                    icon.transform.localPosition = prevLocalPos;
                }
                else
                {
                    Destroy(icon);
                }
            }
            else if (icon.GetComponent<IconManager>().objectClass == IconManager.Class.Ammo)
            {
                im.ammoInv.Add(icon.GetComponent<IconManager>().Ammo);

                if (eqSlot == "Ammo")
                {
                    im.activeAmmo = null;
                    pmim.activeAmmo = null;
                    im.SetWeaponActive(null, "ammo");
                    pmim.SetWeaponActive(null, "ammo");
                }
                if (game.currTab == GameManager.Tabs.Ammo)
                {
                    int emptySpot = 0;
                    for (int i = 19; i >= 0; i--)
                    {
                        if (game.inventoryBox.transform.Find("Inventory Slot (" + i + ")").transform.childCount == 0)
                        {
                            emptySpot = i;
                        }
                    }
                    GameObject newParent = game.inventoryBox.transform.Find("Inventory Slot (" + emptySpot + ")").gameObject;
                    Vector3 prevLocalPos = icon.transform.localPosition;
                    icon.transform.parent = newParent.transform;
                    icon.transform.localPosition = prevLocalPos;
                }
                else
                {
                    Destroy(icon);
                }
            }
            else if (icon.GetComponent<IconManager>().objectClass == IconManager.Class.Gear)
            {
                im.gearInv.Add(icon.GetComponent<IconManager>().Gear);
                im.gearQs.Add(im.gearActiveQs[im.gearActive.IndexOf(icon.GetComponent<IconManager>().Gear)]);

                im.gearActiveQs.RemoveAt(im.gearActive.IndexOf(icon.GetComponent<IconManager>().Gear));
                im.gearActive.Remove(icon.GetComponent<IconManager>().Gear);

                if (game.currTab == GameManager.Tabs.Gear)
                {
                    int emptySpot = 0;
                    for (int i = 19; i >= 0; i--)
                    {
                        if (game.inventoryBox.transform.Find("Inventory Slot (" + i + ")").transform.childCount == 0)
                        {
                            emptySpot = i;
                        }
                    }
                    GameObject newParent = game.inventoryBox.transform.Find("Inventory Slot (" + emptySpot + ")").gameObject;
                    Vector3 prevLocalPos = icon.transform.localPosition;
                    icon.transform.parent = newParent.transform;
                    icon.transform.localPosition = prevLocalPos;
                }
                else
                {
                    Destroy(icon);
                }
            }
            else if (icon.GetComponent<IconManager>().objectClass == IconManager.Class.Clothing)
            {
                im.clothingInv.Add(icon.GetComponent<IconManager>().Clothing);
                InventoryManager inv = GameObject.Find("Player").GetComponent<InventoryManager>();
                switch (icon.GetComponent<IconManager>().Clothing.slot)
                {
                    case ClothingData.ClothingSlot.Head_Covering_Base:
                        inv.activeHead.gameObject.SetActive(false);
                        inv.activeHead = null;
                        pmim.activeHead.gameObject.SetActive(false);
                        pmim.activeHead = null;
                        break;
                    case ClothingData.ClothingSlot.Head_Covering_No_Facial_Hair:
                        inv.activeHead.gameObject.SetActive(false);
                        inv.activeHead = null;
                        pmim.activeHead.gameObject.SetActive(false);
                        pmim.activeHead = null;
                        if (inv.activeFacialHair != null)
                        {
                            inv.activeFacialHair.gameObject.SetActive(true);
                            pmim.activeFacialHair.gameObject.SetActive(true);
                        }
                        break;
                    case ClothingData.ClothingSlot.Head_Covering_No_Hair:
                        inv.activeHead.gameObject.SetActive(false);
                        inv.activeHead = null;
                        pmim.activeHead.gameObject.SetActive(false);
                        pmim.activeHead = null;
                        if (inv.activeFacialHair != null)
                        {
                            inv.activeFacialHair.gameObject.SetActive(true);
                            pmim.activeFacialHair.gameObject.SetActive(true);
                        }
                        if (inv.activeHair != null)
                        {
                            inv.activeHair.gameObject.SetActive(true);
                            pmim.activeHair.gameObject.SetActive(true);
                        }
                        break;
                    case ClothingData.ClothingSlot.Helmet:
                        inv.activeHead.gameObject.SetActive(false);
                        inv.activeHead = null;
                        pmim.activeHead.gameObject.SetActive(false);
                        pmim.activeHead = null;
                        if (inv.activeFacialHair != null)
                        {
                            inv.activeFacialHair.gameObject.SetActive(true);
                            pmim.activeFacialHair.gameObject.SetActive(true);
                        }
                        if (inv.activeHair != null)
                        {
                            inv.activeHair.gameObject.SetActive(true);
                            pmim.activeHair.gameObject.SetActive(true);
                        }
                        if (inv.activeFace != null)
                        {
                            inv.activeFace.gameObject.SetActive(true);
                            pmim.activeFace.gameObject.SetActive(true);
                        }
                        break;
                    case ClothingData.ClothingSlot.Torso:
                        if (icon.GetComponent<IconManager>().Name != "Blank")
                        {
                            inv.activeTorso.gameObject.SetActive(false);
                            inv.activeTorso = null;
                            inv.SetStartTorso();
                            inv.clothingInv.Remove(inv.startTorso);

                            pmim.activeTorso.gameObject.SetActive(false);
                            pmim.activeTorso = null;
                            pmim.SetStartTorso();
                        }
                        break;

                }
                if (icon.GetComponent<IconManager>().Name != "Blank")
                {
                    if (game.currTab == GameManager.Tabs.Clothing)
                    {
                        int emptySpot = 0;
                        for (int i = 19; i >= 0; i--)
                        {
                            if (game.inventoryBox.transform.Find("Inventory Slot (" + i + ")").transform.childCount == 0)
                            {
                                emptySpot = i;
                            }
                        }
                        GameObject newParent = game.inventoryBox.transform.Find("Inventory Slot (" + emptySpot + ")").gameObject;
                        Vector3 prevLocalPos = icon.transform.localPosition;
                        icon.transform.parent = newParent.transform;
                        icon.transform.localPosition = prevLocalPos;
                    }
                    else
                    {
                        Destroy(icon);
                    }
                }
            }
        }
        game.isUnEquipping = false;
        game.StartCoroutine(SelectInv(4, 7));

    }
}

public class ChestUIRunner : MonoBehaviour
{
    static readonly GameManager game = GameManager.gameManager;
    public static void CreateChestUI(ChestManager chest)
    {

        game.cSBox.SetActive(true);
        game.inventoryBox.SetActive(true);
        game.characterBox.SetActive(false);
        game.isInvCreated = true;
        game.isGameActive = false;
        game.cSBox.transform.Find("Main Text").gameObject.GetComponent<TextMeshProUGUI>().text = chest.chestName;
        InventoryUIRunner.CreateInventory(GameManager.Tabs.Weapons, false);

        game.csSelector = Instantiate(game.selector, game.cSBox.transform.Find("Inventory Slot (0)"));
        game.csSelector.transform.SetAsLastSibling();

        int index = 0;
        foreach (WeaponData weapon in chest.chestWeapons)
        {
            GameObject icon = weapon.icon;
            icon.GetComponent<IconManager>().Weapon = weapon;
            GameObject curBox = game.cSBox.transform.Find("Inventory Slot (" + index + ")").gameObject;

            GameObject newIcon = Instantiate(icon, curBox.transform, false);

            index += 1;
        }

        foreach (GearData gear in chest.chestGear)
        {
            GameObject icon = gear.icon;
            icon.GetComponent<IconManager>().Gear = gear;
            GameObject curBox = game.cSBox.transform.Find("Inventory Slot (" + index + ")").gameObject;

            GameObject newIcon = Instantiate(icon, curBox.transform, false);

            GameObject quantity = Instantiate(game.quantityText, curBox.transform);
            quantity.transform.SetParent(newIcon.transform);
            quantity.transform.SetAsFirstSibling();
            quantity.GetComponent<TextMeshProUGUI>().text = gear.quantity.ToString();

            index += 1;
        }


        foreach (AmmoData ammo in chest.chestAmmo)
        {
            GameObject icon = ammo.icon;
            icon.GetComponent<IconManager>().Ammo = ammo;
            GameObject curBox = game.cSBox.transform.Find("Inventory Slot (" + index + ")").gameObject;

            GameObject newIcon = Instantiate(icon, curBox.transform, false);
            GameObject quantity = Instantiate(game.quantityText, curBox.transform);
            quantity.transform.SetParent(newIcon.transform);
            quantity.transform.SetAsFirstSibling();
            quantity.GetComponent<TextMeshProUGUI>().text = ammo.quantity.ToString();
        }
        game.StartCoroutine(SelectCS(4, 5, chest));
    }

    public static void DestroyCS()
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject curBox = game.cSBox.transform.Find("Inventory Slot (" + i + ")").gameObject;
            foreach (Transform child in curBox.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public static void SetCSSelectorPos(float x, float y)
    {
        game.csSelector.transform.parent = game.cSBox.transform.Find("Inventory Slot (" + ((y * 4) + x) + ")");
        game.csSelector.transform.SetAsLastSibling();
        game.csSelector.transform.localPosition = new Vector3(0, 0, game.csSelector.transform.localPosition.z);
    }

    public static IEnumerator SelectCS(int maxW, int maxH, ChestManager chest)
    {
        Vector2 moveVal = game.player.GetComponent<PlayerControl>().moveStick;
        Vector2 currentPos = new(0, 0);
        PlayerControl pc = game.player.GetComponent<PlayerControl>();
        GameObject currBox;

        game.csSelector.SetActive(true);
        SetCSSelectorPos(currentPos.x, currentPos.y);

        game.selectionContainer.SetActive(true);
        game.mainUIBackground.SetActive(true);
        game.characterBox.SetActive(true);

        int prevAltMove = pc.altMove;
        GameManager.Tabs prevTab = game.currTab;
        double prevMoveMag = 1;
        while (game.isInvCreated && !game.isEquipping && !game.isUnEquipping)
        {

            moveVal = pc.moveStick;
            bool select = pc.jump;
            bool unEquip = pc.shield;



            GameObject tabSelector = null;

            if (game.selectionContainer.transform.Find("Clothing Tab").transform.Find("Tab selector") != null)
            {
                tabSelector = game.selectionContainer.transform.Find("Clothing Tab").transform.Find("Tab selector").gameObject;
            }
            else if (game.selectionContainer.transform.Find("Weapon Tab").transform.Find("Tab selector") != null)
            {
                tabSelector = game.selectionContainer.transform.Find("Weapon Tab").transform.Find("Tab selector").gameObject;
            }
            else if (game.selectionContainer.transform.Find("Ammo Tab").transform.Find("Tab selector") != null)
            {
                tabSelector = game.selectionContainer.transform.Find("Ammo Tab").transform.Find("Tab selector").gameObject;
            }
            else if (game.selectionContainer.transform.Find("Spells Tab").transform.Find("Tab selector") != null)
            {
                tabSelector = game.selectionContainer.transform.Find("Spells Tab").transform.Find("Tab selector").gameObject;
            }
            else if (game.selectionContainer.transform.Find("Gear Tab").transform.Find("Tab selector") != null)
            {
                tabSelector = game.selectionContainer.transform.Find("Gear Tab").transform.Find("Tab selector").gameObject;
            }
            else if (game.selectionContainer.transform.Find("Abilities Tab").transform.Find("Tab selector") != null)
            {
                tabSelector = game.selectionContainer.transform.Find("Abilities Tab").transform.Find("Tab selector").gameObject;
            }
            else if (game.selectionContainer.transform.Find("Customize Tab").transform.Find("Tab selector") != null)
            {
                tabSelector = game.selectionContainer.transform.Find("Customize Tab").transform.Find("Tab selector").gameObject;
            }

            if (game.currTab == GameManager.Tabs.Weapons && tabSelector.transform.parent.name != "Weapon Tab")
            {
                tabSelector.transform.parent = game.selectionContainer.transform.Find("Weapon Tab");
                tabSelector.transform.localPosition = new Vector3(0, 0, tabSelector.transform.localPosition.z);
            }
            //allows for tab swapping

            if (prevAltMove != pc.altMove && ((pc.altMove > 0 && ((int)game.currTab) < 7) || (pc.altMove < 0 && ((int)game.currTab) > 0)))
            {
                game.currTab += pc.altMove;

                InventoryUIRunner.DestroyInventory();
                switch (game.currTab)
                {
                    case GameManager.Tabs.Weapons:
                        //weapon tab
                        tabSelector.transform.parent = game.selectionContainer.transform.Find("Weapon Tab");
                        tabSelector.transform.localPosition = new Vector3(0, 0, tabSelector.transform.localPosition.z);
                        InventoryUIRunner.CreateInventory(GameManager.Tabs.Weapons, false);
                        break;
                    case GameManager.Tabs.Ammo:
                        //ammo tab
                        tabSelector.transform.parent = game.selectionContainer.transform.Find("Ammo Tab");
                        tabSelector.transform.localPosition = new Vector3(0, 0, tabSelector.transform.localPosition.z);
                        InventoryUIRunner.CreateInventory(GameManager.Tabs.Ammo, false);
                        break;
                    case GameManager.Tabs.Gear:
                        // gear tab
                        tabSelector.transform.parent = game.selectionContainer.transform.Find("Gear Tab");
                        tabSelector.transform.localPosition = new Vector3(0, 0, tabSelector.transform.localPosition.z);
                        InventoryUIRunner.CreateInventory(GameManager.Tabs.Gear, false);
                        break;
                    case GameManager.Tabs.Spells:
                        tabSelector.transform.parent = game.selectionContainer.transform.Find("Spells Tab");
                        tabSelector.transform.localPosition = new Vector3(0, 0, tabSelector.transform.localPosition.z);
                        InventoryUIRunner.CreateInventory(GameManager.Tabs.Spells, false);
                        break;
                }
            }

            currBox = game.cSBox.transform.Find("Inventory Slot (" + ((currentPos.y * 4) + currentPos.x) + ")").gameObject;

            if (moveVal.x > 0.5 && currentPos.x < maxW - 1)
            {
                SetCSSelectorPos(currentPos.x + 1, currentPos.y);
                currentPos.x++;
            }
            else if (moveVal.x < -0.5 && currentPos.x > 0)
            {
                SetCSSelectorPos(currentPos.x - 1, currentPos.y);
                currentPos.x--;
            }

            if (moveVal.y > 0.5 && currentPos.y > 0)
            {
                SetCSSelectorPos(currentPos.x, currentPos.y - 1);
                currentPos.y--;
            }
            else if (moveVal.y < -0.5 && currentPos.y < maxH - 1)
            {
                SetCSSelectorPos(currentPos.x, currentPos.y + 1);
                currentPos.y++;
            }

            currBox = game.cSBox.transform.Find("Inventory Slot (" + ((currentPos.y * 4) + currentPos.x) + ")").gameObject;
            GameObject icon = null;
            if (currBox.transform.childCount > 0)
            {
                icon = currBox.transform.GetChild(0).gameObject;
            }
            if (currBox.transform.childCount > 1)
            {
                WeaponData currWeaponData = null;
                GearData currGearData = null;
                AmmoData currAmmoManager = null;
                if (icon.GetComponent<IconManager>().Weapon != null)
                {
                    currWeaponData = icon.GetComponent<IconManager>().Weapon;
                }
                else if (icon.GetComponent<IconManager>().Gear != null)
                {
                    currGearData = icon.GetComponent<IconManager>().Gear;
                }
                else if (icon.GetComponent<IconManager>().Ammo != null)
                {
                    currAmmoManager = icon.GetComponent<IconManager>().Ammo;
                }
                if (moveVal.magnitude < 0.05 && moveVal.magnitude != prevMoveMag && icon.GetComponent<IconManager>() != null)
                {

                    GameObject ds = game.inventoryBox.transform.Find("Description Section").gameObject;

                    if (icon.GetComponent<IconManager>().Weapon != null)
                    {
                        ds.transform.Find("Name text").gameObject.GetComponent<TextMeshProUGUI>().text = currWeaponData.weaponName;
                        ds.transform.Find("Description text").gameObject.GetComponent<TextMeshProUGUI>().text = currWeaponData.weaponDescription;

                        TextMeshProUGUI mainValtmp = ds.transform.Find("Main Value").transform.Find("Main Value text").gameObject.GetComponent<TextMeshProUGUI>();
                        if (currWeaponData.damageVal > 0)
                        {
                            ds.transform.Find("Main Value").transform.Find("Damage icon").gameObject.SetActive(true);
                            ds.transform.Find("Main Value").transform.Find("Shield icon").gameObject.SetActive(false);
                            ds.transform.Find("Main Value").transform.Find("Heal icon").gameObject.SetActive(false);
                            ds.transform.Find("Main Value").transform.Find("Mult icon").gameObject.SetActive(false);
                            mainValtmp.text = currWeaponData.damageVal.ToString();
                        }
                        else if (currWeaponData.shieldVal > 0)
                        {
                            ds.transform.Find("Main Value").transform.Find("Shield icon").gameObject.SetActive(true);
                            ds.transform.Find("Main Value").transform.Find("Damage icon").gameObject.SetActive(false);
                            ds.transform.Find("Main Value").transform.Find("Heal icon").gameObject.SetActive(false);
                            ds.transform.Find("Main Value").transform.Find("Mult icon").gameObject.SetActive(false);
                            mainValtmp.text = currWeaponData.shieldVal.ToString();
                        }

                        ds.transform.Find("Price").transform.Find("Price text").gameObject.GetComponent<TextMeshProUGUI>().text = currWeaponData.weaponCost.ToString();

                        switch (currWeaponData.damageEffects)
                        {
                            case "Burning":
                                ds.transform.Find("Effect").transform.Find("Burning Icon").gameObject.SetActive(true);
                                ds.transform.Find("Effect").transform.Find("Poisoned Icon").gameObject.SetActive(false);
                                break;
                            case "Poisoned":
                                ds.transform.Find("Effect").transform.Find("Poisoned Icon").gameObject.SetActive(true);
                                ds.transform.Find("Effect").transform.Find("Burning Icon").gameObject.SetActive(false);

                                break;
                            default:
                                ds.transform.Find("Effect").transform.Find("Burning Icon").gameObject.SetActive(false);
                                ds.transform.Find("Effect").transform.Find("Poisoned Icon").gameObject.SetActive(false);

                                break;
                        }
                    }
                    else if (icon.GetComponent<IconManager>().Gear != null)
                    {
                        ds.transform.Find("Name text").gameObject.GetComponent<TextMeshProUGUI>().text = currGearData.gearName;
                        ds.transform.Find("Description text").gameObject.GetComponent<TextMeshProUGUI>().text = currGearData.gearDescription;

                        TextMeshProUGUI mainValtmp = ds.transform.Find("Main Value").transform.Find("Main Value text").gameObject.GetComponent<TextMeshProUGUI>();
                        if (currGearData.damageVal > 0)
                        {
                            ds.transform.Find("Main Value").transform.Find("Damage icon").gameObject.SetActive(true);
                            ds.transform.Find("Main Value").transform.Find("Shield icon").gameObject.SetActive(false);
                            ds.transform.Find("Main Value").transform.Find("Heal icon").gameObject.SetActive(false);
                            ds.transform.Find("Main Value").transform.Find("Mult icon").gameObject.SetActive(false);
                            mainValtmp.text = currGearData.damageVal.ToString();
                        }
                        else if (currGearData.shieldVal > 0)
                        {
                            ds.transform.Find("Main Value").transform.Find("Shield icon").gameObject.SetActive(true);
                            ds.transform.Find("Main Value").transform.Find("Damage icon").gameObject.SetActive(false);
                            ds.transform.Find("Main Value").transform.Find("Heal icon").gameObject.SetActive(false);
                            ds.transform.Find("Main Value").transform.Find("Mult icon").gameObject.SetActive(false);
                            mainValtmp.text = currGearData.shieldVal.ToString();
                        }

                        ds.transform.Find("Price").transform.Find("Price text").gameObject.GetComponent<TextMeshProUGUI>().text = currGearData.gearCost.ToString();

                        switch (currGearData.damageEffects)
                        {
                            case "Burning":
                                ds.transform.Find("Effect").transform.Find("Burning Icon").gameObject.SetActive(true);
                                ds.transform.Find("Effect").transform.Find("Poisoned Icon").gameObject.SetActive(false);
                                break;
                            case "Poisoned":
                                ds.transform.Find("Effect").transform.Find("Poisoned Icon").gameObject.SetActive(true);
                                ds.transform.Find("Effect").transform.Find("Burning Icon").gameObject.SetActive(false);

                                break;
                            default:
                                ds.transform.Find("Effect").transform.Find("Burning Icon").gameObject.SetActive(false);
                                ds.transform.Find("Effect").transform.Find("Poisoned Icon").gameObject.SetActive(false);

                                break;
                        }
                    }
                    else if (icon.GetComponent<IconManager>().Ammo != null)
                    {
                        ds.transform.Find("Item Name").gameObject.GetComponent<TextMeshProUGUI>().text = currAmmoManager.ammoName;
                        ds.transform.Find("Description text").gameObject.GetComponent<TextMeshProUGUI>().text = currAmmoManager.ammoDescription;

                        TextMeshProUGUI mainValtmp = ds.transform.Find("Main Value").transform.Find("Main Value text").gameObject.GetComponent<TextMeshProUGUI>();

                        ds.transform.Find("Main Value").transform.Find("Mult icon").gameObject.SetActive(true);
                        ds.transform.Find("Main Value").transform.Find("Shield icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Damage icon").gameObject.SetActive(false);
                        ds.transform.Find("Main Value").transform.Find("Heal icon").gameObject.SetActive(false);
                        mainValtmp.text = currAmmoManager.damageMult.ToString();


                        ds.transform.Find("Price").transform.Find("Price text").gameObject.GetComponent<TextMeshProUGUI>().text = currAmmoManager.ammoCost.ToString();

                        switch (currAmmoManager.damageEffects)
                        {
                            case "Burning":
                                ds.transform.Find("Effect").transform.Find("Burning Icon").gameObject.SetActive(true);
                                ds.transform.Find("Effect").transform.Find("Poisoned Icon").gameObject.SetActive(false);
                                break;
                            case "Poisoned":
                                ds.transform.Find("Effect").transform.Find("Poisoned Icon").gameObject.SetActive(true);
                                ds.transform.Find("Effect").transform.Find("Burning Icon").gameObject.SetActive(false);

                                break;
                            default:
                                ds.transform.Find("Effect").transform.Find("Burning Icon").gameObject.SetActive(false);
                                ds.transform.Find("Effect").transform.Find("Poisoned Icon").gameObject.SetActive(false);
                                break;
                        }
                    }
                    if (ds.transform.Find("Icon Slot").childCount > 1)
                    {
                        Destroy(ds.transform.Find("Icon Slot").GetChild(1).gameObject);
                    }

                    GameObject bigIcon = Instantiate(icon, ds.transform.Find("Icon Slot"));
                    bigIcon.transform.localScale = bigIcon.transform.localScale * 500;
                    bigIcon.transform.localPosition = new Vector3(bigIcon.transform.localPosition.x * 500, bigIcon.transform.localPosition.y * 500, bigIcon.transform.localPosition.z);
                    bigIcon.transform.SetAsLastSibling();
                }
            }
            //allows for equipping
            if (select && currBox.transform.childCount > 1)
            {
                if (icon.GetComponent<IconManager>().Weapon != null)
                {
                    game.player.GetComponent<InventoryManager>().addWeapon(icon.GetComponent<IconManager>().Weapon);
                    if (chest != null)
                    {
                        chest.RemoveItem(icon.GetComponent<IconManager>().Weapon, null, null);
                    }
                }
                else if (icon.GetComponent<IconManager>().Gear != null)
                {
                    game.player.GetComponent<InventoryManager>().addGear(icon.GetComponent<IconManager>().Gear);
                    if (chest != null)
                    {
                        chest.RemoveItem(null, icon.GetComponent<IconManager>().Gear, null);
                    }
                }
                else if (icon.GetComponent<IconManager>().Ammo != null)
                {
                    game.player.GetComponent<InventoryManager>().addAmmo(icon.GetComponent<IconManager>().Ammo);
                    if (chest != null)
                    {
                        chest.RemoveItem(null, null, icon.GetComponent<IconManager>().Ammo);
                    }
                }


                Destroy(currBox.transform.GetChild(0).gameObject);
                InventoryUIRunner.DestroyInventory();
                InventoryUIRunner.CreateInventory(game.currTab, false);

            }
            prevAltMove = pc.altMove;
            prevTab = game.currTab;
            prevMoveMag = moveVal.magnitude;

            yield return new WaitForSecondsRealtime(0.1f);
        }

        yield return new WaitUntil(() => !game.isInvCreated || game.isEquipping || game.isUnEquipping);

    }
}