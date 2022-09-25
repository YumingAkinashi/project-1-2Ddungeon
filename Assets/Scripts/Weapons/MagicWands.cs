using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWands : Weapon
{
    // for projectile.
    public GameObject projectilePrefab;
    public Transform launchOffset;
    public float projectileForce = 7f;

    private float wandsCooldown = 0.05f;
    private float wandsLastAttack;

    protected override void OnAttack()
    {

        if (Time.time - wandsLastAttack > wandsCooldown)
        {
            wandsLastAttack = Time.time;
            GameObject magicBall = Instantiate(projectilePrefab, launchOffset.position, launchOffset.rotation);

            // Ignore player and weapon itself's collider2D
            Physics2D.IgnoreCollision(magicBall.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(magicBall.GetComponent<Collider2D>(), GameManager.instance.currentPlayer.GetComponent<Collider2D>());
            Rigidbody2D rb = magicBall.GetComponent<Rigidbody2D>();
            rb.AddForce(launchOffset.up * projectileForce, ForceMode2D.Impulse);
            AttackAnimation();
        }

    }
    protected override void AttackAnimation()
    {
        anim.SetTrigger("spell");
    }
}
