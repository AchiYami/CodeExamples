using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/**
Author : Simon Campbell
Category: Player - Items/Inventory - Weapons - Usable

Notes: None

Changelog:  13/2/22 - Cleaned document up and added comments fitting to CSharp standard.
**/

///<summary>Base Class for All Weapons to draw from.</summary>
public class BaseWeaponData : ItemData
{
    [Header("Weapon Data")]
    ///<summary>The minimum amount of damage this weapon can do per hit.</summary>
    public int MinDamage;
    ///<summary>The maximum amount of damage this weapon can do per hit.</summary>
    public int MaxDamage;
    ///<summary>The chance this weapon has to score a critical hit.</summary>
    public int CritChance;

    ///<summary>The amount of time between shots for ranged weapons, and swings for melee weapons.</summary>
    public float FireRate;

    ///<summary>The amount of time it takes to reload a weapon.</summary>
    public float ReloadTime;

}
