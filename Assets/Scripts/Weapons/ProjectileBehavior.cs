using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : Weapon
{
    
    private Collider2D[] projecthits = new Collider2D[10];
    private CircleCollider2D circleCollider2D;
    private float attackRange = 0.2f;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "CollisionToIgnore")
        {
            Physics2D.IgnoreCollision(circleCollider2D, collision.collider);
        }
        
        if ((1 << collision.gameObject.layer & enemyLayers) == 1 << collision.gameObject.layer)
        {
            if(collision.gameObject.CompareTag("Fighter") || collision.gameObject.CompareTag("Breakable"))
            {
                OnReceiveDamage(collision.collider);
                Destroy(gameObject);
            }
            else
                Destroy(gameObject);
        }
        else
            Destroy(gameObject);

    }

    protected void CalculateDamage()
    {
        basicDamageRange[0] = Mathf.RoundToInt(GameManager.instance.currentPlayerData.MagicDamage.Value * 0.8f);
        basicDamageRange[1] = Mathf.RoundToInt(GameManager.instance.currentPlayerData.MagicDamage.Value * 1.5f);
    }
    protected override void OnReceiveDamage(Collider2D damageReceive)
    {
        CalculateDamage();
        probDigit = Random.Range(0, 101);

        if (probDigit >= 5)
        {
            damage = Random.Range(basicDamageRange[0], basicDamageRange[1]);
        }
        else if (probDigit < 5)
        {
            damage = basicDamageRange[1] * 3;
        }
        base.OnReceiveDamage(damageReceive);
    }
    
}
