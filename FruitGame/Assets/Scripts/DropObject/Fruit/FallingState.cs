using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class FallingState : State
{
    Trickle _storedTrickle;
    float _gravityScale = 1f;
    string _fallingSortingLayerName = "Bowl";

    public FallingState(Trickle trickle)
    {
        _storedTrickle = trickle;
    }

    public override void OnStateEnter()
    {
        _storedTrickle.Rigid.gravityScale = _gravityScale;
        _storedTrickle.Collider.enabled = true;
        _storedTrickle.SpriteRender.sortingLayerName = _fallingSortingLayerName;
    }

    public override void OnStateCollision2DEnter(Collision2D collision)
    {
        _storedTrickle.PositionFSM.
            SetState(Trickle.PositionState.Land, Trickle.Message.ContactWhenFalling, collision);
    }
}
