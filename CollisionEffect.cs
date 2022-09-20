using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEffect : MonoBehaviour
{
    public ParticleSystem particles;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        particles.Play();
        if (particles.name.IndexOf("Explosion") >= 0)
        {
            Destroy(gameObject);
        }
    }
}
