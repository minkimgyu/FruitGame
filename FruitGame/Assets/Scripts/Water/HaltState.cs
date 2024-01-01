using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class HaltState : State
{
    WaterController _storedWaterController;

    public HaltState(WaterController waterController)
    {
        _storedWaterController = waterController;
    }

    public override void OnStateEnter()
    {
        Debug.Log("OnStateEnter");
    }

    public override void OnStateExit()
    {
        Debug.Log("OnStateExit");
    }

    bool ExitFromHaltState()
    {
        bool IsGameOver = _storedWaterController.OnCheckGameOver();
        bool IsGamePause = _storedWaterController.OnCheckGamePause();
        bool isGameClear = _storedWaterController.OnCheckGameClear();

        return IsGameOver == false && isGameClear == false && IsGamePause == false;
    }

    public override void CheckStateChange()
    {
        if (ExitFromHaltState()) // 게임 오버 상태가 아니고 게임 클리어가 아니고 일시 정지가 풀린 경우
        {
            _storedWaterController.LevelFSM.RevertToPreviousState(); // 다시 이전 상태로 돌아감
        }
    }
}
