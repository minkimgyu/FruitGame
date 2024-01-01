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
    Dictionary<Trickle.Type, Action<Trickle>> TaskByTrickleType;
    SpriteRenderer _backgroundSR;

    [SerializeField] Color _fadeColor;
    [SerializeField] Color _originColor;

    Action PauseGame;
    Action ContinueGame;

    [SerializeField] float _effectDuration = 3;
    [SerializeField] float _fadeDuration = 2;

    string _highLightLayerName = "Highlight";

    private void Start()
    {
        GameManager gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        if (gameManager == null) return;

        PauseGame = gameManager.PauseGame;
        ContinueGame = gameManager.ContinueGame;

        _backgroundSR = GetComponentInChildren<SpriteRenderer>();

        TaskByTrickleType = new Dictionary<Trickle.Type, Action<Trickle>> {
            { Trickle.Type.Lemon, (Trickle trickle) => ShowEffect(trickle) }
        };
    }

    void ShowEffect(Trickle trickle)
    {
        // ��� �� ����
        // ����Ʈ ����
        PauseGame?.Invoke();

        string originLayerName = trickle.SpriteRender.sortingLayerName;
        trickle.SpriteRender.sortingLayerName = _highLightLayerName;

        _backgroundSR.DOColor(_fadeColor, _fadeDuration).onComplete = () => DoTaskAfterFade(trickle, originLayerName);
    }

    void DoTaskAfterFade(Trickle trickle, string originLayerName)
    {
        ParticleEffect effect = ObjectPooler.SpawnFromPool<ParticleEffect>("HighlightEffect");
        effect.Initialize(trickle.transform.position);
        effect.PlayEffect(_effectDuration);

        _backgroundSR.DOColor(_originColor, _fadeDuration).SetDelay(_effectDuration).onComplete = () => DoTaskAfterResetFade(trickle, originLayerName);
    }

    void DoTaskAfterResetFade(Trickle trickle, string originLayerName)
    {
        // �̺�Ʈ ������ Lock �ɾ��ֱ�
        trickle.SpriteRender.sortingLayerName = originLayerName;
        ContinueGame?.Invoke();
    }

    public void ShowHighlightEffect(Trickle.Type type, Trickle trickle)
    {
        if (TaskByTrickleType.ContainsKey(type) == false) return;

        TaskByTrickleType[type]?.Invoke(trickle);
    }
}
