using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : BaseDropObject
{
    bool _contactToWater = false;

    public override void OnDrop()
    {
    }

    public override void OnHighlight()
    {
    }

    public override void OnLand()
    {
    }

    public override void OnReady()
    {
    }

    public override void OnSpawn()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spring") == false && _contactToWater == false)
        {
            _contactToWater = true;
            SoundManager.Instance.PlaySFX("WaterDrop");
            gameObject.layer = 7;

            ParticleEffect mergeEffect = ObjectPooler.SpawnFromPool<ParticleEffect>("DropEffect");
            mergeEffect.Initialize(collision.contacts[0].point);
            mergeEffect.PlayEffect();
        }
    }
}
