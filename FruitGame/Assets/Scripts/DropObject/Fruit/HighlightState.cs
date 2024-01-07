using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class HighlightState : State
{
    Fruit _storedTrickle;
    string _highLightLayerName = "Highlight";
    float _gravityScale = 0f;

    public HighlightState(Fruit trickle)
    {
        _storedTrickle = trickle;
    }

    public override void OnStateEnter()
    {
        // 해당 State로 들어오면 Merge 중단 
        // 혹시나 같은 타입의 오브젝트가 닿아있는 경우 이거를 LandState로 돌아갈 때 전달해준다. 
        _storedTrickle.SpriteRender.sortingLayerName = _highLightLayerName;
        _storedTrickle.Collider.enabled = false;
        _storedTrickle.Rigid.gravityScale = _gravityScale;

        ResetConstraints(true);
    }

    public override void OnStateExit() => ResetConstraints(false);

    void ResetConstraints(bool nowFix)
    {
        if (nowFix) _storedTrickle.Rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        else _storedTrickle.Rigid.constraints = RigidbodyConstraints2D.None;
    }

    public override void OnLandRequested()
    {
        _storedTrickle.FSM.SetState(Fruit.PositionState.Land, Fruit.Message.GoToLandAfterHighlight); // 이전 상태로 돌아감
    }
}
