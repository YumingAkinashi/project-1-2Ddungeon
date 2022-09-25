using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Swords,
    Wands
}

public class Weapon : Colliable
{

    // For reference
    public Player player;
    public Animator anim;

    // Damage struct
    public int[] basicDamageRange = new int[2];
    public float pushForce;
    public int probDigit;
    public int damage;

    // factors.
    private float cooldown = 0.5f;
    private float lastAttack;
    public Vector3 moveDelta;

    protected override void Start()
    {
        player = GetComponentInParent<Player>();
        anim = GetComponent<Animator>();
    }
    protected override void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            OnAttack();
        }
    }

    protected virtual void OnAttack()
    {

        if (Time.time - lastAttack > cooldown)
        {
            lastAttack = Time.time;
            AttackAnimation();
        }

    }
    protected virtual void AttackAnimation()
    {
        player.playerMovement.SetTrigger("hit");
    }
    protected virtual void OnReceiveDamage(Collider2D damageReceive)
    {
        probDigit = Random.Range(0, 101);

        // Create a new damage object, then we'll send it to the fighter we've hit
        // inside creating an object, we use "," to end a line instead
        Damage dmg = new Damage
        {
            damageAmount = damage,
            origin = transform.position, // position of which having the dmg object
            pushForce = pushForce,

        };

        if (damageReceive.CompareTag("Fighter"))
            damageReceive.SendMessage("EnemyReceiveDamage", dmg);
        else
            damageReceive.SendMessage("ReceiveDamage", dmg);

    }

}
