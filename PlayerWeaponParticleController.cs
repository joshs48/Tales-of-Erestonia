using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;


public class PlayerWeaponParticleController : StateMachineBehaviour
{


    private int weaponR;
    private int weaponL;
    private bool isMagic;
    private int spell;
    private int ability;
    public GameObject arrow;
    public GameObject bolt;
    public GameObject player;
    public float prevPlayerXRotation;
    private bool hasFired = false;

    [SerializeField] GameObject createParticle1;
    [SerializeField] float ShootDelay;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        weaponR = animator.GetInteger("WeaponR");
        weaponL = animator.GetInteger("WeaponL");
        isMagic = animator.GetBool("IsMagician");
        spell = animator.GetInteger("Spell");
        ability = animator.GetInteger("Ability");

        if (weaponL == 3)
        {

            player = animator.gameObject;
            player.GetComponent<InventoryManager>().activeAmmoQs -= 1;
            if (player.GetComponent<InventoryManager>().activeAmmoQs < 0)
            {
                player.GetComponent<InventoryManager>().activeAmmo = null;
                player.GetComponent<InventoryManager>().SetWeaponActive(null, "ammo");


            }
            else
            {
                GameObject arrow = Instantiate(player.GetComponent<InventoryManager>().activeAmmo.gameObject, player.transform);

                arrow.transform.parent = player.transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_R").transform.Find("Shoulder_R").transform.Find("Elbow_R").transform.Find("Hand_R");
                arrow.transform.localPosition = player.transform.Find("Arrow Mount").localPosition;
                arrow.transform.localRotation = player.transform.Find("Arrow Mount").localRotation;

                arrow.SetActive(true);

                GameObject bow = player.transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_L").transform.Find("Shoulder_L").transform.Find("Elbow_L").transform.Find("Hand_L").transform.Find(player.GetComponent<InventoryManager>().activeWeaponL.gameObject.name + "(Clone)").gameObject;
                bow.GetComponent<Animation>().Play("Draw back");
                player.GetComponent<BaseCharacter>().m_StationaryTurnSpeed /= 5f;

                if (player.GetComponent<CharacterAndWeaponController>().preciseAiming)
                {
                    GameObject.Find("Ranged CAM").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 11;
                    player.GetComponent<BaseCharacter>().m_StationaryTurnSpeed /= 4f;
                }


                prevPlayerXRotation = player.transform.rotation.x;
                hasFired = false;
                player.GetComponent<BaseCharacter>().canMove = false;

                player.GetComponent<CharacterAndWeaponController>().StartCoroutine(aiming(player.transform.Find("Rig 1").transform.Find("Target").gameObject, player));
            }
        }

        if (weaponR == 8)
        {
            player = GameObject.Find("Player");
            player.GetComponent<InventoryManager>().activeAmmoQs -= 1;
            if (player.GetComponent<InventoryManager>().activeAmmoQs < 0)
            {
                player.GetComponent<InventoryManager>().activeAmmo = null;
                player.GetComponent<InventoryManager>().SetWeaponActive(null, "ammo");


            }
            else
            {
                GameObject bolt = Instantiate(player.GetComponent<InventoryManager>().activeAmmo.gameObject, player.transform);
                //GameObject.Find("Characters").transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_R").transform.Find("Shoulder_R").transform.Find("Elbow_R").transform.Find("Hand_R").transform.Find(GameObject.Find("Player").GetComponent<CharacterAndWeaponController>().weaponR + "(Clone)").transform.Find("Root").transform.Find("String").transform.Find("Crossbow Bolt").gameObject.SetActive(true);
                GameObject crossbow = player.transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_R").transform.Find("Shoulder_R").transform.Find("Elbow_R").transform.Find("Hand_R").transform.Find(player.GetComponent<InventoryManager>().activeWeaponR.gameObject.name + "(Clone)").gameObject;


                bolt.transform.parent = crossbow.transform.Find("Root").transform.Find("String");
                bolt.transform.localPosition = player.transform.Find("Bolt Mount").localPosition;
                bolt.transform.localRotation = player.transform.Find("Bolt Mount").localRotation;

                bolt.SetActive(true);

                crossbow.GetComponent<Animation>().Play("Draw back");
                player.GetComponent<BaseCharacter>().m_StationaryTurnSpeed /= 5f;
                if (player.GetComponent<CharacterAndWeaponController>().preciseAiming)
                {
                    GameObject.Find("Ranged CAM").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 11;
                    player.GetComponent<BaseCharacter>().m_StationaryTurnSpeed /= 4f;
                }

                prevPlayerXRotation = player.transform.rotation.x;
                hasFired = false;
                player.GetComponent<BaseCharacter>().canMove = false;

                GameObject target = player.transform.Find("Rig 1").transform.Find("Target").gameObject;
                player.GetComponent<CharacterAndWeaponController>().StartCoroutine(aiming(target, player));
            }
        }
        if (weaponR == 10)
        {
            player = animator.gameObject;
            animator.gameObject.GetComponent<StatsManager>().StartCoroutine(StowLute(ShootDelay, animator.gameObject));
        }
        switch (spell)
        {
            case 1://fireball
            case 4://blue fireball
                animator.gameObject.GetComponent<StatsManager>().StartCoroutine(LaunchProjectile(ShootDelay, createParticle1, animator.gameObject.transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_R").transform.Find("Shoulder_R").transform.Find("Elbow_R").transform.Find("Hand_R").gameObject, animator.gameObject));
                break;
            case 2://ice explosion
            case 7://crystal explosion
            case 9://rock explosion
            case 11://vine explosion
                animator.gameObject.GetComponent<StatsManager>().StartCoroutine(LaunchProjectile(ShootDelay, createParticle1, animator.gameObject.GetComponent<PlayerControl>().FindNearestEnemy().gameObject, animator.gameObject));
                break;
            case 3://ice path
            case 8://crystal path
            case 10://rock path
            case 12://vine path
                animator.gameObject.GetComponent<StatsManager>().StartCoroutine(ShardPath(ShootDelay, createParticle1, animator.gameObject));
                break;
            case 5://power beam
                animator.gameObject.GetComponent<StatsManager>().StartCoroutine(LaunchProjectile(ShootDelay, createParticle1, animator.gameObject.transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_R").transform.Find("Shoulder_R").transform.Find("Elbow_R").transform.Find("Hand_R").gameObject, animator.gameObject));
                break;
            case 6://lightning strike
                animator.gameObject.GetComponent<StatsManager>().StartCoroutine(Lightning(ShootDelay, createParticle1, animator.gameObject.GetComponent<PlayerControl>().FindNearestEnemy().gameObject, animator.gameObject));
                break;


        }

        switch (ability)
        {
            case 1://fire blast
                animator.gameObject.GetComponent<StatsManager>().StartCoroutine(LaunchProjectile(ShootDelay, createParticle1, animator.gameObject.transform.Find("Root").transform.Find("Hips").transform.Find("UpperLeg_R").transform.Find("LowerLeg_R").transform.Find("Ankle_R").transform.Find("Ball_R").gameObject, animator.gameObject));

                break;
        }

        animator.SetInteger("Spell", 0);
        animator.SetInteger("Ability", 0);

    }



    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    /*override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.transform.Rotate(-CrossPlatformInputManager.GetAxis("Vertical") / 4, 0, 0);
    }*/

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*if (weapon == 1 && GameObject.Find("Character").GetComponent<CharacterAndWeaponController>().weaponR.Equals("Axe Bone"))
        {
            trail.emitting = false; 
        }*/
        if (weaponL == 3)
        {
            player = GameObject.Find("Player");
            if (player.GetComponent<InventoryManager>().activeAmmoQs >= 0)
            {
                GameObject arrow = player.transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_R").transform.Find("Shoulder_R").transform.Find("Elbow_R").transform.Find("Hand_R").transform.Find(player.GetComponent<InventoryManager>().activeAmmo.gameObject.name + "(Clone)").gameObject;

                GameObject bow = player.transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_L").transform.Find("Shoulder_L").transform.Find("Elbow_L").transform.Find("Hand_L").transform.Find(player.GetComponent<InventoryManager>().activeWeaponL.gameObject.name + "(Clone)").gameObject;
                bow.GetComponent<Animation>().Play("Fire");

                arrow.transform.parent = null;
                arrow.transform.localScale = new Vector3(1, 1, 1);

                PlayerProjectileManager ppm = arrow.AddComponent<PlayerProjectileManager>();
                AmmoManager am = player.GetComponent<InventoryManager>().activeAmmo;

                ppm.damageVal = (int)(bow.GetComponent<ItemManager>().damageVal * am.damageMult);
                ppm.damageEffects = am.damageEffects;
                ppm.EffectDuration = am.effectDuration;
                ppm.DamagePerSec = am.damagePerSec;
                ppm.damageEffect = am.damageEffect;

                arrow.GetComponent<BoxCollider>().enabled = true;
                arrow.transform.parent = null;
                Rigidbody arrowRb = arrow.GetComponent<Rigidbody>();
                arrowRb.isKinematic = false;
                arrowRb.useGravity = true;
                player.GetComponent<BaseCharacter>().m_StationaryTurnSpeed *= 5f;
                if (player.GetComponent<CharacterAndWeaponController>().preciseAiming)
                {
                    player.GetComponent<BaseCharacter>().m_StationaryTurnSpeed *= 4f;
                }

                float distance = findDistance(arrow);
                if (distance >= 15)
                {
                    arrowRb.AddRelativeForce(Vector3.forward * distance * 7.3f, ForceMode.Impulse);
                }
                else
                {
                    arrowRb.AddRelativeForce(Vector3.forward * 100f, ForceMode.Impulse);
                }
                hasFired = true;
                GameObject.Find("Ranged CAM").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 9;

                player.GetComponent<BaseCharacter>().canMove = true;
            }
        }

        if (weaponR == 8)
        {
            player = GameObject.Find("Player");
            if (player.GetComponent<InventoryManager>().activeAmmoQs >= 0)
            {
                GameObject bolt = player.transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_R").transform.Find("Shoulder_R").transform.Find("Elbow_R").transform.Find("Hand_R").transform.Find(player.GetComponent<InventoryManager>().activeWeaponR.gameObject.name + "(Clone)").transform.Find("Root").transform.Find("String").transform.Find(player.GetComponent<InventoryManager>().activeAmmo.gameObject.name + "(Clone)").gameObject;

                GameObject crossbow = player.transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_R").transform.Find("Shoulder_R").transform.Find("Elbow_R").transform.Find("Hand_R").transform.Find(player.GetComponent<InventoryManager>().activeWeaponR.gameObject.name + "(Clone)").gameObject;
                crossbow.GetComponent<Animation>().Play("Fire");

                bolt.transform.parent = null;
                bolt.transform.localScale = new Vector3(1, 1, 1);

                PlayerProjectileManager ppm = bolt.AddComponent<PlayerProjectileManager>();
                ppm.damageVal = crossbow.GetComponent<ItemManager>().damageVal;
                ppm.damageEffects = crossbow.GetComponent<ItemManager>().damageEffects;
                ppm.damageEffect = crossbow.GetComponent<ItemManager>().damageEffect;


                bolt.GetComponent<BoxCollider>().enabled = true;
                bolt.transform.parent = null;
                Rigidbody boltRb = bolt.GetComponent<Rigidbody>();
                boltRb.isKinematic = false;
                boltRb.useGravity = true;
                player.GetComponent<BaseCharacter>().m_StationaryTurnSpeed *= 5f;
                if (player.GetComponent<CharacterAndWeaponController>().preciseAiming)
                {
                    player.GetComponent<BaseCharacter>().m_StationaryTurnSpeed *= 4f;
                }

                float distance = findDistance(bolt);
                /*if (distance >= 15)
                {
                    boltRb.AddRelativeForce(Vector3.forward * distance * 6.3f, ForceMode.Impulse);
                }
                else
                {*/
                boltRb.AddRelativeForce(Vector3.forward * 100f, ForceMode.Impulse);



                hasFired = true;
                GameObject.Find("Ranged CAM").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 9;

                player.GetComponent<BaseCharacter>().canMove = true;
            }
        }


        if (animator.gameObject.GetComponent<CharacterAndWeaponController>().preciseAiming)
        {

            GameObject camTarget = GameObject.Find("Main CAM target");
            camTarget.transform.eulerAngles = new Vector3(camTarget.transform.rotation.eulerAngles.x, GameObject.Find("Ranged CAM").transform.rotation.eulerAngles.y, camTarget.transform.rotation.eulerAngles.z); ;
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

    IEnumerator aiming(GameObject target, GameObject player)
    {
        target.transform.localPosition = new Vector3(0, 0.6f, 5);
        if (weaponR == 8)
        {
            target.transform.localPosition = new Vector3(2, 0.4f, 5);
        }
        while (!hasFired)
        {

            if (weaponL == 3)
            {
                player.transform.Find("Rig 1").transform.Find("Left Aim").gameObject.GetComponent<TwoBoneIKConstraint>().weight = 1;
                player.transform.Find("Rig 1").transform.Find("Right Aim").gameObject.GetComponent<MultiAimConstraint>().weight = 1;


            }
            if (weaponR == 8)
            {
                GameObject.Find("Player").transform.Find("Rig 1").transform.Find("Right Aim").gameObject.GetComponent<TwoBoneIKConstraint>().weight = 1;

            }


            if (target.transform.localPosition.y < 2f && player.GetComponent<PlayerControl>().moveStick.y > 0 || target.transform.localPosition.y > -1.3f && player.GetComponent<PlayerControl>().moveStick.y < 0)
            {
                target.transform.Translate(Vector3.up * player.GetComponent<PlayerControl>().moveStick.y / 14);
            }

            player.transform.Rotate(Vector3.up * player.GetComponent<PlayerControl>().moveStick.x);

            yield return new WaitForSeconds(0.01f);
        }
        GameObject.Find("Player").transform.Find("Rig 1").transform.Find("Left Aim").gameObject.GetComponent<TwoBoneIKConstraint>().weight = 0;
        GameObject.Find("Player").transform.Find("Rig 1").transform.Find("Right Aim").gameObject.GetComponent<MultiAimConstraint>().weight = 0;
        GameObject.Find("Player").transform.Find("Rig 1").transform.Find("Right Aim").gameObject.GetComponent<TwoBoneIKConstraint>().weight = 0;


        yield return new WaitUntil(() => hasFired);

    }



    float findDistance(GameObject arrow)
    {
        int layerMask = 1 << 3;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(arrow.transform.position, arrow.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            //Debug.DrawRay(arrow.transform.position, arrow.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

            return hit.distance;
        }
        else
        {
            // Debug.DrawRay(arrow.transform.position, arrow.transform.TransformDirection(Vector3.forward) * 1000, Color.white);

            return 15;

        }

    }



    IEnumerator LaunchProjectile(float delay, GameObject projectile, GameObject startPos, GameObject startRot)
    {

        yield return new WaitForSeconds(delay);
        GameObject newProjectile = null;

        if (!projectile.name.Equals("Magic_Blast_03_Red_FX"))
        {

            if (projectile.name.Equals("Fire Blast"))
            {
                newProjectile = Instantiate(projectile, new Vector3(startPos.transform.position.x, 0.05f, startPos.transform.position.z), startRot.transform.rotation);
            }
            else
            {
                newProjectile = Instantiate(projectile, startPos.transform.position, startRot.transform.rotation);
            }
        }
        else
        {
            newProjectile = Instantiate(projectile);
            newProjectile.transform.position = startPos.transform.position;



        }
        //GameObject newProjectile = Instantiate(projectile, startPos.transform.position, startRot.transform.rotation);
        PlayerProjectileManager ppm = newProjectile.AddComponent<PlayerProjectileManager>();
        if (newProjectile.GetComponent<SpellManager>() != null)
        {
            SpellManager sm = newProjectile.GetComponent<SpellManager>();
            ppm.damageVal = sm.damageVal;
            ppm.damageEffects = sm.damageEffects;
            ppm.EffectDuration = sm.effectDuration;
            ppm.DamagePerSec = sm.damagePerSec;
            ppm.damageEffect = sm.damageEffect;
        } else
        {
            AbilityManager am = newProjectile.GetComponent<AbilityManager>();
            ppm.damageVal = am.damageVal;
            ppm.damageEffects = am.damageEffects;
            ppm.EffectDuration = am.effectDuration;
            ppm.DamagePerSec = am.damagePerSec;
            ppm.damageEffect = am.damageEffect;
        }


          
    }

    IEnumerator ShardPath(float delay, GameObject shard, GameObject start)
    {
        yield return new WaitForSeconds(delay);
        GameObject newShards = Instantiate(shard, start.transform.position, start.transform.rotation, null);
        PlayerProjectileManager trailppm = newShards.transform.Find("FX_Shard" + newShards.name.Substring(0, newShards.name.IndexOf(" ")) + "_Smaller_Trail_01").gameObject.AddComponent<PlayerProjectileManager>();
        PlayerProjectileManager explosionppm = newShards.transform.Find("FX_Shard" + newShards.name.Substring(0, newShards.name.IndexOf(" ")) + "_Explosion_01").gameObject.AddComponent<PlayerProjectileManager>();
        SpellManager trailsm = trailppm.gameObject.GetComponent<SpellManager>();
        SpellManager explosionsm = trailppm.gameObject.GetComponent<SpellManager>();

        trailppm.damageVal = trailsm.damageVal;
        trailppm.damageEffects = trailsm.damageEffects;
        explosionppm.EffectDuration = trailsm.effectDuration;
        trailppm.DamagePerSec = trailsm.damagePerSec;
        trailppm.damageEffect = trailsm.damageEffect;

        explosionppm.damageVal = explosionsm.damageVal;
        explosionppm.damageEffects = explosionsm.damageEffects;
        explosionppm.EffectDuration = explosionsm.effectDuration;
        explosionppm.DamagePerSec = explosionsm.damagePerSec;
        explosionppm.damageEffect = explosionsm.damageEffect;
    }

    IEnumerator GeyserPath(float delay, float delayBetween, float distanceBetween, Vector3 target, GameObject geyser, Vector3 startPos)
    {
        Vector3 GeyserPos = startPos;
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < Vector3.Distance(startPos, target) / distanceBetween; i++)
        {
            GeyserPos = Vector3.MoveTowards(GeyserPos, target, distanceBetween);
            GameObject newGeyser = Instantiate(geyser, GeyserPos, Quaternion.Euler(0, 0, 0));
            PlayerProjectileManager ppm = newGeyser.AddComponent<PlayerProjectileManager>();
            SpellManager sm = newGeyser.GetComponent<SpellManager>();

            ppm.damageVal = sm.damageVal;
            ppm.damageEffects = sm.damageEffects;
            ppm.EffectDuration = sm.effectDuration;
            ppm.DamagePerSec = sm.damagePerSec;
            ppm.damageEffect = sm.damageEffect;

            yield return new WaitForSeconds(delayBetween);
        }
    }

    IEnumerator Lightning(float delay, GameObject particles, GameObject startPos, GameObject startRot)
    {
        yield return new WaitForSeconds(delay);
        GameObject newProjectile = null;


        newProjectile = Instantiate(particles);
        newProjectile.transform.position = startPos.transform.position;




        //GameObject newProjectile = Instantiate(projectile, startPos.transform.position, startRot.transform.rotation);
        PlayerProjectileManager ppm = newProjectile.transform.Find("FX_LightningStrike_Impact_01").gameObject.AddComponent<PlayerProjectileManager>();
        SpellManager sm = newProjectile.GetComponent<SpellManager>();

        ppm.damageVal = sm.damageVal;
        ppm.damageEffects = sm.damageEffects;
        ppm.EffectDuration = sm.effectDuration;
        ppm.DamagePerSec = sm.damagePerSec;
        ppm.damageEffect = sm.damageEffect;
    }

    IEnumerator StowLute(float delay, GameObject player)
    {
        yield return new WaitForSeconds(delay);
        player.transform.Find("Root").Find("Hips").Find("Spine_01").Find("Spine_02").Find("Spine_03").Find("Clavicle_L").Find("Shoulder_L").Find("Elbow_L").Find("Lute").gameObject.SetActive(false);

    }

}
