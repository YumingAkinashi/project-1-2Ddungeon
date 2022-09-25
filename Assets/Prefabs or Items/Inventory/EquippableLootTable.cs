using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class EquippableLootTable : LootTable
{

    // This list is populated from the editor
    [SerializeField] private List<EquippableItem> _items;

    // This is NonSerialized as we need it false everytime we run the game.
    // Without this tag, once set to true it will be true even after closing and restarting the game
    // Which means any future modification of our item list is not properly considered
    [System.NonSerialized] private bool isInitialized = false;

    private float _totalWeight;

    // Initializing functions
    private void Initialize()
    {
        if (!isInitialized)
        {
            _totalWeight = _items.Sum(item => item.weight);
            isInitialized = true;
        }
    }

    #region Alternitive Initialize()
    // An alternative version that does the same operation, puts in _totalWeight the sum of the weight of each item
    private void AltInitialize()
    {
        if (!isInitialized)
        {
            _totalWeight = 0;
            foreach(var item in _items)
            {
                _totalWeight += item.weight;
            }

            isInitialized = true;
        }
    }
    #endregion

    public EquippableItem GetRandomEquippableItem()
    {
        // Make sure it is initialized
        Initialize();

        // Roll dice
        float diceRoll = Random.Range(0f, _totalWeight);

        // Circle through item list
        foreach(var item in _items)
        {
            if (item.weight >= diceRoll)
                return item;

            diceRoll -= item.weight;
        }

        throw new System.Exception("Reward generation failed!");
    }

}
