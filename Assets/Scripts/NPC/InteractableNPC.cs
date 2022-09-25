using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableNPC : MonoBehaviour
{

    // Positions
    protected Transform playerTransform;
    protected Vector3 NPCPosition;

    // Animator
    protected Animator Dialogueanim;

    // Logics
    public bool isInRange;
    public bool interactable;
    protected double interactableRange = 0.5f;

    public GameObject popupSign;

    protected virtual void Start()
    {

        playerTransform = GameManager.instance.currentPlayer.transform;
        NPCPosition = transform.position;
        Dialogueanim = GetComponentInChildren<Animator>();

    }

    protected virtual void Update()
    {
        if (Vector3.Distance(NPCPosition, playerTransform.position) < interactableRange)
        {

            interactable = true;
            isInRange = true;

        }
        else
        {

            interactable = false;
            isInRange = false;

        }

    }

}
