using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingFountain : Colliable
{

    public int healingAmount = 1;

    private float healingCooldown = 2.0f;
    private float lastheal;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag != "Player")
            return;

        if(Time.time - lastheal > healingCooldown)
        {

            lastheal = Time.time;
            GameManager.instance.currentPlayerData.Heal(healingAmount);

        }

    }

}
