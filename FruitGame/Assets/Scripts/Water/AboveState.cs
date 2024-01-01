using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class AboveState : State
{
    WaterController _storedWaterController;

    public AboveState(WaterController waterController)
    {
        _storedWaterController = waterController;
    }

    public override void OnStateEnter()
    {
        _storedWaterController.OnGameOverRequested?.Invoke(); // 게임 종료
    }
}
