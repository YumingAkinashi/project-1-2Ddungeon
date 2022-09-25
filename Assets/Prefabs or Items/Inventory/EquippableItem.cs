using UnityEngine;

public enum EquipmentType
{
    Weapon1,
    Weapon2,
}

[CreateAssetMenu]
public class EquippableItem : Item
{
    
    [Header("Item Stats")]
    public int strengthBonus;
    public int intellegenceBonus;
    public int agilityBonus;
    public int vitalityBonus;
    public int maxHitpointBonus;
    public int maxManaBonus;
    [Space]
    public float strengthPercentBonus;
    public float intellegencePercentBonus;
    public float agilityPercentBonus;
    public float vitalityPercentBonus;
    public float maxHitpointPercentBonus;
    public float maxManaPercentBonus;
    [Space]
    public EquipmentType equipmentType;
    public GameObject ItemObject;

    public void Equip(PlayerData p)
    {
        if (strengthBonus != 0)
            p.Strength.AddModifier(new StatModifier(strengthBonus, StatModType.bonusFlat, this));
        if (intellegenceBonus != 0)
            p.Intellegence.AddModifier(new StatModifier(intellegenceBonus, StatModType.bonusFlat, this));
        if (agilityBonus != 0)
            p.Agility.AddModifier(new StatModifier(agilityBonus, StatModType.bonusFlat, this));
        if (vitalityBonus != 0)
            p.Vitality.AddModifier(new StatModifier(vitalityBonus, StatModType.bonusFlat, this));
        if (maxHitpointBonus != 0)
            p.MaxHitpoint.AddModifier(new StatModifier(maxHitpointBonus, StatModType.bonusFlat, this));
        if (maxManaBonus != 0)
            p.MaxMana.AddModifier(new StatModifier(maxManaBonus, StatModType.bonusFlat, this));

        if (strengthPercentBonus != 0)
            p.Strength.AddModifier(new StatModifier(strengthPercentBonus, StatModType.percentMulti, this));
        if (intellegencePercentBonus != 0)
            p.Intellegence.AddModifier(new StatModifier(intellegencePercentBonus, StatModType.percentMulti, this));
        if (agilityPercentBonus != 0)
            p.Agility.AddModifier(new StatModifier(agilityPercentBonus, StatModType.percentMulti, this));
        if (vitalityPercentBonus != 0)
            p.Vitality.AddModifier(new StatModifier(vitalityPercentBonus, StatModType.percentMulti, this));
        if (maxHitpointPercentBonus != 0)
            p.MaxHitpoint.AddModifier(new StatModifier(maxHitpointPercentBonus, StatModType.percentMulti, this));
        if (maxManaPercentBonus != 0)
            p.MaxMana.AddModifier(new StatModifier(maxManaPercentBonus, StatModType.percentMulti, this));

        p.UpdateSubstats();
    }
    public void Unequip(PlayerData p)
    {
        p.Strength.RemoveModifierFromSource(this);
        p.Intellegence.RemoveModifierFromSource(this);
        p.Agility.RemoveModifierFromSource(this);
        p.Vitality.RemoveModifierFromSource(this);
        p.MaxHitpoint.RemoveModifierFromSource(this);
        p.MaxMana.RemoveModifierFromSource(this);
    }
}
