using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinecartController : MonoBehaviour
{
    public Transform[] PathNode;
    public float BaseMoveSpeed;
    private float MoveSpeed;
    public float TurnSpeed;
    public Vector3 CurrentPositionHolder;
    public int CurrentNode;
    private Vector3 startPos;
    public GameObject nodes;
    float t = 0;

    private void Start()
    {
        PathNode = nodes.GetComponentsInChildren<Transform>();
        CheckNode();
    }

    void CheckNode()
    {
        t = 0;
        startPos = transform.position;
        CurrentPositionHolder = PathNode[CurrentNode].position;
        MoveSpeed = BaseMoveSpeed + Random.Range(-1, 1);
    }
    
    private void Update()
    {
        t += MoveSpeed * Time.deltaTime / Vector3.Distance(startPos, CurrentPositionHolder);

        if (CurrentNode == 19 || CurrentNode == 23)
        {
            transform.position = CurrentPositionHolder;
        }

        if (CurrentNode == 18 || CurrentNode == 20)
        {
            MoveSpeed *= 1.06f;
        }

        if (transform.position != CurrentPositionHolder)
        {
            transform.position = Vector3.Lerp(startPos, CurrentPositionHolder, t);
        } else
        {
            if (CurrentNode < PathNode.Length - 1)
            {
                CurrentNode++;
                CheckNode();
            } else
            {
                CurrentNode = 1;
                CheckNode();
            }
        }
        var rotation = Quaternion.LookRotation(CurrentPositionHolder - transform.position);
        // rotation.x = 0; This is for limiting the rotation to the y axis. I needed this for my project so just
        // rotation.z = 0;                 delete or add the lines you need to have it behave the way you want.
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * TurnSpeed);



    }

}


