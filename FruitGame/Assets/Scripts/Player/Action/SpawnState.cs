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
        _storedPlayer.ActionFSM.SetState(PlayerController.ActionState.Idle);
    }
}
