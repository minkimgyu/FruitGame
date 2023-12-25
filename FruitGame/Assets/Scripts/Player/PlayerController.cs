using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FSM;

public class PlayerController : MonoBehaviour
{
    public enum MovementState
    {
        Stop,
        Move,
    }

    public enum ActionState
    {
        Idle,
        Drop,
        Delay,
        Spawn
    }

    [SerializeField] Transform _endPoint; // 최종 라인
    public Transform EndPoint { get { return _endPoint; } }

    [SerializeField] Transform _spawnPoint;
    public Transform SpawnPoint { get { return _spawnPoint; } }

    Transform _cloud;
    public Transform Cloud { get { return _cloud; } }

    [SerializeField] Transform _minX;
    public float MinX { get { return _minX.position.x; } }

    [SerializeField] Transform _maxX;
    public float MaxX { get { return _maxX.position.x; } }

    [SerializeField] float _dropGravityScale;

    StateMachine<MovementState> _movementFSM;
    public StateMachine<MovementState> MovementFSM { get { return _movementFSM; }}

    StateMachine<ActionState> _actionFSM;
    public StateMachine<ActionState> ActionFSM { get { return _actionFSM; } }

    public Func<bool> OnCanDropRequested;
    public Action<Vector3> OnDropRequested;
    public Action OnSpawnRequested;
    public Func<float, bool> IsFruitYPosAboveLine;

    public Action OnGameOverRequested;
    public Func<bool> IsGameOver;

    // Start is called before the first frame update
    void Start()
    {
        _cloud = transform;

        GameManager gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        if (gameManager == null) return;

        OnGameOverRequested = gameManager.GameOver;
        IsGameOver = gameManager.IsGameOver;

         Spawner spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
        if (spawner == null) return;

        OnDropRequested = spawner.DropNextFruit;
        OnSpawnRequested = spawner.SpawnNextDropFruit;
        OnCanDropRequested = spawner.CanDropNextFruit;
        IsFruitYPosAboveLine = spawner.IsFruitYPosAboveLine;

        InitializeFSM();

        OnSpawnRequested?.Invoke(); // 하나 미리 스폰시켜주기
    }

    void InitializeFSM()
    {
        Dictionary<MovementState, BaseState> moveStates = new Dictionary<MovementState, BaseState>();

        _movementFSM = new StateMachine<MovementState>();

        BaseState stop = new StopState(this);
        BaseState move = new MoveState(this);

        moveStates.Add(MovementState.Stop, stop);
        moveStates.Add(MovementState.Move, move);

        _movementFSM.Initialize(moveStates);
        _movementFSM.SetState(MovementState.Stop);


        Dictionary<ActionState, BaseState> actionStates = new Dictionary<ActionState, BaseState>();

        _actionFSM = new StateMachine<ActionState>();

        BaseState idle = new IdleState(this);
        BaseState drop = new DropState(this);
        BaseState delay = new DelayState(this);
        BaseState spawn = new SpawnState(this);

        actionStates.Add(ActionState.Idle, idle);
        actionStates.Add(ActionState.Drop, drop);
        actionStates.Add(ActionState.Delay, delay);
        actionStates.Add(ActionState.Spawn, spawn);

        _actionFSM.Initialize(actionStates);
        _actionFSM.SetState(ActionState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        _movementFSM.OnUpdate();
        _actionFSM.OnUpdate();
    }
}
