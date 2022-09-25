using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : Colliable
{

    // Damage
    public int damage;
    public float pushForce;

    protected override void OnCollide(Collider2D coll)
    {
        
        if(coll.CompareTag("Player"))
        {

            Damage dmg = new Damage
            {

                damageAmount = damage,
                origin = transform.position, // position of which having the dmg object
                pushForce = pushForce,

            };

            coll.SendMessage("ReceiveDamage", dmg);
        }

    }

}
