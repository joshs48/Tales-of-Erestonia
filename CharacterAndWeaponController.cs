using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAndWeaponController : MonoBehaviour
{
    
    GameObject playerObject;

    [HideInInspector] public string weaponR; 
    public GameObject weaponRObject;
    [HideInInspector] [SerializeField] string weaponRType;

    [HideInInspector] public string weaponL;
    public GameObject weaponLObject;
    [HideInInspector] [SerializeField] string weaponLType;

    public bool preciseAiming = false;

    

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        //setWeapons(weaponR, weaponL);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (weaponRType == "Axe" && weaponL != "")
        {
            anim.SetInteger("BasicAtks", 3);
        } else if (weaponRType == "" && weaponLType == "Bow")
        {
            anim.SetInteger("BasicAtks", 1);
        } else if (weaponRType == "Sword" && weaponLType == "")
        {
            anim.SetInteger("BasicAtks", 3);
        } else if (weaponRType == "Sword" && weaponLType == "Sword")
        {
            anim.SetInteger("BasicAtks", 3);
        } else if (weaponRType == "Sword" && weaponLType == "Shield")
        {
            anim.SetInteger("BasicAtks", 3);
        } else if (weaponRType == "Staff" && weaponLType == "")
        {
            anim.SetInteger("BasicAtks", 2);
        } else if (weaponRType == "Crossbow" && weaponLType == "")
        {
            anim.SetInteger("BasicAtks", 1);
        } else if (weaponRType == "" && weaponLType == "")
        {
            anim.SetInteger("BasicAtks", 3);
        }

        switch (weaponRType)
        {
            case "Axe":
                anim.SetInteger("WeaponR", 1);
                break;
            case "Club":
                anim.SetInteger("WeaponR", 2);
                break;
            case "Bow":
                anim.SetInteger("WeaponR", 3);
                break;
            case "Hammer":
                anim.SetInteger("WeaponR", 5);
                break;
            case "Staff":
                anim.SetInteger("WeaponR", 6);
                break;
            case "Sword":
                anim.SetInteger("WeaponR", 7);
                break;
            case "Crossbow":
                anim.SetInteger("WeaponR", 8);
                break;
        }
        switch (weaponLType)
        {
            case "Axe":
                anim.SetInteger("WeaponL", 1);
                break;
            case "Bone":
            case "Club":
                anim.SetInteger("WeaponL", 2);
                break;
            case "Bow":
                anim.SetInteger("WeaponL", 3);
                break;
            case "Staff":
                anim.SetInteger("WeaponL", 6);
                break;
            case "Sword":
                anim.SetInteger("WeaponL", 7);
                break;
            case "Crossbow":
                anim.SetInteger("WeaponL", 8);
                break;
            case "Shield":
                anim.SetInteger("WeaponL", 9);
                break;
        }

        if (GetComponent<StatsManager>().Class == StatsManager.Classes.Bard && weaponR == "" && weaponL == "" && anim.GetFloat("Forward") < 0.05 && anim.GetFloat("Turn") < 0.05)
        {
            anim.SetInteger("WeaponR", 10);
            transform.Find("Root").Find("Hips").Find("Spine_01").Find("Spine_02").Find("Spine_03").Find("Clavicle_L").Find("Shoulder_L").Find("Elbow_L").Find("Lute").gameObject.SetActive(true);
        }
    }

    
    public void setCharacter(string newCharacter)
    {
        
         
        
        
        
        if (weaponLType.Equals("Bow"))
        {
            TwoBoneIKConstraintData leftAimingRig = transform.Find("Rig 1").transform.Find("Left Aim").gameObject.GetComponent<TwoBoneIKConstraint>().data;
            MultiAimConstraintData rightAimingRig = transform.Find("Rig 1").transform.Find("Right Aim").gameObject.GetComponent<MultiAimConstraint>().data;

            leftAimingRig.tip = transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_L").transform.Find("Shoulder_L").transform.Find("Elbow_L").transform.Find("Hand_L").transform.Find(weaponL);
            leftAimingRig.mid = transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_L").transform.Find("Shoulder_L").transform.Find("Elbow_L");
            leftAimingRig.root = transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_L").transform.Find("Shoulder_L");
            rightAimingRig.constrainedObject = transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_R").transform.Find("Shoulder_R").transform.Find("Elbow_R").transform.Find("Hand_R").transform.Find("Arrow");

            transform.Find("Rig 1").transform.Find("Left Aim").gameObject.GetComponent<TwoBoneIKConstraint>().weight = 0;

            transform.Find("Rig 1").transform.Find("Right Aim").gameObject.GetComponent<MultiAimConstraint>().weight = 0;
        }
        if (weaponRType.Equals("Crossbow"))
        {
            TwoBoneIKConstraintData rightAimingRig = transform.Find("Rig 1").transform.Find("Right Aim").gameObject.GetComponent<TwoBoneIKConstraint>().data;
            rightAimingRig.tip = transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_R").transform.Find("Shoulder_R").transform.Find("Elbow_R").transform.Find("Hand_R");
            rightAimingRig.mid = transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_R").transform.Find("Shoulder_R").transform.Find("Elbow_R");
            rightAimingRig.root = transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_R").transform.Find("Shoulder_R");

            transform.Find("Rig 1").transform.Find("Right Aim").gameObject.GetComponent<TwoBoneIKConstraint>().weight = 0;

        }


        
    }
    public void setWeapons(GameObject newRWeapon, GameObject newLWeapon)
    {

        if (newRWeapon != null)
        {
            
            weaponRType = newRWeapon.GetComponent<ItemManager>().itemType.ToString();
            weaponRObject = Instantiate(newRWeapon, transform);
            weaponRObject.SetActive(true);
            weaponRObject.transform.parent = transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_R").transform.Find("Shoulder_R").transform.Find("Elbow_R").transform.Find("Hand_R");
            weaponRObject.transform.localPosition = transform.Find(weaponRType + " Mount").localPosition;
            weaponRObject.transform.localRotation = transform.Find(weaponRType + " Mount").localRotation;

            if (newRWeapon.name == "Sword Straight")
            {
                weaponRObject.transform.eulerAngles = new Vector3(weaponRObject.transform.localRotation.x, weaponRObject.transform.localRotation.y + 90, weaponRObject.transform.localRotation.z + 90);
            }

            switch (weaponRType)
            {
                case "Axe":
                    anim.SetInteger("WeaponR", 1);
                    break;
                case "Club":
                    anim.SetInteger("WeaponR", 2);
                    break;
                case "Bow":
                    anim.SetInteger("WeaponR", 3);
                    break;
                case "Hammer":
                    anim.SetInteger("WeaponR", 5);
                    break;
                case "Staff":
                    anim.SetInteger("WeaponR", 6);
                    break;
                case "Sword":
                    anim.SetInteger("WeaponR", 7);
                    break;
                case "Crossbow":
                    anim.SetInteger("WeaponR", 8);
                    break;
            }
            

        }


        if (newLWeapon != null)
        {
            
            weaponLType = newLWeapon.GetComponent<ItemManager>().itemType.ToString();
            weaponLObject = Instantiate(newLWeapon, transform);
            weaponLObject.SetActive(true);

            float xVal = weaponLObject.transform.localPosition.x;
            Vector3 handPos = new Vector3(-weaponLObject.transform.localPosition.x, weaponLObject.transform.localPosition.y, weaponLObject.transform.localPosition.z);


            weaponLObject.transform.parent = transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_L").transform.Find("Shoulder_L").transform.Find("Elbow_L").transform.Find("Hand_L");

            weaponLObject.transform.localPosition = transform.Find(weaponLType + " Mount").localPosition;
            weaponLObject.transform.localPosition = new Vector3(-weaponLObject.transform.localPosition.x, -weaponLObject.transform.localPosition.y, weaponLObject.transform.localPosition.z);
            weaponLObject.transform.localRotation = transform.Find(weaponLType + " Mount").localRotation;

            if (weaponLType != "Shield")
            {
                weaponLObject.transform.Rotate(Vector3.right * 180);
            }

            if (newLWeapon.name == "Sword Straight")
            {
                weaponLObject.transform.eulerAngles = new Vector3(weaponLObject.transform.localRotation.x, weaponLObject.transform.localRotation.y + 90, weaponLObject.transform.localRotation.z + 90);
            }

            switch (weaponLType)
            {
                case "Axe":
                    anim.SetInteger("WeaponL", 1);
                    break;
                case "Bone":
                case "Club":
                    anim.SetInteger("WeaponL", 2);
                    break;
                case "Bow":
                    anim.SetInteger("WeaponL", 3);
                    break;
                case "Staff":
                    anim.SetInteger("WeaponL", 6);
                    break;
                case "Sword":
                    anim.SetInteger("WeaponL", 7);
                    break;
                case "Crossbow":
                    anim.SetInteger("WeaponL", 8);
                    break;
                case "Shield":
                    anim.SetInteger("WeaponL", 9);
                    break;
                

            }
           

        }
        if (newRWeapon == null && newLWeapon == null)
        {
            
            if (weaponRObject)
            {
                
                anim.SetInteger("WeaponR", 0);
                weaponRType = "";
            }
            if (weaponLObject)
            {
                anim.SetInteger("WeaponL", 0);
                weaponLType = "";

            }
            


            transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_L").transform.Find("Shoulder_L").transform.Find("Elbow_L").transform.Find("Hand_L").gameObject.GetComponent<SphereCollider>().enabled = true;
            transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_R").transform.Find("Shoulder_R").transform.Find("Elbow_R").transform.Find("Hand_R").gameObject.GetComponent<SphereCollider>().enabled = true;

        } else
        {

            transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_L").transform.Find("Shoulder_L").transform.Find("Elbow_L").transform.Find("Hand_L").gameObject.GetComponent<SphereCollider>().enabled = false;
            transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_R").transform.Find("Shoulder_R").transform.Find("Elbow_R").transform.Find("Hand_R").gameObject.GetComponent<SphereCollider>().enabled = false;

        }

        
    }
}
