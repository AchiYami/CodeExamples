using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//using Newtonsoft.Json;
using UnityEngine;


/**
Author : Simon Campbell
Category: Player - Items/Inventory - Item Database

Notes: None

Changelog:  15/2/22 - Cleaned document up and added comments fitting to CSharp standard.
**/

[System.Serializable]
[CreateAssetMenu(menuName = "Directive/Items/ItemDatabase/Create New Database", fileName = "ItemDatabase.asset")]

///<summary>Class to store all created Items for use within game.</summary>
public class ItemDatabase : MonoBehaviour
{
    ///<summary>Singleton static reference of the item database.</summary>
    private static ItemDatabase _instance;

    // ----- SHIP ITEMS :: LISTS ----- \\

    ///<summary>A list of all Ship Light Weapons</summary>
    private List<ItemData> ShipLightWeapons;

    ///<summary>A list of all Ship Medium Weapons</summary>
    private List<ItemData> ShipMediumWeapons;

    ///<summary>A list of all Ship Heavy Weapons</summary>
    private List<ItemData> ShipHeavyWeapons;

    ///<summary>A list of all Ship Prototype Weapons</summary>
    private List<ItemData> ShipPrototypeWeapons;



    ///<summary>A list of all Ship Primary Hull Plating</summary>
    private List<ItemData> ShipPrimaryHullPlating;

    ///<summary>A list of all Ship Secondary Hull Plating</summary>
    private List<ItemData> ShipSecondaryHullPlating;



    ///<summary>A list of all Ship Impulse Engines</summary>
    private List<ItemData> ShipImpulseEngines;

    ///<summary>A list of all Ship Thrusters</summary>
    private List<ItemData> ShipManuervingThrusters;

    ///<summary>A list of all Ship Afterburner Drives</summary>
    private List<ItemData> ShipAfterburnerDrives;

    ///<summary>A list of all Ship Phase Rift</summary>
    private List<ItemData> ShipPhaseRiftDrives;



    ///<summary>A list of all Ship Combat Shields</summary>
    private List<ItemData> ShipCombatShieldArrays;

    ///<summary>A list of all Ship Deflector Shields</summary>
    private List<ItemData> ShipDeflectorShieldArrays;



    ///<summary>A list of all Ship Offsensive Auxiliary Systems</summary>
    private List<ItemData> ShipOffensiveAuxSystems;

    ///<summary>A list of all Ship Defensive Auxiliary Systems</summary>
    private List<ItemData> ShipDefensiveAuxSystems;

    ///<summary>A list of all Ship Tactical Auxiliary</summary>
    private List<ItemData> ShipTacticalAuxSystems;



    ///<summary>A list of all Ship Short Range sensors</summary>
    private List<ItemData> ShipShortRangeSensorArrays;

    ///<summary>A list of all Ship Long Range Sensors</summary>
    private List<ItemData> ShipLongRangeSensorArrays;



    ///<summary>A list of all Ship Transponders</summary>
    private List<ItemData> ShipTransponders;

    ///<summary>A list of all Ship Power Supplies</summary>
    private List<ItemData> ShipPowerSupplies;

    ///<summary>A list of all Ship Life Support Modules</summary>
    private List<ItemData> ShipLifeSupport;

    ///<summary>A list of all Ship Consumables</summary>
    private List<ItemData> ShipConsumables;



    // ----- PERSONAL ITEMS :: LISTS ----- \\

    ///<summary>A list of all Personal Melee Weapons</summary>
    private List<ItemData> PersonalMeleeWeapons;

    ///<summary>A list of all Personal Ranged Weapons</summary>
    private List<ItemData> PersonalRangedWeapons;



    ///<summary>A list of all Personal Header Armour</summary>
    private List<ItemData> PersonalHeadArmor;

    ///<summary>A list of all Personal Body Armour</summary>
    private List<ItemData> PersonalUpperArmour;

    ///<summary>A list of all Person Leg Armour</summary>
    private List<ItemData> PersonalLowerArmour;



    ///<summary>A list of all Personal Life Suppor Modules</summary>
    private List<ItemData> PersonalLifeSupport;

    ///<summary>A list of all Personal Thruster Pack Modules</summary>
    private List<ItemData> PersonalThrusterPack;



    ///<summary>A list of all Personal Gadgets</summary>
    private List<ItemData> PersonalGadgets;

    ///<summary>A list of all Personal Consumables</summary>
    private List<ItemData> PersonalConsumables;



    // ---- MISC ITEMS :: LISTS ----- \\

    ///<summary>A list of all Cargo Items</summary>
    private List<ItemData> CargoItems;

    ///<summary>A list of all Material Items</summary>
    private List<ItemData> Materials;

    ///<summary>A list of all Junk Items</summary>
    private List<ItemData> Junk;



    // ----- ITEM COMPONENTS :: DICTIONARY ----- \\

    ///<summary>A Dictionary containing all the Firing Data for Ship based Weapons</summary>
    public Dictionary<string, WeaponFireData> ShipWeaponFiringData;

    ///<summary>The asset filepath for Weapon Fire Data.</summary>
    private string _shipWeaponFiringDataPath = "ScriptableObjects/Weapon Fire Data";


    ///<summary>A dictionary containing all the GameObject Projectiles for Ship Weapons</summary>
    public Dictionary<string, GameObject> ShipWeaponProjectiles;

    ///<summary>The asset filepath for Ship projectiles.</summary>
    private string _shipWeaponprojectilePath = "Prefabs/Weapons/Projectiles/Ship/";


    ///<summary>A dictionary containing all the weapon sounds for ships.</summary>
    public Dictionary<string, DirectiveAudioData> ShipWeaponSounds;

    ///<summary>The asset filepath for ship weapon audio.</summary>
    private string _shipWeaponSoundsPath = "ScriptableObjects/Audio";




    // ------ ITEM COMPONENTS :: ENUMS
    public enum ProjectileType
    {
        SHIP,
        PERSONAL
    }
    public enum AudioType
    {
        WeaponsFire,
    }
    public enum FiringType
    {
        Ship,
        Personal
    }

    // ------ INITIALIZATION ----- \\

    ///<summary>Builtin Unity method called before the scene starts.</summary>
    void Awake()
    {
        InitializeLists();
        LoadAllItems();
        LoadAllItemComponents();
        //	LoadCustomItems();
    }

    ///<summary>Initializes each kind of item list, and creates the singleton intance link.</summary>
    void InitializeLists()
    {

        if (_instance == null)
        {
            _instance = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDatabase>();
        }

        // WEAPONS //
        ShipLightWeapons = new List<ItemData>();
        ShipMediumWeapons = new List<ItemData>();
        ShipHeavyWeapons = new List<ItemData>();
        ShipPrototypeWeapons = new List<ItemData>();

        // DEFENSIVE //
        ShipPrimaryHullPlating = new List<ItemData>();
        ShipSecondaryHullPlating = new List<ItemData>();
        ShipCombatShieldArrays = new List<ItemData>();
        ShipDeflectorShieldArrays = new List<ItemData>();

        // PROPULSION //
        ShipImpulseEngines = new List<ItemData>();
        ShipAfterburnerDrives = new List<ItemData>();
        ShipManuervingThrusters = new List<ItemData>();
        ShipPhaseRiftDrives = new List<ItemData>();

        // SENSORS //
        ShipShortRangeSensorArrays = new List<ItemData>();
        ShipLongRangeSensorArrays = new List<ItemData>();
        ShipTransponders = new List<ItemData>();

        // CRITICAL //
        ShipLifeSupport = new List<ItemData>();
        ShipPowerSupplies = new List<ItemData>();

        // AUXILIARY //
        ShipOffensiveAuxSystems = new List<ItemData>();
        ShipDefensiveAuxSystems = new List<ItemData>();
        ShipTacticalAuxSystems = new List<ItemData>();

        ShipConsumables = new List<ItemData>();


        PersonalMeleeWeapons = new List<ItemData>();
        PersonalRangedWeapons = new List<ItemData>();
        PersonalHeadArmor = new List<ItemData>();
        PersonalUpperArmour = new List<ItemData>();
        PersonalLowerArmour = new List<ItemData>();
        PersonalThrusterPack = new List<ItemData>();
        PersonalLifeSupport = new List<ItemData>();
        PersonalGadgets = new List<ItemData>();
        PersonalConsumables = new List<ItemData>();

        CargoItems = new List<ItemData>();
        Materials = new List<ItemData>();
        Junk = new List<ItemData>();
    }


    ///<summary>Loads all items of all item types into the database.</summary>
    void LoadAllItems()
    {
        for (int i = 0; i < System.Enum.GetNames(typeof(ItemType)).Length; i++)
        {
            LoadItemsIntoDatabase((ItemType)i);
        }
    }

    ///<summary>Loads the Weapon projectiles, firing data and sounds into the database.</summary>
    void LoadAllItemComponents()
    {
        //Initialize each dictionary.
        ShipWeaponProjectiles = new Dictionary<string, GameObject>();
        ShipWeaponSounds = new Dictionary<string, DirectiveAudioData>();
        ShipWeaponFiringData = new Dictionary<string, WeaponFireData>();

        //Load all the projectiles from their filepath.
        GameObject[] newProjectiles = Resources.LoadAll<GameObject>(_shipWeaponprojectilePath);

        //Add each projectile to the dictionary.
        foreach (GameObject newProjectile in newProjectiles)
        {
            if (newProjectile.TryGetComponent<ItemDatabaseToken>(out ItemDatabaseToken token))
            {
                ShipWeaponProjectiles.Add(token.TokenString, newProjectile);
            }
            else
            {
                Debug.LogError("DIRECTIVE LOGSCOURGE:: ERROR :: ITEMDATABASE :: LOADALLITEMCOMPONENTS :: PROJECTILE DOES NOT CONTAIN AN ITEMDATABASE TOKEN");
            }
        }

        //Load all sounds from their filepath.
        DirectiveAudioData[] newAudioData = Resources.LoadAll<DirectiveAudioData>(_shipWeaponSoundsPath);

        //Add each sound to the dictionary.
        foreach (DirectiveAudioData audioData in newAudioData)
        {
            //	Debug.Log($"Adding {audioData.TokenString} to the Item Database. ");
            //	ShipWeaponSounds.Add(audioData.TokenString, audioData);
        }

        //Load all firing data objects from their filepath.
        WeaponFireData[] newFireData = Resources.LoadAll<WeaponFireData>(_shipWeaponFiringDataPath);

        //Add each of them to the dictionary.
        foreach (WeaponFireData fireData in newFireData)
        {
            Debug.Log($"Adding {fireData.TokenString} to the Item Database. ");
            ShipWeaponFiringData.Add(fireData.TokenString, fireData);
        }

    }

    ///<summary>Load all items of a specified type to the database.</summary>
    ///<param name="type">The itemType to add items by.</param>
    void LoadItemsIntoDatabase(ItemType type)
    {

        ItemData[] itemsToAdd = Resources.LoadAll<ItemData>("Item Database/" + DirectiveEnumUtils.GetPrettyItemType(type));
        GetList(type).AddRange(itemsToAdd);

    }

    ///<summary>Debug method to allow and test for adding custom weapons from a JSON file.</summary>
    void LoadCustomItems()
    {
        string filePath = Application.persistentDataPath + "/Mods/TestDebug.json";

        StreamReader io = new StreamReader(filePath);
        string json = io.ReadToEnd();
        //JsonSerializerSettings settings = new JsonSerializerSettings();
        //  settings.TypeNameHandling = TypeNameHandling.All;
        //  SerializedWeaponData data = JsonConvert.DeserializeObject<SerializedWeaponData>(json, settings);

        //  WeaponData customWeapon = (WeaponData)data.ConvertSerializedDataToItemData();
        //   PlayerInventory.AddItemToInventory(customWeapon);

        io.Close();
    }

    // ----- GETTERS ----- \\

    ///<summary>Gets a List of items of a specified type.</summary>
    ///<param name="type">The type of items we want to retrieve.</param>ho
    private List<ItemData> GetList(ItemType type)
    {
        switch (type)
        {
            // SHIP WEAPONS //
            case ItemType.ShipLightWeapon:
                return ShipLightWeapons;
            case ItemType.ShipHeavyWeapon:
                return ShipHeavyWeapons;
            case ItemType.ShipMediumWeapon:
                return ShipMediumWeapons;
            case ItemType.ShipPrototypeWeapon:
                return ShipPrototypeWeapons;

            // AUXILIARY SYSTEMS //
            case ItemType.ShipTacticalAuxiliarySystem:
                return ShipTacticalAuxSystems;
            case ItemType.ShipDefensiveAuxiliarySystem:
                return ShipDefensiveAuxSystems;
            case ItemType.ShipOffensiveAuxiliarySystem:
                return ShipOffensiveAuxSystems;

            // PROPULSIONS //
            case ItemType.ShipImpulseEngine:
                return ShipImpulseEngines;
            case ItemType.ShipManuveringThrusters:
                return ShipManuervingThrusters;
            case ItemType.ShipAfterburnerDrive:
                return ShipAfterburnerDrives;
            case ItemType.ShipPhaseRiftDrive:
                return ShipPhaseRiftDrives;

            // DEFENSIVE //
            case ItemType.ShipPrimaryHullPlating:
                return ShipPrimaryHullPlating;
            case ItemType.ShipSecondaryHullPlating:
                return ShipSecondaryHullPlating;
            case ItemType.ShipCombatShields:
                return ShipCombatShieldArrays;
            case ItemType.ShipDeflectorShields:
                return ShipDeflectorShieldArrays;

            // SENSORS //
            case ItemType.ShipShortRangeSensors:
                return ShipShortRangeSensorArrays;
            case ItemType.ShipLongRangeSensors:
                return ShipLongRangeSensorArrays;
            case ItemType.ShipTransponder:
                return ShipTransponders;

            // CRITICAL //
            case ItemType.ShipPowerSupply:
                return ShipPowerSupplies;
            case ItemType.ShipLifeSupport:
                return ShipLifeSupport;

            // MISC //
            case ItemType.Junk:
                return Junk;
            case ItemType.Material:
                return Materials;

            //PERSONAL //
            case ItemType.PersonalHeadArmour:
                return PersonalHeadArmor;
            case ItemType.PersonalUpperArmour:
                return PersonalUpperArmour;
            case ItemType.PersonalLowerArmour:
                return PersonalLowerArmour;
            case ItemType.PersonalConsumable:
                return PersonalConsumables;
            case ItemType.PersonalGadget:
                return PersonalGadgets;
            case ItemType.PersonalMeleeWeapon:
                return PersonalMeleeWeapons;
            case ItemType.PersonalRangedWeapon:
                return PersonalRangedWeapons;
            case ItemType.PersonalLifeSupport:
                return PersonalLifeSupport;
            case ItemType.PersonalThrusterPack:
                return PersonalThrusterPack;
            case ItemType.Cargo:
                return CargoItems;
            case ItemType.ShipConsumable:
                return ShipConsumables;

            default:
                throw new System.Exception("DIRECTIVE LOGSCOURGE :: ERROR : ITEM DATABASE : GETLIST(ITEMTYPE) INVALID ITEM TYPE SPECIFIED :: " + type.ToString());

        }
    }

    // ----- ITEM COMPONENT RETREIVAL ---- \\

    ///<summary>Get a Projectile of a certain type and name.</summary>
    ///<param name="type">The type of projectile to get.</param>
    ///<param name="token">The string token of the projectile we are tryting to get.</param>
    public static GameObject GetProjectile(ProjectileType type, string token)
    {

        //If we are searching for a ship projectile...
        if (type == ProjectileType.SHIP)
        {
            //try to get it from the dictionary.
            if (_instance.ShipWeaponProjectiles.TryGetValue(token, out GameObject projectile))
            {
                return projectile;
            }
            else//else return an error.
            {
                Debug.LogError("DIRECTIVE LOGSCOURGE :: ERROR :: ITEMDATABASE GETPROJECTILE :: NO PROJECTILE FOUND WITH TOKEN " + token);
                throw new System.Exception("ItemNotInDatabaseException");
            }
        }
        else
        {
            throw new System.Exception("ItemNotInDatabaseException");
        }
    }


    ///<summary>Get a sound of a certain type and name.</summary>
    ///<param name="type">The type of sound to get.</param>
    ///<param name="token">The string token of the sound we are tryting to get.</param>
        public static DirectiveAudioData GetAudioData(AudioType type, string token)
    {

        //If the audio is for firing a weapon
        if (type == AudioType.WeaponsFire)
        {
            //Try and get it...
            if (_instance.ShipWeaponSounds.TryGetValue(token, out DirectiveAudioData audio))
            {
                return audio;
            }
            else//else return an error.
            {
                Debug.LogError("DIRECTIVE LOGSCOURGE :: ERROR :: ITEMDATABASE GETAUDIODATA :: NO AUDIODATA FOUND WITH TOKEN " + token);
                throw new System.Exception("ItemNotInDatabaseException");
            }
        }
        else
        {
            throw new System.Exception("ItemNotInDatabaseException");
        }
    }



    ///<summary>Get FiringData of a certain type and name.</summary>
    ///<param name="type">The type of FiringData to get.</param>
    ///<param name="token">The string token of the FiringData we are tryting to get.</param>
    public static WeaponFireData GetFiringData(FiringType type, string token)
    {
        //If it's a ship weapon...
        if (type == FiringType.Ship)
        {
            //Try and get it from the dictionary....
            if (_instance.ShipWeaponFiringData.TryGetValue(token, out WeaponFireData data))
            {
                return data;
            }
            else//else throw an error.
            {
                Debug.LogError("DIRECTIVE LOGSCOURGE :: ERROR :: ITEMDATABASE GETFIRINGDATA :: NO FIRING DATA FOUND WITH TOKEN " + token);
                throw new System.Exception("ItemNotInDatabaseException");
            }
        }
        else
        {
            throw new System.Exception("ItemNotInDatabaseException");
        }
    }
}
