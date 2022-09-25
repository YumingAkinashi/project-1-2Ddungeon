using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatingTextManager : MonoBehaviour
{

    public GameObject textContainer;
    public GameObject textPrefab;

    private List<FloatingText> floatingTexts = new List<FloatingText>();

    // Bending FloatingTextUpdate into real Update function
    private void Update()
    {

        foreach (FloatingText txtList in floatingTexts)
            txtList.UpdateFloatingText();

    }

    // Creating or manage the floating text pool, determine what going to show and give them needed arguments
    public void Show(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {

        FloatingText showTexts = GetFloatingText();

        showTexts.txt.text = msg;
        showTexts.txt.fontSize = fontSize;
        showTexts.txt.color = color;

        showTexts.go.transform.position = position;

        showTexts.motion = motion;
        showTexts.duration = duration;
        

        showTexts.Show();
    }
    public void Announce(string msg, int fontSize, Color color, Vector3 position, float duration)
    {

        FloatingText showTexts = GetFloatingText();

        showTexts.txt.text = msg;
        showTexts.txt.fontSize = fontSize;
        showTexts.txt.color = color;

        showTexts.go.transform.position = position;

        showTexts.motion = Vector3.zero;
        showTexts.duration = duration;


        showTexts.Show();
    }

    private FloatingText GetFloatingText()
    {

        FloatingText txtList = floatingTexts.Find(t => !t.active);

        if(txtList == null)
        {

            txtList = new FloatingText();
            txtList.go = Instantiate(textPrefab);
            txtList.go.transform.SetParent(textContainer.transform);
            txtList.txt = txtList.go.GetComponent<TextMeshPro>();

            floatingTexts.Add(txtList);
        }

        return txtList;
    }

}
