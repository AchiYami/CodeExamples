using System.Collections;
using System.Collections.Generic;
//using Newtonsoft.Json;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
#endif

/**
Author : Simon Campbell
Category: Player - Items/Inventory - Item

Notes: None

Changelog:  15/2/22 - Cleaned document up and added comments fitting to CSharp standard.
**/


#region EXTERNAL ENUMS

///<summary>Enum for describing which brand this item was made by.</summary>
public enum ItemManufacturer
{
    TENLOSS_CORPORATION,
    MOTARA_CORPORATION,
    CHITEK_SYSTEMS,
    SINTARA_DESIGNS,
    ARKANGEL_MUNITIONS,
    UNKNOWN
}

///<summary>Enum to describe the type of Item this is.</summary>
public enum ItemType
{
    ShipLightWeapon,
    ShipMediumWeapon,
    ShipHeavyWeapon,
    ShipPrototypeWeapon,
    ShipImpulseEngine,
    ShipManuveringThrusters,
    ShipAfterburnerDrive,
    ShipPhaseRiftDrive,
    ShipPrimaryHullPlating,
    ShipSecondaryHullPlating,
    ShipCombatShields,
    ShipDeflectorShields,
    ShipOffensiveAuxiliarySystem,
    ShipDefensiveAuxiliarySystem,
    ShipTacticalAuxiliarySystem,
    ShipPowerSupply,
    ShipLifeSupport,
    ShipShortRangeSensors,
    ShipLongRangeSensors,
    ShipTransponder,
    ShipConsumable,
    PersonalMeleeWeapon,
    PersonalRangedWeapon,
    PersonalHeadArmour,
    PersonalLowerArmour,
    PersonalUpperArmour,
    PersonalThrusterPack,
    PersonalLifeSupport,
    PersonalConsumable,
    PersonalGadget,
    Cargo,
    Material,
    Junk
}

///<summary>Item Rarity Level</summary>
public enum Rarity
{
    Trash, //Gray
    Common, //White
    Uncommon, //Green
    Rare, //Blue
    Mythic, //Purple
    Legendary, //Yellow
    Set
}
#endregion

[System.Serializable]
///<summary>Class to hold all generic data for all items.</summary>
public class ItemData : ScriptableObject
{

    [Header("Information")]
    ///<summary>The Display name of the Item.</summary>
    public string ItemName;

    ///<summary>Integer representation of this item</summary>
    public int ItemID;

    ///<summary>Display name for brand of this item.</summary>
    public string ItemBrand;

    ///<summary>Shorthand string to represent this item.</summary>
    public string ItemSlug;

    ///<summary>Display friendly description of this item.</summary>
    public string ItemDescription;

    [Header("Visuals")]
    ///<summary>The display image for this item.<summary>
    public Sprite ItemImage;
    ///<summary>The filepath to the image for this item.</summary>
    public string ItemImagePath;

    [Header("Data")]
    ///<summary>Enum representation of the type of this item.</summary>
    public ItemType ItemType;
    ///<summary>Enum representation of the rarity level of this item.</summary>
    public Rarity ItemRarity;
    ///<summary>The power level of this item.</summary>
    public int ItemLevel;
    ///<summary>The currency value of this item</summary>
    public long ItemValue;
    ///<summary>The amount of power an item may consume</summary>
    public int PowerConsumption;


    [Header("Inventory Stacking")]
    ///<sumary>The maximum amount of this item that can be placed on a stack.</summary>
    public int MaxStackAmount;
    public int CurrentStackAmount;

    ///Utility Variables

    ///<summary>The enum representation of this item's brand.</summary>
    ItemManufacturer brandChoice;

    ///<summary>Empty default constructor</summary>
    public ItemData() { }


    [Header("Item Bonuses")]
    ///<summary>An array for containing any bonuses this item may have</summary>
    public ItemBonus[] ItemBonuses;


    // ----- GENERATORS ----- \\
    ///<summary>Generates a random rarity for this item.</summary>
    public void GenerateItemRarity()
    {
        this.ItemRarity = (Rarity)Random.Range(0, System.Enum.GetNames(typeof(Rarity)).Length);
    }

    ///<summary>Generates a random Item Brand for this item.</summary>
    public void GenerateItemBrand()
    {
        this.ItemBrand = ((ItemManufacturer)Random.Range(0, (System.Enum.GetNames(typeof(ItemManufacturer))).Length)).ToString();
    }

    // ----- GETTERS ----- \\
    #region GETTERS
    ///<summary>Returns the display friendly name of an item type.</summary>
    public static string GetItemTypeName(ItemType _type)
    {
        return DirectiveEnumUtils.GetPrettyItemType(_type);
    }

    ///<summary>Returns the display friendly name of this item's type.</summary>
    public virtual string GetThisItemTypeString()
    {
        return DirectiveEnumUtils.GetPrettyItemType(ItemType);
    }

    ///<summary>Fall back method for returning display friendly strings of an item's attributes.</summary>
    public virtual string[] GetAttributeLabels()
    {
        return new string[] { "NO IMPLEMENTATION" }; ;
    }

    ///<summary>Fallback method for returning display friendly strings of an item's attribute values.</summary>
    public virtual string[] GetAttributeValues()
    {
        return new string[] { ItemValue.ToString() };
    }
    #endregion

    // ----- EDITOR ONLY CODE ----- \\
    #region EDITORONLY

    ///<summary>Item Editor method to create a new entry in the Item Bonus array.</summary>
    public void IncreaseBonusCount()
    {

        Debug.Log("Enter Increase");

        if (ItemBonuses == null)
        {
            Debug.Log("Item Bonus is Null.");
            ItemBonuses = new ItemBonus[1];
        }
        else
        {
            Debug.Log("Item Bonus Not NUll");
            ItemBonus[] cachedBonuses = ItemBonuses;
            ItemBonuses = new ItemBonus[ItemBonuses.Length + 1];

            Debug.Log("Cloning Old Item Bonuses");
            for (int i = 0; i < cachedBonuses.Length; i++)
            {
                ItemBonuses[i] = cachedBonuses[i];
            }
        }

    }

    ///<summary>Item Editor method to remove a field from the Item bonuses.</summary>
    public void DecreaseBonusCount()
    {
        if (ItemBonuses != null)
        {
            if (ItemBonuses.Length > 0)
            {
                ItemBonuses = new ItemBonus[ItemBonuses.Length - 1];
            }
        }
    }

    ///<summary>Item Editor method to create a new instance of this item.</summary> 
    public virtual ItemData CreateInstanceOf()
    {
        ItemData newInstance = Instantiate(this);
        newInstance.name = ItemName;
        return newInstance;
    }

#if UNITY_EDITOR

    [Header("EDITOR ONLY")]

    ///<summary>Control variable as to whether to expand this item's foldout</summary>
    public bool expanded = false;

    ///<summary>
    ///Editor Only code for drawing the top panel in the Item Editor,
    ///which holds the Item's header info.
    ///</summary>
    public virtual void DrawItemInspector()
    {

        EditorGUILayout.BeginHorizontal("box");
        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("Item Name: " + ItemName);
        EditorGUILayout.LabelField("Item Rarity: " + ItemRarity);
        EditorGUILayout.LabelField("Item:  " + ItemType);

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        for (int i = 0; i < GetAttributeLabels().Length; i++)
        {
            EditorGUILayout.LabelField(GetAttributeLabels()[i] + GetAttributeValues()[i]);
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    ///<summary>Editor Only code for drawing the item's generic data
    public virtual void DrawItemEditorGenerics()
    {

        //Begins the overall layout
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Generic Item Data");
        EditorGUILayout.BeginVertical("box");

        //Name Block - Draw and allow editing of this item's name.
        ItemName = EditorGUILayout.TextField(new GUIContent("Name: "), ItemName);
        name = ItemName;
        EditorGUILayout.Space();

        //Image Block - Draw and allow the editing of this item's image.
        EditorGUILayout.BeginHorizontal();
        ItemImage = (Sprite)EditorGUILayout.ObjectField(new GUIContent("Image: "), ItemImage, typeof(Sprite), false);
        ItemImagePath = AssetDatabase.GetAssetPath(ItemImage);
        ItemImagePath = ItemImagePath.Replace("Assets/Resources/", "");
        EditorGUILayout.Space();

        //Item Brand Block - Draw and allow the editing of this item's brand.
        EditorGUILayout.BeginVertical();
        brandChoice = (ItemManufacturer)EditorGUILayout.EnumPopup("Brand: ", brandChoice);
        ItemBrand = DirectiveEnumUtils.GetPrettyBrandName(brandChoice);
        EditorGUILayout.Space();

        //Item Rarity Block - Draw and allow the editing of this item's rarity.
        ItemRarity = (Rarity)EditorGUILayout.EnumPopup(new GUIContent("Rarity: "), ItemRarity);
        EditorGUILayout.Space();

        //Placeholder warning for yet to be implemented item sets.
        if (ItemRarity == Rarity.Set)
        {
            EditorGUILayout.LabelField("Item Set : Not yet implemented.");
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        //Power Consumption Block - Draw and allow editing of the item's power consumption
        PowerConsumption = EditorGUILayout.IntField("Power Consumption : ", PowerConsumption);
        EditorGUILayout.Space();

        //Value Block - Draw and allow the editing of the item's value.
        ItemValue = EditorGUILayout.LongField("Item Value: ", ItemValue);
        EditorGUILayout.Space();

        //End the layout.
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndVertical();
    }

    ///<summary>Editor Only method to draw and allow ediitng of the item's description and bonuses.
    public virtual void DrawItemEditorDescriptionAndBonuses()
    {
        //Wrap the text, as the description can be a large string.
        EditorStyles.textField.wordWrap = true;

        //Draw the overall layout,
        EditorGUILayout.BeginHorizontal("box");

        //Create a reasonable amount of space for the text area.
        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true), GUILayout.Width(300));

        //Item Description block - allows the drawing and editing of this item's description.
        EditorGUILayout.LabelField("Item Description");
        ItemDescription = EditorGUILayout.TextArea(ItemDescription, GUILayout.Height(150), GUILayout.ExpandWidth(true));

        //Item Bonuses block - allows the drawing and editing of this item's bonuses.
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Item Bonuses: ");

        //Create a pair of buttons to add and remove bonuses.
        if (GUILayout.Button("+"))
        {
            IncreaseBonusCount();
        }
        if (GUILayout.Button("-"))
        {
            DecreaseBonusCount();
        }

        //Close the overall layout
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

    }

    ///<summary>Method to allow the drawing and editing of values specific to each item type.</summary>
    public virtual void DrawItemEditorSpecifics()
    {

        //Left as a fallback, as there should be no item that stays as just "ItemData".
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("DEFAULT ITEMDATA :: FALLBACK");
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }
#endif

    ///<summary>Method to return a serialized version of this item.</summary>
    public virtual SerializedItemData GetSerializedData()
    {
        SerializedItemData _serializedItem = new SerializedItemData(this);
        return _serializedItem;
    }
    #endregion
}

///<summary>A serialized version of the itemData class, to be used with saving and loading of inventory data.</summary>
public class SerializedItemData
{

    ///<summary>The display name of this item.</summary>
    public string ItemName;
    ///<summary>The display name of this item's brand.<summary>
    public string ItemBrand;
    ///<summary>A shorthand string to represent this item.<summary>
    public string ItemSlug;
    ///<summary>Display friendly description of this item.<summary>
    public string ItemDescription;
    ///<summary>The filepath to the image of this item.</summary>
    public string ItemImagePath;
    ///<summary>The enum representation of this item's type.</summary>
    public ItemType ItemType;
    ///<summary>The enum repreesntation of this item's rarity.</summary>
    public Rarity ItemRarity;
    ///<summary>The power level of this item.</summary>
    public int ItemLevel;
    ///<summary>The maximum amount of items that can be stacked in the inventory.</summary>you
    public int MaxStackAmount;
    ///<summary>The current amount of items within the stack.</summary>
    public int CurrentStackAmount;
    ///<summary>The currency value of this item.</summary>
    public long ItemValue;
    ///<summary>The amount of power this item will consume.</summary>
    public int PowerConsumption;
    ///<summary>An integer representation of this item.</summary>
    public int ItemID;
    ///<summary>An array of bonuses this item has.<summary>
    public ItemBonus[] ItemBonuses;

    ///<summary>Empty constructor, necessary for serialization.<summary>
    public SerializedItemData() { }

    ///<summary>Constructor that takes in an item to create a serialized version of.</summary>
    ///<param name="item">The item to serialize.</param>
    public SerializedItemData(ItemData item)
    {

        ItemName = item.ItemName;
        ItemBrand = item.ItemBrand;
        ItemSlug = item.ItemSlug;
        ItemDescription = item.ItemDescription;
        ItemImagePath = item.ItemImagePath;
        ItemType = item.ItemType;
        ItemRarity = item.ItemRarity;
        ItemLevel = item.ItemLevel;
        MaxStackAmount = item.MaxStackAmount;
        CurrentStackAmount = item.CurrentStackAmount;
        ItemValue = item.ItemValue;
        PowerConsumption = item.PowerConsumption;
        ItemID = item.ItemID;
        ItemBonuses = item.ItemBonuses;

    }

    ///<summary>Method to convert a serialized itemData instance into a concrete ItemData</summary>FUCK
    public virtual ItemData ConvertSerializedDataToItemData()
    {
        ItemData newItem = new ItemData();

        newItem.ItemName = ItemName;
        newItem.ItemBrand = ItemBrand;
        newItem.ItemSlug = ItemSlug;
        newItem.ItemDescription = ItemDescription;
        newItem.ItemImagePath = DirectiveFileUtils.StripFileExtension(ItemImagePath);
        newItem.ItemImage = Resources.Load<Sprite>(newItem.ItemImagePath) as Sprite;
        newItem.ItemType = ItemType;
        newItem.ItemRarity = ItemRarity;
        newItem.ItemLevel = ItemLevel;
        newItem.MaxStackAmount = MaxStackAmount;
        newItem.CurrentStackAmount = CurrentStackAmount;
        newItem.ItemValue = ItemValue;
        newItem.PowerConsumption = PowerConsumption;
        newItem.ItemID = ItemID;
        newItem.ItemBonuses = ItemBonuses;

        return newItem;
    }
}
