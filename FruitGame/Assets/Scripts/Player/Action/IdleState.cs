using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class IdleState : State
{
    PlayerController _storedPlayer;

    public IdleState(PlayerController player)
    {
        _storedPlayer = player;
    }

    bool CanNotGoToNextState()
    {
        bool isGameClear = _storedPlayer.IsGameClear.Invoke();
        bool IsGamePause = _storedPlayer.IsGamePause.Invoke();
        bool IsGameOver = _storedPlayer.IsGameOver.Invoke();

        return IsGameOver || isGameClear || IsGamePause;
    }

    public override void CheckStateChange()
    {
        if (CanNotGoToNextState() == true) return;
        // 게임이 끝났거나 일시 정지 중일 경우 실행 중지

        // 마우스 클릭 감지 시 and 떨어뜨릴 과일이 있는 경우 
        // Input.GetMouseButtonDown(0)
        if (Input.GetKeyDown(KeyCode.Space) && _storedPlayer.OnCanDropRequested()) 
        {
            _storedPlayer.ActionFSM.SetState(PlayerController.ActionState.Drop);
        }
    }
}
