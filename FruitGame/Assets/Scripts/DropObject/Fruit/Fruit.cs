using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FSM;

public class Fruit : BaseDropObject
{
    public enum Message
    {
        None,
        GoToLandAfterFalling,
        GoToLandAfterMerge,
        GoToLandAfterHighlight,
    }

    public enum PositionState
    {
        Init,
        Ready,
        Falling,
        Land,
        Highlight
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


    TrickleStateMachine<PositionState> _fSM;
    public TrickleStateMachine<PositionState> FSM { get { return _fSM; } }

    // Active되는 환경을 만들어주기
    // 예를 들어, 떨어져서 물에 닿은 경우 사운드를 주고 이후에는 사운드 없음 --> 이런 식

    //bool _contactToWater = false;
    //bool _spawnByMerge = true;

    public Action<Type, Vector3> OnSpawnRequested;
    public Action<Fruit, Fruit> OnDestroyRequested;
    public Action OnWaterDecreaseRequested;

    public void Initialize(Action<Type, Vector3> onSpawnRequested, Action<Fruit, Fruit> onDestroyRequested, Action onWaterDecreaseRequested)
    {
        OnSpawnRequested = onSpawnRequested;
        OnDestroyRequested = onDestroyRequested;
        OnWaterDecreaseRequested = onWaterDecreaseRequested;

        InitializeFSM();
    }

    void InitializeFSM()
    {
        Dictionary<PositionState, BaseState> positionStates = new Dictionary<PositionState, BaseState>();

        _fSM = new TrickleStateMachine<PositionState>();

        BaseState init = new InitState(this);
        BaseState below = new ReadyState(this);
        BaseState warning = new FallingState(this);
        BaseState above = new LandState(this);
        BaseState highlight = new HighlightState(this);

        positionStates.Add(PositionState.Init, init);
        positionStates.Add(PositionState.Ready, below);
        positionStates.Add(PositionState.Falling, warning);
        positionStates.Add(PositionState.Land, above);

        positionStates.Add(PositionState.Highlight, highlight);

        _fSM.Initialize(positionStates);
        _fSM.SetState(PositionState.Init);
    }

    public PositionState ReturnStateName() { return _fSM.CurrentStateName; }

    private void OnCollisionEnter2D(Collision2D collision) => _fSM.OnCollision2DEnter(collision);

    private void OnCollisionExit2D(Collision2D collision) => _fSM.OnCollision2DExit(collision);

    public override void OnSpawn() => _fSM.OnSpawn();

    public override void OnReady() => _fSM.OnReady();

    public override void OnDrop() => _fSM.OnDrop();

    public override void OnLand() => _fSM.OnLand();

    public override void OnHighlight() => _fSM.OnHighlight();
}
