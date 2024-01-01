using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FSM;

public class Trickle : MonoBehaviour
{
    public enum Message
    {
        ContactWhenFalling
    }

    public enum PositionState
    {
        Ready,
        Falling,
        Land
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
    }

    [SerializeField] SpriteRenderer _spriteRender;
    public SpriteRenderer SpriteRender { get { return _spriteRender; } }

    [SerializeField] Type _fruitType;
    public Type FruitType { get { return _fruitType; }}

    [SerializeField] bool _canChange = true;
    public bool CanChange { get { return _canChange; } set { _canChange = value; } }

    [SerializeField] Rigidbody2D _rigid;
    public Rigidbody2D Rigid { get { return _rigid; }}

    [SerializeField] CapsuleCollider2D _collider;
    public CapsuleCollider2D Collider { get { return _collider; } }


    StateMachine<PositionState> _positionFSM;
    public StateMachine<PositionState> PositionFSM { get { return _positionFSM; } }

    // Active되는 환경을 만들어주기
    // 예를 들어, 떨어져서 물에 닿은 경우 사운드를 주고 이후에는 사운드 없음 --> 이런 식

    //bool _contactToWater = false;
    //bool _spawnByMerge = true;

    public Action<Type, Vector3> OnSpawnRequested;
    public Action<Trickle, Trickle> OnDestroyRequested;
    public Action OnWaterDecreaseRequested;

    public void Initialize(Action<Type, Vector3> onSpawnRequested, Action<Trickle, Trickle> onDestroyRequested, Action onWaterDecreaseRequested)
    {
        OnSpawnRequested = onSpawnRequested;
        OnDestroyRequested = onDestroyRequested;
        OnWaterDecreaseRequested = onWaterDecreaseRequested;

        InitializeFSM();
    }

    void InitializeFSM()
    {
        Dictionary<PositionState, BaseState> positionStates = new Dictionary<PositionState, BaseState>();

        _positionFSM = new StateMachine<PositionState>();

        BaseState below = new ReadyState(this);
        BaseState warning = new FallingState(this);
        BaseState above = new LandState(this);

        positionStates.Add(PositionState.Ready, below);
        positionStates.Add(PositionState.Falling, warning);
        positionStates.Add(PositionState.Land, above);

        _positionFSM.Initialize(positionStates);
    }

    public void ResetState(PositionState state)
    {
        _positionFSM.SetState(state);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _positionFSM.OnCollision2DEnter(collision);
    }
}
