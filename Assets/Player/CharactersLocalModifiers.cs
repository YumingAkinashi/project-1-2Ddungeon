using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharactersLocalModifiers
{

    // base stats growth mods.
    //public StatModifier StrengthGrowth(float value, StatModType type, object source);
    //public StatModifier IntellegenceGrowth(float value, StatModType type, object source);
    //public StatModifier AgilityGrowth(float value, StatModType type, object source);
    //public StatModifier VitalityGrowth(float value, StatModType type, object source);

    // stats modify by basic stats.
    public StatModifier PhysicalDamageMod(float value, StatModType type, object source);
    //public StatModifier MagicDamageMod();
    //public StatModifier ManaMod();
    //public StatModifier XSpeedMod();
    //public StatModifier YSpeedMod();
    //public StatModifier AttackCoolDownMod();
    //public StatModifier SelfHealSpeedMod();
    //public StatModifier DefenseMod();

    // stats modify by level.
    //public StatModifier MaxHitpointMod();
}
