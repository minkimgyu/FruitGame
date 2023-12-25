using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class DropState : State
{
    PlayerController _storedPlayer;
    public DropState(PlayerController player)
    {
        _storedPlayer = player;
    }

    public void DropFruit()
    {
        _storedPlayer.OnDropRequested?.Invoke
            (new Vector3(_storedPlayer.SpawnPoint.position.x, _storedPlayer.SpawnPoint.position.y, 0));
    }

    public override void OnStateEnter()
    {
        DropFruit();
        _storedPlayer.ActionFSM.SetState(PlayerController.ActionState.Delay);
        // 바로 딜레이로 이동
    }
}
