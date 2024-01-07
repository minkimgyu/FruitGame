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
        // �ش� State�� ������ Merge �ߴ� 
        // Ȥ�ó� ���� Ÿ���� ������Ʈ�� ����ִ� ��� �̰Ÿ� LandState�� ���ư� �� �������ش�. 
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
        _storedTrickle.FSM.SetState(Fruit.PositionState.Land, Fruit.Message.GoToLandAfterHighlight); // ���� ���·� ���ư�
    }
}
