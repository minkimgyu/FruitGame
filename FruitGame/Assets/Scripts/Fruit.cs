using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fruit : MonoBehaviour
{
    public enum Type
    {
        Cherry,
        Strawberry,
        Grape,
        Lemon,
        Orange,
        Apple,
        Pear,
        Watermelon,
        Banana,
        Pineapple,
    }

    [SerializeField] Type _fruitType;
    public Type FruitType { get { return _fruitType; }}

    [SerializeField] bool _canChange = true;
    public bool CanChange { get { return _canChange; } set { _canChange = value; } }

    [SerializeField] Rigidbody2D rigid;

    Action<Type, Vector3> OnSpawnRequested;
    Action<Fruit, Fruit> OnDestroyRequested;

    public void Initialize(Action<Type, Vector3> onSpawnRequested, Action<Fruit, Fruit> onDestroyRequested)
    {
        OnSpawnRequested = onSpawnRequested;
        OnDestroyRequested = onDestroyRequested;
    }

    public void OnChangeGravityScaleRequested(float gravityScale)
    {
        rigid.gravityScale = gravityScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CanChange == false) return; 
        if (collision.gameObject.CompareTag("Fruit") == false) return;

        Fruit contectedFruit = collision.gameObject.GetComponent<Fruit>();
        if (contectedFruit == null || contectedFruit.CanChange == false) return;

        if (CanMerge(contectedFruit.FruitType) == true)
        {
            contectedFruit.CanChange = false;
            Merage(contectedFruit);
        }
    }

    public bool CanMerge(Type fruitType)
    {
        return _fruitType == fruitType;
    }

    void Merage(Fruit fruit)
    {
        Vector3 contectedFruitPos = fruit.transform.position;

        OnSpawnRequested?.Invoke(_fruitType, contectedFruitPos);
        OnDestroyRequested?.Invoke(this, fruit);
        _canChange = false; // 2개 이상 바뀌는거 막기
    }
}
