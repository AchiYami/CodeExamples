using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

/**
Author : Simon Campbell
Category: World Objects - First Person

Notes: None

Changelog:  10/2/22 - Cleaned document up and added comments fitting to CSharp standard.
**/

///<summary>
///Class that handles generic functionality of an object that can be interacted with
///while the player is on-foot.
///</summary>
public abstract class FirstPersonInteractable : MonoBehaviour, InteractableEntity
{

    ///<summary>Provides a reference for the Context Sensitive Control Prompt UI.</summary>
    protected ContextualInteractionUI _context;

    ///<summary>Provides a refrence to the on-disk player saved data.</summary>
    protected PlayerData _playerData;

    ///<summary>Provides a reference for if the cursor is over this object.</summary>
    protected bool _mouseHasEntered = false;

    ///<summary>Reference to builtin Unity Input Modules.</summary>
    protected PlayerInput _inputModule;

    ///<summary>The Primary and most common functionality for this object.</summary>
    protected InputAction _inputPrimaryAction;

    ///<summary>The secondary and optional functionality for this object.</summary>
    protected InputAction _inputSecondaryAction;

    ///<summary>Optional Action that allows the player to "exit" this object.</summary>
    protected InputAction _inputExitAction;

    ///<summary>Key/Controller Binding for this object's primary Action.</summary>
    protected DirectiveBindingPair _inputPrimaryActionBinding;

    ///<summary>Key/Controller Binding for this object's secondary action.</summary>
    protected DirectiveBindingPair _inputSecondaryActionBinding;

    ///<summary>Key/Controller Binding for this object's exit action.</summary>
    protected DirectiveBindingPair _inputExitActionBinding;

    ///<summary>Optional Audio Clip to be played upon this object's primary action firing.</summary>
    protected AudioClip OnPrimaryAudioClip;
    
    ///<summary>Optional Audio Clip to be played upon this object's secondary action firing.</summary>
    protected AudioClip OnSecondaryAudioClip;

    ///<summary>Optional Audio Clip to be played upon this object's exit action firing.</summary>
    protected AudioClip OnExitAudioClip;


    //Builtin Unity Start method. Fires on Scene start.
    private void Start()
    {
        Initialize();
    }

    // ------------ INITIALIZATION ------------ \\

    ///<summary>
    ///Handles the initialization of a FirstPersonInteractable Object.
    ///Will find the Context UI, playerData, and will handle the creation of keybind pairings.
    ///<summary>
    public virtual void Initialize()
    {

        //Grab References
        _context = DirectiveUIManager.Instance.ContextualInteractionUI;
        _playerData = PlayerData.GetPlayerData();

        //Create each Binding, and tie it to Unity's Input System, and csubscribe to listeners.
        _inputPrimaryAction = GetComponent<PlayerInput>().actions.FindAction("PrimaryInteraction");
        _inputPrimaryAction.performed += HandlePrimaryInteraction;

        _inputSecondaryAction = GetComponent<PlayerInput>().actions.FindAction("SecondaryInteraction");
        _inputSecondaryAction.performed += HandleSecondaryInteraction;

        _inputExitAction = GetComponent<PlayerInput>().actions.FindAction("Exit");
        _inputExitAction.performed += HandleExitInteraction;

        InitializeBindingPairs();

    }

    // ------------ DATA METHODS ------------ \\

    //Group of Properties to handle Getting/Setting of Action Bindings
    public DirectiveBindingPair PrimaryActionBinding { get { return _inputPrimaryActionBinding; } set { _inputPrimaryActionBinding = value; } }
    public DirectiveBindingPair SecondaryActionBinding { get { return _inputSecondaryActionBinding; } set { _inputSecondaryActionBinding = value; } }
    public DirectiveBindingPair ExitActionBinding { get { return _inputExitActionBinding; } set { _inputExitActionBinding = value; } }

    
    // ------------ CONTROL METHODS ------------ \\

    ///<summary>
    ///Method to handle detecting that the mouse is over this objects. 
    ///Does NOT handle the functionality of what happens when the mouse is over. See MouseEnter() for that.
    ///</summary>
    public void MousePointerEnter()
    {
        if (!_mouseHasEntered)
        {
            MouseEnter();
            _mouseHasEntered = true;
        }
    }

    ///<summary>
    ///Method to handle detecting that the mouse is no longer over this objects. 
    ///Does NOT handle the functionality of what happens when the mouse is over. See MouseExit() for that.
    ///</summary>
    public void MousePointerExit()
    {
        if (_mouseHasEntered)
        {
            MouseExit();
            _mouseHasEntered = false;
        }
    }

    ///<summary>Method to handle the functionality that occurs when a mouse is over this object.</summary>
    protected virtual void MouseEnter()
    {
        _context.ShowContextMenu(this);

    }

    ///<summary>Method to handle the functionality that occurs when a mouse is no longer over this object.</summary>
    protected virtual void MouseExit()
    {
        _context.Hide();
    }


    // ------------ EVENT HANDLING ------------ \\

    public virtual void InitializeBindingPairs()
    {
        _inputPrimaryActionBinding = BuildBindingPair(_inputPrimaryAction, "Primary Action");
        _inputSecondaryActionBinding = BuildBindingPair(_inputSecondaryAction, "Secondary Action");
        _inputExitActionBinding = BuildBindingPair(_inputExitAction, "Exit Action");
    }

    ///<summary>
    ///Method to "Build" the KeyBind Pairs. Adds appropriate  key or controller button label to the binding,
    ///using Unity's InputControl.
    ///</summary>
    private DirectiveBindingPair BuildBindingPair(InputAction action, string label)
    {

        //Gets the list of Unity's InputControls.
        List<InputControl> _controls = new List<InputControl>();
        _controls.AddRange(action.controls);

        string _k = "";
        string _b = "";
        string _l = label;

        //Assigns each display name if it's a keystroke or controller.
        for (int i = 0; i < _controls.Count; i++)
        {
            if (_controls[i].layout == "Key")
            {
                _k = _controls[i].displayName;
            }

            if (_controls[i].layout == "Button")
            {
                _b = _controls[i].displayName;
            }
        }

        return new DirectiveBindingPair(_l, _k, _b);
    }

    ///<summary>Method to handle the Primary Interaction using Unity's InputSystem context</summary>
    public virtual void HandlePrimaryInteraction(InputAction.CallbackContext context)
    {
        if (_mouseHasEntered)
        {
            OnPrimaryInteraction();
        }
    }

    ///<summary>Method to handle the Secondary Interaction using Unity's InputSystem context</summary>
    public virtual void HandleSecondaryInteraction(InputAction.CallbackContext context)
    {

        if (_mouseHasEntered)
        {
            OnSecondaryInteraction();
        }
    }

    ///<summary>Method to handle the Exit Interaction using Unity's InputSystem context</summary>
    public virtual void HandleExitInteraction(InputAction.CallbackContext context)
    {
        if (_mouseHasEntered)
        {
            OnExitInteraction();
        }
    }


    ///<summary>
    ///Method to handle the functionality of the Primary Interaction. 
    ///If this metohd is not overridden, it will log an error as a fallback.
    ///</summary>
    public virtual void OnPrimaryInteraction()
    {
        if (_mouseHasEntered)
            Debug.LogError("DIRECTIVE:LOGSCOURGE: ERROR :: FirstPersonInteractable:: OnPrimaryInteraction :: Default Fallback. Have you forgotten to override the virtual OnPrimaryInteraction?");
    }

    ///<summary>
    ///Method to handle the functionality of the Secondary Interaction. 
    ///If this metohd is not overridden, it will log an error as a fallback.
    ///</summary>
    public virtual void OnSecondaryInteraction()
    {
        if (_mouseHasEntered)
            Debug.LogError("DIRECTIVE:LOGSCOURGE: ERROR :: FirstPersonInteractable:: OnSecondaryInteraction :: Default Fallback. Have you forgotten to override the virtual OnSecondaryInteraction?");
    }

    ///<summary>
    ///Method to handle the functionality of the Exit Interaction. 
    ///If this metohd is not overridden, it will log an error as a fallback.
    ///</summary>
    public virtual void OnExitInteraction()
    {
        if (_mouseHasEntered)
            Debug.LogError("DIRECTIVE:LOGSCOURGE: ERROR :: FirstPersonInteractable:: OnExitInteraction :: Default Fallback. Have you forgotten to override the virtual OnExitInteraction?");
    }

}


///<summary>
///Helper Class that allows binding a Key, Controller button and a Label to an input action
///For use with the Context Sensitive UI Prompt
///</summary>
public class DirectiveBindingPair
{
    //The name of the action being carried out 
    string _label;
    //A string representing a keystroke to carry out the action
    string _key;
    //A string representing a controller button to carry out the action
    string _button;


    //Public property to retreive and set the label
    public string Label { get { return _label; } set { _label = value; } }

    //Public property to retreive and set the key
    public string Key { get { return _key; } set { _key = value; } }

    //Public property to retreive and set the button
    public string Button { get { return _button; } set { _button = value; } }

    ///<summary>Constructor Method.</summary>
    ///<param name="label">The name of the action to be carried out</param>
    ///<param name="key">The keystroke associated with the action</param>
    ///<param name="button">The controller button to be associated with the action</param>
    public DirectiveBindingPair(string label, string key, string button)
    {
        _label = label;
        _key = key;
        _button = button;
    }
}
