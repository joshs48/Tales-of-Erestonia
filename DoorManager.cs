using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
   

    private bool canOpen = false;
    private bool isOpened = false;

    [SerializeField] GameObject closeBehindWall;
    [SerializeField] float finalLockYPos;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Open(GameObject player)
    {
        while (canOpen && !player.GetComponent<PlayerControl>().shield)
        {
            yield return new WaitForSeconds(0.01f);
        }
        if (player.GetComponent<PlayerControl>().shield)
        {
            

            if (!isOpened)
            {
                if (closeBehindWall != null)
                {
                    StartCoroutine(LockBehind());
                }
                GameObject leftDoor = transform.GetChild(1).gameObject;
                GameObject rightDoor = transform.GetChild(0).gameObject;

                for (int i = 0; i < 157; i++)
                {
                    leftDoor.transform.Rotate(Vector3.up, 0.7f);
                    rightDoor.transform.Rotate(Vector3.up, -0.7f);
                    yield return new WaitForSeconds(0.004f);
                }
            }
            isOpened = true;

            

        }

    }

    IEnumerator LockBehind()
    {
        Vector3 movement = new Vector3(0, 0.1f, 0);
        float distance = (finalLockYPos - closeBehindWall.transform.position.y) * 10;
        for (float i = 0; i < distance; i++)
        {
            closeBehindWall.transform.Translate(movement);
            yield return new WaitForSeconds(0.01f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isOpened)
        {
            UIUpdater.CreateNotificationBar("Press 'shield' to open door!");
            canOpen = true;
            StartCoroutine(Open(other.gameObject));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canOpen = false;
        }
    }

    


}
