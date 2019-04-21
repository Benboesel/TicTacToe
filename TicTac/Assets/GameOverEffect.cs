using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem confettti;
    public Color SkyTopColor;
    public Color SkyHorizonColor;
    public Color SkyBottomColor;
    public Color BoardColor;

    public void ShootConfetti()
    {
        confettti.Play();
    }

    public void EffectOver()
    {
        GameOverEffects.Instance.EffectsOver();
    }
}
