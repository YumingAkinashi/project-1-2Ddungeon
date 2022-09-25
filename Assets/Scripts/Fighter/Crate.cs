using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Fighter
{

    private Animator anim;
    public GameObject parent;

    [SerializeField] List<EquippableLootTable> equipTables;

    private void Start()
    {
        anim = GetComponentInParent<Animator>();

    }

    protected override void Death()
    {

        // Get a random item
        float prob = Random.Range(0f, 100f);
        int howManyItems;

        if (prob >= 30)
        {
            howManyItems = 1;
        }
        else if (prob < 30 || prob >= 5)
        {
            howManyItems = 2;
        }
        else if (prob < 5)
        {
            howManyItems = 3;
        }
        else
        {
            howManyItems = 1;
        }

        for (int i = 0; i < howManyItems; i++)
        {
            Item _getItems = GameManager.instance.GetRandomEquippableLoot(equipTables);

            if (Inventory.inventory.AddItems(_getItems))
            {
                GameManager.instance.ShowText("Get a " + _getItems.itemNames + " !", 9, Color.yellow, transform.position, Vector3.up * 0.5f, 1.0f);
                string itemInfo = "";
                itemInfo += _getItems.itemNames + ", ";
                Debug.Log("get " + itemInfo + "items");
            }
            else
            {
                break;
            }
        }
        

        immuneTime = 50;
        anim.SetTrigger("broke");
        Destroy(parent, 0.5f);
    }

}
