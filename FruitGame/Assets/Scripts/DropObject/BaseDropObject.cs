using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class BaseDropObject : MonoBehaviour
{
    public virtual void Initialize(Action<BaseDropObject> onDestroyRequested) { }

    public virtual void Initialize(Action<Type, Vector3> onSpawnRequested, 
        Action<BaseDropObject, BaseDropObject> onDestroyRequested, Action onWaterDecreaseRequested){ }

    public abstract void OnSpawn();

    public abstract void OnReady();

    public abstract void OnDrop();

    public abstract void OnLand(); // ������ ����� ���� �ı�

    public abstract void OnHighlight(); // ������ ����� ���� �ı�

    public virtual Fruit.Type ReturnType() { return default; }
}
