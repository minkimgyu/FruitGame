using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class ItemPicker : MonoBehaviour, IPointerClickHandler
{
    int _itemCount;
    public int ItemCount 
    { 
        get { return _itemCount; } 
        set 
        { 
            _itemCount = value; 
            if(_itemCountTxt != null) _itemCountTxt.text = _itemCount.ToString();
        } 
    }

    BaseItem.Type _itemType;
    public BaseItem.Type ItemType { get { return _itemType; } }

    Image _itemImage;
    TMP_Text _itemCountTxt;

    bool _nowSelected = false;

    [SerializeField] Image toggleImage;

    public Action<ItemPicker> OnSelectRequested;
    public Action<BaseItem.Type> OnPickItemRequested;
    public Action OnDestroyItemRequested;

    // 타입을 넘겨받아서 초기화 해준다.
    public void Initialize(BaseItem.Type type, Sprite sprite, 
        Action<BaseItem.Type> OnPickItem, Action OnDestroyItem, Action<ItemPicker> OnResetType) 
    {
        _itemImage = GetComponent<Image>();
        _itemCountTxt = GetComponentInChildren<TMP_Text>();

        _itemType = type;
        _itemImage.sprite = sprite;

        OnSelectRequested = OnResetType;
        OnPickItemRequested = OnPickItem;
        OnDestroyItemRequested = OnDestroyItem;
    }

    public void OnOffToggleImage(bool nowToggle)
    {
        _nowSelected = nowToggle;
        toggleImage.gameObject.SetActive(nowToggle);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _nowSelected = !_nowSelected;

        if (_nowSelected == true)
        {
            OnSelectRequested?.Invoke(this);
            OnPickItemRequested?.Invoke(_itemType);
        }
        else
        {
            OnSelectRequested?.Invoke(null);
            OnDestroyItemRequested?.Invoke();
        }
        // 여기에서 스왑 기능 추가 구현해주기
    }
}
