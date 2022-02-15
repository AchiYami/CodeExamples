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

public class PersonalRangedWeaponData : BaseWeaponData
{

    ///<summary>The kind of firing mode this weapon is in.</summary>
    public enum FiringMode
    {
        SemiAutomatic,
        BurstFire,
        Automatic
    }

    ///<summary>The kind of firing mode this weapon is in.</summary>
    public FiringMode FireMode;

    ///<summary>The maximum ammount of ammo this weapon can hold</summary>
    public int MaxCapacity;

    ///<summary>The accuracy rating of this weapon.</summary>
    public int Accuracy;

    ///<summary>Gets the Attriute display labels for tooltips.</summary>
    public override string[] GetAttributeLabels()
    {
        return new string[] { "Damage", "Fire Mode", "Capacity", "FireRate", "Accuracy", "CritChance" }; ;
    }

    ///<summary>Gets the Value display labels for tooltip attributes</summary>
    public override string[] GetAttributeValues()
    {
        return new string[] { $"{MinDamage} - {MaxDamage}", $"{FireMode}", $"{MaxCapacity}", $"{FireRate}", $"{Accuracy}", $"{CritChance}" };
    }


// ----------- EDITOR ONLY CODE


#if UNITY_EDITOR

    ///<summary>Draws a block of fields to allow for Personal Ranged Weapons to be edited</summary>
    public override void DrawItemEditorSpecifics()
    {
        EditorGUILayout.Space();

        EditorGUILayout.BeginVertical("box");

        FireMode = (PersonalRangedWeaponData.FiringMode)EditorGUILayout.EnumPopup("Fire Mode: ", FireMode);
        EditorGUILayout.Space();

        MinDamage = EditorGUILayout.IntField("Min Damage: ", MinDamage);
        EditorGUILayout.Space();

        MaxDamage = EditorGUILayout.IntField("Max Damage: ", MaxDamage);
        EditorGUILayout.Space();

        CritChance = EditorGUILayout.IntField("Critical Hit Rate: ", CritChance);
        EditorGUILayout.Space();

        FireRate = EditorGUILayout.FloatField("Fire Rate: ", FireRate);
        EditorGUILayout.Space();

        Accuracy = EditorGUILayout.IntField("Accuracy: ", Accuracy);
        EditorGUILayout.Space();

        MaxCapacity = EditorGUILayout.IntField("Max Capacity: ", MaxCapacity);
        EditorGUILayout.Space();

        ReloadTime = EditorGUILayout.FloatField("Reload Time: ", ReloadTime);
        EditorGUILayout.Space();

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }
#endif
}

