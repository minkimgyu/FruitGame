using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    //[SerializeField] TMP_Text _scoreTxt;

    [SerializeField] Obstacle obstaclePrefab;

    [SerializeField] List<BaseItem> itemPrefabs;

    [SerializeField] List<Fruit> fruitPrefabs;

    [SerializeField] List<BaseDropObject> spawnedFruits;

    [SerializeField] Transform _nextFruitSpawnPoint;

    // ���� ������ �ٲٴ� ���
    [SerializeField] BaseDropObject _storedDropObject;

    [SerializeField] Transform _storedFruitPoint;


    [SerializeField] BaseDropObject _nextDropFruit;

    [SerializeField] Fruit.Type _nowMaxSpawnType = Fruit.Type.Cherry;

    [SerializeField] Dictionary<Fruit.Type, Fruit.Type[]> _gameRule = new Dictionary<Fruit.Type, Fruit.Type[]>()
    {
        { Fruit.Type.Cherry,
            new Fruit.Type[]{ Fruit.Type.Cherry } },

        { Fruit.Type.Strawberry,
            new Fruit.Type[]{ Fruit.Type.Cherry, Fruit.Type.Strawberry } },

        { Fruit.Type.Grape,
            new Fruit.Type[]{ Fruit.Type.Cherry, Fruit.Type.Strawberry, Fruit.Type.Grape } },

        { Fruit.Type.Lemon,
            new Fruit.Type[]{ Fruit.Type.Cherry, Fruit.Type.Strawberry, Fruit.Type.Grape, Fruit.Type.Lemon } },

        { Fruit.Type.Orange,
            new Fruit.Type[]{ Fruit.Type.Cherry, Fruit.Type.Strawberry, Fruit.Type.Grape, Fruit.Type.Lemon } },

        { Fruit.Type.Apple,
            new Fruit.Type[]{ Fruit.Type.Cherry, Fruit.Type.Strawberry, Fruit.Type.Grape, Fruit.Type.Lemon} },

        { Fruit.Type.Pear,
            new Fruit.Type[]{ Fruit.Type.Cherry, Fruit.Type.Strawberry, Fruit.Type.Grape, Fruit.Type.Lemon} },

        { Fruit.Type.Watermelon,
            new Fruit.Type[]{ Fruit.Type.Cherry, Fruit.Type.Strawberry, Fruit.Type.Grape, Fruit.Type.Lemon} },

        { Fruit.Type.Banana,
            new Fruit.Type[]{ Fruit.Type.Cherry, Fruit.Type.Strawberry, Fruit.Type.Grape, Fruit.Type.Lemon} },
    };

    [SerializeField]
    Dictionary<Fruit.Type, int> _gameScore = new Dictionary<Fruit.Type, int>()
    {
        { Fruit.Type.Cherry, 1},

        { Fruit.Type.Strawberry, 2},

        { Fruit.Type.Grape, 3},

        { Fruit.Type.Lemon, 4},

        { Fruit.Type.Orange, 5},

        { Fruit.Type.Apple, 6},

        { Fruit.Type.Pear, 7},

        { Fruit.Type.Watermelon, 8},

        { Fruit.Type.Banana, 9},
    };

    Action OnGameClearRequested;

    Action OnWaterDecreaseRequested;

    Action<Fruit.Type, BaseDropObject> OnShowHighlightRequested;

    Action<BaseItem.Type> OnAddItemRequested;

    Action OnObstacleCallRequested;

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

        ItemManager itemManager = GameObject.FindWithTag("ItemManager").GetComponent<ItemManager>();
        if (itemManager == null) return;

        OnAddItemRequested = itemManager.AddType;

        ObstacleCaller obstacleCaller = GameObject.FindWithTag("ObstacleCaller").GetComponent<ObstacleCaller>();
        if (obstacleCaller == null) return;

        OnObstacleCallRequested = obstacleCaller.OnBirdCallRequested;
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
            if (spawnedFruits[i].ReturnType() == Fruit.Type.Banana)
            {
                bananaCount++;
                if(bananaCount >= 2) OnGameClearRequested?.Invoke();
            }
        }
    }

    public Fruit.Type ReturnRandomSpawnType()
    {
        Debug.Log(_nowMaxSpawnType);

        Fruit.Type[] tmpTypes = _gameRule[_nowMaxSpawnType];
        int randomIndex = Random.Range(0, tmpTypes.Length);

        return tmpTypes[randomIndex];
    }

    void SetMaxSpawnFruitType(Fruit.Type type)
    {
        int getTypeToInt = (int)type;
        int storedTypeToInt = (int)_nowMaxSpawnType;

        if(getTypeToInt > storedTypeToInt) _nowMaxSpawnType = type;
    }

    public void SpawnNextDropFruit()
    {
        // �������� ���� �����ϴ� �����̹Ƿ� ���⼭�� ���� ������ �����
        // ���� �Լ� ¥�� �ű⿡ �ٸ� �������� �����ϵ��� ���ֱ�
        // ��� ����� _nextDropFruit�� ��ü���ֵ��� ����


        int tmp = Random.Range(0, 10);
        if(tmp > 6)
        {
            OnObstacleCallRequested?.Invoke();
        }


        // ���⼭ ����� DropObject�� �����Ѵٸ� �װ� �����ͼ� ��������ֱ�
        if(_storedDropObject != null)
        {
            ResetNextDropFruit(_storedDropObject);
            return;
        }

        Fruit.Type type = ReturnRandomSpawnType();

        BaseDropObject fruit = SpawnDropObject(type, _nextFruitSpawnPoint.position); 
        _nextDropFruit = fruit;
        _nextDropFruit.OnReady();

        //_nextDropFruit.ResetState(Fruit.PositionState.Ready);
    }

    int ReturnMaxEnumValueToInt<T>()
    {
        System.Array array = System.Enum.GetValues(typeof(T));
        int tmp = (int)array.GetValue(array.Length - 1);
        return tmp;
    }

    public void SpawnFruitWhenMerge(Fruit.Type mytype, Vector3 pos)
    {
        int maxTypeToInt = ReturnMaxEnumValueToInt<Fruit.Type>();

        int nextTypeToInt = (int)mytype + 1;

        if(nextTypeToInt > maxTypeToInt)
        {
            nextTypeToInt = maxTypeToInt;
        }

        //score += _gameScore[mytype]; // --> �̰� GameManager�� ������
        //_scoreTxt.text = score.ToString(); // ���⼭ ���ھ� �߰�

        Fruit.Type nextType = (Fruit.Type)nextTypeToInt; // ���� Ÿ������ ��ȯ
        BaseDropObject fruit = SpawnDropObject(nextType, pos);

        fruit.OnLand();

        //fruit.ResetState(Fruit.PositionState.Land); // Merge�� �ٷ� Land State�� �̵�

        OnAddItemRequested?.Invoke(BaseItem.Type.Lightning);

        // ���⼭ fruit�� Ÿ���� äũ�ؼ� ���� ������ Ÿ���� ��� sortingLayer �ٲ㼭 ȿ�� �ְ� �����ð� �ִٰ� ���ֱ�
        OnShowHighlightRequested?.Invoke(nextType, fruit);

        CheckGameClear();
    }

    BaseDropObject SpawnDropObject(Fruit.Type type, Vector3 pos)
    {
        SetMaxSpawnFruitType(type);

        Fruit fruit = fruitPrefabs.Find(x => x.FruitType == type);
        if (fruit == null) return null;

        Fruit spawnedFruit = Instantiate(fruit, pos, Quaternion.identity);
        spawnedFruit.Initialize(SpawnFruitWhenMerge, DestroyFruitInList, OnWaterDecreaseRequested);
        spawnedFruits.Add(spawnedFruit);

        return spawnedFruit;
    }

    public void DestroyFruitInList(BaseDropObject me, BaseDropObject other)
    {
        RemoveDropObject(me);
        RemoveDropObject(other);
    }

    public void DestroyFruitInList(BaseDropObject me)
    {
        RemoveDropObject(me);
    }

    public bool CanChangeItem()
    {
        if (_nextDropFruit == null) return false;

        return true;
    }

    public void OnSelectItem(BaseItem.Type type)
    {
        // ���� ������ �������� �ִ� ���
        // �ش� �������� �ı��ϰ� ���Ӱ� �����
        if (_storedDropObject != null)
        {
            RestoreNextFruit();
            return;
        }

        BaseDropObject dropObject = SpawnDropObject(type, _nextFruitSpawnPoint.position);
        ChangeNextDropObject(dropObject);
    }

    public void OnDestroyItem()
    {
        if (_storedDropObject == null) return;
        // ���� ������ ������Ʈ�� ���� ���� �������� ����

        RestoreNextFruit();
    }

    BaseDropObject SpawnDropObject(BaseItem.Type type, Vector3 pos)
    {
        BaseDropObject fruit = itemPrefabs.Find(x => x.ItemType == type);
        if (fruit == null) return null;

        BaseDropObject spawnedFruit = Instantiate(fruit, pos, Quaternion.identity);
        spawnedFruit.Initialize(DestroyFruitInList);
        spawnedFruits.Add(spawnedFruit);

        return spawnedFruit;
    }

    BaseDropObject SpawnDropObject(Vector3 pos)
    {
        BaseDropObject spawnedFruit = Instantiate(obstaclePrefab, pos, Quaternion.identity);
        spawnedFruits.Add(spawnedFruit);

        return spawnedFruit;
    }

    void ChangeNextDropObject(BaseDropObject dropObject)
    {
        if(_storedDropObject != null) // �̹� �������� ������ ���
        {
            RemoveDropObject(_nextDropFruit);
            ResetNextDropFruit(dropObject);
        }
        else
        {
            ResetStoredDropFruit(_nextDropFruit);
            ResetNextDropFruit(dropObject);
        }
    }

    void RestoreNextFruit()
    {
        RemoveDropObject(_nextDropFruit);
        ResetNextDropFruit(_storedDropObject);

        _storedDropObject = null;
    }

    void ResetStoredDropFruit(BaseDropObject dropObject)
    {
        _storedDropObject = dropObject;
        _storedDropObject.transform.position = _storedFruitPoint.position;
    }

    void ResetNextDropFruit(BaseDropObject dropObject)
    {
        _nextDropFruit = dropObject;
        _nextDropFruit.transform.position = _nextFruitSpawnPoint.position;
    }

    void RemoveDropObject(BaseDropObject dropObject)
    {
        spawnedFruits.Remove(dropObject);
        Destroy(dropObject.gameObject);
    }

    public void DropNextFruit(Vector3 position)
    {
        if (_nextDropFruit == null) return;

        _nextDropFruit.transform.position = position;

        // ���⿡ �߻�ȭ ����ؼ� �ٸ� ����� ���� �� �ְ� �غ���

        _nextDropFruit.OnDrop();

        //_nextDropFruit.ResetState(Fruit.PositionState.Falling);
        _nextDropFruit = null; // ���� ���� �ʱ�ȭ
    }

    public bool CanDropNextFruit()
    {
        return _nextDropFruit != null;
    }
}
