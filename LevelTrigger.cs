using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTrigger : MonoBehaviour
{



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            UIUpdater.CreateLevelSelect();
        }
    }
}
