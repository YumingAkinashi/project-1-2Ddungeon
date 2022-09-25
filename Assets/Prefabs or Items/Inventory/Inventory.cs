using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{

    public static Inventory inventory;

    [SerializeField] List<Item> itemList; // items we currently have.
    [SerializeField] Transform itemParent; // we use Transform instead of GameObject simply cause we only need reference for transform.
    [SerializeField] ItemSlot[] itemSlots; // item slots under inventory parent.

    // click events
    public event Action<Item> OnItemRightClickedEvent;

    private void Awake()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].OnRightClickEvent += OnItemRightClickedEvent; // Both events take Item as parameter, so if left event is fired, so do the right.
        }

        inventory = this;
    }

    // methods
    private void OnValidate() // get reference for item slots.
    {
        if (itemParent != null)
            itemSlots = itemParent.GetComponentsInChildren<ItemSlot>();

        RefreshUI();
    }
    public void RefreshUI() // assign every item in itemList to itemSlots.
    {
        int i = 0;
        for(; i < itemList.Count && i < itemSlots.Length; i++)
        {
            itemSlots[i].Item = itemList[i];
        }
        for(; i < itemSlots.Length; i++) // if there's false above, then run this.
        {
            itemSlots[i].Item = null;
        }
    }
    public bool AddItems(Item item)
    {
        if (isFull())
            RemoveItems(itemList[itemList.Count - 1]);

        itemList.Add(item);
        RefreshUI();
        return true;
    }
    public bool RemoveItems(Item item)
    {
        if (itemList.Remove(item))
        {
            RefreshUI();
            return true;
        }
        return false;
    }
    public void ClearItems()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            RemoveItems(itemList[i]);
        }
    }
    public bool isFull()
    {
        return itemList.Count >= itemSlots.Length;
    }

}
