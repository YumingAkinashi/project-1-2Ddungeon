using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    // health
    protected int hitpoint;
    protected int maxHitpoint;

    // Immunity
    public float immuneTime = 0.5f;
    protected float lastImmune;

    // Push
    protected float pushRecoverySpeed;
    protected Vector3 pushDirection;
    protected Vector2 pushPlayer;

    // Slow
    protected bool slowDown = false;
    protected float slowDownTime;
    protected float timeBeingSlowed;
    protected float slowFactor;

    // all fighters can ReceiveDamage or Die
    protected virtual void ReceiveDamage(Damage dmg)
    {

        if(Time.time - lastImmune > immuneTime)
        {

            lastImmune = Time.time;
            hitpoint -= dmg.damageAmount;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;
            pushPlayer = (transform.position - dmg.origin).normalized * dmg.pushForce;

            GameManager.instance.ShowText(dmg.damageAmount.ToString(), 8, Color.red, transform.position, Vector3.zero, 0.5f);

            if(hitpoint <= 0)
            {

                hitpoint = 0;
                Death();

            }

        }

    }
    protected virtual void Death()
    {



    }

}
