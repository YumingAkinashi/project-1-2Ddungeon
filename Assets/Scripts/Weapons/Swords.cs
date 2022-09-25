using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swords : Weapon
{

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    protected override void OnAttack()
    {
        base.OnAttack();

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            if(enemy.CompareTag("Fighter") || enemy.CompareTag("Breakable"))
            {
                OnReceiveDamage(enemy);
                Debug.Log("hit" + enemy.name);
            }
        }
    }
    protected void CalculateDamage()
    {
        basicDamageRange[0] = Mathf.RoundToInt(player.stats.MagicDamage.Value * 0.8f);
        basicDamageRange[1] = Mathf.RoundToInt(player.stats.MagicDamage.Value * 1.5f);
    }
    protected override void AttackAnimation()
    {
        base.AttackAnimation();
        anim.SetTrigger("Swing");
    }
    protected override void OnReceiveDamage(Collider2D damageReceive)
    {
        CalculateDamage();

        if (probDigit >= 10)
        {
            damage = Random.Range(basicDamageRange[0], basicDamageRange[1]);
        }
        else if (probDigit < 10)
        {
            damage = basicDamageRange[1] * 2;
        }
        base.OnReceiveDamage(damageReceive);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
