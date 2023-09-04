using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class InventoryItem
{
    public ItemData data;
    public int stackSize;

    public InventoryItem(ItemData _newItemData) {
        data = _newItemData;
        AddStack();
        // add to stack
    }

    public void AddStack() => stackSize++;

    public void RemoveStack() => stackSize--;

}
