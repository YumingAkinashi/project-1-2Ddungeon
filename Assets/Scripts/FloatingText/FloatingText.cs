using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatingText
{

    // define characteristics of a FloatingText object

    public bool active;
    public GameObject go;
    public TextMeshPro txt;
    public Vector3 motion;
    public float duration;
    public float lastShown;

    public void Show()
    {

        active = true;
        lastShown = Time.time;
        go.SetActive(active);

    }

    public void Hide()
    {

        active = false;
        go.SetActive(active);

    }

    public void UpdateFloatingText()
    {

        if (!active)
            return;

        if (Time.time - lastShown > duration)
            Hide();

        if(motion != Vector3.zero)
            txt.fontSize = Mathf.Lerp(txt.fontSize, 0, 0.008f);
        go.transform.position += motion * Time.deltaTime;

    }

}
