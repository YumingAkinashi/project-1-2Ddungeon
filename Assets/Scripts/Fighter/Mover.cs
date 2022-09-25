using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// abstract class means that you can only inherit from it but you can't drag it as a script to a game object
// we put Mover to abstract is because we are confident that only Player, NPC and enemies need a script on them
public abstract class Mover : Fighter
{
    protected BoxCollider2D boxCollider;
    protected Vector3 moveDelta;
    protected RaycastHit2D hit;
    protected Camera cam;

    protected virtual void Start()
    {
        SceneManager.sceneLoaded += OnCameraChanged;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // camera
    protected void OnCameraChanged(Scene theScene, LoadSceneMode Mode)
    {
        cam = Camera.main;
    }

    protected virtual void UpdateMotor(Vector3 input, float xSpeed, float ySpeed, float pushRecovery) // motor through translate.
    {

        // set and reset the moveDelta and mousePos.
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

        // add pushDirection if there's any
        moveDelta += pushDirection;
        // Reduce push force every frame, base off recovery speed
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecovery);

        // slow down mover if there's any
        if (!slowDown && slowFactor != 0f && timeBeingSlowed == 0f)
        {
            slowDown = true;
            timeBeingSlowed = Time.time;
        }

        if (slowDown && Time.time - timeBeingSlowed <= slowDownTime)
        {
            moveDelta *= slowFactor;
        }
        else
        {
            slowDown = false;
            slowFactor = 0f;
            timeBeingSlowed = 0f;
        }

        // make sure we can move in this direction by throwing a box there first, if returns null, we are free to move
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {   
            // make player move
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);

        }

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {

            // make player move
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);

        }

    }
}