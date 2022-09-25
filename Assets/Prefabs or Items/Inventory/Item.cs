using UnityEngine;


[CreateAssetMenu]
public class Item : ScriptableObject // like a virtual object in inventory, so we don't actually need to create a gameObject.
{

    public string itemNames;
    public float weight;
    public Sprite icon;

}
