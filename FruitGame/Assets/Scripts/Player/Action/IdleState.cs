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
        // ������ �����ų� �Ͻ� ���� ���� ��� ���� ����

        // ���콺 Ŭ�� ���� �� and ����߸� ������ �ִ� ��� 
        // Input.GetMouseButtonDown(0)
        if (Input.GetKeyDown(KeyCode.Space) && _storedPlayer.OnCanDropRequested()) 
        {
            _storedPlayer.ActionFSM.SetState(PlayerController.ActionState.Drop);
        }
    }
}
