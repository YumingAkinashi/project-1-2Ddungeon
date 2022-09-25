using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLaunch : MonoBehaviour
{
    // for projectile.
    protected Transform rb;

    // mouse
    private Vector2 mousePos;
    private Vector2 lookDir;
    protected Camera cam;
    public float mouseAngle;

    protected void Start()
    {
        rb = GetComponent<Transform>();
        cam = Camera.main;
    }
    protected void Update() // monitor the mouse.
    {
        if (cam == null)
            cam = GameManager.instance.mainCamera;
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        Vector2 lookDir = mousePos - new Vector2(rb.position.x, rb.position.y);
        mouseAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.eulerAngles = new Vector3(0, 0, mouseAngle);
    }
    protected virtual void FixedUpdate() // calculating values about mouse.
    {
        //Vector2 lookDir = mousePos - new Vector2(rb.position.x, rb.position.y);
        //mouseAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        //rb.eulerAngles = new Vector3(0, 0, mouseAngle);
    }

}
