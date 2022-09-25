using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Colliable
{

    // logic
    // protected basically means it's private for everybody but your children, in this case the structure is Colliable -> Collectable -> Chest
    protected bool collected;

    // override的意思是把原本parent class裡OnCollider的函式內容改寫，但在parent class裡被執行的地方還是會執行 (OnCollide(hits[i]);那行)
    // both use codes behind Colliable but have different behaviour.
    protected override void OnCollide(Collider2D coll)
    {

        if (coll.tag == "Player")
        {

            OnCollect();
        }


    }

    protected virtual void OnCollect()
    {

        collected = true;
    }

}
