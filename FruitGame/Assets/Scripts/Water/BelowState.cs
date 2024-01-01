using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class BelowState : State
{
    WaterController _storedWaterController;

    public BelowState(WaterController waterController)
    {
        _storedWaterController = waterController;
    }

    public override void OnStateEnter()
    {
        _storedWaterController.ResetTransition();
    }

    public override void OnStateUpdate()
    {
        _storedWaterController.ChangeColor(_storedWaterController.BelowColor);
        _storedWaterController.UpdateYPos();
    }

    bool IsAboveWarningLine()
    {
        return _storedWaterController.transform.position.y > _storedWaterController.WarningPoint.position.y;
    }

    bool GoToHaltState()
    {
        bool IsGameOver = _storedWaterController.OnCheckGameOver();
        bool IsGamePause = _storedWaterController.OnCheckGamePause();
        bool isGameClear = _storedWaterController.OnCheckGameClear();

        return IsGameOver || isGameClear || IsGamePause;
    }

    public override void CheckStateChange()
    {
        if (GoToHaltState()) // 게임 오버시
        {
            _storedWaterController.LevelFSM.SetState(WaterController.LevelState.Halt); // 전환
        }

        if (IsAboveWarningLine())
        {
            _storedWaterController.LevelFSM.SetState(WaterController.LevelState.Warning); // 전환
        }
    }
}
