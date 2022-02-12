using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
Author : Simon Campbell
Category: World Objects - First Person - UI

Notes: None

Changelog:  10/2/22 - Cleaned document up and added comments fitting to CSharp standard.
**/

///<summary>
///Abstract base class that all variants of the In Game Context UI should draw from.
///<summary>
public abstract class ContextSensitiveUIConfiguration : MonoBehaviour
{

    ///<summary>The root of the UI Transform that content is under</summary>
    public Transform ContentPanel;

    ///<summary>Contains data representing the key/button binding and action of the primary action</summary>
    protected DirectiveBindingPair _primaryBinding;

    ///<summary>Contains data representing the key/button binding and action of the primary action</summary>
    protected DirectiveBindingPair _secondaryBinding;

    ///<summary>Contains data representing the key/button binding and action of the primary action</summary>
    protected DirectiveBindingPair _exitBinding;
}
