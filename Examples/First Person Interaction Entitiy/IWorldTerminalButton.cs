using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Author : Simon Campbell
Category: World Objects - First Person

Notes: None

Changelog:  10/2/22 - Cleaned document up and added comments fitting to CSharp standard.
**/

///<summary>
///Interface that ensures any button designed to be placed on a World Terminal follows
///the same rules to Activate upon being pressed.
///</summary>
public interface IWorldTerminalButton 
{
    void Activate(GenericWorldTerminal display);
}
