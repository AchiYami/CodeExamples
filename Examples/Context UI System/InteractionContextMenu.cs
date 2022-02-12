using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
Author : Simon Campbell
Category: World Objects - First Person - UI

Notes: None

Changelog:  10/2/22 - Cleaned document up and added comments fitting to CSharp standard.
**/

///<summary>
///Class that handles the Context Sensitive UI in a cofiguration set for when looking at an interactable object.
///</summary>
public class InteractionContextMenu : ContextSensitiveUIConfiguration
{
    ///<summary>UI that shows which Key/Button to press</summary>
    private TMP_Text _keyString;

    ///<summary>UI Text that shows which action the key will fire</summary>
    private TMP_Text _interactionString;

    [SerializeField]
    ///<summary>The prefab that represents a line of the Context UI</summary>
    private GameObject ContextLinePrefab;

    ///<summary>A container for the context lines to be shown</summary>
    private List<InteractionContextMenuLine> _contextLines;


    ///<summary>Method that sets the data for the UI.</summary>
    ///<param name="primaryBinding">The Primary Key/Action Binding for this object</param>
    ///<param name="secondaryBinding">The Optional Secondary Key/Action Binding for this object</param>
    ///<param name="primaryBinding">The Optional Exit Key/Action Binding for this object</param>
    public void SetContext(DirectiveBindingPair primaryBinding, DirectiveBindingPair secondaryBinding = null, DirectiveBindingPair exitBinding = null)
    {
        //Resets the Context
        ClearContext();

        _primaryBinding = primaryBinding;
        _secondaryBinding = secondaryBinding;
        _exitBinding = exitBinding;


        //Instantiates a new Prefabn and sets the Data for the primary binding. -- This is REQUIRED.
        InteractionContextMenuLine newLine = Instantiate(ContextLinePrefab, ContentPanel).GetComponent<InteractionContextMenuLine>();
        newLine.SetData(_primaryBinding);


        //Creates a new instance of a Context Line prefab, and sets the data for the secondary binding,
        //if one is set. 
        if (_secondaryBinding != null)
        {
            newLine = Instantiate(ContextLinePrefab, ContentPanel).GetComponent<InteractionContextMenuLine>();
            newLine.SetData(_secondaryBinding);
        }

        //Creates a new instance of a Context Line prefab, and sets the data for the exit binding,
        //if one is set. 
        if (_exitBinding != null)
        {
            newLine = Instantiate(ContextLinePrefab, ContentPanel).GetComponent<InteractionContextMenuLine>();
            newLine.SetData(_exitBinding);
        }
    }

    ///<summary>Clears the Context UI of it's current content</summary>
    private void ClearContext()
    {
        for (int i = 0; i < ContentPanel.childCount; i++)
        {
            Destroy(ContentPanel.GetChild(i).gameObject);
        }
    }

}
