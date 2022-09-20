using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public class PlayerControl : MonoBehaviour
{
    PlayerControls controls;
    BaseCharacter bc;
    private DualShock4GamepadHID ps4Controller;


    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;
    private GameObject camTarget;

    public Vector2 moveStick;
    public Vector2 cam;
    public int altMove;


    public bool basicAtk;
    public bool advancedAtk;
    public bool shield;
    public bool jump;
    public bool menu;
    public bool QSGCycle;
    public bool QSGUse;
    public bool QSSCycle;
    public bool QSSUse;
    public bool ability1Use;
    public bool ability2Use;
    public bool ability3Use;

    private Vector3 m_Move;

    private bool m_Jump;
    private bool m_BasicAtk;
    private bool m_AdvancedAtk;
    private bool m_Shield;

    private bool prevBasicAtk;
    private bool prevShield;


    // Start is called before the first frame update
    void Start()
    {

        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
            camTarget = GameObject.Find("Main CAM target");
            camTarget.transform.rotation = transform.rotation;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        if (m_Cam != null)
        {
            // rotate cam target
            camTarget.transform.position = transform.position;
            if (Mathf.Abs(moveStick.x) > 0.1 || Mathf.Abs(moveStick.y) > 0.1)
            {
                //StartCoroutine(RotateCAM());


                
            }
            camTarget.transform.Rotate(Vector3.up * cam.x * 3);// CONSTANT HERE
            camTarget.transform.Rotate(Vector3.left * cam.y);




        }
    }

    IEnumerator RotateCAM()
    {
        float lerpVal = Mathf.Lerp(camTarget.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.y, 0.1f);
        camTarget.transform.rotation = Quaternion.Euler(camTarget.transform.rotation.x, lerpVal, camTarget.transform.rotation.z);
        yield return new WaitForEndOfFrame();
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }
    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void Awake()
    {
        controls = new PlayerControls();
        bc = GetComponent<BaseCharacter>();
        ps4Controller = InputSystem.GetDevice<DualShock4GamepadHID>();

        controls.Gameplay.Move.performed += ctx =>
        {
            moveStick = ctx.ReadValue<Vector2>();
        };
        controls.Gameplay.Move.canceled += ctx =>
        {
            moveStick = Vector2.zero;
        };

        controls.Gameplay.MoveCAM.performed += ctx =>
        {
            cam = ctx.ReadValue<Vector2>();
        };
        controls.Gameplay.MoveCAM.canceled += ctx =>
        {
            cam = Vector2.zero;
        };

        controls.Gameplay.Menu.performed += ctx =>
        {
            menu = ctx.ReadValueAsButton();
        };
        controls.Gameplay.Menu.canceled += ctx =>
        {
            menu = false;
        };

        controls.Gameplay.BasicAttack.performed += ctx =>
        {
            basicAtk = ctx.ReadValueAsButton();
        };
        controls.Gameplay.BasicAttack.canceled += ctx =>
        {
            basicAtk = false;
        };

        controls.Gameplay.Jump.performed += ctx =>
        {
            jump = ctx.ReadValueAsButton();
        };
        controls.Gameplay.Jump.canceled += ctx =>
        {
            jump = false;
        };

        controls.Gameplay.AdvancedAttack.performed += ctx =>
        {
            advancedAtk = ctx.ReadValueAsButton();
        };
        controls.Gameplay.AdvancedAttack.canceled += ctx =>
        {
            advancedAtk = false;
        };

        controls.Gameplay.Shield.performed += ctx =>
        {
            shield = ctx.ReadValueAsButton();
        };
        controls.Gameplay.Shield.canceled += ctx =>
        {
            shield = false;
        };

        controls.Gameplay.AltMove.performed += ctx =>
        {
            altMove = (int)ctx.ReadValue<float>();
        };
        controls.Gameplay.AltMove.canceled += ctx =>
        {
            altMove = 0;
        };

        controls.Gameplay.QSGearCycle.performed += ctx =>
        {
            QSGCycle = ctx.ReadValueAsButton();
        };
        controls.Gameplay.QSGearCycle.canceled += ctx =>
        {
            QSGCycle = false;
        };

        controls.Gameplay.QSGearUse.performed += ctx =>
        {
            QSGUse = ctx.ReadValueAsButton();
        };
        controls.Gameplay.QSGearUse.canceled += ctx =>
        {
            QSGUse = false;
        };

        controls.Gameplay.SpellCycle.performed += ctx =>
        {
            QSSCycle = ctx.ReadValueAsButton();
        };
        controls.Gameplay.SpellCycle.canceled += ctx =>
        {
            QSSCycle = false;
        };

        controls.Gameplay.SpellUse.performed += ctx =>
        {
            QSSUse = ctx.ReadValueAsButton();
        };
        controls.Gameplay.SpellUse.canceled += ctx =>
        {
            QSSUse = false;
        };

        controls.Gameplay.Ability1Use.performed += ctx =>
        {
            ability1Use = ctx.ReadValueAsButton();
        };
        controls.Gameplay.Ability1Use.canceled += ctx =>
        {
            ability1Use = false;
        };

        controls.Gameplay.Ability2Use.performed += ctx =>
        {
            ability2Use = ctx.ReadValueAsButton();
        };
        controls.Gameplay.Ability2Use.canceled += ctx =>
        {
            ability2Use = false;
        };

        controls.Gameplay.Ability3Use.performed += ctx =>
        {
            ability3Use = ctx.ReadValueAsButton();
        };
        controls.Gameplay.Ability3Use.canceled += ctx =>
        {
            ability3Use = false;
        };


    }
    void Movement()
    {
        if (m_Cam != null)
        {
            // calculate camera relative direction to move:
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = moveStick.y * m_CamForward + moveStick.x * m_Cam.right;

        }
        else
        {
            // we use world-relative directions in the case of no main camera
            m_Move = moveStick.y * Vector3.forward + moveStick.x * Vector3.right;
        }

        if (jump && !m_Jump)
        {
            m_Jump = true;
        }
        else
        {
            m_Jump = false;
        }

        if (basicAtk && !prevBasicAtk)
        {

            m_BasicAtk = true;

        }
        else
        {
            m_BasicAtk = false;
        }

        if (advancedAtk)
        {
            m_AdvancedAtk = true;
        }
        else
        {
            m_AdvancedAtk = false;
        }

        if (shield)
        {
            m_Shield = true;
        } else
        {
            m_Shield = false;
        }

        if (bc != null)
        {
            bc.Move(m_Move, false, m_Jump, m_BasicAtk, m_AdvancedAtk, m_Shield);
            GetComponent<Animator>().SetBool("IsBasicAtkHeld", basicAtk);

        }
        m_Jump = false;
        prevBasicAtk = basicAtk;
        prevShield = shield;
    }



    public void CreateRumble(float lowFrq, float highFrq, float duration)// 0.01 duration is good for impact, then scale the frqs to make more or less damage
    {
        if (ps4Controller != null)
        {
            StartCoroutine(Rumble(lowFrq, highFrq, duration));
        }
    }
    IEnumerator Rumble(float lowFrq, float highFrq, float duration)
    {
        ps4Controller.SetMotorSpeeds(lowFrq, highFrq);

        yield return new WaitForSeconds(duration);
        ps4Controller.SetMotorSpeeds(0, 0);
    }

    public void ChangeLightBar(Color startTone, Color endTone, float blendSpeed)
    {
        if (ps4Controller != null)
        {
            if (!startTone.Equals(endTone))
            {
                ps4Controller.SetLightBarColor(Color.Lerp(startTone, endTone, Mathf.PingPong(Time.unscaledTime * blendSpeed, 1)));
            }
            else
            {
                ps4Controller.SetLightBarColor(startTone);
            }
        }
    }

    public GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDistance = float.MaxValue;

        GameObject enemyTarget = null;

        foreach (GameObject enemy in enemies)
        {
            float currDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (currDistance < minDistance)
            {
                enemyTarget = enemy;
                minDistance = currDistance;
            }
        }
        return enemyTarget;
    }

   
}

