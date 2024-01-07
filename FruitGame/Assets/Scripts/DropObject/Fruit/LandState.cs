using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class LandState : State
{
    Fruit _storedTrickle;
    int _layerNum;

    Collision2D _collisionFromMessage;
    Fruit _fruitFromMessage;
    Vector3 _contactPosFromMessage;

    float _gravityScale = 1f;
    string _fallingSortingLayerName = "Bowl";
    bool _dontSpawnWaterEffect = false;

    Fruit.Message _message;

    public LandState(Fruit trickle)
    {
        _storedTrickle = trickle;
        _layerNum = LayerMask.NameToLayer("TrickleOnLand");
    }

    bool CanMerge(Fruit contectedFruit)
    {
        if (contectedFruit.ReturnStateName() != Fruit.PositionState.Land) return false; // ��뵵 LandState���� ��

        if (contectedFruit.FruitType != _storedTrickle.FruitType || contectedFruit.CanChange == false) return false;

        return true;
    }

    void StartMerge(Collision2D collision)
    {
        if (_storedTrickle.CanChange == false || collision.gameObject.CompareTag("Trickle") == false) return;

        Fruit contectedFruit = collision.gameObject.GetComponent<Fruit>();
        if (contectedFruit == null) return;

        if (CanMerge(contectedFruit) == false) return;

        Merage(contectedFruit, collision.contacts[0].point);
    }

    void StartMerge(Fruit fruit, Vector3 contactPos)
    {
        if (_storedTrickle.CanChange == false) return;
        if (fruit == null) return;

        if (CanMerge(fruit) == false) return;

        Merage(fruit, contactPos);
    }

    // �޽����� ���� ���� Ȥ�� �ݸ����� ���� ������ �ľ��ؼ� Merge�� �������ش�.
    // ����� ������ �ݸ��� �̺�Ʈ�� ���� ����

    public override void OnMessageReceived(Fruit.Message message, Fruit fruit, Vector3 contactPos)
    {
        _message = message;
        _fruitFromMessage = fruit; // �޼����� �������� �ڵ� ��������ֱ�
        _contactPosFromMessage = contactPos;
    }

    public override void OnMessageReceived(Fruit.Message message, Collision2D collision)
    {
        _message = message;
        _collisionFromMessage = collision; // �޼����� �������� �ڵ� ��������ֱ�
    }

    public override void OnMessageReceived(Fruit.Message message)
    {
        _message = message;
    }

    public override void OnHighlightRequested()
    {
        _storedTrickle.FSM.SetState(Fruit.PositionState.Highlight); // FallingState�� �̵�
    }


    // ����� ������ �ݸ��� �̺�Ʈ�� ���� ����
    public override void OnCollision2DEnterRequested(Collision2D collision)
    {
        ContactToWater(collision);
        StartMerge(collision); 
    }

    void ContactToWater(Collision2D collision) // �̺�Ʈ�� ��밡 �ƴ� ���� ����Ű�Բ� �ϴ°� �´µ�
    {
        if (collision.gameObject.CompareTag("Spring") == false || _dontSpawnWaterEffect == true) return;

        _dontSpawnWaterEffect = true;
        SoundManager.Instance.PlaySFX("WaterDrop");

        ParticleEffect mergeEffect = ObjectPooler.SpawnFromPool<ParticleEffect>("DropEffect");
        mergeEffect.Initialize(collision.contacts[0].point);
        mergeEffect.PlayEffect();
    }

    void Merage(Fruit contectedFruit, Vector3 contactPoint)
    {
        ParticleEffect effect = ObjectPooler.SpawnFromPool<ParticleEffect>("MergeEffect");
        effect.Initialize(contactPoint);
        effect.PlayEffect();

        contectedFruit.CanChange = false;
        _storedTrickle.CanChange = false; // 2�� �̻� �ٲ�°� ����

        SoundManager.Instance.PlaySFX("BubblePop");

        _storedTrickle.OnSpawnRequested?.Invoke(_storedTrickle.FruitType, contectedFruit.transform.position);
        _storedTrickle.OnDestroyRequested?.Invoke(_storedTrickle, contectedFruit);
        _storedTrickle.OnWaterDecreaseRequested?.Invoke();
    }

    public override void OnStateEnter()
    {
        _storedTrickle.gameObject.layer = _layerNum; // ���� ������ �浹������ ���̾�� ��������ֱ�
        _storedTrickle.Rigid.gravityScale = _gravityScale;
        _storedTrickle.Collider.enabled = true;
        _storedTrickle.SpriteRender.sortingLayerName = _fallingSortingLayerName;
        _dontSpawnWaterEffect = false; // �ʱ⿡�� ����Ʈ�� ���� �� �ְ� �������ֱ�

        if (_message == Fruit.Message.None) return;

        if (_message == Fruit.Message.GoToLandAfterFalling)// GoToLandAfterFalling�� ��츸 ��������ֱ�
        {
            ContactToWater(_collisionFromMessage); 
            // --> �� �κ��� ���� ���� ���� ��� �κ��� spring�� ���ư� ��
            // ������ ������ ����Ʈ �� ����
            // ���������� �ݸ����� ���� �� FallingState�� �׷�


            StartMerge(_collisionFromMessage);
        }
        else if(_message == Fruit.Message.GoToLandAfterHighlight) // �� ���� ���� ������ ���� �����
        {
            _dontSpawnWaterEffect = true;
            StartMerge(_fruitFromMessage, _contactPosFromMessage);
        }
        else if (_message == Fruit.Message.GoToLandAfterMerge) // �� ���� ���� ������ ���� �����
        {
            _dontSpawnWaterEffect = true;
        }

        ResetMessage();
    }

    void ResetMessage()
    {
        _collisionFromMessage = null;
        _fruitFromMessage = null;
        _message = Fruit.Message.None;
    }
}
