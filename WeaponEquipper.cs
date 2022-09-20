using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEquipper : MonoBehaviour
{
    
    [SerializeField] string WeaponR_name;
    [SerializeField] GameObject WeaponR_gameObject;
    [SerializeField] string WeaponL_name;
    [SerializeField] GameObject WeaponL_gameObject;
    
    string weaponType;
    
    string weaponName;


    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        SetWeapon("Swords Basic", "R");
        //SetWeapon("Bows Basic", "L");

        
        


        
    }

    // Update is called once per frame
    void Update()
    {
        

    }
    


    public void SetWeapon(string weapon, string hand)
    {
        GameObject weaponObject;
        GameObject handObject;

        if (hand.Equals("R"))
        {
            handObject = transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_R").transform.Find("Shoulder_R").transform.Find("Elbow_R").transform.Find("Hand_R").gameObject;

        } else
        {
            handObject = transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_L").transform.Find("Shoulder_L").transform.Find("Elbow_L").transform.Find("Hand_L").gameObject;

        }

        
        

        

        weaponType = weapon.Substring(0, weapon.IndexOf(" "));
        weaponName = weapon.Substring(weapon.IndexOf(" ") + 1);
        
        weaponObject = handObject.transform.Find(weaponType + " ").transform.Find(weaponName).gameObject;

       
        if (hand.Equals("R"))
        {
            if (WeaponR_gameObject != null)
            {
                WeaponR_gameObject.SetActive(false);

            }

            weaponObject.SetActive(true);
            WeaponR_name = weapon;
            WeaponR_gameObject = weaponObject;

            switch (weaponType)
            {
                case "Axes":
                    anim.SetInteger("WeaponR", 1);
                    break;
                case "Bows":
                    anim.SetInteger("WeaponR", 2);
                    break;
                case "Shields":
                    anim.SetInteger("WeaponR", 3);
                    break;
                case "Swords":
                    
                    anim.SetInteger("WeaponR", 4);
                    break;

            }
        }

        if (hand.Equals("L"))
        {

            if (WeaponL_gameObject != null)
            {
                WeaponL_gameObject.SetActive(false);
            }

            weaponObject.SetActive(true);
            WeaponL_name = weapon;
            WeaponL_gameObject = weaponObject;

            switch (weaponType)
            {
                case "Axes":
                    anim.SetInteger("WeaponL", 1);
                    break;
                case "Bows":
                    anim.SetInteger("WeaponL", 2);
                    break;
                case "Shields":
                    anim.SetInteger("WeaponL", 3);
                    break;
                case "Swords":
                    anim.SetInteger("WeaponL", 4);
                    break;
            }

        }
        

    }

    public string GetWeapon(string hand)
    {
        if (hand.Equals("R"))
        {
            return WeaponR_name;
        } else
        {
            return WeaponL_name;
        }
    }
    
}
