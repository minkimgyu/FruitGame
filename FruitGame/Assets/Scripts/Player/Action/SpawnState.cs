using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class SpawnState : State
{
    PlayerController _storedPlayer;

    public SpawnState(PlayerController player)
    {
        _storedPlayer = player;
    }

    public override void OnStateEnter()
    {
        _storedPlayer.OnSpawnRequested();

        bool canGameOver = _storedPlayer.IsFruitYPosAboveLine(_storedPlayer.EndPoint.position.y);

        if (canGameOver == true && _storedPlayer?.IsGameOver() == false)
        {
            _storedPlayer?.OnGameOverRequested(); // 게임 종료
        }

        _storedPlayer.ActionFSM.SetState(PlayerController.ActionState.Idle);
    }
}
