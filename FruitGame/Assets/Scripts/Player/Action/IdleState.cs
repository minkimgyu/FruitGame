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

        // ���콺 Ŭ�� ���� �� and ����߸� ������ �ִ� ��� 
        if (Input.GetMouseButtonDown(0) && _storedPlayer.OnCanDropRequested()) 
        {
            _storedPlayer.ActionFSM.SetState(PlayerController.ActionState.Drop);
        }
    }
}
