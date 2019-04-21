using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem confettti;

    public void ShootConfetti()
    {
        confettti.Play();
    }
}
