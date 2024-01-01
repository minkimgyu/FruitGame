using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : BaseEffect
{
    [SerializeField]
    List<ParticleSystem> _particle;

    public override void PlayEffect()
    {
        base.PlayEffect();
        if (_particle == null) return;

        for (int i = 0; i < _particle.Count; i++)
        {
            _particle[i].Play();
        }
    }

    public override void PlayEffect(float duration)
    {
        _duration = duration;

        base.PlayEffect();
        if (_particle == null) return;

        for (int i = 0; i < _particle.Count; i++)
        {
            _particle[i].Play();
        }
    }

    void Awake()
    {
        _particle = new List<ParticleSystem>();

        ParticleSystem myParticle = GetComponent<ParticleSystem>();
        _particle.Add(myParticle);

        ParticleSystem[] childrenParticles = GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < childrenParticles.Length; i++)
        {
            _particle.Add(childrenParticles[i]);
        }
    }
}
