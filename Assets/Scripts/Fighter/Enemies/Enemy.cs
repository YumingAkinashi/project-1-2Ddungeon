using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyAI
{

    // Experience
    public int xpValue;

    // stats.
    public int enemyHitpoint;
    public float pushRecovery;

    // GTX.
    public Animator anim;

    // Logic
    protected bool collideWithPlayer;
    private float cooldown = .05f;
    private float lastAttack;

    // Hitbox
    public ContactFilter2D filter;
    public Collider2D hitbox;
    public Collider2D[] hits = new Collider2D[10];

    protected override void Start()
    {
        // hitbox.
        hitbox = GameObject.Find("hitbox").GetComponent<BoxCollider2D>();

        // animator.
        anim = GetComponentInChildren<Animator>();
        base.Start();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        MoveAnimation();
        
        // check if overlap with player. 
        collideWithPlayer = false;
        hitbox.OverlapCollider(filter, hits);

        for (int i = 0; i < hits.Length; i++)
        {

            if (hits[i] == null)
                continue;

            if (hits[i].CompareTag("Player"))
            {
                collideWithPlayer = true;
            }

            // clean up the array
            hits[i] = null;

        }
    }

    protected void EnemyReceiveDamage(Damage dmg)
    {

        if (Time.time - lastAttack > cooldown)
        {
            lastAttack = Time.time;

            enemyHitpoint -= dmg.damageAmount;

            // being pushed.
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

            // being slowed.
            slowFactor = 0.5f;
            slowDownTime = 1.0f;

            // text
            GameManager.instance.ShowText(dmg.damageAmount.ToString(), 8, Color.red, transform.position, Vector3.zero, 0.5f);

            // on damaged animation.
            DamagedAnimation();

            if (enemyHitpoint <= 0)
            {

                enemyHitpoint = 0;
                Death();

            }
        }
    }
    protected void MoveAnimation()
    {
        // make enemy run.
        if (rb.velocity != Vector2.zero)
            anim.SetFloat("speed", 1);
        else
            anim.SetFloat("speed", 0);
    }
    protected void DamagedAnimation()
    {
        anim.Play("goblin_damaged");
    }
    protected  void Death()
    {
        anim.Play("goblin_death");
        Destroy(gameObject, 0.2f);
        GameManager.instance.GrantXp(xpValue);
        GameManager.instance.ShowText("+ " + xpValue + " xp!", 8, Color.magenta, Camera.main.ScreenToWorldPoint(transform.position), Vector3.up * 40, 0.5f);
    }

}
