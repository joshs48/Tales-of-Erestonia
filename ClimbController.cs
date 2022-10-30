using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClimbController : MonoBehaviour
{
    GameObject[] climbAnchors;
    GameObject[] climbUpAnchors;
    Animator m_Animator;
    public GameObject closestClimbAnchor;
    public GameObject closestClimbUpAnchor;
    public float closestClimbAnchorDistance = 500;
    public float closestClimbUpAnchorDistance = 500;
    public bool isClimbing = false;
    public bool isGrounded = true;
    public string climbType;
    public float maxShimmyDistance;
    public float maxHopDistance;

    private GameObject targetAnchor;
    private Vector3 playerOffsetPos;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        climbAnchors = GameObject.FindGameObjectsWithTag("Climb Anchor");
        climbUpAnchors = GameObject.FindGameObjectsWithTag("Climb Up Anchor");
        //closestClimbAnchor = climbAnchors[0];
        

    }

    // Update is called once per frame
    void Update()
    {
        playerOffsetPos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        SetClosestClimbAnchor(playerOffsetPos);

        if (isClimbing)
        {
            Climb();
            
            
        }
       

    }

    public GameObject SetClosestClimbAnchor(Vector3 startLocation)
    {
        closestClimbAnchorDistance = 500;
        climbAnchors = GameObject.FindGameObjectsWithTag("Climb Anchor");
        for (int i = 0; i < climbAnchors.Length; i ++)
        {
            GameObject climbAnchor = climbAnchors[i];
            float currentDistance = Vector3.Distance(startLocation, climbAnchor.transform.position);
            if (currentDistance < closestClimbAnchorDistance)
            {
                
                closestClimbAnchor = climbAnchor;
                closestClimbAnchorDistance = currentDistance;
            }
            
        }
        return closestClimbAnchor;
    }

    public GameObject SetClosestClimbUpAnchor(Vector3 startLocation)
    {
        closestClimbUpAnchorDistance = 500;
        climbUpAnchors = GameObject.FindGameObjectsWithTag("Climb Up Anchor");
        for (int i = 0; i < climbUpAnchors.Length; i++)
        {
            GameObject climbUpAnchor = climbUpAnchors[i];
            float currentDistance = Vector3.Distance(startLocation, climbUpAnchor.transform.position);
            if (currentDistance < closestClimbUpAnchorDistance)
            {

                closestClimbUpAnchor = climbUpAnchor;
                closestClimbUpAnchorDistance = currentDistance;
            }

        }
        return closestClimbUpAnchor;
    }

    private GameObject FindAndGoToNearbyClimbAnchor(string direction, float shimmyDistance, float hopDistance)
    {
        Vector3 findPoint = new Vector3();
        
        
        switch (direction)
        {
            case "up":
                
                findPoint = new Vector3(playerOffsetPos.x, playerOffsetPos.y + hopDistance , playerOffsetPos.z);
                targetAnchor = SetClosestClimbAnchor(findPoint);
                if (Vector3.Distance(playerOffsetPos, targetAnchor.transform.position) > 0.3f && Vector3.Distance(playerOffsetPos, targetAnchor.transform.position) < 1.6)
                {
                    m_Animator.SetInteger("Climb Action", 2);
                    //StartCoroutine(DelayUntilGrabbedHopUp(targetAnchor));
                    return targetAnchor;
                }
                else 
                {
                    targetAnchor = SetClosestClimbUpAnchor(findPoint);
                    if (Vector3.Distance(playerOffsetPos, targetAnchor.transform.position) < 1.6)
                    {
                        m_Animator.applyRootMotion = true;
                        m_Animator.SetInteger("Climb Action", 7);

                    }
                    return null;
                }

            case "down":
                findPoint = new Vector3(playerOffsetPos.x, playerOffsetPos.y - hopDistance, playerOffsetPos.z);
                targetAnchor = SetClosestClimbAnchor(findPoint);
                if (Vector3.Distance(playerOffsetPos, targetAnchor.transform.position) > 0.3f && Vector3.Distance(playerOffsetPos, targetAnchor.transform.position) < 1.6)
                {
                    m_Animator.SetInteger("Climb Action", 0);
                    StartCoroutine(DelayUntilClimbDown(targetAnchor));
                    return targetAnchor;
                }
                else
                {
                    return null;
                }

                
            case "left":
                findPoint = new Vector3(transform.localPosition.x, transform.localPosition.y + 2, transform.localPosition.z - shimmyDistance); 
                targetAnchor = SetClosestClimbAnchor(findPoint);
                if (Vector3.Distance(playerOffsetPos, targetAnchor.transform.position) > 0.3f)
                {
                    m_Animator.SetInteger("Climb Action", 3);
                    StartCoroutine(DelayUntilGrabbedShimmy(targetAnchor, "left"));
                    return targetAnchor;
                }
                else
                {
                    /*findPoint = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - hopDistance);
                    targetAnchor = SetClosestClimbAnchor(findPoint);
                    if (Vector3.Distance(transform.position, targetAnchor.transform.position) < hopDistance)
                    {
                        m_Animator.SetInteger("Climb Action", 4);
                        //TODO need grad code
                        return targetAnchor;

                    }
                    else
                    {
                        return null;
                    }*/
                }
                break;
            case "right":
                findPoint = new Vector3(transform.localPosition.x, transform.localPosition.y + 2, transform.localPosition.z + shimmyDistance);
                targetAnchor = SetClosestClimbAnchor(findPoint);
                if (Vector3.Distance(playerOffsetPos, targetAnchor.transform.position) > 0.3f)
                {
                    m_Animator.SetInteger("Climb Action", 5);
                    StartCoroutine(DelayUntilGrabbedShimmy(targetAnchor, "right"));
                    return targetAnchor;
                }
                else
                {
                    /*findPoint = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + hopDistance);
                    targetAnchor = SetClosestClimbAnchor(findPoint);
                    if (Vector3.Distance(transform.position, targetAnchor.transform.position) < hopDistance)
                    {
                        m_Animator.SetInteger("Climb Action", 6);
                        //TODO need a grab code
                        return targetAnchor;

                    }
                    else
                    {
                        return null;
                    }*/
                }
            
                break;
                    }
        return null;
    }

    private void Climb()
    {/*
        m_Animator.SetInteger("Climb Action", 0);
        if (CrossPlatformInputManager.GetButtonDown("Jump")) 
        {
            Drop();
        }
        if (CrossPlatformInputManager.GetAxis("Horizontal") > 0.5)
        {
            
            FindAndGoToNearbyClimbAnchor("right", maxShimmyDistance, maxHopDistance);
        } else if (CrossPlatformInputManager.GetAxis("Horizontal") < -0.5)
        {
            
            FindAndGoToNearbyClimbAnchor("left", maxShimmyDistance, maxHopDistance);
        }
        if (CrossPlatformInputManager.GetAxis("Vertical") > 0.5 || Input.GetKeyDown(KeyCode.UpArrow))
        {
            FindAndGoToNearbyClimbAnchor("up", maxShimmyDistance, maxHopDistance);
        } else if (CrossPlatformInputManager.GetAxis("Vertical") < -0.5 || Input.GetKeyDown(KeyCode.DownArrow))
        {
            FindAndGoToNearbyClimbAnchor("down", maxShimmyDistance, maxHopDistance);
        }

        */
        
    }

    public void ClipToClimbAnchor(GameObject climbAnchor, float offsetY)
    {
        isGrounded = false;
        //transform.rotation = climbAnchor.transform.rotation;
        transform.eulerAngles = new Vector3(climbAnchor.transform.rotation.eulerAngles.x - 10, climbAnchor.transform.rotation.eulerAngles.y, climbAnchor.transform.rotation.eulerAngles.z);
        //transform.Rotate(Vector3.right, -10);
        Vector3 pos = new Vector3(climbAnchor.transform.position.x + 0.01f, climbAnchor.transform.position.y - 2f-offsetY, climbAnchor.transform.position.z);
        m_Animator.SetInteger("Climb Action", 0);
        transform.position = pos;
        GameObject.Find("Climbing CAM").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 11;
    }

    

    private void Drop()
    {
        m_Animator.SetInteger("Climb Action", 1);
        m_Animator.SetBool("Climbing", false);
        
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<CapsuleCollider>().enabled = true;
        StartCoroutine(DelayUntilGround());
        isClimbing = false;
        transform.Rotate(Vector3.right, 10);
        transform.Translate(Vector3.back * 0.5f);

        GameObject.Find("Climbing CAM").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 9;
    }


    IEnumerator DelayUntilGround()
    {
        yield return new WaitUntil(() => isGrounded);/*
        GetComponent<ThirdPersonUserControl>().enabled = true;
        GetComponent<ThirdPersonCharacter>().enabled = true;*/
    }

    IEnumerator DelayUntilGrabbedShimmy(GameObject anchor, string direction)
    {
            switch (direction)
            {
                case "left":
                    transform.Translate(Vector3.left * 0.01f);
                    break;
                case "right":
                    transform.Translate(Vector3.right * 0.01f);
                    break;
            }
        yield return new WaitUntil(() => Vector3.Distance(anchor.transform.position, transform.position) < 2.015f);
        
        
    }

    IEnumerator DelayUntilGrabbedHopUp(GameObject anchor)
    {
        while (Vector3.Distance(anchor.transform.position, playerOffsetPos) >= 0.5f) {
            transform.Translate(Vector3.up * 0.07f);
            yield return new WaitForSeconds(0.001f);
        }
        
        yield return new WaitUntil(() => Vector3.Distance(anchor.transform.position, playerOffsetPos) < 0.5f);
        ClipToClimbAnchor(anchor, 0.13f);
       

    }

    IEnumerator DelayUntilGrabbedClimbUp(GameObject anchor)
    {
        for (int i = 0; i < 30; i++)
        {
            transform.Translate(Vector3.up * 0.07f);
            yield return new WaitForSeconds(0.001f);
        }
        for (int i = 0; i < 15; i++)
        {
            transform.Translate(Vector3.forward * 0.07f);
            yield return new WaitForSeconds(0.001f);
        }
        
        Drop();
        

    }

    IEnumerator DelayUntilClimbDown(GameObject anchor)
    {
        GetComponent<Rigidbody>().useGravity = true;

        yield return new WaitUntil(() => Vector3.Distance(anchor.transform.position, playerOffsetPos) < 0.5f);
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        ClipToClimbAnchor(anchor, 0);
        

    }

    

    public void ClimbAnimEvent()
    {
        StartCoroutine(DelayUntilGrabbedHopUp(targetAnchor));
        
    }

    public void ClimbUpAnimEvent()
    {
        StartCoroutine(DelayUntilGrabbedClimbUp(targetAnchor));
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        
    }
}
