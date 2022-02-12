using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

/**
Author : Simon Campbell
Category: World Objects

Notes: None

Changelog:  10/2/22 - Cleaned document up and added comments fitting to CSharp standard.
**/

///<summary>
///Interface for InteractableEntitys, to ensure they all contain methods for
///the primary, secondary and exit interactions.
///</summary>
public interface InteractableEntity
{

    void OnPrimaryInteraction();
    void OnSecondaryInteraction();
    void OnExitInteraction();

}
