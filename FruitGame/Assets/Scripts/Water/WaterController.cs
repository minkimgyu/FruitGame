using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FSM;
using UnityEngine.U2D;

public class WaterController : MonoBehaviour
{
    public enum LevelState
    {
        Halt,
        Below,
        Warning,
        Above
    }

    [SerializeField] float _yPos;

    [SerializeField] float _offset = 2f;
    [SerializeField] float _upSpeed = 5f;

    [SerializeField] float _decreaseAmound = 2f;

    [SerializeField] Transform _maxYPoint;
    public Transform MaxYPoint { get { return _maxYPoint; } }

    [SerializeField] Transform _minYPoint;

    [SerializeField] Transform _warningPoint;
    public Transform WarningPoint { get { return _warningPoint; } }

    SpriteShapeRenderer _waterSprite;
    public SpriteShapeRenderer WaterSprite { get { return _waterSprite; } }

    // merge 시 이벤트 보내서 물 빼기
    // 물이 경계를 넘으면 게임이 끝나게 만들어주기

    // 일반적인 상태에서는 물 계속 채움

    [SerializeField] Color _belowColor;
    public Color BelowColor { get { return _belowColor; } }

    [SerializeField] Color _warningColor;
    public Color WarningColor { get { return _warningColor; } }

    public Action OnGameOverRequested;
    public Func<bool> OnCheckGameOver;

    public Action OnGamePauseRequested;
    public Func<bool> OnCheckGamePause;
    public Func<bool> OnCheckGameClear;

    float _targetPoint;
    float _speed = 0.5f;

    StateMachine<LevelState> _levelFSM;
    public StateMachine<LevelState> LevelFSM { get { return _levelFSM; } }

    // Start is called before the first frame update
    void Start()
    {
        _yPos = _minYPoint.position.y;

        GameManager gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        if (gameManager == null) return;

        OnGameOverRequested = gameManager.GameOver;
        OnCheckGameOver = gameManager.IsGameOver;

        OnGamePauseRequested = gameManager.PauseGame;
        OnCheckGamePause = gameManager.IsPauseGame;
        OnCheckGameClear = gameManager.IsGameClear;

        _waterSprite = GetComponentInChildren<SpriteShapeRenderer>();

        InitializeFSM();
    }

    void InitializeFSM()
    {
        Dictionary<LevelState, BaseState> levelStates = new Dictionary<LevelState, BaseState>();

        _levelFSM = new StateMachine<LevelState>();

        BaseState halt = new HaltState(this);

        BaseState below = new BelowState(this);
        BaseState warning = new WarningState(this);
        BaseState above = new AboveState(this);

        levelStates.Add(LevelState.Halt, halt);
        levelStates.Add(LevelState.Below, below);
        levelStates.Add(LevelState.Warning, warning);
        levelStates.Add(LevelState.Above, above);

        _levelFSM.Initialize(levelStates);
        _levelFSM.SetState(LevelState.Below);
    }

    public void ResetTransition()
    {
        _targetPoint = 0;
    }

    public void ChangeColor(Color colorToChange)
    {
        if (_targetPoint > 1) return;

        _targetPoint += Time.deltaTime * _speed;
        WaterSprite.color = Color.Lerp(WaterSprite.color, colorToChange, _targetPoint);
    }

    public void OnWaterDecreaseRequested()
    {
        _yPos -= _decreaseAmound;
        if (_yPos < _minYPoint.position.y) _yPos = _minYPoint.position.y;
    }

    public void UpdateYPos()
    {
        _yPos += _offset * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, _yPos, 0), Time.deltaTime * _upSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        _levelFSM.OnUpdate();
    }
}
