using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class ReadyState : State
{
    Fruit _storedTrickle;
    float _gravityScale = 0f;
    string _readySortingLayerName = "Cloud";

    public ReadyState(Fruit trickle)
    {
        _storedTrickle = trickle;
    }

    public override void OnStateEnter()
    {
        _storedTrickle.Rigid.gravityScale = _gravityScale;
        _storedTrickle.Collider.enabled = false;
        _storedTrickle.SpriteRender.sortingLayerName = _readySortingLayerName;
    }

    public override void OnDropRequested()
    {
        _storedTrickle.FSM.SetState(Fruit.PositionState.Falling); // FallingState�� �̵�
    }
}
