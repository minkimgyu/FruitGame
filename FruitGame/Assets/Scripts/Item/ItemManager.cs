using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemManager : MonoBehaviour
{
    [SerializeField]
    struct ItemData
    {
        public ItemData(BaseItem.Type type, int count)
        {
            _type = type;
            _count = count;
        }

        BaseItem.Type _type;
        public BaseItem.Type Type { get { return _type; } set { _type = value; } }

        int _count;
        public int Count { get { return _count; } set { _count = value; } }
    }

    [SerializeField] ItemPicker _pickerPrefab;
    [SerializeField] Transform _pickerParent;

    [SerializeField] TypeSpriteDictionary typeSpriteDictionary;

    List<ItemData> _containedItemDatas = new List<ItemData>();
    List<ItemPicker> _pickers = new List<ItemPicker>();

    Action<BaseItem.Type> OnSelectItemRequested;
    Action OnDestroyItemRequested;

    ItemPicker _nowSelectedPicker;

    private void Start()
    {
        GameObject go = GameObject.FindWithTag("Spawner");
        if (go == null) return;

        Spawner spawner = go.GetComponent<Spawner>();
        if (spawner == null) return;

        OnSelectItemRequested = spawner.OnSelectItem;
        OnDestroyItemRequested = spawner.OnDestroyItem;
    }

    int ReturnIndexOfItem(BaseItem.Type type)
    {
        for (int i = 0; i < _containedItemDatas.Count; i++)
        {
            if(_containedItemDatas[i].Type == type)
            {
                return i;
            }
        }

        return -1;
    }

    // ���⼭ Picker�� ���� �� ������ ������ 
    public void AddType(BaseItem.Type type)
    {
        int index = ReturnIndexOfItem(type);
        if (index == -1) // �ƿ� ���� ���� �־������
        {
            _containedItemDatas.Add(new ItemData(type, 1));
        }
        else // �ش� Ÿ���� �̹� �����ϴ� ���
        {
            ItemData data = _containedItemDatas[index];
            data.Count += 1;

            _containedItemDatas[index] = data;
        }

        UpdatePickers();
    }

    public void RemoveType(BaseItem.Type type)
    {
        int index = ReturnIndexOfItem(type);
        if (index == -1) return;

        ItemData data = _containedItemDatas[index];
        data.Count -= 1;

        if(data.Count <= 0)
        {
            // �ش� ������ �Ҹ��Ű��
            _containedItemDatas.RemoveAt(index);
        }
        else
        {
            _containedItemDatas[index] = data; // �ƴϸ� �� �ʱ�ȭ �����ֱ�
        }

        UpdatePickers();
    }

    void UpdatePickers()
    {
        for (int i = 0; i < _containedItemDatas.Count; i++)
        {
            ItemPicker picker = _pickers.Find(x => x.ItemType == _containedItemDatas[i].Type);
            if(picker == null) // �ش� Ÿ���� ItemPicker�� �������� �ʴ� ���
            {
                picker = Instantiate(_pickerPrefab, _pickerParent);
                picker.Initialize(_containedItemDatas[i].Type, typeSpriteDictionary[_containedItemDatas[i].Type], 
                    PickItem, DestroyItem, SelectPicker);

                _pickers.Add(picker);
            }
            else
            {
                picker.ItemCount = _containedItemDatas[i].Count; // ������ ���������ش�.
            }
        }

        // ����Ʈ�� Ȯ���ؼ� Picker ���� �� �ı��� ������
    }

    void OnOffPickerToggle()
    {
        for (int i = 0; i < _pickers.Count; i++)
        {
            if (_pickers[i] == _nowSelectedPicker)
            {
                _pickers[i].OnOffToggleImage(true);
            }
            else
            {
                _pickers[i].OnOffToggleImage(false);
            }
        }
    }

    public void OffPickerToggle()
    {
        for (int i = 0; i < _pickers.Count; i++)
        {
            _pickers[i].OnOffToggleImage(false);
        }

        RemoveType(_nowSelectedPicker.ItemType);
    }

    public void SelectPicker(ItemPicker picker)
    {
        _nowSelectedPicker = picker;
        OnOffPickerToggle();
    }

    public void PickItem(BaseItem.Type type)
    {
        OnSelectItemRequested?.Invoke(type);
        // ���⼭ ����Ʈ�� �ִ� ������ Ÿ���� �ϳ� ���ش�.
    }

    public void DestroyItem()
    {
        OnDestroyItemRequested?.Invoke();
        // ����� ���� �������� ����
    }
}
