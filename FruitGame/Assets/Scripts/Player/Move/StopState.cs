using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class StopState : State
{
    PlayerController _storedPlayer;

    public StopState(PlayerController player)
    {
        _storedPlayer = player;
    }

    bool GoToMoveState()
    {
        bool isGameClear = _storedPlayer.IsGameClear.Invoke();
        bool IsGamePause = _storedPlayer.IsGamePause.Invoke();
        bool IsGameOver = _storedPlayer.IsGameOver.Invoke();
        bool nowMouseMove = Input.GetAxisRaw("Mouse X") != 0;

        return IsGameOver == false && isGameClear == false && IsGamePause == false && nowMouseMove == true;
    }

    public override void CheckStateChange()
    {
        if(GoToMoveState()) // 마우스 움직임 감지 시
        {
            _storedPlayer.MovementFSM.SetState(PlayerController.MovementState.Move);
        }
    }
}
