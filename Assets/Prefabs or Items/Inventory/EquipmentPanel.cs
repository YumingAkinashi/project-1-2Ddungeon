using UnityEngine;
using System;

public class EquipmentPanel : MonoBehaviour
{

    [SerializeField] Transform equipmentSlotsParent;
    [SerializeField] EquipmentSlot[] equipmentSlots;
    public EquipmentSlot[] slots
    {
        get
        {
            return equipmentSlots;
        }
    }

    // click events
    public event Action<Item> OnItemRightClickedEvent;

    private void Awake()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].OnRightClickEvent += OnItemRightClickedEvent; // Both events take Item as parameter, so if left event is fired, so do the right.
        }
    }

    private void OnValidate()
    {
        equipmentSlots = equipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
    }

    // methods
    public bool AddItems(EquippableItem item, out EquippableItem previousItem)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if(equipmentSlots[i].equipmentType == item.equipmentType)
            {
                previousItem = (EquippableItem)equipmentSlots[i].Item;
                equipmentSlots[i].Item = item;
                return true;
            }
        }
        previousItem = null;
        return false;
    }
    public bool RemoveItems(EquippableItem item)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].Item == item)
            {
                equipmentSlots[i].Item = null;
                return true;
            }
        }
        return false;
    }
}
