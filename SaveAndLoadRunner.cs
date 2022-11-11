using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveAndLoadRunner : MonoBehaviour
{
    string file_path;
    [SerializeField] GameObject player;
    [SerializeField] Material playerMat;
    [SerializeField] List<GameObject> loadImages = new List<GameObject>();
    [SerializeField] List<Vector3> startPos = new List<Vector3>();

    Scene scene;
    GameObject currLoadImage;
    private Vector3 loadImagePos = new Vector3(-476, 0, -100);
    private Vector3 loadWeaponScale = new Vector3(300, 300, 300);
    private Vector3 loadWeaponRot = new Vector3(45, 90, 0);

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        file_path = Application.persistentDataPath + "/gamedata.mine";
        DontDestroyOnLoad(player);
    }

    public void save_game()
    {
        player = GameObject.Find("Player");

        StatsManager stats = player.GetComponent<StatsManager>();
        InventoryManager inv = player.GetComponent<InventoryManager>();

        //SaveData.data.storyLevel
        SaveData.data.playerMatColors.RemoveRange(0, SaveData.data.playerMatColors.Count);

        SaveData.data.playerMatColors.Add(playerMat.GetColor("_Color_Hair"));
        SaveData.data.playerMatColors.Add(playerMat.GetColor("_Color_Skin"));
        SaveData.data.playerMatColors.Add(playerMat.GetColor("_Color_Stubble"));
        SaveData.data.playerMatColors.Add(playerMat.GetColor("_Color_Scar"));
        SaveData.data.playerMatColors.Add(playerMat.GetColor("_Color_Eyes"));
        SaveData.data.playerMatColors.Add(playerMat.GetColor("_Color_BodyArt"));
        SaveData.data.playerMatColors.Add(playerMat.GetColor("_Color_Primary"));
        SaveData.data.playerMatColors.Add(playerMat.GetColor("_Color_Secondary"));

        SaveData.data.playerMaxHealth = stats.maxHealth;
        SaveData.data.playerXP = stats.XP;
        SaveData.data.playerLevel = stats.Level;
        SaveData.data.playerClass = stats.Class;
        SaveData.data.playerRace = stats.Race;
        SaveData.data.playerBodyType = stats.bodyType;
        SaveData.data.STR = stats.STR;
        SaveData.data.DEX = stats.DEX;
        SaveData.data.CON = stats.CON;
        SaveData.data.INT = stats.INT;
        SaveData.data.WIS = stats.WIS;
        SaveData.data.CHA = stats.CHA;

        SaveData.data.weaponInv = inv.weaponInv;
        SaveData.data.ammoInv = inv.ammoInv;
        SaveData.data.gearInv = inv.gearInv;
        SaveData.data.spellInv = inv.spellInv;
        SaveData.data.clothingInv = inv.clothingInv;
        SaveData.data.gearQs = inv.gearQs;
        SaveData.data.ammoQs = inv.ammoQs;

        SaveData.data.activeWeaponR = inv.activeWeaponR;
        SaveData.data.activeWeaponL = inv.activeWeaponL;

        SaveData.data.activeAmmo = inv.activeAmmo;
        SaveData.data.activeAmmoQs = inv.activeAmmoQs;

        SaveData.data.gearActive = inv.gearActive;
        SaveData.data.gearActiveQs = inv.gearActiveQs;

        SaveData.data.spellsActive = inv.spellsActive;
        SaveData.data.spellQuantitiesBylvl = inv.spellQuantitiesBylvl;

        SaveData.data.activeHead = inv.activeHead;
        SaveData.data.activeTorso = inv.activeTorso;
        SaveData.data.activeHands = inv.activeHands;
        SaveData.data.activeLegs = inv.activeLegs;
        SaveData.data.activeBoots = inv.activeBoots;
        SaveData.data.activeFace = inv.activeFace;
        SaveData.data.activeHair = inv.activeHair;
        SaveData.data.activeFacialHair = inv.activeFacialHair;


        string json_data = JsonUtility.ToJson(SaveData.data);
        File.WriteAllText(file_path, json_data);

    }

    public SaveData load_data()
    {

        if (File.Exists(file_path))
        {
            player = GameObject.Find("Player");

            string loaded_data = File.ReadAllText(file_path);
            SaveData lod = JsonUtility.FromJson<SaveData>(loaded_data);

            StatsManager stats = player.GetComponent<StatsManager>();
            InventoryManager inv = player.GetComponent<InventoryManager>();

            //SaveData.data.storyLevel
            playerMat.SetColor("_Color_Hair", lod.playerMatColors[0]);
            playerMat.SetColor("_Color_Skin", lod.playerMatColors[1]);
            playerMat.SetColor("_Color_Stubble", lod.playerMatColors[2]);
            playerMat.SetColor("_Color_Scar", lod.playerMatColors[3]);
            playerMat.SetColor("_Color_Eyes", lod.playerMatColors[4]);
            playerMat.SetColor("_Color_BodyArt", lod.playerMatColors[5]);
            playerMat.SetColor("_Color_Primary", lod.playerMatColors[6]);
            playerMat.SetColor("_Color_Secondary", lod.playerMatColors[7]);


            stats.maxHealth = lod.playerMaxHealth;
            stats.XP = lod.playerXP;
            stats.Level = lod.playerLevel;
            stats.Class = lod.playerClass;
            stats.Race = lod.playerRace;
            stats.bodyType = lod.playerBodyType;
            stats.STR = lod.STR;
            stats.DEX = lod.DEX;
            stats.CON = lod.CON;
            stats.INT = lod.INT;
            stats.WIS = lod.WIS;
            stats.CHA = lod.CHA;

            inv.weaponInv = lod.weaponInv;
            inv.ammoInv = lod.ammoInv;
            inv.gearInv = lod.gearInv;
            inv.spellInv = lod.spellInv;
            inv.clothingInv = lod.clothingInv;
            inv.gearQs = lod.gearQs;
            inv.ammoQs = lod.ammoQs;

            if (lod.activeWeaponR != null)
            {
                inv.SetWeaponActive(lod.activeWeaponR.GetComponent<Weapon>().data, "right");
            }
            if (lod.activeWeaponL != null)
            {
                inv.SetWeaponActive(lod.activeWeaponL.GetComponent<Weapon>().data, "left");
            }
            if (lod.activeAmmo != null)
            {
                inv.SetAmmoActive(lod.activeAmmo);
            }
            inv.activeAmmoQs = lod.activeAmmoQs;

            inv.gearActive = lod.gearActive;
            inv.gearActiveQs = lod.gearActiveQs;

            inv.spellsActive = lod.spellsActive;
            inv.spellQuantitiesBylvl = lod.spellQuantitiesBylvl;

            if (lod.activeHead != null)
            {
                inv.SetClothingActive(lod.activeHead.GetComponent<Clothing>().data);
            }
            if (lod.activeTorso != null)
            {
                inv.SetClothingActive(lod.activeTorso.GetComponent<Clothing>().data);
            }
            if (lod.activeHands != null)
            {
                inv.SetClothingActive(lod.activeHands.GetComponent<Clothing>().data);
            }
            if (lod.activeLegs != null)
            {
                inv.SetClothingActive(lod.activeLegs.GetComponent<Clothing>().data);
            }
            if (lod.activeBoots)
            {
                inv.SetClothingActive(lod.activeBoots.GetComponent<Clothing>().data);
            }
            if (lod.activeFace != null)
            {
                inv.SetClothingActiveLite(lod.activeFace.name, ClothingData.ClothingSlot.Face);
            }
            if (lod.activeHair != null)
            {
                inv.SetClothingActiveLite(lod.activeHair.name, ClothingData.ClothingSlot.Hair);
            }
            if (lod.activeFacialHair != null)
            {
                inv.SetClothingActiveLite(lod.activeFacialHair.name, ClothingData.ClothingSlot.Facial_Hair);
            }

            switch (lod.playerRace)
            {
                case StatsManager.Races.Tiefling:
                    player.transform.Find("Modular Characters").Find("Head Attachments").Find("Long Bent Ear").gameObject.SetActive(true);
                    player.transform.Find("Modular Characters").Find("Head Attachments").Find("Horns").gameObject.SetActive(true);

                    break;
                case StatsManager.Races.Elf:
                    player.transform.Find("Modular Characters").Find("Head Attachments").Find("Long Straight Ear").gameObject.SetActive(true);

                    break;
                case StatsManager.Races.Bremri:
                    player.transform.Find("Modular Characters").Find("Head Attachments").Find("Long Bent Ear").gameObject.SetActive(true);
                    break;
            }

            return lod;
        }
        else
        {
            Debug.Log("File doesn't exist");
            return null;
        }
    }
    public void BeginLoading(string sceneName)
    {
        CreateLoadImage();
        GameObject.Find("Canvas").transform.Find("General background").gameObject.SetActive(true);
        StartCoroutine(LoadScene(sceneName));
    }

    IEnumerator LoadScene(string sceneName)
    {


        yield return null;

        if (SceneManager.GetSceneByName(sceneName) != null)
        {
            //Begin to load the Scene you specify
            scene = SceneManager.GetSceneByName(sceneName);



            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            asyncOperation.allowSceneActivation = true;

            while (!asyncOperation.isDone)
            {
                currLoadImage.transform.Rotate(Vector3.up, 1);

                yield return new WaitForEndOfFrame();
            }
            player.GetComponent<InventoryManager>().isActualPlayer = true;

            switch (sceneName)
            {
                case "Level 1":
                    player.transform.position = startPos[1];
                    break;
                case "Islebury":
                    player.transform.position = startPos[0];
                    break;
                case "Goblin Cave":
                    player.transform.position = startPos[2];
                    player.transform.eulerAngles = new Vector3(0, 180, 0);
                    break;
            }
            player.GetComponent<PlayerControl>().enabled = true;
            player.GetComponent<BaseCharacter>().enabled = true;
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            player.GetComponent<InventoryManager>().UpdateActiveUI();
            StopAllCoroutines();

        }
        else
        {
            Debug.Log("scene name not recognized");
        }
    }

    void CreateLoadImage()
    {
        if (currLoadImage != null)
        {
            Destroy(currLoadImage);
        }

        currLoadImage = Instantiate(loadImages[Random.Range(0, loadImages.Count)], GameObject.Find("Canvas").transform);
        currLoadImage.transform.localPosition = loadImagePos;
        if (currLoadImage.layer != 6)
        {

            currLoadImage.transform.localScale = loadWeaponScale;
        }
        StartCoroutine(Delay(10));
    }

    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
        CreateLoadImage();
    }


}

public class SaveData
{
    public static SaveData data = new SaveData();

    public float storyLevel;

    public int playerMaxHealth;

    public int playerXP;
    public int playerLevel;

    public StatsManager.Classes playerClass;
    public StatsManager.Races playerRace;
    public StatsManager.BodyTypes playerBodyType;

    public List<Color> playerMatColors = new List<Color>(8);

    public int STR;
    public int DEX;
    public int CON;
    public int INT;
    public int WIS;
    public int CHA;

    public List<WeaponData> weaponInv = new List<WeaponData>();
    public List<AmmoData> ammoInv = new List<AmmoData>();
    public List<GearData> gearInv = new List<GearData>();
    public List<SpellData> spellInv = new List<SpellData>();
    public List<ClothingData> clothingInv = new List<ClothingData>();


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


    public GameObject activeHead;
    public GameObject activeTorso;
    public GameObject activeHands;
    public GameObject activeLegs;
    public GameObject activeBoots;

    public GameObject activeFace;
    public GameObject activeHair;
    public GameObject activeFacialHair;
}

