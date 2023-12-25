using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FSM;

public class Trickle : MonoBehaviour
{
    public enum Message
    {
        SendTrickleForMerage,
    }

    public enum State
    {
        Idle,
        Ready,
        Merge,
        Falling
    }

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

    [SerializeField] SpriteRenderer _spriteRender;

    [SerializeField] string _readySortingLayerName = "Cloud";

    [SerializeField] string _actionSortingLayerName = "Bowl";

    [SerializeField] Type _fruitType;
    public Type FruitType { get { return _fruitType; }}

    [SerializeField] bool _canChange = true;
    public bool CanChange { get { return _canChange; } set { _canChange = value; } }

    [SerializeField] Rigidbody2D _rigid;
    public Rigidbody2D Rigid { get { return _rigid; }}

    [SerializeField] CapsuleCollider2D _collider;
    public CapsuleCollider2D Collider { get { return _collider; } }


    Action<Type, Vector3> OnSpawnRequested;
    Action<Trickle, Trickle> OnDestroyRequested;
    Action OnWaterDecreaseRequested;

    public void Initialize(Action<Type, Vector3> onSpawnRequested, Action<Trickle, Trickle> onDestroyRequested, Action onWaterDecreaseRequested)
    {
        OnSpawnRequested = onSpawnRequested;
        OnDestroyRequested = onDestroyRequested;
        OnWaterDecreaseRequested = onWaterDecreaseRequested;
    }

    public void OnChangeGravityScaleRequested(bool nowColliderEnable, float gravityScale)
    {
        _rigid.gravityScale = gravityScale;
        _collider.enabled = nowColliderEnable;

        if(nowColliderEnable) _spriteRender.sortingLayerName = _actionSortingLayerName;
        else _spriteRender.sortingLayerName = _readySortingLayerName;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CanChange == false) return; 
        if (collision.gameObject.CompareTag("Trickle") == false) return;

        Trickle contectedFruit = collision.gameObject.GetComponent<Trickle>();
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

    void Merage(Trickle fruit)
    {
        Vector3 contectedFruitPos = fruit.transform.position;

        OnSpawnRequested?.Invoke(_fruitType, contectedFruitPos);
        OnDestroyRequested?.Invoke(this, fruit);
        OnWaterDecreaseRequested?.Invoke();

        _canChange = false; // 2개 이상 바뀌는거 막기
    }
}
