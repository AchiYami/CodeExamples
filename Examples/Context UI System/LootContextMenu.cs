using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/**
Author : Simon Campbell
Category: World Objects - First Person - UI

Notes: None

Changelog:  10/2/22 - Cleaned document up and added comments fitting to CSharp standard.
**/

///<summary>
///Class that handles the Context Sensitive UI in a cofiguration set for when looking at an lootable object.
///</summary>
public class LootContextMenu : ContextSensitiveUIConfiguration
{

    [SerializeField]
    ///<summary>A prefab representing a line in the Loot List UI</summary>
    private GameObject LootListBlockPrefab;

    [SerializeField]
    ///<summary>Integer that represents the currently selected loot item in the list</summary>
    private int chosenItemIndex;

    ///<summary>Temporary colour for the selected item background</summary>
    public Color HighlightColor;


    //These UI Elements are defined, rather than be instantiates like the InteractionContextMenu, as these are not sensitive to
    //what objcct is being looked at. 

    [Header("Help Menu Transforms")]
    [SerializeField]
    ///<summary>UI Text for which button to press to loot the highlighted item.</summary>
    private TMP_Text LootKeyText;

    [SerializeField]
    ///<summary>UI Text for displaying the action label to loot an item.</summary>
    private TMP_Text LootActionText;

    [SerializeField]
    ///<summary>UI Text for displaying which button to press to loot all items</summary>
    private TMP_Text LootAllKeyText;

    [SerializeField]
    ///<summary>UI Text for displaying the action label to loot all items</summary>
    private TMP_Text LootAllActionText;


    [SerializeField]
    ///<summary>UI Text for displaying which button to press to exit the interaction</summary>
    private TMP_Text ExitKeyText;

    [SerializeField]
    ///<summary>UI Text for displaying the action label to exit the interaction</summary>
    private TMP_Text ExitActionText;


    ///<summary>Creates a Loot List UI Block for each Item within the lootable object<summary>
    ///<param name="itemList">The list of Items to populate the Loot List with</param>
    public void PopulateList(List<ItemData> itemList)
    {
        //Clears the list and resets the chosen item to first in the list
        ClearList();
        chosenItemIndex = 0;

        //For each item in the list
        for (int i = 0; i < itemList.Count; i++)
        {

            //Create the UI content, and assign the data
            LootListItem lootBlock = Instantiate(LootListBlockPrefab, ContentPanel).GetComponent<LootListItem>();
            lootBlock.SetData(itemList[i]);

            //If this is the chosen item, highligh it.
            if (i == chosenItemIndex)
            {
                lootBlock.SetBackgroundColor(HighlightColor);
            }
            else //Alternating colour pattern for each other item, this is purely to help readability
            if (i % 2 == 0)
            {
                lootBlock.SetBackgroundColor(Color.black + new Color(0.1f, 0.1f, 0.1f));
            }
            else
            {
                lootBlock.SetBackgroundColor(Color.black);
            }
        }
    }

    ///<summary>Will clear the UI content of each line of UI</summary>
    public void ClearList()
    {
        for (int i = ContentPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(ContentPanel.GetChild(i).gameObject);
        }
    }

    ///<summary>
    ///Refreshes the colour patterns of each item, and checks if an item should be highlighted,
    ///without regenerating the entire list.
    ///</summary>
    public void RefreshBackgroundColors()
    {
        for (int i = 0; i < ContentPanel.childCount; i++)
        {
            LootListItem lootBlock = ContentPanel.GetChild(i).GetComponent<LootListItem>();

            if (i == chosenItemIndex)
            {
                lootBlock.SetBackgroundColor(HighlightColor);
            }
            else
            if (i % 2 == 0)
            {
                lootBlock.SetBackgroundColor(Color.black + new Color(0.1f, 0.1f, 0.1f));
            }
            else
            {
                lootBlock.SetBackgroundColor(Color.black);
            }
        }
    }

    ///<summary>Builtin Unity Update Method.</summary>
    private void Update()
    {
        UpdateKeyboardControls();
        RefreshContext();
    }

    ///<summary>Polls for Keyboard and Mouse Input input for the loot list.</summary>
    private void UpdateKeyboardControls()
    {

        //Grabs the current Mouse and Keyboard.
        Mouse _mouse = Mouse.current;
        Keyboard _keyboard = Keyboard.current;


        //Checks for a downwards mouse scrollwheel, or down arrow key, selects the next item,
        //or returns the start if there is no next item.
        if (_mouse.scroll.ReadValue().y < 0 || _keyboard.downArrowKey.wasPressedThisFrame)
        {
            if (chosenItemIndex > ContentPanel.childCount - 1)
            {
                chosenItemIndex++;
            }
            else
            {
                chosenItemIndex = 0;
            }
            RefreshBackgroundColors();
        }

        //Checks for an upwards mouse scroll or an up arrow key. Selects the previous item
        //or jumps to the bottom if we were at the first item already.
        if (_mouse.scroll.ReadValue().y > 0 || _keyboard.upArrowKey.wasPressedThisFrame)
        {
            if (chosenItemIndex > 0)
            {
                chosenItemIndex--;
            }
            else
            {
                chosenItemIndex = ContentPanel.childCount - 1;
            }
            RefreshBackgroundColors();
        }

    }

    ///<summary>Removes an item from the list.<summary>
    ///<remarks>- Destroy immediate used to ensure the player cannot loot the item while it's being destroyed -</remarks>
    private void RemoveAt(int index)
    {

        DestroyImmediate(ContentPanel.GetChild(index).gameObject);

    }

    ///<summary> Will grab the Item from the LootList, and return it to be added to the player's inventory.</summary>
    public ItemData LootItem()
    {
        //Grabs the item of the currently selected loot list entry
        ItemData _itemToReturn = ContentPanel.GetChild(chosenItemIndex).GetComponent<LootListItem>().GetItem();

        //Removes the item from this container
        RemoveAt(chosenItemIndex);

        //Pushes the highlighted item back one, if there is an item to go backwards to
        if (chosenItemIndex > 0)
        {
            chosenItemIndex--;
        }

        //Refresh the colours
        RefreshBackgroundColors();
        return _itemToReturn;
    }

    ///<summary>Assigns the correct Key/Button label and action labels from the bindings of the object to this UI</summary>
    ///<param name="primaryBinding">The Primary Key/Action Binding for this object</param>
    ///<param name="secondaryBinding">The Optional Secondary Key/Action Binding for this object</param>
    ///<param name="primaryBinding">The Optional Exit Key/Action Binding for this object</param>
    public void SetContext(DirectiveBindingPair primaryBinding, DirectiveBindingPair secondaryBinding, DirectiveBindingPair exitBinding)
    {
        //Sets the Bindings
        _primaryBinding = primaryBinding;
        _secondaryBinding = secondaryBinding;
        _exitBinding = exitBinding;

        //Sets the In Game UI to show the binding Action Labels
        LootActionText.text = _primaryBinding.Label;
        LootAllActionText.text = _secondaryBinding.Label;
        ExitActionText.text = _exitBinding.Label;

        //Sets the In Game UI to show the correct Key or Button to press, depending on what input
        //method was last detected.
        if (DirectivePlayer.INPUT_STATE == DirectivePlayer.InputState.KEYBOARD)
        {
            LootKeyText.text = _primaryBinding.Key;
            LootAllKeyText.text = _secondaryBinding.Key;
            ExitKeyText.text = _exitBinding.Key;
        }
        else
        {
            LootKeyText.text = _primaryBinding.Button;
            LootAllKeyText.text = _secondaryBinding.Button;
            ExitKeyText.text = _exitBinding.Button;
        }

    }

    ///<summary>
    ///Assigns the correct Key/Button label and action labels from the bindings of the object to this UI without reloading the bindings.
    ///</summary>
    public void RefreshContext()
    {
        //Sets the In Game UI to show the binding Action Labels
        LootActionText.text = _primaryBinding.Label;
        LootAllActionText.text = _secondaryBinding.Label;
        ExitActionText.text = _exitBinding.Label;

        //Sets the In Game UI to show the correct Key or Button to press, depending on what input
        //method was last detected.
        if (DirectivePlayer.INPUT_STATE == DirectivePlayer.InputState.KEYBOARD)
        {
            LootKeyText.text = _primaryBinding.Key;
            LootAllKeyText.text = _secondaryBinding.Key;
            ExitKeyText.text = _exitBinding.Key;
        }
        else
        {
            LootKeyText.text = _primaryBinding.Button;
            LootAllKeyText.text = _secondaryBinding.Button;
            ExitKeyText.text = _exitBinding.Button;
        }
    }
}
