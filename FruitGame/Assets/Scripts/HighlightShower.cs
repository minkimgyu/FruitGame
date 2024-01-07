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
        // 배경 색 변경
        // 이팩트 생성
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
        // 이벤트 보내서 Lock 걸어주기
        trickle.OnLand();
        ContinueGame?.Invoke();
    }

    public void ShowHighlightEffect(Fruit.Type type, BaseDropObject trickle)
    {
        if (TaskByTrickleType.ContainsKey(type) == false) return;

        TaskByTrickleType[type]?.Invoke(trickle);
    }
}
