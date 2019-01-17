using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereParticelColllisionScript : MonoBehaviour
{
    public ParticleSystem sphereParticel;

    public void PlaySphereParticel()
    {
        sphereParticel.Play();
        print("11111111111111111111111111111111111111111111");
    }
    
    void OnParticleCollision(GameObject other)
    {
        print("trifft irgendwas");
        if (other.tag == "Enemy")
        {
            //other.GetComponentInParent<EnemyAI>().ShockedAnimationEvent();
            other.GetComponent<EnemyAI>().ShockedAnimationEvent();
            print("Collision Sphere" + other.name);
        }
    }
}
