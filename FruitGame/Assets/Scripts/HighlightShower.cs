using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class HighlightShower : MonoBehaviour
{
    // 화면 어둡게 하고 오브젝트는 sortingLayer 바꿔주기
    // 3초 있다가 다시 원래 상태로 돌려주기

    // 딕셔너리로 타입에 따른 함수 실행해주게끔 해보기
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
        // 배경 색 변경
        // 이팩트 생성
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
        // 이벤트 보내서 Lock 걸어주기
        trickle.SpriteRender.sortingLayerName = originLayerName;
        ContinueGame?.Invoke();
    }

    public void ShowHighlightEffect(Trickle.Type type, Trickle trickle)
    {
        if (TaskByTrickleType.ContainsKey(type) == false) return;

        TaskByTrickleType[type]?.Invoke(trickle);
    }
}
