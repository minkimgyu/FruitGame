using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class LandState : State
{
    Trickle _storedTrickle;
    int _layerNum;
    bool _contactToWater = false;
    bool _comingFromFallingState = false;

    float _gravityScale = 1f;
    string _fallingSortingLayerName = "Bowl";

    public LandState(Trickle trickle)
    {
        _storedTrickle = trickle;
        _layerNum = LayerMask.NameToLayer("TrickleOnLand");
    }

    bool CanMerge(Trickle contectedFruit)
    {
        if (contectedFruit.FruitType != _storedTrickle.FruitType || contectedFruit.CanChange == false) return false;

        return true;
    }

    void StartMerge(Collision2D collision)
    {
        if (_storedTrickle.CanChange == false || collision.gameObject.CompareTag("Trickle") == false) return;

        Trickle contectedFruit = collision.gameObject.GetComponent<Trickle>();
        if (contectedFruit == null) return;

        if (CanMerge(contectedFruit) == false) return;

        Merage(contectedFruit, collision);
    }

    // �޽����� ���� ���� Ȥ�� �ݸ����� ���� ������ �ľ��ؼ� Merge�� �������ش�.
    // ����� ������ �ݸ��� �̺�Ʈ�� ���� ����

    public override void OnMessageReceived(Trickle.Message message, Collision2D collision)
    {
        if (message != Trickle.Message.ContactWhenFalling) return;
        _comingFromFallingState = true;

        ContactToWater(collision);

        StartMerge(collision);
    }

    // ����� ������ �ݸ��� �̺�Ʈ�� ���� ����
    public override void OnStateCollision2DEnter(Collision2D collision)
    {
        ContactToWater(collision);

        StartMerge(collision); 
    }

    void ContactToWater(Collision2D collision) // �̺�Ʈ�� ��밡 �ƴ� ���� ����Ű�Բ� �ϴ°� �´µ�
    {
        if (collision.gameObject.CompareTag("Spring") == false || _contactToWater == true || _comingFromFallingState == false) return;

        _contactToWater = true;
        SoundManager.Instance.PlaySFX("WaterDrop");

        ParticleEffect mergeEffect = ObjectPooler.SpawnFromPool<ParticleEffect>("DropEffect");
        mergeEffect.Initialize(collision.contacts[0].point);
        mergeEffect.PlayEffect();
    }

    void Merage(Trickle contectedFruit, Collision2D collision)
    {
        ParticleEffect effect = ObjectPooler.SpawnFromPool<ParticleEffect>("MergeEffect");
        effect.Initialize(collision.contacts[0].point);
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
    }
}
