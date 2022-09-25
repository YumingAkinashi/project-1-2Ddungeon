using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMessage : MonoBehaviour
{
    public static PlayerMessage playerMessage;

    private Animator messagePanel;
    private Text message;

    private void Start()
    {
        playerMessage = this;
        messagePanel = GetComponent<Animator>();
        message = GetComponentInChildren<Text>();
    }

    public void GetItemMessage(string itemInfos)
    {
        message.text = "Got " + itemInfos + "in inventory!";
        messagePanel.SetTrigger("show");
    }
}
