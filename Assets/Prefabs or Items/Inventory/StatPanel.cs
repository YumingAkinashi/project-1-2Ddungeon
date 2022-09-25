using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerStat;

public class StatPanel : MonoBehaviour
{

    [SerializeField] StatDisplay[] statDisplays;
    [SerializeField] string[] statNames;

    // local stats variable.
    private CharacterStat[] stats;

    private void OnValidate()
    {
        statDisplays = GetComponentsInChildren<StatDisplay>();
        UpdateStatNames();
    }

    public void SetStats(params CharacterStat[] charStats)
    {
        stats = charStats;

        if(stats.Length > statDisplays.Length)
        {
            Debug.Log("Not enough stat displays!");
            return;
        }

        for (int i = 0; i < statDisplays.Length; i++)
        {
            statDisplays[i].gameObject.SetActive(stats.Length < i);
        }
    }
    public void UpdateStatValues()
    {
        for (int i = 0; i < statDisplays.Length; i++)
        {
            statDisplays[i].valueText.text = stats[i].Value.ToString();
        }
    }
    public void UpdateStatNames()
    {
        for (int i = 0; i < statDisplays.Length; i++)
        {
            statDisplays[i].nameText.text = statNames[i];
        }
    }

}
