using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using DG.Tweening;

public class WarningState : State
{
    WaterController _storedWaterController;
    

    public WarningState(WaterController waterController)
    {
        _storedWaterController = waterController;
    }

    public override void OnStateEnter()
    {
        _storedWaterController.ResetTransition();
    }

    public override void OnStateUpdate()
    {
        _storedWaterController.ChangeColor(_storedWaterController.WarningColor);
        _storedWaterController.UpdateYPos();
    }

    bool IsAboveMaxLine()
    {
        return _storedWaterController.transform.position.y >= _storedWaterController.MaxYPoint.position.y;
    }

    bool IsBelowWarningLine()
    {
        return _storedWaterController.transform.position.y <= _storedWaterController.WarningPoint.position.y;
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
        if (GoToHaltState()) // ���� ������
        {
            _storedWaterController.LevelFSM.SetState(WaterController.LevelState.Halt); // ��ȯ
        }

        if (IsAboveMaxLine())
        {
            _storedWaterController.LevelFSM.SetState(WaterController.LevelState.Above); // ��ȯ
        }
        else if(IsBelowWarningLine())
        {
            _storedWaterController.LevelFSM.SetState(WaterController.LevelState.Below); // ��ȯ
        }
    }
}
