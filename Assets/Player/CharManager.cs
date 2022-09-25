using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterNames
{
    Olivia,
    Herrington
}

public class CharManager : MonoBehaviour
{
    public Player[] chars;
    public PlayerData[] characterDatas;

    public int whichCharIsOn = 0;
    public int lastCharIsOn;
    public Player previousCharacter;
    public Player currentCharacter;

    // Set and switch character
    public Player SetCurrentCharacter()
    {
        if(currentCharacter == null)
        {
            whichCharIsOn = 0;
        }
        
        for (int i = 0; i < chars.Length; i++)
        {
            if(i == whichCharIsOn)
            {
                currentCharacter = chars[whichCharIsOn];
                currentCharacter.gameObject.SetActive(true);
            }
            else if (i != whichCharIsOn)
            {
                chars[i].gameObject.SetActive(false);
            }
        }
        return currentCharacter;
    }
    public void SwitchCharacter()
    {
        if (whichCharIsOn >= chars.Length)
            whichCharIsOn = 0;
        
        if (whichCharIsOn < 0)
            whichCharIsOn = chars.Length - 1;

        previousCharacter = chars[lastCharIsOn];
        currentCharacter = chars[whichCharIsOn];

        previousCharacter.gameObject.SetActive(false);
        currentCharacter.transform.position = previousCharacter.transform.position;
        currentCharacter.gameObject.SetActive(true);
    }
}
