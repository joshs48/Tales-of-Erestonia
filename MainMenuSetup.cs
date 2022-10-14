using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSetup : MonoBehaviour
{
    [SerializeField] GameObject[] minis;
    [SerializeField] GameObject[] BGPlayers;
    [SerializeField] GameObject SelectionTabs;

    [SerializeField] GameObject playerChoice1;
    [SerializeField] GameObject playerChoice2;
    [SerializeField] GameObject playerChoice3;
    [SerializeField] GameObject playerChoice4;
    [SerializeField] GameObject playerSlot;

    [SerializeField] GameObject playerChoice1Decals;
    [SerializeField] GameObject playerChoice2Decals;
    [SerializeField] GameObject playerChoice3Decals;
    [SerializeField] GameObject playerChoice4Decals;

    [SerializeField] GameObject player;

    [SerializeField] Material[] classMats;
    [SerializeField] Material[] raceMats;
    [SerializeField] Material[] bodytypeMats;


    //in this order:
    /**
     * Bard,
     * Druid,
     * Warlock,
     * Wizard,
     * Fighter,
     * Ranger,
     * Rogue,
     * Monk,
     * Barbarian,
     * Cleric,
     * Paladin,
     * Tinkerer
     **/

    GameObject selector;
    GameObject spotLight;
    StatsManager playerStats;
    // Start is called before the first frame update
    void Start()
    {
        selector = SelectionTabs.transform.Find("New Game Tab").transform.Find("Selector").gameObject;
        spotLight = playerChoice1.transform.Find("Spot Light").gameObject;
        playerStats = player.GetComponent<StatsManager>();

        foreach (GameObject mini in minis)
        {
            Animator anim = mini.GetComponent<Animator>();
            switch (mini.name)
            {
                case "Bard":
                    anim.SetInteger("Mini", 1);
                    break;
                case "Artificer":
                    anim.SetInteger("Mini", 2);
                    break;
                case "Skeleton":
                    anim.SetInteger("Mini", 3);
                    break;
            }
        }

        foreach (GameObject player in BGPlayers) {
            StartCoroutine(RunPlayer(player));
        }
        StartCoroutine(SelectMainMenu());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator RunPlayer(GameObject player)
    {
        Animator anim = player.GetComponent<Animator>();
        yield return new WaitForSeconds((Random.value + Random.value) * 2f);
        anim.SetInteger("Action", 1);
        
    }

    IEnumerator SelectMainMenu()
    {
        Vector2 moveVal;
        bool select = GetComponent<PlayerControl>().jump;
        int position = 0;
        Vector3 selectorPos = selector.transform.localPosition;

        while (!select)
        {
            moveVal = GetComponent<PlayerControl>().moveStick;
            select = GetComponent<PlayerControl>().jump;
            if (moveVal.y < 0 && position < 2)
            {
                position += 1;
                selector.transform.parent = SelectionTabs.transform.GetChild(position);
                selector.transform.localPosition = selectorPos;
                yield return new WaitForSeconds(0.2f);

            } else if (moveVal.y > 0 && position > 0)
            {
                position -= 1;
                selector.transform.parent = SelectionTabs.transform.GetChild(position);
                selector.transform.localPosition = selectorPos;
                yield return new WaitForSeconds(0.2f);

            }
            yield return new WaitForSeconds(0.01f);
        }
        switch (position)
        {
            case 0:
                GameObject.Find("Choices cam").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 11;
                StartCoroutine(SetupNewChar(1));
                yield return new WaitForSeconds(1);//camera swap time
                GameObject.Find("Canvas").transform.Find("Main screen assets").gameObject.SetActive(false);
                playerChoice1Decals.SetActive(true);
                playerChoice2Decals.SetActive(true);
                playerChoice3Decals.SetActive(true);
                playerChoice4Decals.SetActive(true);
                GameObject.Find("Canvas").transform.Find("Bottom Text").gameObject.SetActive(true);
                GameObject.Find("Canvas").transform.Find("Bottom Text").GetComponent<TextMeshProUGUI>().text = "Choose Your Class";
                break;
            case 1:
                GameObject.Find("SaveAndLoadRunner").GetComponent<SaveAndLoadRunner>().load_data();

                player.transform.position = new Vector3(4.27f, 1.19f, 0);
                GameObject.Find("Choices cam").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 11;
                yield return new WaitForSeconds(1);
                Vector3 spotlightPos = spotLight.transform.localPosition;
                spotLight.transform.parent = playerSlot.transform;
                spotLight.transform.localPosition = spotlightPos;
                spotLight.SetActive(true);

                GameObject.Find("Canvas").transform.Find("Main screen assets").gameObject.SetActive(false);
                GameObject.Find("Canvas").transform.Find("Bottom Text").gameObject.SetActive(true);
                GameObject.Find("Canvas").transform.Find("Bottom Text").GetComponent<TextMeshProUGUI>().text = "Ready to Continue?";
                yield return new WaitUntil(() => GetComponent<PlayerControl>().jump);
                GameObject.Find("SaveAndLoadRunner").GetComponent<SaveAndLoadRunner>().BeginLoading("Goblin Cave");
                break;
            case 2:
                Debug.Log("should there even be settings?");
                break;
        
        }
    }

    IEnumerator SetupNewChar(int section)
    {
        //CHOOSE CLASS
        switch (section)
        {
            case 1:
                playerChoice1.transform.Find("Bard").gameObject.SetActive(true);
                playerChoice1.transform.Find("Bard").GetComponent<Animator>().SetInteger("ChoiceVal", 1);

                playerChoice2.transform.Find("Druid").gameObject.SetActive(true);
                playerChoice2.transform.Find("Druid").GetComponent<Animator>().SetInteger("ChoiceVal", 2);

                playerChoice3.transform.Find("Warlock").gameObject.SetActive(true);
                playerChoice3.transform.Find("Warlock").GetComponent<Animator>().SetInteger("ChoiceVal", 3);

                playerChoice4.transform.Find("Wizard").gameObject.SetActive(true);
                playerChoice4.transform.Find("Wizard").GetComponent<Animator>().SetInteger("ChoiceVal", 4);

                break;
            case 2:
                playerChoice1.transform.Find("Fighter").gameObject.SetActive(true);
                playerChoice1.transform.Find("Fighter").GetComponent<Animator>().SetInteger("ChoiceVal", 5);

                playerChoice2.transform.Find("Monk").gameObject.SetActive(true);
                playerChoice2.transform.Find("Monk").GetComponent<Animator>().SetInteger("ChoiceVal", 6);

                playerChoice3.transform.Find("Ranger").gameObject.SetActive(true);
                playerChoice3.transform.Find("Ranger").GetComponent<Animator>().SetInteger("ChoiceVal", 7);

                playerChoice4.transform.Find("Rogue").gameObject.SetActive(true);
                playerChoice4.transform.Find("Rogue").GetComponent<Animator>().SetInteger("ChoiceVal", 8);

                break;
            case 3:
                playerChoice1.transform.Find("Barbarian").gameObject.SetActive(true);
                playerChoice1.transform.Find("Barbarian").GetComponent<Animator>().SetInteger("ChoiceVal", 9);

                playerChoice2.transform.Find("Cleric").gameObject.SetActive(true);
                playerChoice2.transform.Find("Cleric").GetComponent<Animator>().SetInteger("ChoiceVal", 10);

                playerChoice3.transform.Find("Paladin").gameObject.SetActive(true);
                playerChoice3.transform.Find("Paladin").GetComponent<Animator>().SetInteger("ChoiceVal", 11);

                playerChoice4.transform.Find("Tinkerer").gameObject.SetActive(true);
                playerChoice4.transform.Find("Tinkerer").GetComponent<Animator>().SetInteger("ChoiceVal", 12);

                break;
        }

        for (int i = 1; i < 5; i++)
        {
            GameObject curDecals = GameObject.Find("Canvas").transform.Find("Slot " + i + " Decals").gameObject;
            int matNum = (section - 1) * 4 + i - 1;
            curDecals.transform.Find("Text").GetComponent<TextMeshProUGUI>().color = new Color(classMats[matNum].GetColor("_TintColor").r, classMats[matNum].GetColor("_TintColor").g, classMats[matNum].GetColor("_TintColor").b);
            curDecals.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = classMats[matNum].name.Substring(0, classMats[matNum].name.IndexOf(" "));

            curDecals.transform.Find("Lightbeam").transform.Find("SM_LightRay_Cube").GetComponent<SkinnedMeshRenderer>().materials[0].SetColor("_TintColor", classMats[matNum].GetColor("_TintColor"));

            yield return new WaitForSeconds(0.01f);
        }

        Vector2 moveVal;
        bool select = GetComponent<PlayerControl>().jump;
        int position = 1;
        Vector3 selectorPos = spotLight.transform.localPosition;
        spotLight.SetActive(true);
        while (!select)
        {
            moveVal = GetComponent<PlayerControl>().moveStick;
            select = GetComponent<PlayerControl>().jump;
            if (moveVal.x > 0 && position < 4)
            {
                position += 1;
                spotLight.transform.parent = GameObject.Find("Choice " + position).transform;
                spotLight.transform.localPosition = selectorPos;
                yield return new WaitForSeconds(0.2f);

            }
            else if (moveVal.x < 0 && position > 1)
            {
                position -= 1;
                spotLight.transform.parent = GameObject.Find("Choice " + position).transform;
                spotLight.transform.localPosition = selectorPos;
                yield return new WaitForSeconds(0.2f);

            }
            yield return new WaitForSeconds(0.01f);
        }

        switch (section)
        {
            case 1:
                switch (position)
                { 
                    case 1:
                        playerStats.Class = StatsManager.Classes.Bard;
                        break;
                    case 2:
                        playerStats.Class = StatsManager.Classes.Druid;
                        break;
                    case 3:
                        playerStats.Class = StatsManager.Classes.Warlock;
                        break;
                    case 4:
                        playerStats.Class = StatsManager.Classes.Wizard;
                        break;
                }
                
                playerChoice1.transform.Find("Bard").gameObject.SetActive(false);

                playerChoice2.transform.Find("Druid").gameObject.SetActive(false);

                playerChoice3.transform.Find("Warlock").gameObject.SetActive(false);

                playerChoice4.transform.Find("Wizard").gameObject.SetActive(false);
                break;
            case 2:
                switch (position)
                {
                    case 1:
                        playerStats.Class = StatsManager.Classes.Fighter;
                        break;
                    case 2:
                        playerStats.Class = StatsManager.Classes.Monk;
                        break;
                    case 3:
                        playerStats.Class = StatsManager.Classes.Ranger;
                        break;
                    case 4:
                        playerStats.Class = StatsManager.Classes.Rogue;
                    
                        break;
                }
                playerChoice1.transform.Find("Fighter").gameObject.SetActive(false);

                playerChoice2.transform.Find("Monk").gameObject.SetActive(false);

                playerChoice3.transform.Find("Ranger").gameObject.SetActive(false);

                playerChoice4.transform.Find("Rogue").gameObject.SetActive(false);
                break;
            case 3:
                switch (position)
                {
                    case 1:
                        playerStats.Class = StatsManager.Classes.Barbarian;
                        break;
                    case 2:
                        playerStats.Class = StatsManager.Classes.Cleric;
                        break;
                    case 3:
                        playerStats.Class = StatsManager.Classes.Paladin;
                        break;
                    case 4:
                        playerStats.Class = StatsManager.Classes.Tinkerer;
                        break;
                }
                playerChoice1.transform.Find("Barbarian").gameObject.SetActive(false);

                playerChoice2.transform.Find("Cleric").gameObject.SetActive(false);

                playerChoice3.transform.Find("Paladin").gameObject.SetActive(false);

                playerChoice4.transform.Find("Tinkerer").gameObject.SetActive(false);
                break;
        }        
        yield return new WaitUntil(() => !GetComponent<PlayerControl>().jump);





        //CHOOSE RACE
        GameObject.Find("Canvas").transform.Find("Bottom Text").GetComponent<TextMeshProUGUI>().text = "Choose Your Race";

        playerChoice1.transform.Find("Tiefling").gameObject.SetActive(true);
        playerChoice1.transform.Find("Tiefling").GetComponent<Animator>().SetInteger("ChoiceVal", 13);

        playerChoice2.transform.Find("Elf").gameObject.SetActive(true);
        playerChoice2.transform.Find("Elf").GetComponent<Animator>().SetInteger("ChoiceVal", 14);

        playerChoice3.transform.Find("Human").gameObject.SetActive(true);
        playerChoice3.transform.Find("Human").GetComponent<Animator>().SetInteger("ChoiceVal", 15);

        playerChoice4.transform.Find("Bremri").gameObject.SetActive(true);
        playerChoice4.transform.Find("Bremri").GetComponent<Animator>().SetInteger("ChoiceVal", 16);

        for (int i = 1; i < 5; i++)
        {
            GameObject curDecals = GameObject.Find("Canvas").transform.Find("Slot " + i + " Decals").gameObject;
            int matNum = i-1;
            curDecals.transform.Find("Text").GetComponent<TextMeshProUGUI>().color = new Color(raceMats[matNum].GetColor("_TintColor").r, raceMats[matNum].GetColor("_TintColor").g, raceMats[matNum].GetColor("_TintColor").b);
            curDecals.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = raceMats[matNum].name.Substring(0, raceMats[matNum].name.IndexOf(" "));

            curDecals.transform.Find("Lightbeam").transform.Find("SM_LightRay_Cube").GetComponent<SkinnedMeshRenderer>().materials[0].SetColor("_TintColor", raceMats[matNum].GetColor("_TintColor"));

            yield return new WaitForSeconds(0.01f);
        }

        select = GetComponent<PlayerControl>().jump;
        position = 1;
        selectorPos = spotLight.transform.localPosition;
        spotLight.transform.parent = GameObject.Find("Choice 1").transform;
        spotLight.transform.localPosition = selectorPos;
        spotLight.SetActive(true);
        while (!select)
        {
            moveVal = GetComponent<PlayerControl>().moveStick;
            select = GetComponent<PlayerControl>().jump;
            if (moveVal.x > 0 && position < 4)
            {
                position += 1;
                spotLight.transform.parent = GameObject.Find("Choice " + position).transform;
                spotLight.transform.localPosition = selectorPos;
                yield return new WaitForSeconds(0.2f);

            }
            else if (moveVal.x < 0 && position > 1)
            {
                position -= 1;
                spotLight.transform.parent = GameObject.Find("Choice " + position).transform;
                spotLight.transform.localPosition = selectorPos;
                yield return new WaitForSeconds(0.2f);

            }
            yield return new WaitForSeconds(0.01f);
        }

        switch (position)
        {
            case 1:
                playerStats.Race = StatsManager.Races.Tiefling;
                break;
            case 2:
                playerStats.Race = StatsManager.Races.Elf;
                break;
            case 3:
                playerStats.Race = StatsManager.Races.Human;
                break;
            case 4:
                playerStats.Race = StatsManager.Races.Bremri;
                break;
        }

        playerChoice1.transform.Find("Tiefling").gameObject.SetActive(false);
        playerChoice2.transform.Find("Elf").gameObject.SetActive(false);
        playerChoice3.transform.Find("Human").gameObject.SetActive(false);
        playerChoice4.transform.Find("Bremri").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("Slot 1 Decals").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("Slot 4 Decals").gameObject.SetActive(false);
        yield return new WaitUntil(() => !GetComponent<PlayerControl>().jump);




        //Set body type
        GameObject.Find("Canvas").transform.Find("Bottom Text").GetComponent<TextMeshProUGUI>().text = "Choose Your Body Type";
        playerChoice2.transform.Find("Male").gameObject.SetActive(true);
        playerChoice2.transform.Find("Male").GetComponent<Animator>().SetInteger("ChoiceVal", 17);
        playerChoice3.transform.Find("Female").gameObject.SetActive(true);
        playerChoice3.transform.Find("Female").GetComponent<Animator>().SetInteger("ChoiceVal", 17);

        for (int i = 2; i < 4; i++)
        {
            GameObject curDecals = GameObject.Find("Canvas").transform.Find("Slot " + i + " Decals").gameObject;
            int matNum = i - 2;
            curDecals.transform.Find("Text").GetComponent<TextMeshProUGUI>().color = new Color(bodytypeMats[matNum].GetColor("_TintColor").r, bodytypeMats[matNum].GetColor("_TintColor").g, bodytypeMats[matNum].GetColor("_TintColor").b);
            curDecals.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = bodytypeMats[matNum].name.Substring(0, bodytypeMats[matNum].name.IndexOf(" "));

            curDecals.transform.Find("Lightbeam").transform.Find("SM_LightRay_Cube").GetComponent<SkinnedMeshRenderer>().materials[0].SetColor("_TintColor", bodytypeMats[matNum].GetColor("_TintColor"));

            yield return new WaitForSeconds(0.01f);
        }

        select = GetComponent<PlayerControl>().jump;
        position = 2;
        selectorPos = spotLight.transform.localPosition;
        spotLight.transform.parent = GameObject.Find("Choice 2").transform;
        spotLight.transform.localPosition = selectorPos;
        spotLight.SetActive(true);
        while (!select)
        {
            moveVal = GetComponent<PlayerControl>().moveStick;
            select = GetComponent<PlayerControl>().jump;
            if (moveVal.x > 0 && position < 3)
            {
                position += 1;
                spotLight.transform.parent = GameObject.Find("Choice " + position).transform;
                spotLight.transform.localPosition = selectorPos;
                yield return new WaitForSeconds(0.2f);

            }
            else if (moveVal.x < 0 && position > 2)
            {
                position -= 1;
                spotLight.transform.parent = GameObject.Find("Choice " + position).transform;
                spotLight.transform.localPosition = selectorPos;
                yield return new WaitForSeconds(0.2f);

            }
            yield return new WaitForSeconds(0.01f);
        }

        switch (position)
        {
            case 2:
                playerStats.bodyType = StatsManager.BodyTypes.Male;
                break;
            case 3:
                playerStats.bodyType = StatsManager.BodyTypes.Female;
                break;
        }
        yield return new WaitUntil(() => !GetComponent<PlayerControl>().jump);

        GameObject.Find("Canvas").transform.Find("Bottom Text").GetComponent<TextMeshProUGUI>().text = "Ready to Begin?";
        playerChoice2.transform.Find("Male").gameObject.SetActive(false);
        playerChoice3.transform.Find("Female").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("Slot 2 Decals").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("Slot 3 Decals").gameObject.SetActive(false);
        Vector3 spotlightPos = spotLight.transform.localPosition;
        spotLight.transform.parent = playerSlot.transform;
        spotLight.transform.localPosition = spotlightPos;
        spotLight.SetActive(true);

        playerStats.SetUpStatsAndInv();
        player.transform.position = new Vector3(4.27f, 1.19f, 0);
        yield return new WaitUntil(() => GetComponent<PlayerControl>().jump);
        
        GameObject.Find("SaveAndLoadRunner").GetComponent<SaveAndLoadRunner>().save_game();

        GameObject.Find("SaveAndLoadRunner").GetComponent<SaveAndLoadRunner>().BeginLoading("Goblin Cave");






    }

    
}
