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

    public override void CheckStateChange()
    {
        if (_storedPlayer.IsGameOver() == true) return;

        // 마우스 클릭 감지 시 and 떨어뜨릴 과일이 있는 경우 
        if (Input.GetMouseButtonDown(0) && _storedPlayer.OnCanDropRequested()) 
        {
            _storedPlayer.ActionFSM.SetState(PlayerController.ActionState.Drop);
        }
    }
}
