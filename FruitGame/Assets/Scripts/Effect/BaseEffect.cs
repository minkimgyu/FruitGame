using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseEffect : MonoBehaviour
{
    [SerializeField]
    protected float _duration;
    protected Timer _timer = new Timer();

    public virtual void Initialize(Vector3 spawnPosition)
    {
        transform.position = spawnPosition;
    }

    public virtual void Initialize(Vector3 spawnPosition, float disableDuration)
    {
        transform.position = spawnPosition;
        _duration = disableDuration;
    }

    void Update()
    {
        _timer.Update();

        if (_timer.IsFinish)
        {
            DisableObject();
            _timer.Reset();
        }
    }

    void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
        CancelInvoke();
    }

    public virtual void PlayEffect()
    {
        _timer.Start(_duration);
    }

    public virtual void PlayEffect(float duration)
    {
        _timer.Start(_duration);
    }

    void DisableObject() => gameObject.SetActive(false);
}