using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class DelayState : State
{
    PlayerController _storedPlayer;

    Timer _delayUntilNextSpawnTimer;
    float _nextSpawnDelay = 3f;

    public DelayState(PlayerController player)
    {
        _storedPlayer = player;
        _delayUntilNextSpawnTimer = new Timer();
    }

    public override void OnStateEnter()
    {
        _delayUntilNextSpawnTimer.Start(_nextSpawnDelay);
        // 바로 딜레이로 이동
    }

    public override void CheckStateChange()
    {
        _delayUntilNextSpawnTimer.Update();

        if (_delayUntilNextSpawnTimer.IsFinish)
        {
            _delayUntilNextSpawnTimer.Reset();
            _storedPlayer.ActionFSM.SetState(PlayerController.ActionState.Spawn);
        }
    }
}
