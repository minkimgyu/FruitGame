using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class InitState : State
{
    Fruit _storedTrickle;

    public InitState(Fruit trickle)
    {
        _storedTrickle = trickle;
    }

    public override void OnReadyRequested()
    {
        _storedTrickle.FSM.SetState(Fruit.PositionState.Ready);
    }

    public override void OnLandRequested()
    {
        _storedTrickle.FSM.SetState(Fruit.PositionState.Land, Fruit.Message.GoToLandAfterMerge);
    }
}
