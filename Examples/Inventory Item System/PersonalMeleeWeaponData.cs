using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/**
Author : Simon Campbell
Category: Player - Items/Inventory - Weapons - Usable

Notes: None

Changelog:  13/2/22 - Cleaned document up and added comments fitting to CSharp standard.
**/


///<summary>Class that contains all data relevant to an on-foot Melee Weapon Item</summary>
public class PersonalMeleeWeaponData : BaseWeaponData
{

    ///<summary>The kind of damage this weapon will output.</summary>
    public enum DamageType
    {
        Slice,
        Crush,
        Plasma,
        Ion,
        Phase

    }

    ///<summary>The kind of damage this weapon will output.</summary>
    public DamageType weaponType;


    ///<summary>Gets the Attriute display labels for tooltips.</summary>
    public override string[] GetAttributeLabels()
    {
        return new string[] { "Damage", "Damage Type: ", "Attack Speed: ", "Value: "};
    }

    ///<summary>Gets the Value display labels for tooltip attributes</summary>
    public override string[] GetAttributeValues()
    {

        return new string[] { $" {MinDamage.ToString()}  -  {MaxDamage.ToString()}", $"{weaponType.ToString()}", $"{FireRate.ToString()}", $"{ItemValue.ToString()}" };
    }



    // ----------- EDITOR ONLY CODE

#if UNITY_EDITOR

    ///<summary>Draws a block of fields to allow for Personal Melee Weapons to be edited</summary>
    public override void DrawItemEditorSpecifics()
    {
        EditorGUILayout.Space();

        EditorGUILayout.BeginVertical("box");

        weaponType = (PersonalMeleeWeaponData.DamageType)EditorGUILayout.EnumPopup("Weapon Type: ", weaponType);
        EditorGUILayout.Space();

        MinDamage = EditorGUILayout.IntField("Min Damage: ", MinDamage);
        EditorGUILayout.Space();

        MaxDamage = EditorGUILayout.IntField("Max Damage: ", MaxDamage);
        EditorGUILayout.Space();

        CritChance = EditorGUILayout.IntField("Critical Hit Rate: ", CritChance);
        EditorGUILayout.Space();

        FireRate = EditorGUILayout.FloatField("Fire Rate: ", FireRate);
        EditorGUILayout.Space();


        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }
#endif
}

// ---------- CUSTOM SERIALIZATION


///<summary>Class that holds a serializable form of a PersonalMeleeWeaponData</summary>
public class SerializedPersonalMeleeWeaponData : SerializedItemData
{
    public int MinDamage;
    public int MaxDamage;
    public float FireRate;
    public PersonalMeleeWeaponData.DamageType weaponType;
    public int CritChance;

    ///<summary>Empty Constructor - Required for Serialization</summary>
    public SerializedPersonalMeleeWeaponData() : base() { }

    ///<summary>Standard Constructor</summary>
    ///<param name="item">The PersonalMeleeWeaponData we wish to Serialize</param>
    public SerializedPersonalMeleeWeaponData(PersonalMeleeWeaponData item) : base(item)
    {
        MinDamage = item.MinDamage;
        MaxDamage = item.MaxDamage;
        FireRate = item.FireRate;
        weaponType = item.weaponType;
        CritChance = item.CritChance;
    }

    ///<summary>Creates a Concrete Item from Serialized Data</summary>
    public override ItemData ConvertSerializedDataToItemData()
    {
        PersonalMeleeWeaponData newItem = new PersonalMeleeWeaponData();

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

        newItem.MinDamage = MinDamage;
        newItem.MaxDamage = MaxDamage;
        newItem.FireRate = FireRate;
        newItem.weaponType = weaponType;
        newItem.CritChance = CritChance;
        return newItem;

    }

}
