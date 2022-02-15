using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Author : Simon Campbell
Category: Player - Items/Inventory - Inventory

Notes: None

Changelog:  15/2/22 - Cleaned document up and added comments fitting to CSharp standard.
**/


[CreateAssetMenu(menuName = "Directive/ScriptableObjects/Inventory/InventoryData", fileName = "New Inventory Data")]
///<summary>Scriptable Object version of PlayerInventory to allow for Data to be written to disk.</summary>
public class PlayerInventoryData : ScriptableObject
{

    [Header("Options")]
    ///<summary>Bool to control if we wipe this inventory on play ending. Currently for debugging + testing.</summary>
    public bool ClearOnPlayEnd;
    ///<summary>Bool to control if we revert this inventory back to it's inital state after play's end. Debug + testing.</summary>
    public bool RetainInitialData;

    [SerializeField]
    ///<summary>A list of the Items in this inventory.</summary>
    private List<ItemData> ItemData;

    ///<summary>The amount of credits the player currently has.</summary>
    public long Credits;

    ///<summary>Cached version of the items, for debugging + testing purposes.</summary>
    private List<ItemData> cachedItemData;

    ///<summary>Public property, return all items.</summary>
    public List<ItemData> Items
    {
        get { return ItemData; }
    }

    ///<summary>Credits a new blank inventory.</summary>
    public void CreateNewInventory()
    {
        ItemData = new List<ItemData>();
    }

    ///<summary>Caches this inventory's initial state. Debugging + testing.</summary>
    public void SaveInitialData()
    {
        if (RetainInitialData)
        {
            cachedItemData = new List<ItemData>();
            cachedItemData.AddRange(ItemData);
        }
    }

    ///<summary>Restores this inventory to it's cached state. Debugging + testing.</summary>
    public void RestoreInitialData()
    {
        if (RetainInitialData)
        {
            ItemData = new List<ItemData>();
            ItemData.AddRange(cachedItemData);
        }
    }

    ///<summary>Clears an inventory on the game ending. Debugging + testing.</summary>
    public void OnPlayEnd()
    {
        if (ClearOnPlayEnd)
        {
            CreateNewInventory();
        }
    }

    ///<summary>Adds an item from the inventory to a loadout slot.</summary>
    ///<param name="item">The item to add to a loadout.</param>
    ///<param name="index">Used for items that have more than one possible slot to go into. i.e, Weapons.</param>
    public void AddToLoadout(ItemData item, int index)
    {
        if (item != null)
        {
            ///Ranged Weapons
            if (item.ItemType == ItemType.PersonalRangedWeapon)
            {
                if (index == 0)
                {
                    _playerLoadout.RangedWeapon1 = (PersonalRangedWeaponData)item;
                }
                else if (index == 1)
                {
                    _playerLoadout.RangedWeapon2 = (PersonalRangedWeaponData)item;
                }
            }

            ///Melee Weapons
            if (item.ItemType == ItemType.PersonalMeleeWeapon)
            {
                _playerLoadout.MeleeWeapon = (PersonalMeleeWeaponData)item;
            }

            ///Armour
            if (item.ItemType == ItemType.PersonalUpperArmour)
            {
                _playerLoadout.UpperArmour = (PersonalUpperArmourData)item;
            }

            if (item.ItemType == ItemType.PersonalLowerArmour)
            {
                _playerLoadout.LowerArmour = (PersonalLowerArmourData)item;
            }

            if (item.ItemType == ItemType.PersonalHeadArmour)
            {
                _playerLoadout.HeadArmour = (PersonalHeadArmorData)item;
            }

            ///Systems
            if (item.ItemType == ItemType.PersonalThrusterPack)
            {
                _playerLoadout.ThrusterPack = (PersonalThrusterPackData)item;
            }

            if (item.ItemType == ItemType.PersonalLifeSupport)
            {
                _playerLoadout.LifeSupport = (PersonalLifeSupportData)item;
            }

            ///Gadgets
            if (item.ItemType == ItemType.PersonalGadget)
            {
                if (index == 0)
                {
                    _playerLoadout.Gadget1 = (PersonalGadgetData)item;
                }
                else
                {
                    if (index == 1)
                    {
                        _playerLoadout.Gadget2 = (PersonalGadgetData)item; ;
                    }
                    else
                    {
                        if (index == 2)
                        {
                            _playerLoadout.Gadget3 = (PersonalGadgetData)item; ;
                        }
                    }
                }
            }
        }

    }

    ///<summary>Removes an item from the loadout.</summary>
    ///<param name="item">The item to remove from a loadout.</param>
    ///<param name="index">Used for items that have more than one possible slot be taken from. i.e, Weapons.</param>
    public void RemoveFromLoadout(ItemData item, int index)
    {
        if (item != null)
        {

            ///Ranged Weapons
            if (item.ItemType == ItemType.PersonalRangedWeapon)
            {
                if (index == 0)
                {
                    _playerLoadout.RangedWeapon1 = null;
                }
                else if (index == 1)
                {
                    _playerLoadout.RangedWeapon2 = null;
                }
            }


            ///Melee Weapons
            if (item.ItemType == ItemType.PersonalMeleeWeapon)
            {
                _playerLoadout.MeleeWeapon = null;
            }

            ///Armour
            if (item.ItemType == ItemType.PersonalUpperArmour)
            {
                _playerLoadout.UpperArmour = null;
            }

            if (item.ItemType == ItemType.PersonalLowerArmour)
            {
                _playerLoadout.LowerArmour = null;
            }

            if (item.ItemType == ItemType.PersonalHeadArmour)
            {
                _playerLoadout.HeadArmour = null;
            }

            ///Systems
            if (item.ItemType == ItemType.PersonalThrusterPack)
            {
                _playerLoadout.ThrusterPack = null;
            }

            if (item.ItemType == ItemType.PersonalLifeSupport)
            {
                _playerLoadout.LifeSupport = null;
            }

            ///Gadgets
            if (item.ItemType == ItemType.PersonalGadget)
            {
                if (index == 0)
                {
                    _playerLoadout.Gadget1 = null;
                }
                else
                {
                    if (index == 1)
                    {
                        _playerLoadout.Gadget2 = null;
                    }
                    else
                    {
                        if (index == 2)
                        {
                            _playerLoadout.Gadget3 = null;
                        }
                    }
                }
            }
        }
    }

    ///</summary>Reference to the player's current loadout.</summary>
    public PlayerLoadout _playerLoadout;

    [System.Serializable]
    ///<summary>Serializable Data class for storing data on a player's loadout,</summary>
    public class PlayerLoadout
    {
        ///Weapons

        ///<summary>The player's main ranged weapon.</summary>
        public PersonalRangedWeaponData RangedWeapon1;

        ///<summary>The player's secondary ranged weapon.</summary>
        public PersonalRangedWeaponData RangedWeapon2;

        ///<summary>The player's melee weapon.</summary>
        public PersonalMeleeWeaponData MeleeWeapon;


        ///Armour
        ///<summary>The player's head armour.</summary>
        public PersonalHeadArmorData HeadArmour;

        ///<summary>The player's main body armour.</summary>
        public PersonalUpperArmourData UpperArmour;

        ///<summary>The player's lower body armour.</summary>
        public PersonalLowerArmourData LowerArmour;


        ///Systems
        ///<summary>The player's life support system.</summary>
        public PersonalLifeSupportData LifeSupport;

        ///<summary>The player's main thruster pack.</summary>
        public PersonalThrusterPackData ThrusterPack;


        ///Gadgets
        ///<summary>The player's main gadget.</summary>
        public PersonalGadgetData Gadget1;

        ///<summary>The player's secondary gadget.</summary>
        public PersonalGadgetData Gadget2;

        ///<summary>The player's tertiary gadget.</summary>
        public PersonalGadgetData Gadget3;
    }

}