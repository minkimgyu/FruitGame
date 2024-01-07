using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class BaseItem : BaseDropObject
{
    public enum Type
    {
        Lightning
    }

    [SerializeField] protected Type _type;
    public Type ItemType { get { return _type; } }

    Action<BaseDropObject> OnDestroyRequested;
    Action OffPickerToggleRequested;

    public override void Initialize(Action<BaseDropObject> onDestroyRequested) 
    {
        OnDestroyRequested = onDestroyRequested;
        ItemManager itemManager = GameObject.FindWithTag("ItemManager").GetComponent<ItemManager>();
        OffPickerToggleRequested = itemManager.OffPickerToggle;
    }

    public override void OnDrop()
    {
        OnDestroyRequested?.Invoke(this);
        OffPickerToggleRequested?.Invoke();
        // 여기서 아이템을 제거하는 함수를 호출시켜준다.
    }
}
