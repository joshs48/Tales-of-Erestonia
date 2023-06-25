using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponParticleController : StateMachineBehaviour
{
    private int weaponType;
    private int enemyID;
    private static ParticleSystem particles1;
    private static ParticleSystem particles2;

    private static GameObject handL;
    private static GameObject handR;

    
    [SerializeField] GameObject createParticle1; 
    [SerializeField] float ShootDelay;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        weaponType = animator.GetInteger("Weapon Type");
        enemyID = animator.GetInteger("AI_ID");

        handR = animator.gameObject.transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_R").transform.Find("Shoulder_R").transform.Find("Elbow_R").transform.Find("Hand_R").gameObject;
        handL = animator.gameObject.transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_L").transform.Find("Shoulder_L").transform.Find("Elbow_L").transform.Find("Hand_L").gameObject;


        if (weaponType == 8)
        {
            if (enemyID == 4 || enemyID == 1 || enemyID == 31)
            {
                if (stateInfo.IsName("Magic Grounded") || stateInfo.IsName("Magic Flying"))
                {
                    particles1 = animator.gameObject.transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_R").transform.Find("Shoulder_R").transform.Find("Elbow_R").transform.Find("Hand_R").transform.Find("Fireball_Collecting_FX").gameObject.GetComponent<ParticleSystem>();
                    particles2 = animator.gameObject.transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_R").transform.Find("Shoulder_R").transform.Find("Elbow_R").transform.Find("Hand_R").transform.Find("FX_Fireball_01").gameObject.GetComponent<ParticleSystem>();
                    particles2.gameObject.SetActive(true);
                    //animator.gameObject.GetComponent<AICharacterControl>().StartCoroutine(ChangeAfterDelay(0.4f));
                }
                else if (stateInfo.IsName("Throw Magic") || stateInfo.IsName("Beam Magic"))
                {
                    animator.gameObject.GetComponent<StatsManager>().StartCoroutine(LaunchProjectile(ShootDelay, createParticle1, particles2.gameObject, animator.gameObject));
                }
            }
            if (enemyID == 17)
            {
                if (stateInfo.IsName("Magic Geyser Path"))
                {
                    animator.gameObject.GetComponent<StatsManager>().StartCoroutine(GeyserPath(ShootDelay, 0.9f, 3f, animator.gameObject.GetComponent<AICharacter>().FindNearestTarget(false).transform.position, createParticle1, animator.gameObject.transform.position));
                } else if (stateInfo.IsName("Beam Magic 2"))
                {
                    animator.gameObject.GetComponent<StatsManager>().StartCoroutine(LaunchProjectile(ShootDelay, createParticle1, animator.gameObject.transform.Find("Root").transform.Find("Hips").gameObject, animator.gameObject));
                }
            }
            if (enemyID == 22)
            {
                if (stateInfo.IsName("Magic Fire"))
                {
                    animator.gameObject.GetComponent<StatsManager>().StartCoroutine(LaunchProjectile(ShootDelay, createParticle1, animator.gameObject.GetComponent<AICharacter>().FindNearestTarget(false).gameObject, animator.gameObject));
                } else if (stateInfo.IsName("Frozen breath"))
                {
                    animator.gameObject.GetComponent<StatsManager>().StartCoroutine(LaunchProjectile(ShootDelay, createParticle1, animator.gameObject.transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").gameObject, animator.gameObject)); 

                }
            }


            if (enemyID == 23)
            {
                if (stateInfo.IsName("Magic Grounded") || stateInfo.IsName("Magic Flying"))
                {
                    particles1 = animator.gameObject.transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_R").transform.Find("Shoulder_R").transform.Find("Elbow_R").transform.Find("Hand_R").transform.Find("Fireball_Collecting_FX").gameObject.GetComponent<ParticleSystem>();
                    particles2 = animator.gameObject.transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_R").transform.Find("Shoulder_R").transform.Find("Elbow_R").transform.Find("Hand_R").transform.Find("FX_Fireball_01").gameObject.GetComponent<ParticleSystem>();
                    particles2.gameObject.SetActive(true);
                    //animator.gameObject.GetComponent<AICharacterControl>().StartCoroutine(ChangeAfterDelay(0.4f));
                }
                else if (stateInfo.IsName("Throw Magic blue") || stateInfo.IsName("Beam Magic blue"))
                {
                    animator.gameObject.GetComponent<StatsManager>().StartCoroutine(LaunchProjectile(ShootDelay, createParticle1, particles2.gameObject, animator.gameObject));
                }
            }

            if ((enemyID >= 28 && enemyID <= 30) || enemyID == 7)
            {
                if (stateInfo.IsName("Rock Path") || stateInfo.IsName("Ice Path") || stateInfo.IsName("Vine Path") || stateInfo.IsName("Crystal Path"))
                {
                    animator.gameObject.GetComponent<StatsManager>().StartCoroutine(ShardPath(ShootDelay, createParticle1, animator.gameObject));
                }
                if (stateInfo.IsName("Rock Explosion") || stateInfo.IsName("Ice Explosion") || stateInfo.IsName("Vine Explosion") || stateInfo.IsName("Crystal Explosion"))
                {
                    animator.gameObject.GetComponent<StatsManager>().StartCoroutine(LaunchProjectile(ShootDelay, createParticle1, animator.gameObject.GetComponent<AICharacter>().FindNearestTarget(false).gameObject, animator.gameObject));

                }
            }
            
        } else if (weaponType == 5)
        {
            if (enemyID == 21)
            {
                if (stateInfo.IsName("Staff Hit 2"))
                {
                    GameObject startPos = animator.gameObject.transform.Find("Root").transform.Find("Hips").transform.Find("Spine_01").transform.Find("Spine_02").transform.Find("Spine_03").transform.Find("Clavicle_R").transform.Find("Shoulder_R").transform.Find("Elbow_R").transform.Find("Hand_R").transform.Find("Warchief's Staff").transform.Find("Magic Mount").gameObject;
                    startPos.transform.eulerAngles = Vector3.forward;
                    animator.gameObject.GetComponent<AICharacter>().StartCoroutine(LaunchProjectile(ShootDelay, createParticle1, startPos, animator.gameObject));
                }
            }
        }

        

        
    }

    IEnumerator ChangeAfterDelay(float delay)
    {
        particles1.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        particles2.gameObject.SetActive(true);
        particles1.gameObject.SetActive(false);
    }

    IEnumerator LaunchProjectile(float delay, GameObject projectile, GameObject startPos, GameObject startRot)
    {

        yield return new WaitForSeconds(delay);
        GameObject newProjectile = null;
        if (projectile.name.Equals("Fireball_Shooting_FX"))
        {
            particles2.gameObject.SetActive(false);
        }
        if (!projectile.name.Equals("Magic_Blast_03_Red_FX"))
        {
            newProjectile = Instantiate(projectile, startPos.transform.position, startRot.transform.rotation);
            var collision = newProjectile.GetComponent<ParticleSystem>().collision;
            collision.collidesWith |= (1 << 3);
            collision.collidesWith |= (1 << 9);
        }
        else
        {
            newProjectile = Instantiate(projectile);
            var collision = newProjectile.GetComponent<ParticleSystem>().collision;
            collision.collidesWith |= (1 << 3);
            collision.collidesWith |= (1 << 9);

        }
        EnemyProjectileManager epm = newProjectile.AddComponent<EnemyProjectileManager>();
        SpellData sm = newProjectile.GetComponent<Spell>().data;

        epm.damageVal = sm.damageVal;
        epm.damageEffects = sm.damageEffects;
        epm.EffectDuration = sm.effectDuration;
        epm.DamagePerSec = sm.damagePerSec;
        epm.damageEffect = sm.damageEffect;
    }

    IEnumerator ShardPath(float delay, GameObject shard, GameObject start)
    {
        yield return new WaitForSeconds(delay);
        GameObject newShards = Instantiate(shard, start.transform.position, start.transform.rotation, null);

        EnemyProjectileManager trailepm = newShards.transform.Find("FX_Shard" + newShards.name.Substring(0, newShards.name.IndexOf(" ")) + "_Smaller_Trail_01").gameObject.AddComponent<EnemyProjectileManager>();
        EnemyProjectileManager explosionepm = newShards.transform.Find("FX_Shard" + newShards.name.Substring(0, newShards.name.IndexOf(" ")) + "_Explosion_01").gameObject.AddComponent<EnemyProjectileManager>();
        SpellData trailsm = trailepm.gameObject.GetComponent<Spell>().data;
        SpellData explosionsm = trailepm.gameObject.GetComponent<Spell>().data;

        var collision = trailepm.gameObject.GetComponent<ParticleSystem>().collision;
        collision.collidesWith |= (1 << 3);
        collision = explosionepm.gameObject.GetComponent<ParticleSystem>().collision;
        collision.collidesWith |= (1 << 3);

        trailepm.damageVal = trailsm.damageVal;
        trailepm.damageEffects = trailsm.damageEffects;
        trailepm.EffectDuration = trailsm.effectDuration;
        trailepm.DamagePerSec = trailsm.damagePerSec;
        trailepm.damageEffect = trailsm.damageEffect;

        explosionepm.damageVal = explosionsm.damageVal;
        explosionepm.damageEffects = explosionsm.damageEffects;
        explosionepm.EffectDuration = explosionsm.effectDuration;
        explosionepm.DamagePerSec = explosionsm.damagePerSec;
        explosionepm.damageEffect = explosionsm.damageEffect;
    }

    IEnumerator GeyserPath(float delay, float delayBetween, float distanceBetween, Vector3 target, GameObject geyser, Vector3 startPos)
    {
        Vector3 GeyserPos = startPos;
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < Vector3.Distance(startPos, target) / distanceBetween; i++) {
            GeyserPos = Vector3.MoveTowards(GeyserPos, target, distanceBetween);
            GameObject newGeyser = Instantiate(geyser, GeyserPos, Quaternion.Euler(0, 0, 0));
            EnemyProjectileManager epm = newGeyser.AddComponent<EnemyProjectileManager>();
            SpellData sm = newGeyser.GetComponent<Spell>().data;
            var collision = newGeyser.GetComponent<ParticleSystem>().collision;
            collision.collidesWith |= (1 << 3);

            epm.damageVal = sm.damageVal;
            epm.damageEffects = sm.damageEffects;
            epm.EffectDuration = sm.effectDuration;
            epm.DamagePerSec = sm.damagePerSec;
            epm.damageEffect = sm.damageEffect;

            yield return new WaitForSeconds(delayBetween);
        }
    }

    

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
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
}
