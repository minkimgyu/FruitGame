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
        if (contectedFruit.ReturnStateName() != Fruit.PositionState.Land) return false; // 상대도 LandState여야 함

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

    // 메시지를 받은 시점 혹은 콜리젼이 들어온 시점을 파악해서 Merge를 시작해준다.
    // 여기는 최초의 콜리전 이벤트가 들어올 예정

    public override void OnMessageReceived(Fruit.Message message, Fruit fruit, Vector3 contactPos)
    {
        _message = message;
        _fruitFromMessage = fruit; // 메세지를 바탕으로 코드 실행시켜주기
        _contactPosFromMessage = contactPos;
    }

    public override void OnMessageReceived(Fruit.Message message, Collision2D collision)
    {
        _message = message;
        _collisionFromMessage = collision; // 메세지를 바탕으로 코드 실행시켜주기
    }

    public override void OnMessageReceived(Fruit.Message message)
    {
        _message = message;
    }

    public override void OnHighlightRequested()
    {
        _storedTrickle.FSM.SetState(Fruit.PositionState.Highlight); // FallingState로 이동
    }


    // 여기는 이후의 콜리전 이벤트가 들어올 예정
    public override void OnCollision2DEnterRequested(Collision2D collision)
    {
        ContactToWater(collision);
        StartMerge(collision); 
    }

    void ContactToWater(Collision2D collision) // 이벤트를 상대가 아닌 내가 일으키게끔 하는게 맞는듯
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
        _storedTrickle.CanChange = false; // 2개 이상 바뀌는거 막기

        SoundManager.Instance.PlaySFX("BubblePop");

        _storedTrickle.OnSpawnRequested?.Invoke(_storedTrickle.FruitType, contectedFruit.transform.position);
        _storedTrickle.OnDestroyRequested?.Invoke(_storedTrickle, contectedFruit);
        _storedTrickle.OnWaterDecreaseRequested?.Invoke();
    }

    public override void OnStateEnter()
    {
        _storedTrickle.gameObject.layer = _layerNum; // 게임 오버에 충돌가능한 레이어로 변경시켜주기
        _storedTrickle.Rigid.gravityScale = _gravityScale;
        _storedTrickle.Collider.enabled = true;
        _storedTrickle.SpriteRender.sortingLayerName = _fallingSortingLayerName;
        _dontSpawnWaterEffect = false; // 초기에는 이펙트가 나올 수 있게 지정해주기

        if (_message == Fruit.Message.None) return;

        if (_message == Fruit.Message.GoToLandAfterFalling)// GoToLandAfterFalling인 경우만 진행시켜주기
        {
            ContactToWater(_collisionFromMessage); 
            // --> 이 부분은 보통 가장 먼저 닿는 부분이 spring라서 돌아갈 듯
            // 문제는 없으면 이펙트 안 나옴
            // 실질적으로 콜리젼이 들어온 게 FallingState라서 그럼


            StartMerge(_collisionFromMessage);
        }
        else if(_message == Fruit.Message.GoToLandAfterHighlight) // 이 경우는 같은 종류에 닿은 경우임
        {
            _dontSpawnWaterEffect = true;
            StartMerge(_fruitFromMessage, _contactPosFromMessage);
        }
        else if (_message == Fruit.Message.GoToLandAfterMerge) // 이 경우는 같은 종류에 닿은 경우임
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
