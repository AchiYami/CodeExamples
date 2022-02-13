using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Author : Simon Campbell
Category: Player - Items/Inventory - Weapons - Usable

Notes: None

Changelog:  13/2/22 - Cleaned document up and added comments fitting to CSharp standard.
**/

///<summary>Interface for all Personal Weapons</summary>
public interface IPersonalWeapon 
{
    void Attack();

    void Reload();

}
