using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlickerControl : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(delay());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(1 + Random.value - 0.5f);
        //GetComponent<Animation>().Play();
    }
}
