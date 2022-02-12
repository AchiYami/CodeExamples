using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

/**
Author : Simon Campbell
Category: World Objects - First Person

Notes: None

Changelog:  10/2/22 - Cleaned document up and added comments fitting to CSharp standard.
**/

///<summary>
///Abstract base class that contains the common methods applicale to all world terminals
///<summary>
public abstract class WorldTerminal : FirstPersonInteractable
{
    [Title("Camera Objects")]
    ///<summary>Reference to world position to view this terminal from.</summary>
    public GameObject CameraLockPosition;

    [TitleGroup("Terminal Screens")]
    ///<summary>Default screen that shows when the terminal is not in use.</summary>
    public GameObject EntryScreen;

    [TitleGroup("Terminal Screens")]
    ///<summary>Screen that will appear when player begins to interact.</summary>
    public GameObject DefaultOpenScreen;

    [TitleGroup("Terminal Options")]
    ///<summary>Keeps a track of whether the terminal is locked or not.</summary>
    public bool Locked = false;

    [TitleGroup("Terminal Options")]
    [ShowIf("@this.Locked == true")]
    ///<summary>The screen to display if the terminal is locked.</summary>
    public GameObject LockScreen;

    ///<summary>Reference to player's first person camera.</summary>
    private FirstPersonCamera _camera;

    ///<summary>The world collider for this terminal.</summary>
    private BoxCollider _collider;

    ///<summary>Used to check if this terminal currently in use.</summary>
    private bool _isInUse;


    ///<summary>Default Unity Method - Called upon scene start.</summary>
    public virtual void Start() => Initialize();


    ///<summary>
    ///Performs initialization tasks for this terminal, Binds Inputs for use with the Context Sensitive tooltip, 
    ///loads sound data, and sets the terminal to not in use.
    ///</summary>
    public override void Initialize()
    {
        //Parent Intiailization
        base.Initialize();

        //Binds inputs to the context sensitive tooltip
        _inputPrimaryActionBinding.Label = "Use Terminal";
        _inputSecondaryActionBinding = null;
        _inputExitActionBinding = null;

        //Loads sound clips from data store.
        OnPrimaryAudioClip = DirectiveAudioData.Instance.InteractableSounds.WorldTerminalEnter;
        OnExitAudioClip = DirectiveAudioData.Instance.InteractableSounds.WorldTerminalExit;

        //Sets terminal to not in use.
        _isInUse = false;
    }


    ///<summary>
    ///Called upon when the player looks at this terminal.
    ///Shows the context sensitive tooltip.
    ///</summary>
    protected override void MouseEnter()
    {
        _context.ShowContextMenu(this);
    }


    ///<summary>
    ///Called upon when the play stops looking at this terminal.
    ///Hides the context sensitive tooltip.
    ///</summary>
    protected override void MouseExit()
    {
        _context.Hide();
    }

    ///<summary>
    ///Activates the world terminal, moving the player's camera to the terminal,
    ///and locking the view, and enabling the pointer cursor.
    /// </summary>
    private void Activate()
    {
        //Shows the terminal is in use
        _isInUse = true;

        //Play the audio clip for activating this terminal.
        DirectiveAudioController.Instance.FPSPlayerSource.PlayOneShot(OnPrimaryAudioClip);

        //Grab reference the the player's camera if needed
        if (_camera == null)
        {
            _camera = DirectivePlayer.Instance.PlayerCharacterCamera;
        }

        //Disable camera movement
        LockCameraToTerminal();

        //Grab reference to this terminal's collider
        if (_collider == null)
        {
            _collider = GetComponent<BoxCollider>();
        }

        //Prevent any strange collider issues
        _collider.enabled = false;

        //Swap the visible display screens on the terminal
        EntryScreen.SetActive(false);
        DefaultOpenScreen.SetActive(true);
    }

    ///<summary>
    ///Deactivates this world terminal, returning the position of the camera smoothly to the expected
    ///location of the players head. Returns control of the camera to the player.
    ///</summary>

    public virtual void Deactivate()
    {

        //Play's this terminals exit sound.
        DirectiveAudioController.Instance.FPSPlayerSource.PlayOneShot(OnExitAudioClip);

        //Stop locking the cursor to the center of the screen briefly.
        Cursor.lockState = CursorLockMode.None;

        //Swap this terminal's display screens
        EntryScreen.SetActive(true);
        DefaultOpenScreen.SetActive(false);

        //Return the camera to the player's position
        ReturnCameraToPlayer();

        //Re-enable the collider for this terminal.
        _collider.enabled = true;

        //Clear the raycast and all selected buttons, this prevents the player "pressing" a now disabled button.
        _camera.ClearCachedRaycast();
        base.MousePointerExit();
        EventSystem.current.SetSelectedGameObject(null);

        //Set status to not in use and lock the cursor to the center of the screen 
        //for the first person crosshair effect.
        _isInUse = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>Fires this terminals Activate Method</summary>
    /// <seealso>See Also: FirstPersonInteractable.cs</seealso>
    public override void OnPrimaryInteraction()
    {

        if (_mouseHasEntered && !_isInUse)
            Activate();
    }


    ///<summary>
    /// Locks the first person camera to a designated spot to the terminal.
    /// Locks rotation to match the orientation of the terminal screen.
    /// </summary>
    public void LockCameraToTerminal()
    {
        _camera.Lock(CameraLockPosition.transform.position, CameraLockPosition.transform.rotation.eulerAngles);
    }

    /// <summary>
    /// Returns the camera's position and rotation to match that of the player's head.
    /// </summary>
    public void ReturnCameraToPlayer()
    {
        _camera.ResetCamera();
    }
}
