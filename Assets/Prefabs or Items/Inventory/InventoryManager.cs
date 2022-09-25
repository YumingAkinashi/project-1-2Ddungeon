using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;

    // subscibe to right click event.
    private void Awake()
    {
        inventory.OnItemRightClickedEvent += EquipFromInventory;
        equipmentPanel.OnItemRightClickedEvent += UnequipFromPanel;
    }

    // methods.
    public void EquipFromInventory(Item item)
    {
        if(item is EquippableItem)
        {
            Equip((EquippableItem)item);
        }
    }
    public void UnequipFromPanel(Item item)
    {
        if (item is EquippableItem)
        {
            Unequip((EquippableItem)item);
        }
    }
    public void Equip(EquippableItem item)
    {
        if (inventory.RemoveItems(item))
        {
            EquippableItem previousItem;
            if(equipmentPanel.AddItems(item, out previousItem))
            {
                if(previousItem != null)
                {
                    // unequip previous item.
                    inventory.AddItems(previousItem);
                    previousItem.Unequip(GameManager.instance.currentPlayerData);
                    DestroyOnUnequip(previousItem);
                }
                item.Equip(GameManager.instance.currentPlayerData);
                GameManager.instance.OnHealthChange();
                GameManager.instance.characterMenu.UpdateMenu();
                InstantiateOnEquip(item);
            }
            else
            {
                inventory.AddItems(item);
            }
        }
    }
    public void Unequip(EquippableItem item)
    {
        if(!inventory.isFull() && equipmentPanel.RemoveItems(item))
        {
            item.Unequip(GameManager.instance.currentPlayerData);
            GameManager.instance.OnHealthChange();
            GameManager.instance.characterMenu.UpdateMenu();
            DestroyOnUnequip(item);
            inventory.AddItems(item);
        }
    }

    public void InstantiateOnEquip(EquippableItem item)
    {
        Object itemObject = item.ItemObject;
        Instantiate(itemObject, GameManager.instance.currentPlayer.transform);
    }
    public void DestroyOnUnequip(EquippableItem item)
    {
        Destroy(GameObject.Find(item.ItemObject.name + "(Clone)"));
    }

    public void ClearInventory()
    {
        inventory.ClearItems();
        for (int i = 0; i < equipmentPanel.slots.Length; i++)
        {
            UnequipFromPanel(equipmentPanel.slots[i].Item);
            inventory.ClearItems();
        }
    }
}
