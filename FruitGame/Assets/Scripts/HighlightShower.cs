using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class HighlightShower : MonoBehaviour
{
    // ȭ�� ��Ӱ� �ϰ� ������Ʈ�� sortingLayer �ٲ��ֱ�
    // 3�� �ִٰ� �ٽ� ���� ���·� �����ֱ�

    // ��ųʸ��� Ÿ�Կ� ���� �Լ� �������ְԲ� �غ���
    Dictionary<Fruit.Type, Action<BaseDropObject>> TaskByTrickleType;
    SpriteRenderer _backgroundSR;

    [SerializeField] Color _fadeColor;
    [SerializeField] Color _originColor;

    Action PauseGame;
    Action ContinueGame;

    [SerializeField] float _effectDuration = 3;
    [SerializeField] float _fadeDuration = 2;

    private void Start()
    {
        GameManager gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        if (gameManager == null) return;

        PauseGame = gameManager.PauseGame;
        ContinueGame = gameManager.ContinueGame;

        _backgroundSR = GetComponentInChildren<SpriteRenderer>();

        TaskByTrickleType = new Dictionary<Fruit.Type, Action<BaseDropObject>> {
            { Fruit.Type.Banana, (BaseDropObject trickle) => ShowEffect(trickle) }
        };
    }

    void ShowEffect(BaseDropObject trickle)
    {
        // ��� �� ����
        // ����Ʈ ����
        PauseGame?.Invoke();

        trickle.OnHighlight();
        _backgroundSR.DOColor(_fadeColor, _fadeDuration).onComplete = () => DoTaskAfterFade(trickle);
    }

    void DoTaskAfterFade(BaseDropObject trickle)
    {
        ParticleEffect effect = ObjectPooler.SpawnFromPool<ParticleEffect>("HighlightEffect");
        effect.Initialize(trickle.transform.position);
        effect.PlayEffect(_effectDuration);

        _backgroundSR.DOColor(_originColor, _fadeDuration).SetDelay(_effectDuration).onComplete = () => DoTaskAfterResetFade(trickle);
    }

    void DoTaskAfterResetFade(BaseDropObject trickle)
    {
        // �̺�Ʈ ������ Lock �ɾ��ֱ�
        trickle.OnLand();
        ContinueGame?.Invoke();
    }

    public void ShowHighlightEffect(Fruit.Type type, BaseDropObject trickle)
    {
        if (TaskByTrickleType.ContainsKey(type) == false) return;

        TaskByTrickleType[type]?.Invoke(trickle);
    }
}
