using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingUtil : MonoBehaviour
{
    private double startTime;
    private double endTime;

    private void OnTriggerEnter(Collider other)
    {
        startTime = Time.timeAsDouble;
    }

    private void OnTriggerExit(Collider other)
    {
        endTime = Time.timeAsDouble;
        Debug.Log(other.gameObject.name + "'s time is: " + (endTime - startTime));
    }


}
