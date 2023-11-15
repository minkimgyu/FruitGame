using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] List<Fruit> fruitPrefabs;

    [SerializeField] List<Fruit> spawnedFruits;

    [SerializeField] Transform _nextFruitSpawnPoint;
    [SerializeField] Fruit _nextDropFruit;

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
            new Fruit.Type[]{ Fruit.Type.Cherry, Fruit.Type.Strawberry, Fruit.Type.Grape, Fruit.Type.Lemon, Fruit.Type.Orange } },

        { Fruit.Type.Pear,
            new Fruit.Type[]{ Fruit.Type.Cherry, Fruit.Type.Strawberry, Fruit.Type.Grape, Fruit.Type.Lemon, Fruit.Type.Orange, Fruit.Type.Apple } },

        { Fruit.Type.Watermelon,
            new Fruit.Type[]{ Fruit.Type.Cherry, Fruit.Type.Strawberry, Fruit.Type.Grape, Fruit.Type.Lemon, Fruit.Type.Orange, Fruit.Type.Apple, Fruit.Type.Pear } },

        { Fruit.Type.Banana,
            new Fruit.Type[]{ Fruit.Type.Cherry, Fruit.Type.Strawberry, Fruit.Type.Grape, Fruit.Type.Lemon, Fruit.Type.Orange, Fruit.Type.Apple, Fruit.Type.Pear, Fruit.Type.Watermelon } },

        { Fruit.Type.Pineapple,
            new Fruit.Type[]{ Fruit.Type.Cherry, Fruit.Type.Strawberry, Fruit.Type.Grape, Fruit.Type.Lemon, Fruit.Type.Orange, Fruit.Type.Apple, Fruit.Type.Pear, Fruit.Type.Watermelon, Fruit.Type.Banana } }
    };

    public bool IsFruitYPosAboveLine(float endYPos)
    {
        for (int i = 0; i < spawnedFruits.Count; i++)
        {
            if(spawnedFruits[i].transform.position.y > endYPos)
            {
                return true;
            }
        }

        return false;
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

    Fruit ReturnFruitUsingType(Fruit.Type type)
    {
        return fruitPrefabs.Find(x => x.FruitType == type);
    }

    public void SpawnNextDropFruit()
    {
        Fruit.Type type = ReturnRandomSpawnType();

        Fruit fruit = SpawnFruit(type, _nextFruitSpawnPoint.position);
        _nextDropFruit = fruit;
        _nextDropFruit.OnChangeGravityScaleRequested(0);
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

        Fruit.Type nextType = (Fruit.Type)nextTypeToInt; // 다음 타입으로 변환
        SpawnFruit(nextType, pos);
    }

    Fruit SpawnFruit(Fruit.Type type, Vector3 pos)
    {
        SetMaxSpawnFruitType(type);

        Fruit fruit = ReturnFruitUsingType(type);
        if (fruit == null) return null;

        Fruit spawnedFruit = Instantiate(fruit, pos, Quaternion.identity);
        spawnedFruit.Initialize(SpawnFruitWhenMerge, DestroyFruitInList);
        spawnedFruits.Add(spawnedFruit);

        return spawnedFruit;
    }

    public void DestroyFruitInList(Fruit me, Fruit other)
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
        _nextDropFruit.OnChangeGravityScaleRequested(2);
    }
}
