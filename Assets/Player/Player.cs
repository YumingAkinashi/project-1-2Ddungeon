using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerStat;

public class Player : Mover
{
    // character stats
    public PlayerData stats;
    public int statsCount = 12;

    // movement.
    protected Vector2 mousePos;

    // references.
    public Animator playerMovement;
    public SpriteRenderer spriteRenderer;
    public StatPanel statPanel;

    // inherit / override methods.
    protected override void Start()
    {
        base.Start();
        stats = GetComponent<PlayerData>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<Animator>();
    }
    protected void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // set and reset the moveDelta
        moveDelta = new Vector3(x, y, 0);

        if(moveDelta.x > 0 || moveDelta.y > 0)
            playerMovement.SetFloat("speed", 1);
        else if(x == 0 & y == 0)
            playerMovement.SetFloat("speed", 0);
        else
            playerMovement.SetFloat("speed", 0);

        UpdateMotor(moveDelta, stats.XSpeed.Value, stats.YSpeed.Value, stats.PushRecoverySpeed.BaseValue);
    }

    protected override void UpdateMotor(Vector3 input, float xSpeed, float ySpeed, float pushRecovery)
    {

        mousePos = Input.mousePosition;

        // swap sprite direction, whether you're going right or left
        if (mousePos.x > 400)
            transform.localScale = Vector3.one;
        else if (mousePos.x < 400)
            transform.localScale = new Vector3(-1, 1, 1);

        base.UpdateMotor(input, xSpeed, ySpeed, pushRecovery);
    }
    protected override void ReceiveDamage(Damage dmg)
    {
        
        if (Time.time - lastImmune > immuneTime)
        {

            lastImmune = Time.time;
            stats.HitPoint -= dmg.damageAmount;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;
            pushPlayer = (transform.position - dmg.origin).normalized * dmg.pushForce;
            slowFactor = 0.7f;
            slowDownTime = 0.5f;

            GameManager.instance.ShowText(dmg.damageAmount.ToString(), 8, Color.red, transform.position, Vector3.zero, 0.5f);

            if (stats.HitPoint <= 0)
            {

                stats.HitPoint = 0;
                Death();

            }

        }

        GameManager.instance.OnHealthChange();
    }

}
