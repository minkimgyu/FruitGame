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

    public override void CheckStateChange()
    {
        if(Input.GetAxisRaw("Mouse X") != 0) // ���콺 ������ ���� ��
        {
            _storedPlayer.MovementFSM.SetState(PlayerController.MovementState.Move);
        }
    }
}
