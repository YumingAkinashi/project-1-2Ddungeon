using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;
using System;

public class EnemyAI : MonoBehaviour
{

    // field for path finding
    public Transform target;

    public float speed;
    public float nextWaypointDistance;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndofPath = false;

    Seeker seeker;
    public Rigidbody2D rb;

    //field for enemy being slowed or push
    // Push
    protected float pushRecoverySpeed;
    protected Vector3 pushDirection;
    protected Vector2 pushPlayer;

    // Slow
    protected bool slowDown = false;
    protected float slowDownTime;
    protected float timeBeingSlowed;
    public float slowFactor;

    // Switch player event.
    public Button SwitchPlayerRightArrow;
    public Button SwitchPlayerLeftArrow;

    protected virtual void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        target = GameManager.instance.currentPlayer.transform;

        GameManager.instance.OnPlayerSwitched.AddListener(OnPlayerSwitched);

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    private void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.transform.position, target.transform.position, OnPathCompleted);
    }
    private void OnPathCompleted(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    public void OnPlayerSwitched()
    {
        target = GameManager.instance.currentPlayer.transform;
    }

    protected virtual void FixedUpdate()
    {
        if (path == null)
            return; // if no path generated then retrun.

        if(currentWaypoint >= path.vectorPath.Count) // check if we're in the end of the path.
        {
            reachedEndofPath = true;
            return;
        }
        else if(currentWaypoint < path.vectorPath.Count)
        {
            reachedEndofPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized; // make enemy move.
        Vector2 force = direction * speed * Time.deltaTime;

        // slowed down or being pushed.
        UpdateMotorEffect(force);
        rb.AddForce(force);

        float distance = Vector2.Distance(path.vectorPath[currentWaypoint], rb.position);

        if(distance < nextWaypointDistance) // if distance between current position and target waypoint small enough to move on to next waypoint.
        {
            currentWaypoint++;
        }

        if (rb.velocity.x >= 0.01f) // flip enemy sprite base on velocity on x direction.
            transform.localScale = new Vector3(1f, 1f, 1f);
        else if(rb.velocity.x <= -0.01f)
            transform.localScale = new Vector3(-1f, 1f, 1f);
    }

    
    protected void UpdateMotorEffect(Vector2 force)
    {
        //force += (Vector2)pushDirection;
        //pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

        if (pushDirection != Vector3.zero)
        {
            rb.AddForce(pushDirection, ForceMode2D.Impulse);
            pushDirection = Vector3.zero;
        }

        // slow down mover if there's any
        if (!slowDown && slowFactor != 0f && timeBeingSlowed == 0f)
        {
            slowDown = true;
            timeBeingSlowed = Time.time;
        }

        if (slowDown && Time.time - timeBeingSlowed <= slowDownTime)
        {
            force *= slowFactor;
        }
        else
        {
            slowDown = false;
            slowFactor = 0f;
            timeBeingSlowed = 0f;
        }
    }
    
}
