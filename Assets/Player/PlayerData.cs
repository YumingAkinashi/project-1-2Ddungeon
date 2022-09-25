using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using PlayerStat;

public class PlayerData : MonoBehaviour
{
    // character name / player reference
    public string characterName;
    public Player player;
    public CharacterMenu characterMenu;

    // frameworking bools.
    public bool isFirstAttach = true;

    // character stats accessible.
    public CharacterStat Strength;
    public CharacterStat Intellegence;
    public CharacterStat Agility;
    public CharacterStat Vitality;
    public CharacterStat MaxHitpoint;
    public CharacterStat PhysicalDamage;
    public CharacterStat MagicDamage;
    public CharacterStat MaxMana;
    public CharacterStat XSpeed;
    public CharacterStat YSpeed;
    public CharacterStat SelfHealSpeed;
    public CharacterStat PushRecoverySpeed;
    public CharacterStat Defense;

    // basic basic value when level 1.
    private int strengthBase = 10;
    private int intellegenceBase = 5;
    private int agilityBase = 7;
    private int vitalityBase = 2;
    private int maxHitpointBase = 10;

    // stats that not really change.
    private float xspeedBase = 1.0f;
    private float yspeedBase = 1.2f;
    private float pushRecoverySpeedBase = 0.5f;
    [Space]
    // modify each character.
    public int strengthAdd;
    public int intellegenceAdd;
    public int agilityAdd;
    public int vitalityAdd;
    [Space]
    // stats modify by level and characters.
    public float strengthGrowthRate;
    public float intellegenceGrowthRate;
    public float agilityGrowthRate;
    public float vitalityGrowthRate;
    public float maxHitpointGrowthRate;
    public float maxManaGrowthRate;

    // substats.
    [Header("Hitpoint and Mana")]
    public float maxHitPoint;
    public float maxMana;
    public float HitPoint;
    public float Mana;
    [Header("Substats")]
    public float physicalDamage;
    public float magicDamage;
    public float selfHealSpeed;
    public float pushRecoverySpeed;
    public float defense;

    // level up modifiers.
    public StatModifier s, i, a, v, h, m; // shorts for basic stats.

    private void Start()
    {
        Debug.Log("PlayerData Start - 1");
        characterName = gameObject.name;
        player = GetComponent<Player>();

        // level1 basic values.
        // prepare level mods.
        SetBasicValues();
        LevelUpModifiers();

        // load player (hitpoint, mana...)
        GameManager.instance.LoadPlayer();
        if (GameManager.instance.GetCurrentLevel() != 1)
        {
            SetLevel(GameManager.instance.GetCurrentLevel());
            characterMenu.UpdateMenu();
        }

        Debug.Log("PlayerData Start - 2");
    }

    // being called only when switching player.
    private void OnEnable()
    {
        Debug.Log("PlayerData OnEnable - 1");
        // don't call this method when first start.
        if (isFirstAttach)
        {
            isFirstAttach = false;
            return;
        }
        
        Debug.Log("PlayerData OnEnable - 2");

        // reset level when switching player.
        UnloadLevelModifiers();
        SetLevel(GameManager.instance.GetCurrentLevel());
        characterMenu.UpdateMenu();
        Debug.Log("PlayerData OnEnable - 3");
    }

    // level system.
    public void SetBasicValues()
    {
        // level1 basic values
        // basic stats.
        float strengthBaseValue = strengthBase + strengthAdd;
        float intellegenceBaseValue = intellegenceBase + intellegenceAdd;
        float agilityBaseValue = agilityBase + agilityAdd;
        float vitalityBaseValue = vitalityBase + vitalityAdd;

        // hitpoint / mana.
        float maxHitpointBaseValue = maxHitpointBase + (strengthBaseValue * 2);
        float maxManaBaseValue = (intellegenceBaseValue * 10) + 20;

        // substats.
        float physicalDamageBaseValue = strengthBaseValue * 0.3f;
        float magicDamageBaseValue = intellegenceBaseValue * 0.6f;
        float selfHealSpeedBaseValue = vitalityBaseValue * 0.1f;
        float defenseBaseValue = (strengthBaseValue * 0.2f) + (vitalityBaseValue * 0.5f);

        // limited stats.
        float xSpeedBaseValue = xspeedBase;
        float ySpeedBaseValue = yspeedBase;
        float pushRecoverySpeedBaseValue = pushRecoverySpeedBase;

        //

        Strength = new(strengthBaseValue);
        Intellegence = new(intellegenceBaseValue);
        Agility = new(agilityBaseValue);
        Vitality = new(vitalityBaseValue);
        MaxHitpoint = new(maxHitpointBaseValue);
        MaxMana = new(maxManaBaseValue);

        PhysicalDamage = new(physicalDamageBaseValue);
        MagicDamage = new(magicDamageBaseValue);
        SelfHealSpeed = new(selfHealSpeedBaseValue);
        Defense = new(defenseBaseValue);

        XSpeed = new(xSpeedBaseValue);
        YSpeed = new(ySpeedBaseValue);
        PushRecoverySpeed = new(pushRecoverySpeedBaseValue);

        maxHitPoint = MaxHitpoint.BaseValue;
        maxMana = MaxMana.BaseValue;

        physicalDamage = PhysicalDamage.BaseValue;
        magicDamage = MagicDamage.BaseValue;
        selfHealSpeed = SelfHealSpeed.BaseValue;
        pushRecoverySpeed = PushRecoverySpeed.BaseValue;
        defense = Defense.BaseValue;
}
    public void UpdateSubstats()
    {
        float physicalDamageBaseValue = Strength.Value * 0.3f;
        float magicDamageBaseValue = Intellegence.Value * 0.6f;
        float selfHealSpeedBaseValue = Vitality.Value * 0.1f;
        float defenseBaseValue = (Strength.Value * 0.2f) + (Vitality.Value * 0.5f);

        PhysicalDamage = new(physicalDamageBaseValue);
        MagicDamage = new(magicDamageBaseValue);
        SelfHealSpeed = new(selfHealSpeedBaseValue);
        Defense = new(defenseBaseValue);

        physicalDamage = PhysicalDamage.BaseValue;
        magicDamage = MagicDamage.BaseValue;
        selfHealSpeed = SelfHealSpeed.BaseValue;
        defense = Defense.BaseValue;
    }
    public void ResetPlayerData()
    {
        UnloadLevelModifiers();
        SetBasicValues();

        HitPoint = maxHitPoint;
        Mana = maxMana;
        GameManager.instance.OnHealthChange();
    }

    public void LevelUpModifiers()
    {
        // level up modifiers
        s = new StatModifier(strengthGrowthRate, StatModType.percentMulti, source: characterName);
        i = new StatModifier(intellegenceGrowthRate, StatModType.percentMulti, source: characterName);
        a = new StatModifier(agilityGrowthRate, StatModType.percentMulti, source: characterName);
        v = new StatModifier(vitalityGrowthRate, StatModType.percentMulti, source: characterName);
        h = new StatModifier(maxManaGrowthRate, StatModType.percentMulti, source: characterName);
        m = new StatModifier(maxHitpointGrowthRate, StatModType.percentMulti, source: characterName);
    }
    public void UnloadLevelModifiers()
    {
        Strength.RemoveModifierFromSource(characterName);
        Intellegence.RemoveModifierFromSource(characterName);
        Agility.RemoveModifierFromSource(characterName);
        Vitality.RemoveModifierFromSource(characterName);
        MaxHitpoint.RemoveModifierFromSource(characterName);
        MaxMana.RemoveModifierFromSource(characterName);
    }

    public void OnLevelUp()
    {
        Strength.AddModifier(s);
        Intellegence.AddModifier(i);
        Agility.AddModifier(a);
        Vitality.AddModifier(v);
        MaxHitpoint.AddModifier(h);
        MaxMana.AddModifier(m);

        HitPoint = MaxHitpoint.Value;
        Mana = MaxMana.Value;
        UpdateSubstats();
    }
    public void LoadingLevelUp()
    {
        Strength.AddModifier(s);
        Intellegence.AddModifier(i);
        Agility.AddModifier(a);
        Vitality.AddModifier(v);
        MaxHitpoint.AddModifier(h);
        MaxMana.AddModifier(m);
    }
    public void SetLevel(int level)
    {
        for (int i = 0; i < level; i++)
            LoadingLevelUp();
        UpdateSubstats();
    }

    // reset player stats


    // healing.
    public void Heal(int healingAmount)
    {
        if (HitPoint == MaxHitpoint.Value)
            return;

        HitPoint += healingAmount;
        GameManager.instance.OnHealthChange();
        if (HitPoint > MaxHitpoint.Value)
            HitPoint = MaxHitpoint.Value;
        GameManager.instance.ShowText("+ " + healingAmount.ToString() + " hp!", 7, Color.green, transform.position, Vector3.up, 1.0f);

    }
}
