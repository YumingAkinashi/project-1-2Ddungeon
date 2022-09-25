using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using UnityEngine;

namespace PlayerStat
{
    public class CharacterStat
    {
        // fields
        public float BaseValue;
        public float Value // only changing when modifier comes in or base value changes.
        {
            get
            {
                if (isDirty || BaseValue != lastBaseValue)
                {
                    lastBaseValue = BaseValue;
                    _value = CalculateFianlValue();
                    isDirty = false;
                }
                return _value;
            }
        }

        protected bool isDirty = true;
        protected float _value; // temporary hold the final value, if there's no adding or removing upon modifiers then don't pass this to Value.
        protected float lastBaseValue = float.MinValue; // detect base value changes.

        protected readonly List<StatModifier> statModifiers; // A keyword "readonly" for reference type, reference must be fixed, but object itself can change.
        public readonly ReadOnlyCollection<StatModifier> StatModifiers; // accessible version of statModifiers.

        // constructor for the class
        public CharacterStat()
        {
            statModifiers = new List<StatModifier>();   // a
            StatModifiers = statModifiers.AsReadOnly(); // b
                                                        // b can reflect changes on a, but b can't physically change a.
        }
        public CharacterStat(float baseValue) : this()
        {
            BaseValue = baseValue;
        }

        // methods
        public virtual void AddModifier(StatModifier mod)
        {
            isDirty = true;
            statModifiers.Add(mod);
            statModifiers.Sort(CompareModifierOrder); // place every new modifier to the right order.
        }
        public virtual int CompareModifierOrder(StatModifier a, StatModifier b) // algorithm for defualt method Sort().
        {
            if (a.Order < b.Order)
                return -1;
            else if (a.Order > b.Order)
                return 1;
            return 0; // (a.Order == b.Order)
        }
        public virtual bool RemoveModifier(StatModifier mod)
        {
            if (statModifiers.Remove(mod))
            {
                isDirty = true;
                return true;
            }
            return false;
        }
        public virtual bool RemoveModifierFromSource(object source)
        {
            bool didRemove = false;

            for (int i = statModifiers.Count - 1; i >= 0; i--)
            {
                if (statModifiers[i].Source == source)
                {
                    isDirty = true;
                    didRemove = true;
                    statModifiers.RemoveAt(i);
                }
            }

            return didRemove;
        } // An equipment may has multiple modifiers, define an equiment as a source.
        public virtual float CalculateFianlValue()
        {
            float finalValue = BaseValue;

            for (int i = 0; i < statModifiers.Count; i++)
            {
                StatModifier mod = statModifiers[i];
                float sumPercentAdd = 0;

                if (mod.Type == StatModType.flat)
                {
                    finalValue += mod.Value;
                }
                else if (mod.Type == StatModType.bonusFlat)
                {
                    finalValue += mod.Value;
                }
                else if (mod.Type == StatModType.percentMulti)
                {
                    sumPercentAdd += mod.Value;

                    if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.percentMulti)
                    {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }
                }
                else if (mod.Type == StatModType.percentAdd)
                {
                    finalValue *= 1 + mod.Value;
                }

            }

            return (float)Mathf.RoundToInt(finalValue);
        }

    }
}
