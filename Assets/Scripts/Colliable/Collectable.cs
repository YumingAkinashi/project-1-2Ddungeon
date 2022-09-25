using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Colliable
{

    // logic
    // protected basically means it's private for everybody but your children, in this case the structure is Colliable -> Collectable -> Chest
    protected bool collected;

    // override���N��O��쥻parent class��OnCollider���禡���e��g�A���bparent class�̳Q���檺�a���٬O�|���� (OnCollide(hits[i]);����)
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
