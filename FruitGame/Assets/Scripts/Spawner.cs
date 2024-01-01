using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] TMP_Text _scoreTxt;

    [SerializeField] List<Trickle> fruitPrefabs;

    [SerializeField] List<Trickle> spawnedFruits;

    [SerializeField] Transform _nextFruitSpawnPoint;
    [SerializeField] Trickle _nextDropFruit;

    [SerializeField] Trickle.Type _nowMaxSpawnType = Trickle.Type.Cherry;


    int score = 0;

    [SerializeField] Dictionary<Trickle.Type, Trickle.Type[]> _gameRule = new Dictionary<Trickle.Type, Trickle.Type[]>()
    {
        { Trickle.Type.Cherry,
            new Trickle.Type[]{ Trickle.Type.Cherry } },

        { Trickle.Type.Strawberry,
            new Trickle.Type[]{ Trickle.Type.Cherry, Trickle.Type.Strawberry } },

        { Trickle.Type.Grape,
            new Trickle.Type[]{ Trickle.Type.Cherry, Trickle.Type.Strawberry, Trickle.Type.Grape } },

        { Trickle.Type.Lemon,
            new Trickle.Type[]{ Trickle.Type.Cherry, Trickle.Type.Strawberry, Trickle.Type.Grape, Trickle.Type.Lemon } },

        { Trickle.Type.Orange,
            new Trickle.Type[]{ Trickle.Type.Cherry, Trickle.Type.Strawberry, Trickle.Type.Grape, Trickle.Type.Lemon } },

        { Trickle.Type.Apple,
            new Trickle.Type[]{ Trickle.Type.Cherry, Trickle.Type.Strawberry, Trickle.Type.Grape, Trickle.Type.Lemon} },

        { Trickle.Type.Pear,
            new Trickle.Type[]{ Trickle.Type.Cherry, Trickle.Type.Strawberry, Trickle.Type.Grape, Trickle.Type.Lemon} },

        { Trickle.Type.Watermelon,
            new Trickle.Type[]{ Trickle.Type.Cherry, Trickle.Type.Strawberry, Trickle.Type.Grape, Trickle.Type.Lemon} },

        { Trickle.Type.Banana,
            new Trickle.Type[]{ Trickle.Type.Cherry, Trickle.Type.Strawberry, Trickle.Type.Grape, Trickle.Type.Lemon} },
    };

    [SerializeField]
    Dictionary<Trickle.Type, int> _gameScore = new Dictionary<Trickle.Type, int>()
    {
        { Trickle.Type.Cherry, 1},

        { Trickle.Type.Strawberry, 2},

        { Trickle.Type.Grape, 3},

        { Trickle.Type.Lemon, 4},

        { Trickle.Type.Orange, 5},

        { Trickle.Type.Apple, 6},

        { Trickle.Type.Pear, 7},

        { Trickle.Type.Watermelon, 8},

        { Trickle.Type.Banana, 9},
    };

    Action OnGameClearRequested;

    Action OnWaterDecreaseRequested;

    Action<Trickle.Type, Trickle> OnShowHighlightRequested;

    private void Start()
    {
        WaterController waterController = GameObject.FindWithTag("WaterController").GetComponent<WaterController>();
        if (waterController == null) return;

        OnWaterDecreaseRequested = waterController.OnWaterDecreaseRequested;


        HighlightShower highlightShower = GameObject.FindWithTag("HighlightShower").GetComponent<HighlightShower>();
        if (highlightShower == null) return;

        OnShowHighlightRequested = highlightShower.ShowHighlightEffect;

        GameManager gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        if (gameManager == null) return;

        OnGameClearRequested = gameManager.GameClear;
    }

    //public bool IsFruitYPosAboveLine(float endYPos)
    //{
    //    for (int i = 0; i < spawnedFruits.Count; i++)
    //    {
    //        if(spawnedFruits[i].transform.position.y > endYPos)
    //        {
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    public void CheckGameClear()
    {
        int bananaCount = 0;

        for (int i = 0; i < spawnedFruits.Count; i++)
        {
            if (spawnedFruits[i].FruitType == Trickle.Type.Banana)
            {
                bananaCount++;
                if(bananaCount >= 2) OnGameClearRequested?.Invoke();
            }
        }
    }

    public Trickle.Type ReturnRandomSpawnType()
    {
        Debug.Log(_nowMaxSpawnType);

        Trickle.Type[] tmpTypes = _gameRule[_nowMaxSpawnType];
        int randomIndex = Random.Range(0, tmpTypes.Length);

        return tmpTypes[randomIndex];
    }

    void SetMaxSpawnFruitType(Trickle.Type type)
    {
        int getTypeToInt = (int)type;
        int storedTypeToInt = (int)_nowMaxSpawnType;

        if(getTypeToInt > storedTypeToInt) _nowMaxSpawnType = type;
    }

    Trickle ReturnFruitUsingType(Trickle.Type type)
    {
        return fruitPrefabs.Find(x => x.FruitType == type);
    }

    public void SpawnNextDropFruit()
    {
        Trickle.Type type = ReturnRandomSpawnType();

        Trickle fruit = SpawnFruit(type, _nextFruitSpawnPoint.position);
        _nextDropFruit = fruit;
        _nextDropFruit.ResetState(Trickle.PositionState.Ready);
    }

    int ReturnMaxEnumValueToInt<T>()
    {
        System.Array array = System.Enum.GetValues(typeof(T));
        int tmp = (int)array.GetValue(array.Length - 1);
        return tmp;
    }

    public void SpawnFruitWhenMerge(Trickle.Type mytype, Vector3 pos)
    {
        int maxTypeToInt = ReturnMaxEnumValueToInt<Trickle.Type>();

        int nextTypeToInt = (int)mytype + 1;

        if(nextTypeToInt > maxTypeToInt)
        {
            nextTypeToInt = maxTypeToInt;
        }

        score += _gameScore[mytype]; // --> �̰� GameManager�� ������
        _scoreTxt.text = score.ToString(); // ���⼭ ���ھ� �߰�

        Trickle.Type nextType = (Trickle.Type)nextTypeToInt; // ���� Ÿ������ ��ȯ
        Trickle fruit = SpawnFruit(nextType, pos);
        fruit.ResetState(Trickle.PositionState.Land); // Merge�� �ٷ� Land State�� �̵�

        // ���⼭ fruit�� Ÿ���� äũ�ؼ� ���� ������ Ÿ���� ��� sortingLayer �ٲ㼭 ȿ�� �ְ� �����ð� �ִٰ� ���ֱ�
        OnShowHighlightRequested?.Invoke(fruit.FruitType, fruit);

        CheckGameClear();
    }

    Trickle SpawnFruit(Trickle.Type type, Vector3 pos)
    {
        SetMaxSpawnFruitType(type);

        Trickle fruit = ReturnFruitUsingType(type);
        if (fruit == null) return null;

        Trickle spawnedFruit = Instantiate(fruit, pos, Quaternion.identity);
        spawnedFruit.Initialize(SpawnFruitWhenMerge, DestroyFruitInList, OnWaterDecreaseRequested);
        spawnedFruits.Add(spawnedFruit);

        return spawnedFruit;
    }

    public void DestroyFruitInList(Trickle me, Trickle other)
    {
        spawnedFruits.Remove(me);
        spawnedFruits.Remove(other);

        Destroy(me.gameObject);
        Destroy(other.gameObject);
    }

    public void DropNextFruit(Vector3 position)
    {
        if (_nextDropFruit == null) return;

        _nextDropFruit.transform.position = position;

        // ���⿡ �߻�ȭ ����ؼ� �ٸ� ����� ���� �� �ְ� �غ���

        _nextDropFruit.ResetState(Trickle.PositionState.Falling);
        _nextDropFruit = null; // ���� ���� �ʱ�ȭ
    }

    public bool CanDropNextFruit()
    {
        return _nextDropFruit != null;
    }
}
