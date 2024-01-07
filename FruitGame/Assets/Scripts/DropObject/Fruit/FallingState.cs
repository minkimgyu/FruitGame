using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class FallingState : State
{
    Fruit _storedTrickle;
    float _gravityScale = 1f;
    string _fallingSortingLayerName = "Bowl";

    public FallingState(Fruit trickle)
    {
        _storedTrickle = trickle;
    }

    public override void OnStateEnter()
    {
        _storedTrickle.Rigid.gravityScale = _gravityScale;
        _storedTrickle.Collider.enabled = true;
        _storedTrickle.SpriteRender.sortingLayerName = _fallingSortingLayerName;
    }

    public override void OnCollision2DEnterRequested(Collision2D collision)
    {
        _storedTrickle.FSM.SetState(Fruit.PositionState.Land, Fruit.Message.GoToLandAfterFalling, collision);
    }
}
