using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : Colliable
{

    public string[] sceneNames;


    protected override void OnCollide(Collider2D coll)
    {

        if (coll.tag == "Player")
        {

            // save state when teleport
            GameManager.instance.SaveState();

            // teleport the Player
            string scenePick = sceneNames[Random.Range(0, sceneNames.Length)];
            SceneManager.LoadScene(scenePick);
            GameManager.instance.cameraMotor.SwitchPlayer();
        }

    }


}
