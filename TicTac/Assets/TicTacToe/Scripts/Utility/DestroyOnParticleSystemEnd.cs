using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnParticleSystemEnd : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;

    void Update()
    {
        if (!particle.IsAlive())
        {
            Destroy(this.gameObject);
        }
    }
}
