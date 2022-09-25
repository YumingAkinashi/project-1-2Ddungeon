using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueNPC : InteractableNPC
{

    private bool isPopUp = false;

    protected override void Update()
    {
        
        base.Update();

        if(interactable == true)
        {

            if (!isPopUp)
            {

                Instantiate(popupSign, transform.position, Quaternion.identity);
                isPopUp = true;
            }

            if (Input.GetKeyDown("x"))
            {

                ShowDialogue();

            }

        }
        else
        {

            if (GameObject.Find("PopUpSign(Clone)") == true)
            {

                Destroy(GameObject.Find("PopUpSign(Clone)"));
                isPopUp = false;

            }

        }

    }

    public void ShowDialogue()
    {

        Dialogueanim.SetTrigger("showdiag");

    }
}
