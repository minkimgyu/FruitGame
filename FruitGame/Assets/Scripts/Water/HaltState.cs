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
        if (ExitFromHaltState()) // ���� ���� ���°� �ƴϰ� ���� Ŭ��� �ƴϰ� �Ͻ� ������ Ǯ�� ���
        {
            _storedWaterController.LevelFSM.RevertToPreviousState(); // �ٽ� ���� ���·� ���ư�
        }
    }
}
