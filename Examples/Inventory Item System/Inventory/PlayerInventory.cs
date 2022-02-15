using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

/**
Author : Simon Campbell
Category: Player - Items/Inventory - Inventory

Notes: None

Changelog:  15/2/22 - Cleaned document up and added comments fitting to CSharp standard.
**/


[CreateAssetMenu(menuName = "Directive/Shared Data/PlayerInventory", fileName = "Player Inventory")]
///<summary>A class dedicated to holding information about the player's inventory.</summary>
public class PlayerInventory : MonoBehaviour, ISavableData
{

    ///<summary>Temporary placeholder string for the player's inventory saved data.</summary>
    private string _savePath = "/SavedData/Inventory/playerInventory.json";

    ///<summary>Static reference to allow for singleton access to the inventory.</summary>
    private static PlayerInventoryData _inventory;

    ///<summary>
    ///Public static reference ensures we only have one instance of the inventory,
    ///if it isn't present, it will be loaded, and then returned.
    ///</summary>
    public static PlayerInventoryData Inventory
    {
        get
        {
            if (_inventory == null)
            {
                _inventory = Resources.Load<PlayerInventoryData>("ScriptableObjects/Inventory/DefaultPlayerInventory");

                if (_inventory.Items == null)
                {
                    _inventory.CreateNewInventory();
                }
            }
            return _inventory;
        }
    }

    [SerializeField]
    ///<summary>A list of all items in the inventory.</summary>
    private List<ItemData> _inventoryList;

    ///<summary>Returns the list of all items from the inventory.</summary>
    public static List<ItemData> GetAllItems()
    {
        return Inventory.Items;
    }

    ///<summary>
    ///Builtin Unity Enable method. 
    ///Checks to see if we want to revert the inventory back to it's intial state on restarting the game
    ///</summary>
    private void OnEnable()
    {

        if (Inventory.RetainInitialData)
        {
            Inventory.SaveInitialData();
        }
    }

    // ----- RETRIEVAL METHODS ----- \\

    ///<summary>Returns a list of all items of a specified type from the inventory.</summary>
    ///<param name="itemType">The item type to search for</param> 
    public static List<ItemData> GetItemsOfType(ItemType itemType)
    {
        //Create a new list.
        List<ItemData> itemsToFind = new List<ItemData>();

        //Find the inventory list.
        List<ItemData> inventoryList = Inventory.Items;

        //Iterate through the inventory, if an item is of itemType, add it to the list.
        for (int i = 0; i < inventoryList?.Count; i++)
        {
            if (inventoryList[i]?.ItemType == itemType)
            {
                itemsToFind.Add(inventoryList[i]);
            }
        }
        return itemsToFind;
    }

    ///<summary>Returns a list of all items of specified types from the inventory.</summary>
    ///<param name="itemTypes">An array of item types to search for</param>
    public static List<ItemData> GetItemsOfType(ItemType[] itemTypes)
    {

        List<ItemData> returnList = new List<ItemData>();

        //Uses each Item Type in turn on GetItemsOfType(ItemType) method,
        //and adds them to a list.
        for (int i = 0; i < itemTypes.Length; i++)
        {
            returnList.AddRange(GetItemsOfType(itemTypes[i]));
        }

        return returnList;
    }

    ///<summary>Method to get an item from the inventory by it's display name.<summary>
    public static ItemData GetItem(string itemName)
    {

        for (int i = 0; i < Inventory?.Items.Count; i++)
        {
            if (itemName == Inventory?.Items[i].ItemName)
            {
                return Inventory.Items[i];
            }
        }
        throw new System.Exception("Directive Starscourge :: Item Not Found Exception");
    }

    ///<summary>Method to Add a single item to the inventory</summary>
    public static void AddItemToInventory(ItemData item)
    {
        if (item != null)
        {
            Inventory.Items.Add(item);
        }
    }

    ///<summary>Method to remove a single item from an inventory.</summary>
    public static void RemoveItemFromInventory(ItemData item)
    {
        Inventory.Items?.Remove(item);
    }

    ///<summary>Method to remove a single item from inventory using it's display name.</summary>
    public static void RemoveItemFromInventory(string itemName)
    {

        for (int i = 0; i < Inventory?.Items.Count; i++)
        {
            if (itemName == Inventory?.Items[i].ItemName)
            {
                Inventory.Items.RemoveAt(i);
                return;
            }
        }

        Debug.Log("DIRECTIVE LOGSCOURGE:: PlayerInventory :: RemoveItemFromiventory(String) :: No Item Found to Remove");
    }

    ///<summary>Method to check if the inventory contains an item.</summary>
    public static bool Contains(ItemData item)
    {
        return Inventory.Items.Contains(item);
    }

    // ----- SORTING METHODS ----- \\

    ///<summary>Sort the inventory by name.</summary>
    ///<param name="ascending">True = sort by ascending, False = sort by descending</param>
    public static List<ItemData> SortInventoryByName(bool ascending)
    {
        if (!ascending)
            return Inventory.Items.OrderBy(x => x.ItemName).ToList();

        return Inventory.Items.OrderByDescending(x => x.ItemName).ToList();
    }

    ///<summary>Sort Inventory by item type.</summary>
    ///<param name="ascending">True = sort by ascending, False = sort by descending</param>
    public static List<ItemData> SortInventoryByType(bool ascending)
    {
        if (ascending)
        {
            return Inventory.Items.OrderBy(x => x.GetThisItemTypeString()).ThenBy(y => y.ItemValue).ToList();
        }
        else
        {
            return Inventory.Items.OrderByDescending(x => x.GetThisItemTypeString()).ThenBy(y => y.ItemValue).ToList();
        }
    }

    ///<summary>Sort Inventory by it's value.</summary>
    ///<param name="ascending">True = sort by ascending, False = sort by descending</param>
    public static List<ItemData> SortInventoryByCost(bool ascending)
    {
        if (ascending)
        {
            return Inventory.Items.OrderBy(x => x.GetThisItemTypeString()).ToList();
        }
        else
        {
            return Inventory.Items.OrderByDescending(x => x.GetThisItemTypeString()).ToList();
        }
    }

    // ----- CREDIT METHODS ----- \\

    ///<summary>Add credits to the player inventory.</summary>
    ///<param name="creditsToAdd">The amount of credits to add.</param>
    public static void AddCredits(int creditsToAdd)
    {
        Inventory.Credits += creditsToAdd;
    }

    ///<summary>Add a large amount of credits to the players inventory.</summary>
    ///<param name="creditsToAdd">The amount of credits to add.</param>
    public static void AddCredits(long creditsToAdd)
    {
        Inventory.Credits += creditsToAdd;
    }

    ///<summary>Remove an amount of credits from a players inventory. Returns true if possible, false if not.</summary>
    ///<param name-"creditsToTake">The amount of credits to attempt to remove.</param>
    public static bool TakeCredits(int creditsToTake)
    {
        if (Inventory.Credits >= creditsToTake)
        {
            Inventory.Credits -= creditsToTake;
            return true;
        }
        return false;
    }

    ///<summary>Remove a large amount of credits from a players inventory. Returns true if possible, false if not.</summary>
    ///<param name-"creditsToTake">The amount of credits to attempt to remove.</param>
    public static bool TakeCredits(long creditsToTake)
    {
        if (Inventory.Credits >= creditsToTake)
        {
            Inventory.Credits -= creditsToTake;
            return true;
        }
        return false;
    }

    ///<summary>Checks to see if a player has enough credits</summary>
    ///<param name-"creditsToTake">The amount of credits to check.</param>
    public static bool HasEnoughCredits(int creditsToCheck)
    {
        if (Inventory.Credits >= creditsToCheck)
        {
            return true;
        }
        return false;
    }

    ///<summary>Checks to see if a player has enough credits</summary>
    ///<param name-"creditsToTake">The amount of credits to check.</param>
    public static bool HasEnoughCredits(long creditsToCheck)
    {
        if (Inventory.Credits >= creditsToCheck)
        {
            return true;
        }
        return false;
    }

    ///<summary>Helper method to remove all null items.</summary>
    public static void RemoveNulls()
    {
        Inventory.Items.RemoveAll(item => item == null);
    }

    ///<sumamry>Removes all item's from the inventory.</summary>
    public static void ClearInventory()
    {
        for (int i = Inventory.Items.Count - 1; i >= 0; i--)
        {
            RemoveItemFromInventory(Inventory.Items[i]);
        }
    }

    ///<summary>
    ///Builtin unity method.
    ///Will revert the inventory back to it's intial state if prompted to do so.
    ///</summary>
    public void OnDisable()
    {

        if (Inventory.RetainInitialData)
        {
            Inventory.RestoreInitialData();

        }
    }

    ///<summary>
    ///Builtin Unity Method.
    ///Will call all on end methods on the inventory backend.
    ///</summary>
    public void OnDestroy()
    {
        if (Inventory.ClearOnPlayEnd)
        {
            Inventory.OnPlayEnd();
        }
        else
        {
            //SaveData();
        }
    }

    ///<summary>Method to save the inventory to a JSON file.</summary>
    public void SaveData()
    {

        //Create Serializable Version of Items
        List<SerializedItemData> serializedItemList = new List<SerializedItemData>();

        //Add all current items into the list
        for (int i = 0; i < _inventoryList.Count; i++)
        {
            serializedItemList.Add(_inventoryList[i].GetSerializedData());
        }

        //Create the JSON Container
        InventoryData data = new InventoryData(serializedItemList, Inventory.Credits);
        var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
        string json = JsonConvert.SerializeObject(data, Formatting.Indented, settings);

        //Write file to disk
        File.WriteAllText(Application.dataPath + "/SavedData/Inventory/playerInventory.json", json);

    }

    ///<summary>Method to Load the Inventory Data from a JSON File.</summary>
    public void LoadData()
    {
        //Create the JSON Settings
        StreamReader file = File.OpenText(Application.dataPath + _savePath);
        var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };

        //Deserialize into Data
        InventoryData data = JsonConvert.DeserializeObject<InventoryData>(file.ReadToEnd(), settings);

        //Re-Add Items to Inventory
        for (int i = 0; i < data._items.Count; i++)
        {
            PlayerInventory.AddItemToInventory(data._items[i].ConvertSerializedDataToItemData());
        }

        Inventory.Credits = data._credits;


    }
}

[System.Serializable]

///<summary>A serialized version of the PlayerInventory data.</summary>
class InventoryData
{
    public List<SerializedItemData> _items;

    public long _credits;

    public InventoryData(List<SerializedItemData> inventory, long credits)
    {
        _items = inventory;
        _credits = credits;
    }
}
