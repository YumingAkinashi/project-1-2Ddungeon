using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colliable : MonoBehaviour
{
    public ContactFilter2D filter; // 偵測Collider彼此碰撞
    private BoxCollider2D boxCollider; // 儲存有這個腳本的角色的Collider資訊的變項
    private Collider2D[] hits = new Collider2D[10]; // 存放碰撞資訊的array

    protected virtual void Start()
    {

        boxCollider = GetComponent<BoxCollider2D>(); // 取得有這個腳本的角色的Collider資訊

    }
    protected virtual void Update()
    {

        // collision work 偵測和這個角色碰撞的Colliders並儲存資訊
        boxCollider.OverlapCollider(filter, hits);

        for (int i = 0; i < hits.Length; i++)
        {

            if (hits[i] == null) // hits[i]就是每次角色碰撞的另外一個角色
                continue;

            OnCollide(hits[i]);
            
            // clean up the array
            hits[i] = null;

        }

    }
    protected virtual void OnCollide(Collider2D coll)
    {

        Debug.Log("OnCollide was not implemented on " + this.name);
    }

}
