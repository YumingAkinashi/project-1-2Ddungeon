using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{

    // if there's item in the slot, enable image of the item.
    private Item _item;
    public Item Item
    {
        get { return _item; }
        set
        {
            _item = value;
            if(_item == null)
            {
                image.enabled = false;
            }
            else
            {
                image.enabled = true;
                image.sprite = _item.icon;
            }
        }
    }

    [SerializeField] Image image;

    // give every slot a item image.
    protected virtual void OnValidate()
    {
        if(image == null)
        {
            image = GetComponent<Image>();
        }
    }

    // pointer event
    public event Action<Item> OnRightClickEvent;

    public void OnPointerClick(PointerEventData eventData) // define eventData as a right clicking event and pass it to OnRightClickEvent.
    {
        if(eventData != null && eventData.button == PointerEventData.InputButton.Right)
        {
            if (Item != null && OnRightClickEvent != null)
                OnRightClickEvent(Item);
        }
    }
}
