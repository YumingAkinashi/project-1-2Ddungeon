using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // static means this object can be accessed everywhere
    // so we kinda need to specify which instance we are talking about --> instance = this;
    // thus if we use GameManager.instance.xxxxx, we are referring this class right here
    public static GameManager instance;

    private void Awake()
    {
        Debug.Log("GameManager Awake - 1");
        //這裡的gameObject = hierarchy裡的Game Manager物件, 這個script是gameObject的component
        if (instance != null) {

            // avoid duplicating Game Manager object in the very beginning
            Destroy(gameObject);
            Destroy(charManager.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(hud);
            Destroy(menu);
            return;

        }

        instance = this;

        Debug.Log("GameManager Awake - 2");
        currentPlayer = charManager.SetCurrentCharacter();
        currentPlayerData = charManager.characterDatas[charManager.whichCharIsOn];

        // run the OnLoadState function everytime loading a scene.
        SceneManager.sceneLoaded += OnLoadState;
        SceneManager.sceneLoaded += LoadState;
        SceneManager.sceneLoaded += OnCameraChanged;
        isInitialised = true;
        Debug.Log("GameManager Awake - 3");
    }

    #region References
    // frameworking bools.
    [HideInInspector]
    public bool isInitialised = false; // is GameManager initialized.
    public bool isFirstSwitching = false;
    public bool isASectionSpawning = false; // is an section on tile map spawning enemies.
    
    // resources.
    [Header("Resources")]
    public List<Sprite> playerSpriteList;
    public List<Sprite> weaponSprites;
    public List<int> xpTable;
    [Space]

    // player.
    [Header("Player")]
    public CharManager charManager;
    public Player currentPlayer;
    public PlayerData currentPlayerData;

    // references.
    [Header("References")]
    public Camera mainCamera;
    public CameraMotor cameraMotor;
    public void OnCameraChanged(Scene theScene, LoadSceneMode Mode)
    {
        mainCamera = Camera.main;
        cameraMotor = mainCamera.GetComponent<CameraMotor>();
    }

    public FloatingTextManager floatingTextManager;
    public Weapon weapon;
    public void OnWeaponEquip()
    {
        weapon = currentPlayer.GetComponentInChildren<Weapon>();
    }

    public GameObject hud;
    public GameObject menu;
    public CharacterMenu characterMenu;
    public RectTransform healthBar;
    public InventoryManager inventoryManager;
    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;
    [Space]

    // logic.
    [Header("Game Resources")]
    public int pesos;
    public int experience;
    #endregion

    #region Game managing functions
    // floating text system.
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {

        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);

    }

    // change character.
    public UnityEvent OnPlayerSwitched = new();

    public Player SetPlayer()
    {
        return charManager.SetCurrentCharacter();
    }
    public void SwitchPlayer()
    {
        SavePlayer(); // save necessary info before switch player.

        charManager.SwitchCharacter(); // switching by enable and disable.
        currentPlayer = charManager.currentCharacter;
        currentPlayerData = charManager.characterDatas[charManager.whichCharIsOn];
        //weapon.transform.parent = currentPlayer.transform;
        //weapon.player = currentPlayer;

        LoadPlayer();
        Debug.Log("Switch Player");
        //OnPlayerSwitched.Invoke();
    }

    // health bar.
    public void OnHealthChange()
    {

        if(currentPlayerData.HitPoint > currentPlayerData.MaxHitpoint.Value)
        {
            currentPlayerData.HitPoint = currentPlayerData.MaxHitpoint.Value;
        }
        float ratio = (float)currentPlayerData.HitPoint / (float)currentPlayerData.MaxHitpoint.Value;
        healthBar.localScale = new Vector3(1, ratio, 1);

    }

    // level / experience system.
    public int GetCurrentLevel()
    {
        int r = 0;
        int add = 0;

        while (experience >= add)
        {
            add += xpTable[r];
            r++;

            if (r == xpTable.Count)
                return r;
        }
        return r;
    }
    public int GetXpToLevel(int level)
    {

        int r = 0;
        int xp = 0;

        while (r < level)
        {
            xp += xpTable[r];
            r++;
        }

        return xp;

    }
    public void GrantXp(int xp)
    {
        int currLevel = GetCurrentLevel();
        experience += xp;

        if (currLevel < GetCurrentLevel())
            OnLevelUp();
    }
    public void OnLevelUp()
    {
        Debug.Log("Level up!");
        currentPlayer.stats.OnLevelUp();
        OnHealthChange();
    }

    // get random loot
    public Object GetRandomLootFromTables(List<EquippableLootTable> eTables, List<SupplementLootTable> sTables, float eTablePickRate)
    {
        float _totalTableWeight = 0;

        // Pick eTable or sTable
        float pickType = Random.Range(0f, 100f);
        if(pickType >= eTablePickRate)
        {
            foreach (SupplementLootTable table in sTables)
            {
                _totalTableWeight += table.tableWeight;
            }

            // Roll dice
            float diceRoll = Random.Range(0f, _totalTableWeight);
            SupplementLootTable pickTable;

            // Circle through table list
            foreach (var table in sTables)
            {
                if (table.tableWeight >= diceRoll)
                {
                    pickTable = table;
                    return pickTable.GetRandomSupplementItem();
                }

                diceRoll -= table.tableWeight;
            }
        }
        else
        {
            foreach (EquippableLootTable table in eTables)
            {
                _totalTableWeight += table.tableWeight;
            }

            // Roll dice
            float diceRoll = Random.Range(0f, _totalTableWeight);
            EquippableLootTable pickTable;

            // Circle through table list
            foreach (var table in eTables)
            {
                if (table.tableWeight >= diceRoll)
                {
                    pickTable = table;
                    return pickTable.GetRandomEquippableItem();
                }

                diceRoll -= table.tableWeight;
            }
        }

        throw new System.Exception("Reward generation failed!");
    }
    public EquippableItem GetRandomEquippableLoot(List<EquippableLootTable> eTables)
    {
        float _totalTableWeight = 0;

        foreach (EquippableLootTable table in eTables)
        {
            _totalTableWeight += table.tableWeight;
        }

        // Roll dice
        float diceRoll = Random.Range(0f, _totalTableWeight);

        // Circle through table list
        foreach (var table in eTables)
        {
            if (table.tableWeight >= diceRoll)
            {
                EquippableItem getItem = table.GetRandomEquippableItem();
                _totalTableWeight = 0;
                return getItem;
            }

            diceRoll -= table.tableWeight;
        }

        throw new System.Exception("Reward generation failed in game manager!");
    }
    public Object GetRandomEquippableLoot(List<SupplementLootTable> sTables)
    {
        float _totalTableWeight = 0;

        foreach (SupplementLootTable table in sTables)
        {
            _totalTableWeight += table.tableWeight;
        }

        // Roll dice
        float diceRoll = Random.Range(0f, _totalTableWeight);
        SupplementLootTable pickTable;

        // Circle through table list
        foreach (var table in sTables)
        {
            if (table.tableWeight >= diceRoll)
            {
                pickTable = table;
                return pickTable.GetRandomSupplementItem();
            }

            diceRoll -= table.tableWeight;
        }

        throw new System.Exception("Reward generation failed!");
    }
    #endregion

    #region Save and load state
    public void ResetGame()
    {
        // prefs in game manager.
        pesos = 0;
        experience = 0;

        // clear inventory.
        inventoryManager.ClearInventory();

        // prefs in player datas.
        foreach (PlayerData data in charManager.characterDatas)
        {
            if (!data.isFirstAttach)
            {
                data.ResetPlayerData();
            }
        }

        PlayerPrefs.DeleteAll();
        SaveState();

        SceneManager.LoadScene("Main");
    }

    // save player when switching player or saving state.
    public void SavePlayer()
    {
        string p = "";

        p += currentPlayerData.HitPoint.ToString() + "|";
        p += currentPlayerData.Mana.ToString() + "|";

        PlayerPrefs.SetString("Save Player " + currentPlayer.name, p);

        Debug.Log("Save Player " + currentPlayer.name);
    }
    // save state when changing scene or engaging with save point.
    public void SaveState()
    {
        string s = "";

        s += "0" + "|";
        s += pesos.ToString() + "|";
        s += experience.ToString() + "|";
        SavePlayer();

        // first argument sets the key to access to our save state now
        PlayerPrefs.SetString("SaveState", s);

        Debug.Log("Save State");

    }

    // use to load neccessary codes when changing scene
    public void OnLoadState(Scene theScene, LoadSceneMode Mode)
    {

        // Spawn at spawn point
        currentPlayer.transform.position = GameObject.Find("SpawnPoint").transform.position;

    }
    // loading only when first scene
    public void LoadState(Scene theScene, LoadSceneMode Mode)
    {

        SceneManager.sceneLoaded -= LoadState;

        // if we are in the very beginning we wouldn't run anything
        if (!PlayerPrefs.HasKey("SaveState"))
            return;

        // using split to decompose the vector
        string[] generalData = PlayerPrefs.GetString("SaveState").Split("|");

        // Pesos
        pesos = int.Parse(generalData[1]);

        // experience
        experience = int.Parse(generalData[2]);

        Debug.Log("Load State");
    }
    // load player when first loading a player or switching player.
    public void LoadPlayer()
    {
        if (!PlayerPrefs.HasKey("Save Player " + currentPlayer.name))
        {
            currentPlayerData.HitPoint = currentPlayerData.MaxHitpoint.Value;
            currentPlayerData.Mana = currentPlayerData.MaxMana.Value;
            SavePlayer();
        }
        
        string[] playerData = PlayerPrefs.GetString("Save Player " + currentPlayer.name).Split("|");

        currentPlayerData.HitPoint = int.Parse(playerData[0]);
        currentPlayerData.Mana = int.Parse(playerData[1]);
        OnHealthChange();
    }
    #endregion

}
