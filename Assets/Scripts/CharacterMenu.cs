using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerStat;

public class CharacterMenu : MonoBehaviour
{

    // text fields / stats panel.
    public Text levelText, hitpointText, pesosText, xpText;
    public Text strengthText, manaText, agilityText, vitalityText;

    // inventory field.
    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;

    // logic fields.
    public CharManager charManager;
    public Image characterSelectionSprite, weaponSelectionSprite;
    public RectTransform xpBar;

    // Character Selection
    public void OnArrowClick(bool right)
    {

        if (right)
        {
            charManager.lastCharIsOn = charManager.whichCharIsOn;
            charManager.whichCharIsOn++;
            OnSelectionChanged();
        }
        else
        {
            charManager.lastCharIsOn = charManager.whichCharIsOn;
            charManager.whichCharIsOn--;
            OnSelectionChanged();
        }

    }
    private void OnSelectionChanged()
    {
        Debug.Log("Switch Player - 1");
        GameManager.instance.SwitchPlayer();
        UpdateMenu();
        Debug.Log("Switch Player - 2");
        characterSelectionSprite.sprite = GameManager.instance.playerSpriteList[charManager.whichCharIsOn];
    }

    // update when menu button being clicked.
    public void UpdateMenu()
    {
        // character stats
        strengthText.text = GameManager.instance.currentPlayerData.Strength.Value.ToString();
        manaText.text = GameManager.instance.currentPlayerData.MaxMana.Value.ToString();
        agilityText.text = GameManager.instance.currentPlayerData.Agility.Value.ToString();
        vitalityText.text = GameManager.instance.currentPlayerData.Vitality.Value.ToString();

        // meta
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();
        hitpointText.text = GameManager.instance.currentPlayerData.HitPoint.ToString() + " / " + GameManager.instance.currentPlayerData.MaxHitpoint.Value.ToString();
        pesosText.text = GameManager.instance.pesos.ToString();

        // xp Bar
        int currLevel = GameManager.instance.GetCurrentLevel();

        if(currLevel == GameManager.instance.xpTable.Count)
        {

            xpText.text = GameManager.instance.experience.ToString() + " total experience points";
            xpBar.localScale = Vector3.one;

        }
        else
        {

            int prevLevelXp = GameManager.instance.GetXpToLevel(currLevel - 1);
            int currLevelXp = GameManager.instance.GetXpToLevel(currLevel);

            int diff = currLevelXp - prevLevelXp;
            int currXpIntoLevel = GameManager.instance.experience - prevLevelXp;

            float completionRatio = (float)currXpIntoLevel / (float)diff;
            xpBar.localScale = new Vector3(completionRatio, 1, 1);

            xpText.text = currXpIntoLevel.ToString() + " / " + diff.ToString();

        }

        // 
    }
}
